namespace wdw_mobile_client
{
    public class User
    {
        public string token { get; set; }
        public string user { get; set; }
        public Enrollment[] enrollments { get; set; }
    }
}
