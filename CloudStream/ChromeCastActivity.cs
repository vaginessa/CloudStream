using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.V7.App;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using static CloudStream.MainActivity;
using CloudStream.Fragments;
using SupportToolbar = Android.Support.V7.Widget.Toolbar;
using Android.Content.PM;

//using static DesignLibrary..MainActivity;

namespace CloudStream
{
    [Activity(Theme = "@style/Theme.Design.NoActionBar", ConfigurationChanges = ConfigChanges.KeyboardHidden | ConfigChanges.LayoutDirection | ConfigChanges.Orientation | ConfigChanges.ScreenSize)]
    public class ChromeCastActivity : AppCompatActivity
    {
        public static ChromeCastActivity chromeCastActivity;

        FloatingActionButton fab;
        static int offset = 0;
        static ImageView poster;
        protected override void OnCreate(Bundle savedInstanceState)
        {


            base.OnCreate(savedInstanceState);
            //Window.RequestFeature(WindowFeatures.NoTitle);

            SetContentView(Resource.Layout.Activity_ChromeCast);
            poster = FindViewById<ImageView>(Resource.Id.posterID);
            ImageButton pause = FindViewById<ImageButton>(Resource.Id.pauseButton);
            poster.SetImageResource(Resource.Drawable.ic_media_route_connected_dark_00_mtrl + 0);
            //print(poster.);

            // SupportToolbar toolBar = FindViewById<SupportToolbar>(Resource.Id.toolBar);
            //  SetSupportActionBar(toolBar);
            //  toolBar.Visibility = ViewStates.Gone;


            // TabLayout tabs = FindViewById<TabLayout>(Resource.Id.tabs);

            // ViewPager viewPager = FindViewById<ViewPager>(Resource.Id.viewpager);

            //SetUpViewPager(viewPager);

            //tabs.SetupWithViewPager(viewPager);

            //    fab = FindViewById<FloatingActionButton>(Resource.Id.fab);
            //   fab.Visibility = ViewStates.Gone;
            //fab.Visibility = ViewStates.Gone;

        }

        private void Pause_Click(object sender, EventArgs e)
        {

        }

        /*
        private void SetUpViewPager(ViewPager viewPager)
        {
            TabAdapter adapter = new TabAdapter(SupportFragmentManager);

           // adapter.AddFragment(new ax_ChromeCastInfo(), "Chromecast");


            // adapter.Fragments[1].set = "currentMain";

            viewPager.Adapter = adapter;
        }
        public class MyBottomSheetCallBack : BottomSheetBehavior.BottomSheetCallback
        {
            public override void OnSlide(View bottomSheet, float slideOffset)
            {
                //Sliding
            }

            public override void OnStateChanged(View bottomSheet, int newState)
            {
                //State changed
            }
        }
        */
    }
}