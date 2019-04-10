using System;
using System.Collections.Generic;
using System.Text;

namespace wdw_mobile_client
{
    public class Lecture
    {
        public int idLecture { get; set; }
        public string name { get; set; }
        public string lecturer { get; set; }
        public int ects { get; set; }
        public string aditorium { get; set; }
        public string weekday { get; set; }
        public string week { get; set; }
        public string hour { get; set; }
        public int slots { get; set; }
        public int slotsOccupied { get; set; }
        public int freeSlots
        {
            get
            {
                return slots - slotsOccupied;
            }
        }
        public string color
        {
            get
            {
                if (Array.IndexOf(LectureListPage.student.lectures, $"/lectures/{idLecture}") > -1)
                {
                    return "#2196f3";
                }
                else if (freeSlots != 0)
                {
                    return "#43a047";
                }
                else
                {
                    return "#d32f2f";
                }
            }
        }
    }
}
