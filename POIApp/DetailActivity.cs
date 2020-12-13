using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using POIApp.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POIApp
{
    [Activity(Label = "DetailActivity")]
    public class DetailActivity : AppCompatActivity
    {
        EditText edtName, edtAddress, edtDescription, edtLongitude, edtLatitude;
        PointOfInterest _poi;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);

            SetContentView(Resource.Layout.detail_poi);
            edtName = FindViewById<EditText>(Resource.Id.edtName);
            edtAddress = FindViewById<EditText>(Resource.Id.edtAddress);
            edtDescription = FindViewById<EditText>(Resource.Id.edtDescription);
            edtLongitude = FindViewById<EditText>(Resource.Id.edtLongitude);
            edtLatitude = FindViewById<EditText>(Resource.Id.edtLatitude);
            if (Intent.HasExtra("poi"))
            {
                string poiJson = Intent.GetStringExtra("poi");
                _poi = JsonConvert.DeserializeObject<PointOfInterest>(poiJson);
            }
            else
            {
                _poi = new PointOfInterest();
            }
            UpdateUI();

        }
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_detailview, menu);
            return base.OnCreateOptionsMenu(menu);
        }
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.actionSave:
                    SavePOI();
                    return true;
                    
                    
            case Resource.Id.actionDelete:
                    DeletePOI();
                    return true;
                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        private void DeletePOI()
        {
            DeletePOIAsync();
        }

        private void SavePOI()
        {
            double tempLatitude = Convert.ToDouble(edtLatitude.Text);
            double tempLongitude = Convert.ToDouble(edtLongitude.Text);
            _poi.Name = edtName.Text;
            _poi.Description = edtDescription.Text;
            _poi.Address = edtAddress.Text;
            _poi.Latitude = tempLatitude;
            _poi.Longitude = tempLongitude;
            CreateOrUpdatePOIAsync(_poi);
        }

        

        protected void UpdateUI()
        {
            edtName.Text = _poi.Name;
            edtAddress.Text = _poi.Address;
            edtDescription.Text = _poi.Description;
            edtLongitude.Text = _poi.Longitude.ToString();
            edtLatitude.Text = _poi.Latitude.ToString();
        }

        private async void CreateOrUpdatePOIAsync(PointOfInterest poi)
        {
            POIService service = new POIService();
            if (!service.isConnected(this))
            {
                Toast toast = Toast.MakeText(this, "Not conntected to internet.Please check your device network settings.", ToastLength.Short);
                toast.Show();
                return;
            }
            string response = await service.CreateOrUpdatePOIAsync(_poi);
            if (!string.IsNullOrEmpty(response))
            {
                Toast toast = Toast.MakeText(this, String.Format("{0} saved.", _poi.Name), ToastLength.Short);
                toast.Show();
                Finish();
            }
            else
            {
                Toast toast = Toast.MakeText(this, "Something went Wrong!", ToastLength.Short);
                toast.Show();
            }
        }

        private async void DeletePOIAsync()
        {
            POIService service = new POIService();
            if (!service.isConnected(this))
            {
                Toast toast = Toast.MakeText(this, "Not conntected to internet. Please check your device network settings.", ToastLength.Short);
                toast.Show();
                return;
            }
            string response = await service.DeletePOIAsync(_poi.Id);
            if (!string.IsNullOrEmpty(response))
            {
                Toast toast = Toast.MakeText(this, String.Format("{0} deleted.", _poi.Name), ToastLength.Short);
                toast.Show();
                Finish();
            }
            else
            {
               Toast toast = Toast.MakeText(this, "Something went Wrong!",ToastLength.Short);
               toast.Show();
            }
        }
    }
}