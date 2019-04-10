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
            label.Text = $"This is {lecture.name} lecture page.";
		}
	}
}