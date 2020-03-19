using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eleven.Models
{
    public class contactInformation
    {
        public string primary { get; set; }
        public string secondary { get; set; }

        public contactInformation(string val)
        {
            int i = 0;
            while (i < val.Length)
            {
                if (val[i] == ',' || i == val.Length)
                {
                    break;
                }
                primary += val[i];
                i++;
            }
            if (i !=  val.Length)
            while (i < val.Length)
            {
                if (val[i] == ',')
                {
                    break;
                }
                    secondary += val[i];
                    i++;
            }
            else
                secondary = "";
        }
    }
}
