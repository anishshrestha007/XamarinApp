using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Net.Http;
using static Android.Provider.SyncStateContract;
using System.Text;
using System.Net.Http.Headers;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace XamarinFormsTestApp.Droid
{
	[Activity(Label = "XamarinFormsTestApp", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]

	
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
	{
		
		protected override void OnCreate(Bundle bundle)
		{
			TabLayoutResource = Resource.Layout.Tabbar;
			ToolbarResource = Resource.Layout.Toolbar;

			base.OnCreate(bundle);

			global::Xamarin.Forms.Forms.Init(this, bundle);
			SetContentView(Resource.Layout.Home);
			Button btnAdd = FindViewById<Button>(Resource.Id.btnAdd);
			btnAdd.Click += BtnAdd_Click;
			GetAllData();

		}

		private async void BtnAdd_Click(object sender, EventArgs e)
		{
			EmployeeModel p = new EmployeeModel();

			TextView txtNum1 = FindViewById<TextView>(Resource.Id.txtNum1);
			TextView txtNum2 = FindViewById<TextView>(Resource.Id.txtNum2);

			p.Id = int.Parse(txtNum1.Text.Trim());
			p.Name = txtNum2.Text.Trim();

			//Uri requestUri = new Uri("http://localhost:2070/api/todos"); //replace your Url  
			var objClint = new HttpClient();
			//objClint.DefaultRequestHeaders.Accept.Clear();
			//objClint.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
			//objClint.MaxResponseContentBufferSize = 256000;
			//objClint.BaseAddress = new Uri("http://localhost:4954/");
			var RestUrl = "http://172.18.11.159:9091/api/todos/";
			var json = Newtonsoft.Json.JsonConvert.SerializeObject(p);
			var content = new StringContent(json, Encoding.UTF8, "application/json");

			var uri = new Uri(string.Format(RestUrl, string.Empty));
			try
			{
				HttpResponseMessage respon = await objClint.PostAsync(uri, content);
				if (respon.IsSuccessStatusCode)
				{
					TextView result1 = FindViewById<TextView>(Resource.Id.textView1);
					result1.Text = "Data Saved Successfully.";
					//label1.Text = "Data Saved Succssfully";

				}
			}
			catch (Exception ex)
			{

				throw ex;
			}


		}

		public async void GetAllData()
		{
			EmployeeModel p = new EmployeeModel();
			//p.Id = 2;


			//Uri requestUri = new Uri("http://localhost:2070/api/todos"); //replace your Url  

			string json = "";
			json = Newtonsoft.Json.JsonConvert.SerializeObject(p);
			var objClint = new System.Net.Http.HttpClient();
			objClint.BaseAddress = new Uri("http://172.18.11.159:9091/");
												  //System.Net.Http.HttpResponseMessage respon = await objClint.GetAsync("api/todos/" + p.Id);
			System.Net.Http.HttpResponseMessage respon = await objClint.GetAsync("api/todos/");
			if (respon.IsSuccessStatusCode)
			{
				ListView result1 = FindViewById<ListView>(Resource.Id.listView1);
				var result = await respon.Content.ReadAsStringAsync();

				var Items = JsonConvert.DeserializeObject<String[]>(result);
				
				var data = new string[] {
   "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"
};
				result1.Adapter = new ArrayAdapter(
					this, Resource.Layout.listViewTemplate, Items);




			}
		}
	}
}

