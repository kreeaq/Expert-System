using SEiED_1.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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
                    if (!line.Contains("->"))
                    {
                        MessageBox.Show("Knowledge database has wrong structure");
                        return null;
                    }
                    ruleID++;
                    Rule rule = new Rule
                    {
                        ID = ruleID,
                        Facts = new List<Predicate>(),
                        Conclusions = new List<Predicate>()
                    };
                    string[] splittedLine = line.Split(new string[] { "->" }, StringSplitOptions.None);
                    foreach (var it in splittedLine[0].Split(','))
                    {
                        if (!rules.Any(x => x.Facts.Any(f => f.Name == it)))
                        {
                            Fact fact = new Fact
                            {
                                Name = it
                            };
                            rule.Facts.Add(fact);
                        }
                        else
                        {
                            rule.Facts.Add(rules.Where(r => r.Facts.FirstOrDefault(f => f.Name == it) != null).Select(r => r.Facts.First(f => f.Name == it)).First());
                        }                        
                    }
                    foreach (var it in splittedLine[1].Split(','))
                    {
                        if (!rules.Any(x => x.Facts.Any(f => f.Name == it) && !rules.Any(r => r.Conclusions.Any(c => c.Name == it))))
                        {
                            Conclusion conclusion = new Conclusion
                            {
                                Name = it
                            };
                            rule.Conclusions.Add(conclusion);
                        }
                        else if (rules.Any(x => x.Facts.Any(f => f.Name == it)))
                        {
                            //Predicate that parser read as a Conclusion is already a Fact!
                            rule.Conclusions.Add(rules.Where(r => r.Facts.FirstOrDefault(f => f.Name == it) != null).Select(r => r.Facts.First(f => f.Name == it)).First());
                        }
                        else if (rules.Any(r => r.Conclusions.Any(c => c.Name == it)))
                        {
                            //Predicate that parser read as a Conclusion is already a Conclusion somewhere!
                            rule.Conclusions.Add(rules.Where(r => r.Conclusions.FirstOrDefault(c => c.Name == it) != null).Select(r => r.Conclusions.First(c => c.Name == it)).First());
                        }
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
