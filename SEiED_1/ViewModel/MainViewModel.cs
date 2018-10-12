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

        private string _facts;

        public string Facts
        {
            get { return _facts; }
            set
            {
                _facts = value;
                RaisePropertyChanged("Facts");
            }
        }

        private string _conclusions;

        public string Conclusions
        {
            get { return _conclusions; }
            set
            {
                _conclusions = value;
                RaisePropertyChanged("Conclusions");
            }
        }

        StackPanel stackPanel;


        private void StartProcess()
        {
            
            Facts = "";
            Conclusions = "";
            List<Rule> rules = Parser(@"C:\Users\Maciej Kuœnierz\source\repos\SEiED_1\SEiED_1\Resources\BazaWiedzy.txt");
            FactsToBool(rules);
            Inference.Calculate(rules);
            stackPanel = new StackPanel
            {
                
                Orientation = Orientation.Vertical
            };

            foreach (Rule rule in rules)
            {                
                foreach (Fact fact in rule.Facts)
                {
                    CheckBox cb = new CheckBox();
                    //cb.Name = fact.Name.ToString();
                    //cb.Content = fact.Name.ToString();
                    cb.IsChecked = fact.Value;
                    stackPanel.Children.Add(cb);

                    Facts += fact.Name + "=" + fact.Value.ToString() +  '\n';
                }
                foreach (Conclusion conclusion in rule.Conclusions)
                {
                    Conclusions += conclusion.Name + "=" + conclusion.Value.ToString() + '\n';
                }
            }
        }

        private void FactsToBool(List<Rule> rules)
        {
            //TODO: Apply boolean values to facts
            rules[0].Facts[0].Value = true;
            rules[0].Facts[1].Value = false;
            rules[0].Facts[2].Value = true;
            rules[1].Facts[0].Value = false;
            rules[1].Facts[1].Value = true;

            return;
        }

        private List<Rule> Parser(string filePath)
        {
            String line;
            List<Rule> rules = new List<Rule>();
            try
            {
                //Pass the file path and file name to the StreamReader constructor
                StreamReader sr = new StreamReader(filePath);

                //Read the first line of text
                line = sr.ReadLine();
                line = line.Replace(" ", "");

                //Continue to read until you reach end of file
                while (line != null)
                {
                    Rule rule = new Rule
                    {
                        Facts = new List<Fact>(),
                        Conclusions = new List<Conclusion>()
                    };
                    string[] splittedLine = line.Split(new string[] { "->" }, StringSplitOptions.None);
                    foreach (var it in splittedLine[0].Split(','))
                    {
                        Fact fact = new Fact
                        {
                            Name = it
                        };
                        rule.Facts.Add(fact);
                    }
                    foreach (var it in splittedLine[1].Split(','))
                    {
                        Conclusion conclusion = new Conclusion
                        {
                            Name = it
                        };
                        rule.Conclusions.Add(conclusion);
                    }
             

                    //Add rule to rules
                    rules.Add(rule);

                    //Read the next line
                    line = sr.ReadLine();
                }

                //close the file
                sr.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            return rules;
        }
    }
}