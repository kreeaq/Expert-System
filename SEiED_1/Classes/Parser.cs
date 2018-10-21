using SEiED_1.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEiED_1.Classes
{
    public static class Parser
    {
        public static List<Rule> Parse(string filePath)
        {
            String line;
            List<Rule> rules = new List<Rule>();
            try
            {
                //Pass the file path and file name to the StreamReader constructor
                StreamReader sr = new StreamReader(filePath);

                //Read the first line of text
                line = sr.ReadLine();
                int ruleID = 0;
                //Continue to read until you reach end of file
                while (line != null)
                {
                    line = line.Replace(" ", "");
                    ruleID++;
                    Rule rule = new Rule
                    {
                        ID = ruleID,
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
                //HWDPMessageBox.Show(e.Message);
            }
            return rules;
        }
    }
}
