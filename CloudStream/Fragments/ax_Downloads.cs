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
        static string TAG = "MX.IntentTest";

        static string RESULT_VIEW = "com.mxtech.intent.result.VIEW";
        // static  int RESULT_ERROR = Activity.RESULT_FIRST_USER + 0;
        private static int REQUEST_CODE = 0x8001; // This can be changed as you wish.

        const string EXTRA_DECODER = "decode_mode";    // (byte)
        const string EXTRA_VIDEO = "video";
        const string EXTRA_EXPLICIT_LIST = "video_list_is_explicit";
        const string EXTRA_DURATION = "duration";
        const string EXTRA_SUBTITLES = "subs";
        const string EXTRA_SUBTITLE_NAMES = "subs.name";
        const string EXTRA_SUBTITLE_FILENAMES = "subs.filename";
        const string EXTRA_ENABLED_SUBTITLES = "subs.enable";
        const string EXTRA_POSITION = "position";
        const string EXTRA_RETURN_RESULT = "return_result";
        const string EXTRA_HEADERS = "headers";
        const string EXTRA_END_BY = "end_by";
        const string EXTRA_VIDEO_ZOOM = "video_zoom";
        const string EXTRA_DAR_HORZ = "DAR_horz";
        const string EXTRA_DAR_VERT = "DAR_vert";
        const string EXTRA_STICKY = "sticky";
        const string EXTRA_ORIENTATION = "orientation";
        const string EXTRA_SUPPRESS_ERROR_MESSAGE = "suppress_error_message";
        const string EXTRA_SECURE_URI = "secure_uri";
        const string EXTRA_KEYS_DPAD_UPDOWN = "keys.dpad_up_down";
        const string EXTRA_LIST = "video_list";
        const string EXTRA_TITLE = "title";
        const string EXTRA_TITLES = "video_list.name";
        const string EXTRA_SIZE = "size";
        const string EXTRA_SIZES = "video_list.size";
        const string EXTRA_FILENAME = "filename";
        const string EXTRA_FILENAMES = "video_list.filename";
        const string EXTRA_HASH_OPENSUBTITLES = "hash.opensubtitles";
        const string EXTRA_HASHES_OPENSUBTITLES = "video_list.hash.opensubtitles";

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
                // try {

                DoLink(0, position);

                // }
                // catch (System.Exception) {

                //     print("Error getting:" + position);
                // }


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
            IDictionary<string, object> allData = localC.All;
            ICollection<string> allTitles = allData.Keys;
            string[] tempVal = new string[allData.Count];
            allTitles.CopyTo(tempVal, 0);

            values = new List<string>();

            for (int i = 0; i < tempVal.Length; i++) {
                if (!tempVal[i].Contains("P___")) {
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
                HistoryPressTitle(movieTitles[moveSelectedID] + "|" + activeLinksNames[e], true);
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
                simpleHolder.mTxtView.Text = mValues[position].Replace("B___", "").Replace(" (Bookmark)", "").Replace("_", " ").Replace(".mp4", "");

                simpleHolder.mTxtView.SetTextColor(Color.White);

                simpleHolder.mImgBtt.Visibility = ViewStates.Visible;
                simpleHolder.mImgBtt.Background.SetVisible(false, true);
                simpleHolder.mImgBtt.SetBackgroundColor(new Color(0, 0, 0, 0));
                simpleHolder.mImgBtt.SetImageResource(Resource.Drawable.warrow2);

                simpleHolder.mImgBtt.Click += (o, e) =>
                {
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

                menu.DismissEvent += (s, arg) =>
                {
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
                PlayDownloadFileFromTitle(storage);
            }

            else if (id == 1) {
                RemoveDownload(allValues[pos], mainActivity, this.View);
            }
        }

        public static void PlayDownloadFileFromTitle(string storage) 
        {
            storage = storage.Replace(".mp4", "") + ".mp4";
            var localC = Application.Context.GetSharedPreferences("Downloads", FileCreationMode.Private);
            long getID = localC.GetLong(storage, -1);
            print("STORAGE: " + storage);
            int vlcRequestCode = 42;
            /*
            if (getID != -1) {
                DownloadManager manager = (DownloadManager)Context.GetSystemService(Context.DownloadService);
                Android.Net.Uri uri = manager.GetUriForDownloadedFile(getID);

                /*
                intent.SetData(uri);

                intent.PutExtra(EXTRA_TITLE, storage);
                intent.PutExtra(EXTRA_RETURN_RESULT, true);
                StartActivityForResult(intent, REQUEST_CODE);


                Intent vlcIntent = new Intent(Intent.ActionView);
                vlcIntent.SetPackage("org.videolan.vlc");

                vlcIntent.SetDataAndType(uri, "video/*");

                vlcIntent.PutExtra("title", storage);
                vlcIntent.AddFlags(ActivityFlags.GrantReadUriPermission);
                StartActivityForResult(vlcIntent, vlcRequestCode); //IF GETTING ERROR DOWNGRADE TO API BELOW 24; HINT: https://stackoverflow.com/questions/38200282/android-os-fileuriexposedexception-file-storage-emulated-0-test-txt-exposed

            }
            else { */
            string path = localC.GetString("P___" + storage, "-1");
            if (path == "-1") {
                path = "YouTube/" + storage;
            }

            string truePath = ("file://" + Android.OS.Environment.ExternalStorageDirectory + "/" + Android.OS.Environment.DirectoryDownloads + "/" + path).Replace(" ", "_").Replace(".mp4", "") + ".mp4";
                // string truePath = Android.OS.Environment.ExternalStorageDirectory + "/" + Android.OS.Environment.DirectoryDownloads + "/" + path;

                print("TRUEPATH: " + truePath);

                Android.Net.Uri uri = Android.Net.Uri.Parse(truePath);

                print("uri" + uri.Path);

                Intent vlcIntent = new Intent(Intent.ActionView);
                vlcIntent.SetPackage("org.videolan.vlc");

                vlcIntent.SetDataAndType(uri, "video/mp4");

                vlcIntent.PutExtra("title", storage.Replace("_", " "));
                vlcIntent.AddFlags(ActivityFlags.GrantReadUriPermission);

                mainActivity.StartActivityForResult(vlcIntent, vlcRequestCode); //IF GETTING ERROR DOWNGRADE TO API BELOW 24; HINT: https://stackoverflow.com/questions/38200282/android-os-fileuriexposedexception-file-storage-emulated-0-test-txt-exposed

                //  Java.Net.URI url = new Java.Net.URI(truePath);
                //   Java.IO.File f = new Java.IO.File(url);
                /*
   Intent intent = new Intent(Intent.ActionView);


   print(uri.EncodedPath);

   intent.SetData(uri);
   intent.PutExtra(EXTRA_TITLE, storage);
   intent.PutExtra(EXTRA_RETURN_RESULT, true);
   intent.PutExtra(EXTRA_VIDEO, true);

   print("Put extra");
   intent.SetType ("video/mp4");
   StartActivityForResult(intent, REQUEST_CODE);
   print("Started Activity Download file");
   */
                //vlcIntent.PutExtra("from_start", false);
                //vlcIntent.PutExtra("position", 90000l);
                //vlcIntent.PutExtra("subtitles_location", "/sdcard/Movies/Fifty-Fifty.srt");

                //StartActivity(intent);

            

            //}


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