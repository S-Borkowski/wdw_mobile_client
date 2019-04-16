using System.Collections.ObjectModel;

namespace wdw_mobile_client
{
    public class GroupLectures : ObservableCollection<Lecture>
    {
        public string longName { get; set; }
        public string shortName { get; set; }
    }
}
