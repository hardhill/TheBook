using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
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
    public class DetailActivity : Activity
    {
        EditText edtName, edtAddress, edtDescription, edtLongitude, edtLatitude;
        PointOfInterest _poi;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

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

        protected void UpdateUI()
        {
            edtName.Text = _poi.Name;
            edtAddress.Text = _poi.Address;
            edtDescription.Text = _poi.Description;
            edtLongitude.Text = _poi.Longitude.ToString();
            edtLatitude.Text = _poi.Latitude.ToString();
        }
    }
}