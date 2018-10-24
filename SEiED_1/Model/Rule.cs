using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEiED_1.Model
{
    public class Rule
    {
        public int ID { get; set; }
        public List<Predicate> Facts { get; set; }
        public List<Predicate> Conclusions { get; set; }
        public bool ToSkip { get; set; } = false;
    }
}
