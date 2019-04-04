using System;
using System.Collections.Generic;
using System.Text;

namespace wdw_mobile_client
{
    class Lecture
    {
        public string name { get; set; }
        public string lecturer { get; set; }
        public int ects { get; set; }
        public string aditorium { get; set; }
        public string weekday { get; set; }
        public string week { get; set; }
        public string hour { get; set; }
        public int slots { get; set; }
        public int slotsOccupied { get; set; }
    }
}
