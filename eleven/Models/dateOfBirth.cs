using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eleven.Models
{
    public class dateOfBirth
    {
        public int day { get; set; }
        public int month { get; set; }
        public int year { get; set; }
      

        public dateOfBirth(string d , string m , string y )
        {
        
            day = Int32.Parse(d);
            month = Int32.Parse(m);
            year = Int32.Parse(y);
        }
    }
}
