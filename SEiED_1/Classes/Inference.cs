using SEiED_1.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEiED_1.Classes
{
    public static class Inference
    {
        public static void Calculate(List<Rule> rules)
        {
            foreach(Rule rule in rules)
            {
                //Sprawdz czy regula jest wykonywalna
                bool result = true;
                foreach(Fact fact in rule.Facts)
                {
                    if (fact.IsKnown)
                    {
                        result = result && fact.Value;
                    }
                    else
                    {
                        rule.ToSkip = true;
                        break;
                    }
                }
                foreach(Conclusion conclusion in rule.Conclusions)
                {
                    conclusion.Value = result;
                    //foreach (Rule rule in rules)
                    //{
                    //    foreach(Fact fact in rule)
                    //    {
                    //        if (fact.Name == conclusion.Name)
                    //        {
                    //            fact.Value = conclusion.Value;
                    //        }
                    //    }
                    //}
                }
            }
        }
       
    }
}
