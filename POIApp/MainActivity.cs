using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using POIApp.Classes;
using System.Collections.Generic;

namespace POIApp
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {

        private ListView poiListView;
        private ProgressBar progressBar;
        private List<PointOfInterest> poiListData;
        private POIListViewAdapter poiListAdapter;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.list_poi);
            poiListView = FindViewById<ListView>(Resource.Id.listViewPOI);
            poiListView.ItemClick += PoiListView_ItemClick;
            progressBar = FindViewById<ProgressBar>(Resource.Id.progressBar);
            DownloadPoisListAsync();
        }

        private void PoiListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            PointOfInterest poi = poiListData[(int)e.Position];
            Intent poiDetailIntent = new Intent(this, typeof(DetailActivity));
            string data = JsonConvert.SerializeObject(poi);
            poiDetailIntent.PutExtra("poi", data);
            this.StartActivity(poiDetailIntent);

        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_poilistview, menu);
            return base.OnCreateOptionsMenu(menu);
        }
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.actionNew:
                    StartActivity(typeof(DetailActivity));
                    return true;
                case Resource.Id.actionRefresh:
                    DownloadPoisListAsync();
                    return true;
                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        public async void DownloadPoisListAsync()
        {
            progressBar.Visibility = ViewStates.Visible;
            POIService service = new POIService();
            if (!service.isConnected(this))
            {
                Toast toast = Toast.MakeText(this, "Not conntected to internet.Please check your device network settings.", ToastLength.Long);
                toast.Show();
            }
            else
            {
                poiListData = await service.GetPOIListAsync();
                progressBar.Visibility = ViewStates.Gone;
                poiListAdapter = new POIListViewAdapter(this, poiListData);
                poiListView.Adapter = poiListAdapter;
            }
        }





    }
}