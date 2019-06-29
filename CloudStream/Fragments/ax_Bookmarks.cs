using Android.OS;
using Android.Views;
using Android.Widget;
using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Support.V4.Widget;
using SupportFragmentManager = Android.Support.V4.App.FragmentManager;
using SupportToolbar = Android.Support.V7.Widget.Toolbar;
using SupportActionBar = Android.Support.V7.App.ActionBar;

using Android.Support.V7.App;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Support.V4.App;
using System.Collections.Generic;
using Java.Lang;
using CloudStream.Fragments;
using System.ComponentModel;
using System.Linq;
using System.Text;

using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization;
using System.IO;
using System.Diagnostics;
using System.Text.RegularExpressions;
using SupportFragment = Android.Support.V4.App.Fragment;
using static CloudStream.MainActivity;
using _SupportFragmentManager = Android.Support.V4.App.FragmentManager;
using _w = Android.Support.V7.Widget;
using DesignLibrary.Helpers;
using Android.Util;
using Android.Content.Res;
using Android.Views;
using System.Threading;
using Android.Support.V4.Content;
using Android.Graphics;
using Android.Support.V4.Content.Res;
using static Android.Content.Res.Resources;
using System.Threading.Tasks;

namespace CloudStream.Fragments
{
    public class ax_Bookmarks : SupportFragment

    {

        public static ax_Bookmarks ax_bookmarks;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        List<Button> btts = new List<Button>();
        static View __view;
        View ___view;
        ProgressBar pbar;
        static Java.Lang.Thread sThred;
        static bool searchDone = true;
        _w.RecyclerView _re;
        static ImageButton refreshButton;
        public override void OnResume()
        {
            UpdateList();
            base.OnResume();
        }
        public override void OnStart()
        {
            UpdateList();
            base.OnStart();
        }

        static bool refreshing = false;
        static int thredNum = 0;
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {

            View view = inflater.Inflate(Resource.Layout.ax_Bookmarks, container, false);
            // _w.RecyclerView rez = view as _w.RecyclerView;

            __view = view;
            ___view = view;
            ax_bookmarks = this;

            // _w.RecyclerView rez = view as _w.RecyclerView; //inflater.Inflate(Resource.Layout.ax_Search, container, false) as _w.RecyclerView;
            _w.RecyclerView re = view.FindViewById<_w.RecyclerView>(Resource.Id.recyclerView_movies);
            _re = re;

            refreshButton = view.FindViewById<ImageButton>(Resource.Id.reloadbtt);
            refreshBar = view.FindViewById<ProgressBar>(Resource.Id.refreshBar);
            // refreshBar.Visibility = ViewStates.Gone;

            refreshButton.Click += (o, e) =>
            {
                //  refreshBar.Visibility = ViewStates.Visible;
                // if(!refreshing) { 
                thredNum++;
                thredNumber++;
                print("RefreshBtt clicked");

                if (!refreshing) {
                    ShowSnackBar("Loading the amount of links");


                    MethodInvoker simpleDelegate;
                    simpleDelegate = new MethodInvoker(GetAllEps);
                    simpleDelegate.BeginInvoke(null, null);
                }
                else {
                    ShowSnackBar("Loading Progress Aborted");

                    refreshBar.Progress = 0;
                    refreshButton.Rotation = 0;
                    refreshRot = 0;
                    ax_Bookmarks.ax_bookmarks.UpdateList();
                }

                refreshing = !refreshing;

            };


            re.SetItemClickListener((rv, position, _view) =>
            {
                //An item has been clicked
                print(wlink.Count.ToString());
                print(position.ToString());

                moveSelectedID = wlink[position];
                Context context = _view.Context;
                Intent intent = new Intent(context, typeof(SearchResultsActivity));
                // intent.PutExtra(CheeseDetailActivity.EXTRA_NAME, values[position]);

                context.StartActivity(intent);

            });
            GetBookMarks();
            SetUpRecyclerView(_re);




            /*
            refresher = view.FindViewById<SwipeRefreshLayout>(Resource.Id.refresher);

            refresher.Refresh += (o,e) =>
            {
                print("DAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAaLLLLLLLLLLLLLLLLLLLLLLLL");
                MethodInvoker simpleDelegate;
                    simpleDelegate = new MethodInvoker(GetAllEps);
                    simpleDelegate.BeginInvoke(null, null);
               


            };
            */


            return view;
        }
        // static SwipeRefreshLayout refresher;
        static ProgressBar refreshBar;
        void _UpdateList()
        {

            SetUpRecyclerView(_re);
            //refresher.Refreshing = false;

        }

        static float refreshRot = 0;

