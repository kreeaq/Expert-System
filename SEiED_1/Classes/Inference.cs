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
        public static List<Fact> factsThatAreAlsoConclusions(List<Rule> rules)
        {
            List<List<Predicate>> conclusions = rules.Select(r => r.Conclusions).Select(x => x).ToList();
            List<Predicate> predsCon = new List<Predicate>();
            foreach (var listPred in conclusions)
            {
                foreach (var pred in listPred)
                {
                    predsCon.Add(pred);
                }
            }

            List<List<Predicate>> facts = rules.Select(r => r.Facts).ToList();
            List<Fact> predFacts = new List<Fact>();
            foreach (var listPred in facts)
            {
                foreach (var pred in listPred)
                {
                    predFacts.Add((Fact)pred);
                }
            }
            return predFacts.Where(f => predsCon.Any(c => f.Name == c.Name)).Distinct().ToList();
        }

        public static void Calculate(List<Rule> rules)
        {
            bool shouldBreak = false;
            do
            {
                foreach (Rule rule in rules)
                {
                    //Sprawdz czy regula jest wykonywalna
                    bool result = true;
                    foreach (Fact fact in rule.Facts)
                    {
                        if (fact.IsKnown)
                        {
                            result = result && fact.Value;
                        }
                        else
                        {
                            rule.ToSkip = true;
                            shouldBreak = true;
                            break; 
                        }
                    }

                    if (shouldBreak)
                    {
                        shouldBreak = false;
                        break;
                    }

                    foreach (Conclusion conclusion in rule.Conclusions)
                    {
                        conclusion.Value = result;
                        //Find facts with similar name to conclusion
                        List<Fact> tmp = factsThatAreAlsoConclusions(rules);
                        if(tmp.Count > 0)
                        {
                            Fact exactFact = tmp.Where(c => c.Name == conclusion.Name).Single();
                            exactFact.Value = result;
                        }                        
                    }
                    rule.ToSkip = false;
                }
            }
            while (rules.Where(r => r.ToSkip == true).Count() > 0);
        }
       
    }
}
