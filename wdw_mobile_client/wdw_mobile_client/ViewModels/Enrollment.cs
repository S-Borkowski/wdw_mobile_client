namespace wdw_mobile_client
{
    public class Enrollment
    {
        public int id { get; set; }
        public string startDate { get; set; }
        public string endDate { get; set; }
        public string specialisation { get; set; }
        public Lecture[] lectures { get; set; }
        public int ectsLimit { get; set; }
        public int ects { get; set; }
        public int availableEcts
        {
            get
            {
                return ectsLimit - ects;
            }
        }
    }
}
