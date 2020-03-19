using eleven.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eleven.Models
{

    public class employees
    {

        public int userId { get; set; }
        public string Name { get; set; }
        public string nationality { get; set; }
        public Boolean isIndian => nationality == "India" ? true : false;
        public string Address { get; set; }
        public contactInformation contactDetail { get; set; }
        public dateOfBirth Age { get; set; }
        public string currentCompanyExp { get; set; }

        
        
    }
}