        static void RefreshLoading()
        {
            print("RefreshLoading");
            //refreshButton.SetColorFilter(Color.ParseColor("#b5fff8"));
            string tempThred = thredNum.ToString();
            int cThred = int.Parse(tempThred);
            // refreshButton.SetBackgroundColor(Color.BlueViolet);
            while (refreshing) {
                Java.Lang.Thread.Sleep(10);
                refreshRot += 1f;
                refreshButton.Rotation = refreshRot;
                if (cThred != thredNum) {
                    return;
                }
            }

            // refreshButton.SetBackgroundColor(Color.White);
            refreshButton.Rotation = 0;
            refreshRot = 0;
            //  refreshButton.SetColorFilter(Color.ParseColor("#FFFFFF"));


        }

        static List<string> addWatched = new List<string>();

        static int totalMovies = 0;
        static int currentMovieId = 0;

        public static void AddTitleEps(int _addToTitleEps, int movie, int cThred)
        {


            if (cThred != thredNum) {
                return;
            }
            currentMovieId++;
            refreshBar.Progress = (int)System.Math.Round((currentMovieId) * 100f / (totalMovies));
            string newEps = "";
            if (movieProvider[movie] == 4) {
                newEps = " [" + _addToTitleEps + "]";

            }
            else {

                newEps = " [" + HighestHistory(movieTitles[movie].Replace("B___", "").Replace(" (Bookmark)", "")) + "/" + _addToTitleEps + "]";
            }

            if (_addToTitleEps == -1) {
                newEps = "";
            }

            addWatched.Add(movieTitles[movie] + ";" + newEps);
            if (currentMovieId >= totalMovies) {
                print("Done adding!");
                ax_Bookmarks.ax_bookmarks.UpdateList();
                refreshing = false;
            }
        }

        static void GetAllEps()
        {
            MethodInvoker simpleDelegate2 = new MethodInvoker(RefreshLoading);
            simpleDelegate2.BeginInvoke(null, null);

            string tempThred = thredNum.ToString();
            int cThred = int.Parse(tempThred);
            addWatched = new List<string>();
            List<int> watch = new List<int>();
            List<int> watch_multi = new List<int>();

            for (int i = 0; i < movieTitles.Count; i++) {
                if (movieTitles[i].Contains("B___")) {
                    if (movieProvider[i] != 3) { // 3 cant be multithreded
                        watch_multi.Add(i);
                        print("Multi add: " + movieTitles[i]);
                    }
                    else {
                        watch.Add(i);
                        print("Watch add: " + movieTitles[i]);

                    }
                }
            }

            currentMovieId = 0;
            totalMovies = watch.Count + watch_multi.Count;

            for (int q = 0; q < watch.Count; q++) {
                int i = watch[q];
                thredNumber++;
                refreshBar.Progress = (int)System.Math.Round((q + 0.5f) * 100f / (totalMovies));
                if (cThred != thredNum) {
                    return;
                }
                GetURLFromTitle(i, true, thredNum);
                if (cThred != thredNum) {
                    return;
                }
                //AddTitleEps(addToTitleEps, i);

            }
            for (int i = 0; i < watch_multi.Count; i++) {
                DelegateWithParameters_int linkServer =
                               new DelegateWithParameters_int(MultiWatchGetEps);

                linkServer.BeginInvoke(watch_multi[i], null, null);


            }
            if (cThred != thredNum) {
                return;
            }


        }

        public static void MultiWatchGetEps(int movieId)
        {
            // if(movieProvider[movieId] != 3) {
            //  print(">>>>>>>>>>>>>>>>>>>>>" + movieTitles[movieId]);
            GetURLFromTitle(movieId, true, thredNum);
            //}
        }



        public void UpdateList()
        {
            Runnable m = new Runnable(_UpdateList);
            Activity.RunOnUiThread(m);
        }


