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
using System.Threading.Tasks;
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
using Android.Views.InputMethods;
using Android.Graphics;

namespace CloudStream.Fragments
{
  
    public class ax_Search : SupportFragment

    {
        
        public static ax_Search ax_search;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        List<Button> btts = new List<Button>();
       static View __view;
        View ___view;
        public  ProgressBar pbar;
        static Java.Lang.Thread sThred;
        static bool searchDone = true;
        _w.RecyclerView _re;
        public static   SearchView searchView;
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {

            View view = inflater.Inflate(Resource.Layout.ax_Search, container, false);
           // _w.RecyclerView rez = view as _w.RecyclerView;

            __view = view;
            ___view = view;
            ax_search = this;

            var bar = __view.FindViewById<ProgressBar>(Resource.Id.progressBar1);
            //bar.Visibility = ViewStates.Gone;
            pbar = bar;

            // _w.RecyclerView rez = view as _w.RecyclerView; //inflater.Inflate(Resource.Layout.ax_Search, container, false) as _w.RecyclerView;
            SearchView search = view.FindViewById<SearchView>(Resource.Id.movie_seach);
            _w.RecyclerView re = view.FindViewById<_w.RecyclerView>(Resource.Id.recyclerView_movies);
            _re = re;

            search.SetQueryHint(new Java.Lang.String("Movie Search"));
            //search.SetQuery(new Java.Lang.String("HELLO"), true);
       
            search.QueryTextSubmit += async (o, e) =>
            {
                search.ClearFocus();

                if (searchDone) {
                    searchDone = false;
                MainActivity.searchFor = e.Query;
                // Intent intent = new Intent(search.Context, typeof(SearchResultsActivity));
                //StartActivity(intent);
                Action onCompleted = () =>
                {
                    //print("daaaaaaaaaaaaaaaa")
                    // UpdateList();
                    ChangeBar(100);
                     searchDone = true;
                    // 
                };
                
                sThred = new Java.Lang.Thread(
                  () =>
                  {
                      try {
                          Search(e.Query);
                          //Thread.Sleep(1000);
                      }
                      finally {
                          onCompleted();
                          sThred.Join();
                         //invoke(onCompleted);
                      }
                  });
                sThred.Start();

                ChangeBar(0);
                }
            };
            re.SetItemClickListener((rv, position, _view) =>
            {

                SelectMovie(rv, position, _view);


            });
            
            re.LongClickable = true;
            
            /*
            re.LongClickable = true;
            re.SetOnLongClickListener((rv, position, _view) =>
            {
                //An item has been clicked
                HistoryPressTitle(movieTitles[wlink[position]]);
                moveSelectedID = wlink[position];
                Context context = _view.Context;
                Intent intent = new Intent(context, typeof(SearchResultsActivity));
                // intent.PutExtra(CheeseDetailActivity.EXTRA_NAME, values[position]);

                context.StartActivity(intent);

            });
            */
            /*
            var buttons = view.FindViewById<LinearLayout>(Resource.Id.s_Buttons);

            LinearLayout linearLayout = buttons; //new LinearLayout(this);
            linearLayout.Orientation = (Android.Widget.Orientation.Vertical);
            
            for (int i = 0; i < 100; i++) {
                var button = new Button(this.Context);
                button.Text = "Button " + i;
                button.LayoutParameters = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);
                button.Click += (o, e) =>
                {
                    //selectedTitle = button.Text;
                    //ClosePoster(ViewStates.Visible);
                    Intent intent = new Intent(search.Context, typeof(SearchResultsActivity));
                    StartActivity(intent);
                };
                linearLayout.AddView(button);
                btts.Add(button);
            }*/


            // bar.Progress = MainActivity.bar_progress;

            //IRunnable
            // search.SetQuery("test", true);
            //  search.SetFocusable(ViewFocusability.Focusable);
            //search.SetIconifiedByDefault(false);
            search.OnActionViewExpanded();
            search.ClearFocus();
            searchView = view.FindViewById<SearchView>(Resource.Id.movie_seach);

            // imm.HideSoftInputFromInputMethod(search.WindowToken, HideSoftInputFlags.None);
            //search.RequestFocusFromTouch();
        

            return view;
        }

        Button btnShow;

        public void UpdateList()
        {
            SetUpRecyclerView(_re);
        
        }

       

        


