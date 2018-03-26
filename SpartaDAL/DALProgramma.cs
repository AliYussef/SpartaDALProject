using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sparta.Model;

namespace Sparta.Dal
{
    public class DALProgramma
    {
        public static List<string> GetTestLijst()
        {
            // make a list for the test tap
            List<string> test = new List<string>();

            test.Add("Ali");
            test.Add("Nadia");
            test.Add("Danuta");
            test.Add("Ali");
            test.Add("Nadia");
            test.Add("Danuta");

            return test;
        }

    }
}
