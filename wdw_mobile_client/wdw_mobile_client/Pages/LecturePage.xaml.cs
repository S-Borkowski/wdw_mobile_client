using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace wdw_mobile_client
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LecturePage : ContentPage
	{
        private Lecture lecture;

		public LecturePage (Lecture lect)
		{
			InitializeComponent ();
            lecture = lect;
            label.Text = $"You can sign - {lecture.isSigned}.";

            if(lecture.isSigned == null)
            {
                enroll.IsEnabled = true;
                disenroll.IsEnabled = false;
            }
            else if(lecture.isSigned == true)
            {
                enroll.IsEnabled = false;
                disenroll.IsEnabled = true;
            }
            else
            {
                enroll.IsEnabled = false;
                disenroll.IsEnabled = false;
            }
        }
	}
}