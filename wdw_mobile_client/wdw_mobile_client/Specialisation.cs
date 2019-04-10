using System;
using System.Collections.Generic;
using System.Text;

namespace wdw_mobile_client
{
    public class Specialisation
    {
        public int idSpecialisation { get; set; }
        public string name { get; set; }
        public string studiesType { get; set; }
        public string degreeCourse { get; set; }
        public string semester { get; set; }
        public int ectsLimit { get; set; }
        public string[] lectures { get; set; }
    }
}
