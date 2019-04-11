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
                    Heading = "Enrolled";
                    return "#2196f3"; //blue

                }
                else if (LectureListPage.student.ects == 0)
                {
                    Heading = "Unavailable";
                    return "#757575"; //gray
                }
                else if (freeSlots != 0)
                {
                    Heading = "Available";
                    return "#43a047"; //green
                }
                else
                {
                    Heading = "Full";
                    return "#d32f2f"; //red
                }
            }
            set
            {
                color = value;
            }
        }
        public string Heading
        {
            get
            {
                if (color == "#2196f3") //blue
                {
                    return "Enrolled";
                }
                else if (color == "#757575") //gray
                {
                    return "Unavailable";
                }
                else if (color == "#43a047") //green
                {
                    return "Available";
                }
                else
                {
                    return "Full";
                }
            }
            set
            {
                Heading = value;
            }
        }
        public bool? isSigned
        {
            get 
            {
                if (color == "#2196f3") //blue
                {
                    return true; //can disenroll
                }
                else if (color == "#757575") //gray
                {
                    return false; //cant enroll or disenroll
                }
                else if (color == "#43a047") //green
                {
                    return null; //can enroll
                }
                else
                {
                    return false; //cant enroll or disenroll
                }
            }
            set
            {
                isSigned = value;
            }
        }
    }
}
