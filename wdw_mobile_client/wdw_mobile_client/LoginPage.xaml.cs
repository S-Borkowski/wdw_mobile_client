using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace wdw_mobile_client
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LoginPage : ContentPage
	{
		public LoginPage ()
		{
			InitializeComponent ();
        }

        private void LoginBtn_Clicked(object sender, EventArgs e)
        {
            HttpClient _client;

            string id = student_id.Text;
            string pass = password.Text;

            _client = new HttpClient();

            loginBtn.IsEnabled = false;
            App.Current.MainPage = new LectureListPage();
            loginBtn.IsEnabled = true;
        }
    }
}