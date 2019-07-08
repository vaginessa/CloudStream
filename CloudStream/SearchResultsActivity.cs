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
    public class SearchResultsActivity : AppCompatActivity
    {
        public static int veiws = 0;

        public static SearchResultsActivity searchResultsActivity;

        FloatingActionButton fab;
        void CheckBookmark(int over = -1)
        {
            bool get = (over == 1);
            if (over == -1) {
                get = GetIfBookmarked(moveSelectedID);
            }
            fab.SetImageResource((get ? Resource.Drawable.abc_btn_rating_star_on_mtrl_alpha : Resource.Drawable.abc_btn_rating_star_off_mtrl_alpha));

        }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            if (ax_Settings.SettingsGetChecked(5)) {
                try {
                    HistoryPressTitle(movieTitles[moveSelectedID]);

                }
                catch (Exception) {

                }
            }
            searchResultsActivity = this;

            ax_Search.ax_search.UpdateList();
            print(DateTime.Now.Millisecond.ToString());
            GetAllChromeDevices();
            print(DateTime.Now.Millisecond.ToString());

            base.OnCreate(savedInstanceState);
            //Window.RequestFeature(WindowFeatures.NoTitle);

            SetContentView(Resource.Layout.Activity_SeachResults);


            SupportToolbar toolBar = FindViewById<SupportToolbar>(Resource.Id.toolBar);
            SetSupportActionBar(toolBar);
            toolBar.Visibility = ViewStates.Gone;


            TabLayout tabs = FindViewById<TabLayout>(Resource.Id.tabs);

            ViewPager viewPager = FindViewById<ViewPager>(Resource.Id.viewpager);

            SetUpViewPager(viewPager);

            tabs.SetupWithViewPager(viewPager);

            fab = FindViewById<FloatingActionButton>(Resource.Id.fab);
            //fab.Visibility = ViewStates.Gone;
            CheckBookmark();

            fab.Click += (o, e) =>
            {
                View anchor = o as View;
                bool savedIn = GetIfBookmarked(moveSelectedID);
                if (!savedIn) {
                    AddBookMark(moveSelectedID);
                }
                else {

                    if (movieTitles[moveSelectedID].Contains("B___")) {
                        Finish();
                    }
                    RemoveBookMark(moveSelectedID);
                }
                CheckBookmark((savedIn ? 0 : 1));
                Snackbar.Make(anchor, (savedIn ? "Removed From Bookmarks" : "Saved In Bookmarks"), Snackbar.LengthLong).Show();

                /*
                        .SetAction("Undo", v =>
                        {
                            if (savedIn) {
                                AddBookMark(moveSelectedID);
                            }
                            else {
                                RemoveBookMark(moveSelectedID);
                            }
                            //Do something here
                            //Intent intent = new Intent(fab.Context, typeof(SearchResultsActivity));
                            //StartActivity(intent);
                        })
                        .Show();
                        */
            };

            /*
            LinearLayout sheet = FindViewById<LinearLayout>(Resource.Id.bottom_sheet);
            BottomSheetBehavior bottomSheetBehavior = BottomSheetBehavior.From(sheet);

            bottomSheetBehavior.PeekHeight = 300;
            bottomSheetBehavior.Hideable = true;

            bottomSheetBehavior.SetBottomSheetCallback(new MyBottomSheetCallBack());

            FloatingActionButton fab = FindViewById<FloatingActionButton>(Resource.Id.fab);
            fab.Click += (o, e) =>
            {
                bottomSheetBehavior.State = BottomSheetBehavior.StateCollapsed;
            };
            */

        }
        static int seasons = 0;
        public static bool done = false;
        public void AddTabs(int _seasons)
        {
            // done = true;
            seasons = _seasons;
            // Intent intent = new Intent(fab.Context, typeof(SearchResultsActivity));
            // StartActivity(intent);

            // Finish();


        }

        private void SetUpViewPager(ViewPager viewPager)
        {
            TabAdapter adapter = new TabAdapter(SupportFragmentManager);

            veiws = 0;
            adapter.AddFragment(new ax_Info(), "Info");
            if (movieProvider[moveSelectedID] == 3) {
                if (!done) {
                    adapter.AddFragment(new ax_Links(), "Links");
                }
                else {
                    veiws = 1;

                    for (int i = 0; i < seasons; i++) {
                        adapter.AddFragment(new ax_Links(), "" + (i + 1));
                    }
                }
            }
            else {


                if (movieProvider[moveSelectedID] == 0) {
                    adapter.AddFragment(new ax_Links(), "Dub");
                    adapter.AddFragment(new ax_Links(), "Sub");
                }
                else {
                    if (movieProvider[moveSelectedID] == 4) {
                        adapter.AddFragment(new ax_Links(), "Episodes");
                        adapter.AddFragment(new ax_Links(), "Links");
                    }
                    else {
                        adapter.AddFragment(new ax_Links(), "Links");
                    }
                }
            }

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
    }
}