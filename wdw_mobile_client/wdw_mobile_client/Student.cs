using System;
using System.Collections.Generic;
using System.Text;

namespace wdw_mobile_client
{
    public class Student
    {
        public int idUser { get; set; }
        public string[] roles { get; set; }
        public string password { get; set; }
        public string salt { get; set; }
        public string username { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string email { get; set; }
        public string[] lectures { get; set; }
        public string[] specialisations { get; set; }
        public int ects { get; set; }
    }
}
