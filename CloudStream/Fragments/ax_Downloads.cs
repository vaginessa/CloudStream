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
using Android.Graphics;

namespace CloudStream.Fragments
{
   
    
    public class ax_Downloads : SupportFragment

    {
         public static ax_Downloads ax_downloads;
         static  string TAG						= "MX.IntentTest";
	
	 static  string RESULT_VIEW				= "com.mxtech.intent.result.VIEW";
        // static  int RESULT_ERROR = Activity.RESULT_FIRST_USER + 0;
        private static int REQUEST_CODE = 0x8001; // This can be changed as you wish.

        static string EXTRA_DECODER			= "decode_mode";	// (byte)
	static string EXTRA_VIDEO 				= "video";
	static string EXTRA_EXPLICIT_LIST		= "video_list_is_explicit";
	static string EXTRA_DURATION			= "duration";
	static string EXTRA_SUBTITLES			= "subs";
	static string EXTRA_SUBTITLE_NAMES 	= "subs.name";
	static string EXTRA_SUBTITLE_FILENAMES = "subs.filename";
	static string EXTRA_ENABLED_SUBTITLES	= "subs.enable";
	static string EXTRA_POSITION			= "position";
	static string EXTRA_RETURN_RESULT		= "return_result";
	static string EXTRA_HEADERS			= "headers";
	static string EXTRA_END_BY				= "end_by";
	static string EXTRA_VIDEO_ZOOM			= "video_zoom";
	static string EXTRA_DAR_HORZ			= "DAR_horz";						
	static string EXTRA_DAR_VERT			= "DAR_vert";
	static string EXTRA_STICKY				= "sticky";
	static string EXTRA_ORIENTATION 		= "orientation";
	static string EXTRA_SUPPRESS_ERROR_MESSAGE = "suppress_error_message";  
	static string EXTRA_SECURE_URI 		= "secure_uri";
	static string EXTRA_KEYS_DPAD_UPDOWN 	= "keys.dpad_up_down";
           
	static string EXTRA_LIST				= "video_list";
	       
	static string EXTRA_TITLE				= "title";
	static string EXTRA_TITLES				= "video_list.name";
           
	static string EXTRA_SIZE               = "size";
	static string EXTRA_SIZES              = "video_list.size";
	       
	static string EXTRA_FILENAME           = "filename";
	static string EXTRA_FILENAMES          = "video_list.filename";
	       
	static string EXTRA_HASH_OPENSUBTITLES	= "hash.opensubtitles";
	static string EXTRA_HASHES_OPENSUBTITLES = "video_list.hash.opensubtitles";

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        _w.RecyclerView _re;
        
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
           
            View view = inflater.Inflate(Resource.Layout.ax_Downloads, container, false);
            ax_downloads = this;
            Button btt = view.FindViewById<Button>(Resource.Id.newLink);
            btt.Click += (o, e) =>
            {
                Context context = view.Context;
                Intent intent = new Intent(context, typeof(LinkdownloadActivity));

                StartActivity(intent);
            };

            _w.RecyclerView re = view.FindViewById<_w.RecyclerView>(Resource.Id.recyclerView_links);
            _re = re;
            
            re.SetItemClickListener((rv, position, _view) =>
            {
                try {

                    DoLink(0, position);
                  
                }
                catch (System.Exception) {

                    print("Error getting:" + position);
                }


            });
            
            SetUpRecyclerView(_re);

            return view;
        }
        public void UpdateList()
        {
            SetUpRecyclerView(_re);
        }



        List<string> allValues = new List<string>();
  