        public void ChangeBar(int i)
        {

            // System.Diagnostics.Debug.WriteLine("CCC:" +i);

            // if (i >= 100) {
            //    System.Diagnostics.Debug.WriteLine("BBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBB:" + movieTitles.Count.ToString());
            //_re.CallOnClick();
            //}
           pbar.Progress = i;
            if (i >= 100) {
                Runnable m = new Runnable(UpdateList);
                try {
                    Activity.RunOnUiThread(m);

                }
                catch (System.Exception) {

                }
            }
            // pbar.Visibility = ((i >= 100) ? ViewStates.Gone : ViewStates.Visible);
        }
        List<int> wlink = new List<int>();
        void SetUpRecyclerView(_w.RecyclerView recyclerView)
        {
            ax_Bookmarks.ax_bookmarks.UpdateList();
            //var values = GetRandomSubList(Cheeses.CheeseStrings, 30);
            var values = new List<string>();

            wlink = new List<int>();
            for (int i = 0; i < movieTitles.Count; i++) {
                if(!movieTitles[i].Contains("B___")) { 
                    values.Add(MainActivity.movieTitles[i]);
                    wlink.Add(i);
                }
            }
            //.Replace("B___", "")
            recyclerView.SetLayoutManager(new _w.LinearLayoutManager(recyclerView.Context));
            var adapter = new SimpleStringRecyclerViewAdapter(recyclerView.Context, values, Activity.Resources);
            adapter.ItemClick += MAdapter_ItemLongClick;
            recyclerView.SetAdapter(adapter);
          // System.Diagnostics.Debug.WriteLine("QQQ:" + movieTitles.Count.ToString());

            // if(   !recyclerView.itemc) { 


            //  }
        }


        void SelectMovie(_w.RecyclerView rv, int position, View _view)
        {
          var check=  __view.FindViewById<CheckBox>(Resource.Id.checkBox1);
            if (!check.Checked) {

                //An item has been clicked
                try {
                    SearchResultsActivity.done = false;
                }
                catch (System.Exception) {

                }

                moveSelectedID = wlink[position];
                Context context = _view.Context;
                Intent intent = new Intent(context, typeof(SearchResultsActivity));
                // intent.PutExtra(CheeseDetailActivity.EXTRA_NAME, values[position]);

                context.StartActivity(intent);
            }
            else {
                HistoryPressTitle(movieTitles[position], true);
                UpdateList();
                _re.ScrollToPosition(position);

            }
        }
        private void MAdapter_ItemLongClick(object sender, int e)
        {
           // UpdateList();
            print("LongClick: " + e);
           // _re.ClearFocus();

            //Toast.MakeText(this, "This is photo number " + photoNum, ToastLength.Short).Show();
        }

        public class SimpleStringRecyclerViewAdapter : _w.RecyclerView.Adapter
        {
            private readonly TypedValue mTypedValue = new TypedValue();
            private int mBackground;
            private List<string> mValues;
            Resources mResource;
            private Dictionary<int, int> mCalculatedSizes;


            public event EventHandler<int> ItemClick;

            private void OnClick(int obj)
            {
                if (ItemClick != null)
                    ItemClick(this, obj);
            }
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

            public override async void OnBindViewHolder(_w.RecyclerView.ViewHolder holder, int position)
            {
                var simpleHolder = holder as SimpleViewHolder;

                simpleHolder.mBoundString = mValues[position];
                simpleHolder.mTxtView.Text = mValues[position];
                simpleHolder.mImgBtt.Visibility = ViewStates.Gone;

                if (HistoryGetTitlePressed(mValues[position])) {
                  //  print(mValues[position]);
                    simpleHolder.mTxtView.SetTextColor(Color.ParseColor("#b5fff8"));
                }
                else {
                    simpleHolder.mTxtView.SetTextColor(Color.White);
                }
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
                return new SimpleViewHolder(view, OnClick);
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

            public SimpleViewHolder(View itemview, Action<int> listener) : base(itemview)
            {
                mTxtView = itemview.FindViewById<TextView>(Resource.Id.text1);
                mImgBtt = itemview.FindViewById<ImageButton>(Resource.Id.imageButton1);

                // mTxtView.LongClick += (sender, e) => listener(base.Position);
                // mTxtView. += (sender, e) => listener(base.Position);

            }
        }


    }
}