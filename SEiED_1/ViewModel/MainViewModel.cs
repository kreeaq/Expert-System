using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using SEiED_1.Classes;
using SEiED_1.Model;
using System;
using System.Collections.Generic;
using System.IO;
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
        public RelayCommand LoadFileCommand { get; private set; }

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            ////if (IsInDesignMode)
            ////{
            ////    // Code runs in Blend --> create design time data.
            ////}
            ////else
            ////{
            ////    // Code runs "for real"
            ////}
            ///

            StartProcessCommand = new RelayCommand(StartProcess);
            LoadFileCommand = new RelayCommand(LoadFile);
        }

        private string _filePath;

        public string FilePath
        {
            get { return _filePath; }
            set
            {
                _filePath = value;
                RaisePropertyChanged("FilePath");
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


        List<Rule> rules; 

        private void LoadFile()
        {
            rules = Parser.Parse(@"C:\Users\Maciej Kuœnierz\source\repos\SEiED_1\SEiED_1\Resources\BazaWiedzy.txt");
            FactsToBool(rules);
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
                    if (CheckFact(fact.Name))
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
                    
                    //Conclusions += conclusion.Name + "=" + conclusion.Value.ToString() + '\n';
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
            rules[1].Facts[0].Value = false;
            //rules[1].Facts[1].Value = true;

            return;
        }

        

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