        List<int> wlink = new List<int>();
        void SetUpRecyclerView(_w.RecyclerView recyclerView)
        {
            //  refreshBar.Visibility = ViewStates.Gone;

            /*
            List<string> _movieTitles = new List<string>();
            List<string> _fwordLink = new List<string>();
            List<Movie> _moviesActive = new List<Movie>();
            List<bool> _movieIsAnime = new List<bool>();
            List<int> _movieProvider = new List<int>();
            for (int i = 0; i < movieTitles.Count; i++) {
                if (!movieTitles[i].Contains("B___")) {
                    _movieTitles.Add(movieTitles[i]);
                    _moviesActive.Add(moviesActive[i]);
                    _movieIsAnime.Add(movieIsAnime[i]);
                    _movieProvider.Add(movieProvider[i]);
                    _fwordLink.Add(fwordLink[i]);
                }
            }
            movieTitles = _movieTitles;
            fwordLink = _fwordLink;
            moviesActive = _moviesActive;
            movieIsAnime = _movieIsAnime;
            movieProvider = _movieProvider;
            */
            GetBookMarks();


            //var values = GetRandomSubList(Cheeses.CheeseStrings, 30);
            wlink = new List<int>();

            List<string> values = new List<string>();
            for (int i = 0; i < movieTitles.Count; i++) {
                if (movieTitles[i].Contains("B___")) {
                    print("--" + movieTitles[i]);
                    string addTo = "";
                    for (int q = 0; q < addWatched.Count; q++) {
                        if (addWatched[q].StartsWith(movieTitles[i] + ";")) {
                            addTo = addWatched[q].Substring(addWatched[q].IndexOf(";") + 1, addWatched[q].Length - addWatched[q].IndexOf(";") - 1);
                        }
                    }
                    values.Add(movieTitles[i].Replace("B___", "").Replace(" (Bookmark)", "") + addTo);
                    wlink.Add(i);
                }
            }


            //.Replace("B___", "")
            recyclerView.SetLayoutManager(new _w.LinearLayoutManager(recyclerView.Context));

            recyclerView.SetAdapter(new SimpleStringRecyclerViewAdapter(recyclerView.Context, values, Activity.Resources));
            // System.Diagnostics.Debug.WriteLine("QQQ:" + movieTitles.Count.ToString());

            // if(   !recyclerView.itemc) { 


            //  }
        }


        public class SimpleStringRecyclerViewAdapter : _w.RecyclerView.Adapter
        {
            private readonly TypedValue mTypedValue = new TypedValue();
            private int mBackground;
            private List<string> mValues;
            Resources mResource;
            private Dictionary<int, int> mCalculatedSizes;

            public SimpleStringRecyclerViewAdapter(Context context, List<string> items, Resources res)
            {
                context.Theme.ResolveAttribute(Resource.Attribute.selectableItemBackground, mTypedValue, true);
                mBackground = mTypedValue.ResourceId;
                mValues = items;
                mResource = res;

                mCalculatedSizes = new Dictionary<int, int>();
            }

            public override int ItemCount
            {
                get {
                    return mValues.Count;
                }
            }
            public static Color ConvertAndroidColorToReal(int c)
            {
                return new Color(ContextCompat.GetColor(__view.Context, c));
            }
            public override async void OnBindViewHolder(_w.RecyclerView.ViewHolder holder, int position)
            {
                var simpleHolder = holder as SimpleViewHolder;

                simpleHolder.mBoundString = mValues[position];
                simpleHolder.mTxtView.Text = mValues[position];
                simpleHolder.mImgBtt.Visibility = ViewStates.Gone;

                // if(position == 2 || position == 5 || position == 4) { 
                //simpleHolder.mTxtView.SetTextColor(Color.ParseColor("#b5fff8"));
                //}
                //ConvertAndroidColorToReal(Resource.Color.design_textinput_error_color_light)); 
                //int drawableID = Cheeses.RandomCheeseDrawable;
                // BitmapFactory.Options options = new BitmapFactory.Options();
                /*
                 if (mCalculatedSizes.ContainsKey(drawableID)) {
                     options.InSampleSize = mCalculatedSizes[drawableID];
                 }

                 else {
                     options.InJustDecodeBounds = true;

                     BitmapFactory.DecodeResource(mResource, drawableID, options);

                     options.InSampleSize = Cheeses.CalculateInSampleSize(options, 100, 100);
                     options.InJustDecodeBounds = false;

                     mCalculatedSizes.Add(drawableID, options.InSampleSize);
                 }


                 var bitMap = await BitmapFactory.DecodeResourceAsync(mResource, drawableID, options);

                 simpleHolder.mImageView.SetImageBitmap(bitMap);*/
            }

            public override _w.RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
            {
                View view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.List_Item, parent, false);
                view.SetBackgroundResource(mBackground);

                return new SimpleViewHolder(view);
            }
        }

        public class SimpleViewHolder : _w.RecyclerView.ViewHolder
        {
            public string mBoundString;
            public readonly View mView;
            public readonly TextView mTxtView;
            public readonly ImageButton mImgBtt;

            public SimpleViewHolder(View view) : base(view)
            {
                mView = view;
                mTxtView = view.FindViewById<TextView>(Resource.Id.text1);
                mImgBtt = view.FindViewById<ImageButton>(Resource.Id.imageButton1);

            }

            public override string ToString()
            {
                return base.ToString() + " '" + mTxtView.Text;
            }
        }


    }
}