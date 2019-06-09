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
   
    
    public class ax_Links : SupportFragment

    {
         public static ax_Links ax_links;
        public static ax_Links ax_links_sub;
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

        List<Button> btts = new List<Button>();
        static View __view;
        //View ___view;
        ProgressBar pbar;
        static Java.Lang.Thread sThred;
        static bool linksDone = false;
        _w.RecyclerView _re;
        public bool currentMain = false;
        SearchView searchView;
        public static List<ax_Links> ax_Links_all = new List<ax_Links>();
        int currnView = 0;
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
           
            View view = inflater.Inflate(Resource.Layout.ax_Links, container, false);
            
            // _w.RecyclerView rez = view as _w.RecyclerView;
          //  print(view.AccessibilityClassName);
           
           // System.Diagnostics.Debug.WriteLine("TTTTTTTTTTTTTTT" + view.);
            var bar = view.FindViewById<ProgressBar>(Resource.Id.progressBarLinks);
            //bar.Visibility = ViewStates.Gone;
            pbar = bar;
            if (SearchResultsActivity.veiws == 0) {
               // pbar.Visibility = ViewStates.Gone;
                currentMain = true;
                ax_links = this;

            }
            else {
                ax_links_sub = this;
            }
            if(SearchResultsActivity.veiws == 1) {
                ax_Links_all = new List<ax_Links>();
            }
            ax_Links_all.Add(this);
            currnView = SearchResultsActivity.veiws;
            SearchResultsActivity.veiws++;

            // _w.RecyclerView rez = view as _w.RecyclerView; //inflater.Inflate(Resource.Layout.ax_Search, container, false) as _w.RecyclerView;
            _w.RecyclerView re = view.FindViewById<_w.RecyclerView>(Resource.Id.recyclerView_links);
            _re = re;
            __view = view;
            re.SetItemClickListener((rv, position, _view) =>
            {
                var check =view.FindViewById<CheckBox>(Resource.Id.checkBox1);
                if (!check.Checked) {
                    ChangeBar(pbar.Progress);
                    if (ax_Settings.SettingsGetChecked(4)) {

                        HistoryPressTitle(movieTitles[moveSelectedID] + "|" + activeLinksNames[flink[position]]);
                    }
                    bool downloadFile = false;
                    string link = activeLinks[flink[position]];
                    if (!downloadFile) {
                        DoLink(0, position);
                    }
                    else {
                        // HistoryPressTitle("D___" + movieTitles[moveSelectedID] + "|" + activeLinksNames[flink[position]]);
                        DoLink(2, position);

                    }
                }
                else {
                    DoLink(1, position);

                 
                    
                }


            });
            
            Action onCompleted = () =>
            {
                //print("daaaaaaaaaaaaaaaa")
                // UpdateList();
                // ChangeBar(100);
                if (moveSelectedID >= movieTitles.Count || moveSelectedID < 0) {
                }
                else { 
                ChangeBar(100);
                if(ax_links_sub != null) { 
                    if(movieProvider[moveSelectedID] == 0) { 
                        ax_links_sub.ChangeBar(100);
                    }
                }
                print("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
                System.Diagnostics.Debug.WriteLine("BBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBB");
                print(movieProvider[moveSelectedID].ToString());
                 linksDone = true;
                }
                // 
            };


            if(currentMain) {  // not request twice
                thredNumber++;
                 sThred = new Java.Lang.Thread(
                 () =>
                 {
                     try {
                         ChangeBar(0);

                         GetURLFromTitle(moveSelectedID);
                         //Thread.Sleep(1000);
                     }
                     finally {
                         onCompleted();
                         sThred.Join();
                          //invoke(onCompleted);
                      }
                 });
            sThred.Start();
            }

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
            //Activity.OnBackPressed();
            return view;
        }

 

        public void UpdateList()
        {
            SetUpRecyclerView(_re);

        }


        public void ChangeBar(int i,bool update = true)
        {

            // System.Diagnostics.Debug.WriteLine("CCC:" +i);

            // if (i >= 100) {
            //    System.Diagnostics.Debug.WriteLine("BBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBB:" + movieTitles.Count.ToString());
            //_re.CallOnClick();
            //}
            pbar.Progress = i;
            //if (i >= 100) {
            if(update) { 
                Runnable m = new Runnable(UpdateList);
                try {
                    Activity.RunOnUiThread(m);
                }
                catch (System.Exception) {
                }
            }
            // }

            // pbar.Visibility = ((i >= 100) ? ViewStates.Gone : ViewStates.Visible);
        }
        List<int> flink = new List<int>();
        void SetUpRecyclerView(_w.RecyclerView recyclerView)
        {
            //var values = GetRandomSubList(Cheeses.CheeseStrings, 30);
            var values = new List<string>();
            flink = new List<int>();
            for (int i = 0; i < activeLinksNames.Count; i++) {
                try {

                
                if(movieProvider[moveSelectedID] == 0) { 
                    if( (activeLinksNames[i].Contains("(Dub)") && currentMain) || (activeLinksNames[i].Contains("(Sub)") && !currentMain)) {
                        values.Add(activeLinksNames[i]);
                        flink.Add(i);
                    }
                    
                }
                else if(movieProvider[moveSelectedID] == 3 && __selSeason != 0) {
                        if (activeLinksNames[i].Contains("Season " + __selSeason + " ")) {
                           values.Add(activeLinksNames[i]);
                           flink.Add(i);
                       }
                        
                    }
                else {
                    values.Add(activeLinksNames[i]);
                    flink.Add(i);
                }
                }
                catch (System.Exception) {
                }
            }
            


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
                HistoryPressTitle(movieTitles[moveSelectedID] + "|" + activeLinksNames[flink[e]],true);
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
            private int hidingItemIndex;

     

            public override async void OnBindViewHolder(_w.RecyclerView.ViewHolder holder, int position)
            {
                var simpleHolder = holder as SimpleViewHolder;

                simpleHolder.mBoundString = mValues[position];
                simpleHolder.mTxtView.Text = mValues[position];
               
               // simpleHolder.mImgBtt.SetImageResource(());
                simpleHolder.mImgBtt.Visibility = ViewStates.Visible;
                simpleHolder.mImgBtt.Background.SetVisible(false,true);
                simpleHolder.mImgBtt.SetBackgroundColor(new Color(0, 0, 0, 0));
                simpleHolder.mImgBtt.SetImageResource(Resource.Drawable.warrow2);

                if(!  simpleHolder.mImgBtt.HasOnClickListeners) { 
                simpleHolder.mImgBtt.Click += (o,e) => {
                    print("Click" + position + "|");
                    print(simpleHolder.mImgBtt.RootView.ToString());
                    BtnShow_Click(o, e, position, holder.ItemView);
                    
                    };
                }

                if (DownloadsGetIfDownloaded(movieTitles[moveSelectedID] + " | " + mValues[position])) {
                    simpleHolder.mTxtView.SetTextColor(Color.ParseColor("#fffcb4"));
                }
                else {
                    if (HistoryGetTitlePressed(movieTitles[moveSelectedID] + "|" + mValues[position])) {
                        simpleHolder.mTxtView.SetTextColor(Color.ParseColor("#b5fff8"));
                    }
                    else {
                        simpleHolder.mTxtView.SetTextColor(Color.White);
                    }
                }
            }
            public override _w.RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
            {
                View view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.List_Item, parent, false);
                view.SetBackgroundResource(mBackground);
                return new SimpleViewHolder(view, OnClick);
            }

            private void BtnShow_Click(object sender, EventArgs e, int pos, View v)
            {
               
                PopupMenu menu = new PopupMenu(ax_links.Context, v);
                menu.MenuInflater.Inflate(Resource.Menu.menu1, menu.Menu);

                menu.MenuItemClick += (s, arg) =>
                {
                  // Toast.MakeText(mainActivity, string.Format("Menu {0} clicked", arg.Item.TitleFormatted), ToastLength.Short).Show();
                   ax_links.DoLink(TitleToInt(arg.Item.TitleFormatted.ToString()), pos);
                };

                menu.DismissEvent += (s, arg) => {
                    //Toast.MakeText(mainActivity, string.Format("Menu dissmissed"), ToastLength.Short).Show();

                };

                menu.Show();
            }

        }

        static int TitleToInt(string title)
        {
            string[] checks = { "Play", "Toggle Viewstate", "Download", "Remove Download", "Play Downloaded File", "Copy Link" };
            for (int i = 0; i < checks.Length; i++) {
                if(checks[i] == title) {
                    return i;
                }
            }
            return -1;
        }

        void DoLink(int id,int pos)
        {
            print("AAA" + id + "|||" + pos);
            if (id == 0) {
                string link = activeLinks[flink[pos]];

                Intent intent = new Intent(Intent.ActionView);

                Android.Net.Uri uri = Android.Net.Uri.Parse(link);
                intent.SetData(uri);
                intent.PutExtra(EXTRA_TITLE, movieTitles[moveSelectedID].Replace("B___", "").Replace(" (Bookmark)", "") + " | " + activeLinksNames[flink[pos]]);
                intent.PutExtra(EXTRA_RETURN_RESULT, true);
                StartActivityForResult(intent, REQUEST_CODE);
            }
            else if (id == 1) {
                HistoryPressTitle(movieTitles[moveSelectedID].Replace("B___","").Replace(" (Bookmark)","") + "|" + activeLinksNames[flink[pos]], true);
                UpdateList();
                _re.ScrollToPosition(pos);
            }
            else if (id == 2) {
                string link = activeLinks[flink[pos]];
                UpdateList();
                _re.ScrollToPosition(pos);
                StartNewDownload(link, movieTitles[moveSelectedID].Replace("B___", "").Replace(" (Bookmark)", "") + " | " + activeLinksNames[flink[pos]], (movieIsAnime[moveSelectedID] ? "Anime" : "Movie"));

            }
            else if(id == 3) {
                UpdateList();
                _re.ScrollToPosition(pos);

                RemoveDownload(movieTitles[moveSelectedID].Replace("B___", "").Replace(" (Bookmark)", "") + " | " + activeLinksNames[flink[pos]], mainActivity,this.View);
            }
            else if(id == 4) {
                string storage = movieTitles[moveSelectedID].Replace("B___", "").Replace(" (Bookmark)", "") + " | " + activeLinksNames[flink[pos]];
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
            }
            else if(id == 5) {
                ClipboardManager clip = (ClipboardManager)Context.GetSystemService(Context.ClipboardService);
                clip.PrimaryClip = ClipData.NewPlainText("Link", activeLinks[flink[pos]]);
                ShowSnackBar("Copied Link To Clipboard!", ax_links.View);
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