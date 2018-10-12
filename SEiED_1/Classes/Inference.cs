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
                bool result = true;
                foreach(Fact fact in rule.Facts)
                {
                    result = result && fact.Value;
                }
                foreach(Conclusion conclusion in rule.Conclusions)
                {
                    conclusion.Value = result;
                }
            }
        }
       
    }
}