        void SetUpRecyclerView(_w.RecyclerView recyclerView)
        {
            //var values = GetRandomSubList(Cheeses.CheeseStrings, 30);

           
            List<string> values = new List<string>();
            var localC = Application.Context.GetSharedPreferences("Downloads", FileCreationMode.Private);
            IDictionary<string,object> allData = localC.All;
            ICollection<string> allTitles = allData.Keys;
            string[] tempVal = new string[allData.Count];
            allTitles.CopyTo(tempVal,0);

            values = new List<string>();

            for (int i = 0; i < tempVal.Length; i++) {
                if(!tempVal[i].Contains("P___")) { 
                    values.Add(tempVal[i]);
                }
            }
          
            allValues = values;

            recyclerView.SetLayoutManager(new _w.LinearLayoutManager(recyclerView.Context));
            try {
                var adapter = new SimpleStringRecyclerViewAdapter(recyclerView.Context, values, Activity.Resources);
                adapter.ItemClick += MAdapter_ItemLongClick;
                recyclerView.SetAdapter(adapter);
            }
            catch (System.Exception) {

            }

            void MAdapter_ItemLongClick(object sender, int e)
            {
                HistoryPressTitle(movieTitles[moveSelectedID] + "|" + activeLinksNames[e],true);
                //UpdateList();
            }
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
                simpleHolder.mTxtView.Text = mValues[position].Replace("B___", "").Replace(" (Bookmark)", "");

                simpleHolder.mTxtView.SetTextColor(Color.White);

                simpleHolder.mImgBtt.Visibility = ViewStates.Visible;
                simpleHolder.mImgBtt.Background.SetVisible(false, true);
                simpleHolder.mImgBtt.SetBackgroundColor(new Color(0, 0, 0, 0));
                simpleHolder.mImgBtt.SetImageResource(Resource.Drawable.warrow2);

                simpleHolder.mImgBtt.Click += (o, e) => {
                    print("Click" + position + "|");
                    BtnShow_Click(o, e, position, holder.ItemView);
                };

                //simpleHolder.mImgBtt.Visibility = ViewStates.Gone;


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
            private void BtnShow_Click(object sender, EventArgs e, int pos, View v)
            {

                PopupMenu menu = new PopupMenu(ax_downloads.Context, v);
                menu.MenuInflater.Inflate(Resource.Menu.menu2, menu.Menu);

                menu.MenuItemClick += (s, arg) =>
                {
                    // Toast.MakeText(mainActivity, string.Format("Menu {0} clicked", arg.Item.TitleFormatted), ToastLength.Short).Show();
                    ax_downloads.DoLink(TitleToInt(arg.Item.TitleFormatted.ToString()), pos);
                };

                menu.DismissEvent += (s, arg) => {
                    //Toast.MakeText(mainActivity, string.Format("Menu dissmissed"), ToastLength.Short).Show();

                };

                menu.Show();
            }

        }
        static int TitleToInt(string title)
        {
            string[] checks = { "Play", "Remove Download" };
            for (int i = 0; i < checks.Length; i++) {
                if (checks[i] == title) {
                    return i;
                }
            }
            return -1;
        }

        void DoLink(int id, int pos)
        {
            if (id == 0) {
                string storage = allValues[pos];
                var localC = Application.Context.GetSharedPreferences("Downloads", FileCreationMode.Private);
                long getID = localC.GetLong(storage, -1);
                if (getID != -1) {
                    Intent intent = new Intent(Intent.ActionView);

                    DownloadManager manager = (DownloadManager)Context.GetSystemService(Context.DownloadService);
                    Android.Net.Uri uri = manager.GetUriForDownloadedFile(getID);

                    intent.SetData(uri);

                    intent.PutExtra(EXTRA_TITLE, storage);
                    intent.PutExtra(EXTRA_RETURN_RESULT, true);
                    StartActivityForResult(intent, REQUEST_CODE);
                }
                else {
                    string path = localC.GetString("P___" + storage, "-1");
                    if(path != "-1") { 
                        string truePath = "file://" + Android.OS.Environment.ExternalStorageDirectory + "/" + Android.OS.Environment.DirectoryDownloads + "/" + path;
                        print(truePath);
                        Intent intent = new Intent(Intent.ActionView);
                        Java.Net.URI url = new Java.Net.URI(truePath);
                        
                        Java.IO.File f = new Java.IO.File(url);

                        Android.Net.Uri uri = Android.Net.Uri.Parse(truePath);
                        intent.SetData(uri);
                        intent.PutExtra(EXTRA_TITLE, storage);
                        intent.PutExtra(EXTRA_RETURN_RESULT, true);
                        StartActivityForResult(intent, REQUEST_CODE);
                    }

                }


            }

            else if (id == 1) {
                RemoveDownload(allValues[pos], mainActivity,this.View);
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
                //                mTxtView.LongClick += (sender, e) => listener(base.Position);
                // mTxtView.Focusable = false;
            }
        }

    }
}