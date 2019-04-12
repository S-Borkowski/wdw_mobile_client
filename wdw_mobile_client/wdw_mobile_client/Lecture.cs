using System;

namespace wdw_mobile_client
{
    public class Lecture
    {
        public int id { get; set; }
        public string name { get; set; }
        public string lecturer { get; set; }
        public int ects { get; set; }
        public string auditorium { get; set; }
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
                if (Array.IndexOf(LectureListPage.student.lectures, $"/lectures/{id}") > -1)
                {
                    return "#2196f3"; //blue
                }
                else if (LectureListPage.enrollment.availableEcts == 0)
                {
                    return "#757575"; //gray
                }
                else if (freeSlots != 0)
                {
                    return "#43a047"; //green
                }
                else
                {
                    return "#d32f2f"; //red
                }
            }
            set
            {
                color = value;
            }
        }
        //public string heading
        //{
        //    get
        //    {
        //        if (color == "#2196f3") //blue
        //        {
        //            return "enrolled";
        //        }
        //        else if (color == "#757575") //gray
        //        {
        //            return "unavailable";
        //        }
        //        else if (color == "#43a047") //green
        //        {
        //            return "available";
        //        }
        //        else
        //        {
        //            return "full";
        //        }
        //    }
        //    set
        //    {
        //        heading = value;
        //    }
        //}
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
