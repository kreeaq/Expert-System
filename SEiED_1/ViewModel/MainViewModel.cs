using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using SEiED_1.Classes;
using SEiED_1.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace SEiED_1.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {

        public RelayCommand StartProcessCommand { get; private set; }
        public RelayCommand PreviousStepCommand { get; private set; }
        public RelayCommand NextStepCommand { get; private set; }
        public RelayCommand<object> DragDropCommand { get; private set; }

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            StartProcessCommand = new RelayCommand(StartProcess);
            PreviousStepCommand = new RelayCommand(PreviousStep);
            NextStepCommand = new RelayCommand(NextStep);
            DragDropCommand = new RelayCommand<object>((x) => DragDrop(x));
            CurrentView = 0;
        }

        private int _currentView = 0;

        public int CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                RaisePropertyChanged("CurrentView");
            }
        }

        private List<Fact> _facts;

        public List<Fact> Facts
        {
            get { return _facts; }
            set
            {
                _facts = value;
                RaisePropertyChanged("Facts");
            }
        }

        private List<Conclusion> _conclusions;

        public List<Conclusion> Conclusions
        {
            get { return _conclusions; }
            set
            {
                _conclusions = value;
                RaisePropertyChanged("Conclusions");
            }
        }

        private bool _isPreviousStepEnabled = false;

        public bool IsPreviousStepEnabled
        {
            get { return _isPreviousStepEnabled; }
            set
            {
                _isPreviousStepEnabled = value;
                RaisePropertyChanged("IsPreviousStepEnabled");
            }
        }

        private bool _isNextStepEnabled = false;

        public bool IsNextStepEnabled
        {
            get { return _isNextStepEnabled; }
            set
            {
                _isNextStepEnabled = value;
                RaisePropertyChanged("IsNextStepEnabled");
            }
        }

        private string _windowText = "Drop file here";

        public string WindowText
        {
            get { return _windowText; }
            set
            {
                _windowText = value;
                RaisePropertyChanged("WindowText");
            }
        }

        private void PreviousStep()
        {
            CurrentView--;
            IsNextStepEnabled = true;
            IsPreviousStepEnabled = false;
        }

        private bool fileDroppedCorrectly = false;
        private void NextStep()
        {
            if (fileDroppedCorrectly)
            {
                CurrentView++;
                IsNextStepEnabled = false;
                IsPreviousStepEnabled = true;
                StartProcess();
            }
        }

        private void DragDrop(object args)
        {
            DragEventArgs DEA = args as DragEventArgs;
            string filePath = "";
            if (DEA.Data.GetDataPresent(DataFormats.FileDrop))
            {
                // Note that you can have more than one file.
                filePath = ((string[])DEA.Data.GetData(DataFormats.FileDrop)).FirstOrDefault();

                FileInfo FI = new FileInfo(filePath);

                if (!FI.Extension.Contains("txt"))
                {
                    MessageBox.Show("Dropped file has wrong extension!");
                    fileDroppedCorrectly = false;
                    return;
                }
                WindowText = filePath;
                fileDroppedCorrectly = true;
                LoadFile(filePath);
                IsNextStepEnabled = true;
            }
            else
            {
                fileDroppedCorrectly = false;
            }
        }


        List<Rule> rules; 

        private void LoadFile(string filePath)
        {
            rules = Parser.Parse(filePath);
            SetUnknownFacts(rules);
        }

        /// <summary>
        /// Set "niedopytywalne" fakty IsKnown to false
        /// </summary>
        /// <param name="rules"></param>
        private void SetUnknownFacts(List<Rule> rules)
        {
            var unknownFacts = Inference.factsThatAreAlsoConclusions(rules);
            unknownFacts.Select(uf => { uf.IsKnown = false; return uf; }).ToList();
        }

        StackPanel stackPanel;
        StackPanel stackPanel1;

        private void StartProcess()
        {
            Inference.Calculate(rules);
            stackPanel = new StackPanel
            {  
                Orientation = Orientation.Vertical
            };

            stackPanel1 = new StackPanel
            {
                Orientation = Orientation.Vertical
            };


            List<Fact> tmpFacts = new List<Fact>();
            List<Conclusion> tmpConclusions= new List<Conclusion>();
            foreach (Rule rule in rules)
            {
                foreach (Fact fact in rule.Facts)
                {
                    if (CheckFact(fact.Name) && !tmpFacts.Any(f => f.Name==fact.Name))
                    {
                        CheckBox cb = new CheckBox();
                        cb.IsChecked = fact.Value;
                        stackPanel.Children.Add(cb);

                        tmpFacts.Add(fact);
                    }
                }
                foreach (Conclusion conclusion in rule.Conclusions)
                {
                    CheckBox cb = new CheckBox();
                    cb.IsChecked = conclusion.Value;
                    stackPanel1.Children.Add(cb);

                    tmpConclusions.Add(conclusion);
                }
            }
            Facts = new List<Fact>(tmpFacts);
            Conclusions = new List<Conclusion>(tmpConclusions);
        }

        private void FactsToBool(List<Rule> rules)
        {
            //TODO: Apply boolean values to facts
            rules[0].Facts[0].Value = true;
            rules[0].Facts[1].Value = false;
            rules[0].Facts[2].Value = true;
            rules[2].Facts[0].Value = false;
            return;
        }

        
        /// <summary>
        /// Check if fact is not also a conclusion
        /// In such a situation, fact is "niedopytywalny"
        /// Taht means, that user should be unable
        /// to change its (this fact) logic value
        /// </summary>
        /// <param name="it"></param>
        /// <returns></returns>
        private bool CheckFact(string it)
        {
            foreach (Rule rule in rules)
            {
                foreach(Conclusion conclusion in rule.Conclusions)
                {
                    if (conclusion.Name == it)
                        return false;
                }
            }
            return true;
        }
    }
}