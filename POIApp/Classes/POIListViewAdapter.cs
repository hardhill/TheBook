using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using FFImageLoading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POIApp.Classes
{
    public class POIListViewAdapter:BaseAdapter<PointOfInterest>
    {
        private readonly Activity context;
        private List<PointOfInterest> poiListData;
        public POIListViewAdapter(Activity _context, List<PointOfInterest>
        _poiListData)
         : base()
        {
            this.context = _context;
            this.poiListData = _poiListData;
        }

        public override PointOfInterest this[int position] => poiListData[position];

        public override int Count => poiListData.Count;

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView;
            var config = new FFImageLoading.Config.Configuration()
            {
                ExecuteCallbacksOnUIThread = true
            };
            ImageService.Instance.Initialize(config);

            if (view == null)
            {
                view = context.LayoutInflater.Inflate(Resource.Layout.item_list_poi,null);

            }
            PointOfInterest poi = this[position];
            view.FindViewById<TextView>(Resource.Id.txtName).Text = poi.Name;
            if (String.IsNullOrEmpty(poi.Address))
            {
                view.FindViewById<TextView>(Resource.Id.txtAdress).Visibility = ViewStates.Gone;
            }
            else
            {
                view.FindViewById<TextView>(Resource.Id.txtAdress).Text = poi.Address;
            }
            if (String.IsNullOrEmpty(poi.Image))
            {
                // ну пустой адрес
            }
            else
            {
                ImageService.Instance.LoadUrl(poi.Image)
                .Finish(workScheduled =>
                {
                    
                })
                .Into(view.FindViewById<ImageView>(Resource.Id.imgPOI));
            }
            return view;
        }
    }
}