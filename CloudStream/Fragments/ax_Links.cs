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
        static string TAG = "MX.IntentTest";

        static string RESULT_VIEW = "com.mxtech.intent.result.VIEW";
        // static  int RESULT_ERROR = Activity.RESULT_FIRST_USER + 0;
        private static int REQUEST_CODE = 0x8001; // This can be changed as you wish.

        static string EXTRA_DECODER = "decode_mode";    // (byte)
        static string EXTRA_VIDEO = "video";
        static string EXTRA_EXPLICIT_LIST = "video_list_is_explicit";
        static string EXTRA_DURATION = "duration";
        static string EXTRA_SUBTITLES = "subs";
        static string EXTRA_SUBTITLE_NAMES = "subs.name";
        static string EXTRA_SUBTITLE_FILENAMES = "subs.filename";
        static string EXTRA_ENABLED_SUBTITLES = "subs.enable";
        static string EXTRA_POSITION = "position";
        static string EXTRA_RETURN_RESULT = "return_result";
        static string EXTRA_HEADERS = "headers";
        static string EXTRA_END_BY = "end_by";
        static string EXTRA_VIDEO_ZOOM = "video_zoom";
        static string EXTRA_DAR_HORZ = "DAR_horz";
        static string EXTRA_DAR_VERT = "DAR_vert";
        static string EXTRA_STICKY = "sticky";
        static string EXTRA_ORIENTATION = "orientation";
        static string EXTRA_SUPPRESS_ERROR_MESSAGE = "suppress_error_message";
        static string EXTRA_SECURE_URI = "secure_uri";
        static string EXTRA_KEYS_DPAD_UPDOWN = "keys.dpad_up_down";

        static string EXTRA_LIST = "video_list";

        static string EXTRA_TITLE = "title";
        static string EXTRA_TITLES = "video_list.name";

        static string EXTRA_SIZE = "size";
        static string EXTRA_SIZES = "video_list.size";

        static string EXTRA_FILENAME = "filename";
        static string EXTRA_FILENAMES = "video_list.filename";

        static string EXTRA_HASH_OPENSUBTITLES = "hash.opensubtitles";
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
            if (SearchResultsActivity.veiws == 1) {
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
                var check = view.FindViewById<CheckBox>(Resource.Id.checkBox1);

                string linkName = activeLinksNames[flink[position]];

                bool overideSettings = (movieProvider[moveSelectedID] == 4 && linkName.StartsWith("Episode") && currentMain);

                int rAct = 0;

                if (!check.Checked) {
                    //Toast t = new Toast(Context);
                    print("Loading: " + movieTitles[moveSelectedID] + " | " + activeLinksNames[flink[position]]);

                    rAct = 0;


                    if (!overideSettings) {
                        rAct = ax_Settings.SettingsGetDef(0, true);
                    }

                    if (ax_Settings.SettingsGetChecked(4) && (rAct == 0 || rAct == 5)) {
                        //  DoLink(ax_Settings.SettingsGetDef(1), position);
                        DoLink(9, position);
                    }
                    ChangeBar(pbar.Progress);


                    //  bool downloadFile = false;
                    //  string link = activeLinks[flink[position]];
                    //  if (!downloadFile) {
                    // }
                    // else {
                    // HistoryPressTitle("D___" + movieTitles[moveSelectedID] + "|" + activeLinksNames[flink[position]]);
                    //      DoLink(2, position);

                    // }
                }
                else {
                    //  DoLink(1, position);

                    rAct = 1;
                    if (!overideSettings) {

                        rAct = ax_Settings.SettingsGetDef(1, true);
                    }

                }

                DoLink(rAct, position);


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
                    if (ax_links_sub != null) {
                        if (movieProvider[moveSelectedID] == 0) {
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


            if (currentMain) {  // not request twice
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


        public void ChangeBar(int i, bool update = true)
        {
            // System.Diagnostics.Debug.WriteLine("CCC:" +i);

            // if (i >= 100) {
            //    System.Diagnostics.Debug.WriteLine("BBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBB:" + movieTitles.Count.ToString());
            //_re.CallOnClick();
            //}
            pbar.Progress = i;
            //if (i >= 100) {
            if (update) {
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


                    if (movieProvider[moveSelectedID] == 0) {
                        if ((activeLinksNames[i].Contains("(Dub)") && currentMain) || (activeLinksNames[i].Contains("(Sub)") && !currentMain)) {
                            values.Add(activeLinksNames[i]);
                            flink.Add(i);
                        }

                    }
                    else if (movieProvider[moveSelectedID] == 3 && __selSeason != 0) {
                        if (activeLinksNames[i].Contains("Season " + __selSeason + " ")) {
                            values.Add(activeLinksNames[i]);
                            flink.Add(i);
                        }
                    }
                    else if (movieProvider[moveSelectedID] == 4) {
                        if ((currentMain && activeLinksNames[i].StartsWith("Episode")) || (!currentMain && !activeLinksNames[i].StartsWith("Episode"))) {
                            values.Add(activeLinksNames[i]);
                            flink.Add(i);
                            print(activeLinksNames[i] + " |||| >> " + i);
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
                var adapter = new SimpleStringRecyclerViewAdapter(recyclerView.Context, values, Activity.Resources, this);
                adapter.ItemClick += MAdapter_ItemLongClick;
                recyclerView.SetAdapter(adapter);

            }
            catch (System.Exception) {

            }
            void MAdapter_ItemLongClick(object sender, int e)
            {


                // HistoryPressTitle(movieTitles[moveSelectedID] + "|" + activeLinksNames[flink[e]], true);
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
            public ax_Links tt;

            public event EventHandler<int> ItemClick;

            private void OnClick(int obj)
            {
                if (ItemClick != null)
                    ItemClick(this, obj);
            }
            public SimpleStringRecyclerViewAdapter(Context context, List<string> items, Resources res, ax_Links _tt)
            {
                context.Theme.ResolveAttribute(Resource.Attribute.selectableItemBackground, mTypedValue, true);
                mBackground = mTypedValue.ResourceId;
                mValues = items;
                mResource = res;
                tt = _tt;
                mCalculatedSizes = new Dictionary<int, int>();
            }


            public override int ItemCount
            {
                get {
                    return mValues.Count;
                }
            }
            private int hidingItemIndex;


            int StringToPos(string inp)
            {
                for (int i = 0; i < mValues.Count; i++) {
                    if (mValues[i] == inp) {
                        return i;
                    }
                }

                return -1;

            }

            public override async void OnBindViewHolder(_w.RecyclerView.ViewHolder holder, int position)
            {
                var simpleHolder = holder as SimpleViewHolder;

                simpleHolder.mBoundString = mValues[position];
                simpleHolder.mTxtView.Text = mValues[position];

                // simpleHolder.mImgBtt.SetImageResource(());
                simpleHolder.mImgBtt.Background.SetVisible(false, true);
                simpleHolder.mImgBtt.SetBackgroundColor(new Color(0, 0, 0, 0));
                simpleHolder.mImgBtt.SetImageResource(Resource.Drawable.warrow2);

                /*
                if(movieProvider[moveSelectedID] == 4 && mValues[position].StartsWith("Episode")) {
                    simpleHolder.mImgBtt.Visibility = ViewStates.Gone;
                }
                else {
                    simpleHolder.mImgBtt.Visibility = ViewStates.Visible;
                }
                */

                simpleHolder.mImgBtt.Visibility = ViewStates.Visible;
                if (!simpleHolder.mImgBtt.HasOnClickListeners) {
                    simpleHolder.mImgBtt.Click += (o, e) =>
                    {

                        print("Click" + simpleHolder.Position + "|");
                        print(simpleHolder.mImgBtt.RootView.ToString());
                        BtnShow_Click(o, e, simpleHolder.Position, holder.ItemView);

                    };
                }

                if (DownloadsGetIfDownloaded(movieTitles[moveSelectedID] + " | " + mValues[position])) {
                    simpleHolder.mTxtView.SetTextColor(Color.ParseColor("#fffcb4"));
                }
                else {
                    if (!tt.currentMain && movieProvider[moveSelectedID] == 4) {
                        if (HistoryGetTitlePressed(movieTitles[moveSelectedID].Replace("B___", "").Replace(" (Bookmark)", "") + "|" + currentEpisodeName + "|" + mValues[position])) {
                            simpleHolder.mTxtView.SetTextColor(Color.ParseColor("#b5fff8"));
                        }
                        else {
                            simpleHolder.mTxtView.SetTextColor(Color.White);
                        }
                    }
                    else {
                        if (HistoryGetTitlePressed(movieTitles[moveSelectedID].Replace("B___", "").Replace(" (Bookmark)", "") + "|" + mValues[position])) {
                            simpleHolder.mTxtView.SetTextColor(Color.ParseColor("#b5fff8"));
                        }
                        else {
                            simpleHolder.mTxtView.SetTextColor(Color.White);
                        }
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

                if (tt.currentMain && movieProvider[moveSelectedID] == 4) {
                    menu.Menu.Add(checks[8]);
                    menu.Menu.Add(checks[1]);
                    menu.Menu.Add(checks[7]);
                }
                else {
                    for (int i = 0; i < checks.Length; i++) {
                        bool add = true;
                        if ((checks[i] == "Remove Download" || checks[i] == "Play Downloaded File") && !DownloadsGetIfDownloaded(movieTitles[moveSelectedID] + " | " + mValues[pos])) { add = false; }
                        if (checks[i] == "Chromecast" && !IsConnectedToChromecast()) { add = false; }
                        if (checks[i] == "Copy Browser Link (ADS)") { add = false; }
                        if (checks[i] == "Mark As Watched") { add = false; }
                        if (checks[i] == "Load Links") { add = false; }
                        if (checks[i] == "Copy Subtitle Link") { add = currentActiveSubtitle >= 0; }
                        if (checks[i] == "Play With Subtitles") { add = currentActiveSubtitle >= 0; }

                        if (add) {
                            //  menu.Menu.Add(i,pos,i, checks[i]);
                            menu.Menu.Add(checks[i]);
                        }
                    }
                }

                menu.MenuItemClick += (s, arg) =>
                {
                    print("ITEMID:" + arg.Item.ItemId.ToString());
                    // Toast.MakeText(mainActivity, string.Format("Menu {0} clicked", arg.Item.TitleFormatted), ToastLength.Short).Show();

                    tt.DoLink(TitleToInt(arg.Item.TitleFormatted.ToString()), pos, v);
                };

                menu.DismissEvent += (s, arg) =>
                {
                    //Toast.MakeText(mainActivity, string.Format("Menu dissmissed"), ToastLength.Short).Show();

                };

                menu.Show();
                GetAllChromeDevices();
            }

        }
        /// <summary>
        /// { "Play", "Toggle Viewstate", "Download", "Remove Download", "Play Downloaded File", "Copy Link", "Chromecast", "Copy Browser Link (ADS)", "Load Links","Mark As Watched" ,"Copy Subtitle Link", "Play With Subtitles" };
        /// </summary>
        public static readonly string[] checks = { "Play", "Toggle Viewstate", "Download", "Remove Download", "Play Downloaded File", "Copy Link", "Chromecast", "Copy Browser Link (ADS)", "Load Links", "Mark As Watched", "Copy Subtitle Link", "Play With Subtitles" };
        public static int TitleToInt(string title)
        {
            //string[] checks = { "Play", "Toggle Viewstate", "Download", "Remove Download", "Play Downloaded File", "Copy Link" };
            for (int i = 0; i < checks.Length; i++) {
                if (checks[i] == title) {
                    return i;
                }
            }

            return -1;
        }

        static Java.Lang.Thread tempThred;
        /// <summary>
        /// id 0 = run VLC, 1 = Only history, 2 = Download, 3 = remove download, 4 = play download, 5 = copy link, 6 = chromecast, 7 = copy browser, 8 = Load links, 9 = Non invert history
        /// </summary>
        /// <param name="id"></param>
        /// <param name="pos"></param>
        /// <param name="v"></param>
        void DoLink(int id, int pos, View v = null, string extra = "")
        {
            print("AAA" + id + "|||" + pos);
            string link = activeLinks[flink[pos]];
            string linkName = activeLinksNames[flink[pos]];
            print("AAA" + link + ":::" + linkName);
            if (movieProvider[moveSelectedID] == 4 && linkName.StartsWith("Episode") && currentMain && (id == 0 || id == 8)) {
                string episode = FindHTML(linkName, "Episode ", ":");
                print("Episode id:" + episode);
                List<string> _activeLinks = new List<string>();
                List<string> _activeLinksNames = new List<string>();

                for (int i = 0; i < activeLinks.Count; i++) {
                    if (activeLinksNames[i].StartsWith("Episode")) {
                        _activeLinksNames.Add(activeLinksNames[i]);
                        _activeLinks.Add(activeLinks[i]);
                    }
                }
                activeLinks = _activeLinks;
                activeLinksNames = _activeLinksNames;
                thredNumber++;
                ShowSnackBar("Loading Links For Episode " + episode, ax_Links.__view);

                tempThred = new Java.Lang.Thread(
                     () =>
                     {
                         try {
                             GetUrlFromMovie123(moveSelectedID, thredNumber, "https://movies123.pro" + link, false, episode);

                             //Thread.Sleep(1000);
                         }
                         finally {
                             tempThred.Join();
                             //invoke(onCompleted);
                         }

                     });
                tempThred.Start();



            }
            else if (id == 0) {

                Intent intent = new Intent(Intent.ActionView);

                Android.Net.Uri uri = Android.Net.Uri.Parse(link);
                intent.SetData(uri);
                intent.PutExtra(EXTRA_TITLE, movieTitles[moveSelectedID].Replace("B___", "").Replace(" (Bookmark)", "") + " | " + activeLinksNames[flink[pos]]);
                intent.PutExtra(EXTRA_FILENAME, movieTitles[moveSelectedID].Replace("B___", "").Replace(" (Bookmark)", "") + " | " + activeLinksNames[flink[pos]]);

                try {
                    if (currentActiveSubtitle >= 0) {
                        intent.PutExtra(EXTRA_ENABLED_SUBTITLES, true);

                        intent.PutExtra(EXTRA_SUBTITLES, activeSubtitles[currentActiveSubtitle]);
                        intent.PutExtra(EXTRA_SUBTITLE_NAMES, activeSubtitlesNames[currentActiveSubtitle]);
                        print("Loaded subtitles: " + activeSubtitlesNames[currentActiveSubtitle] + "||" + activeSubtitles[currentActiveSubtitle]);
                    }
                }
                catch (System.Exception) {

                }

                intent.PutExtra(EXTRA_RETURN_RESULT, true);
                StartActivityForResult(intent, REQUEST_CODE);
            }
            else if (id == 1) {
                //HistoryPressTitle(movieTitles[moveSelectedID].Replace("B___", "").Replace(" (Bookmark)", "") + "|" + activeLinksNames[flink[pos]], true);
                if (!currentMain && movieProvider[moveSelectedID] == 4) {
                    HistoryPressTitle(movieTitles[moveSelectedID].Replace("B___", "").Replace(" (Bookmark)", "") + "|" + currentEpisodeName + "|" + activeLinksNames[flink[pos]], true);
                }
                else {
                    HistoryPressTitle(movieTitles[moveSelectedID].Replace("B___", "").Replace(" (Bookmark)", "") + "|" + activeLinksNames[flink[pos]], true);

                }

                UpdateList();
                _re.ScrollToPosition(pos);
            }
            else if (id == 2) {
                // string link = activeLinks[flink[pos]];
                UpdateList();
                _re.ScrollToPosition(pos);
                StartNewDownload(link, movieTitles[moveSelectedID].Replace("B___", "").Replace(" (Bookmark)", "") + " | " + activeLinksNames[flink[pos]], (movieIsAnime[moveSelectedID] ? "Anime" : "Movie"));

            }
            else if (id == 3) {
                UpdateList();
                _re.ScrollToPosition(pos);

                RemoveDownload(movieTitles[moveSelectedID].Replace("B___", "").Replace(" (Bookmark)", "") + " | " + activeLinksNames[flink[pos]], mainActivity, this.View);
            }
            else if (id == 4) {
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
            else if (id == 5) {
                print("Loading: " + movieTitles[moveSelectedID] + " | " + activeLinksNames[flink[pos]]);

                ClipboardManager clip = (ClipboardManager)Context.GetSystemService(Context.ClipboardService);
                clip.PrimaryClip = ClipData.NewPlainText("Link", activeLinks[flink[pos]]);
                ShowSnackBar("Copied " + activeLinksNames[flink[pos]] + " Link To Clipboard!", ax_links.View);
            }
            else if (id == 6) {
                //string link = activeLinks[flink[pos]];

                PlayLink(link);

            }
            else if (id == 7) {
                //string link = activeLinks[flink[pos]];
                print("Loading: " + movieTitles[moveSelectedID] + " | " + activeLinksNames[flink[pos]]);

                ClipboardManager clip = (ClipboardManager)Context.GetSystemService(Context.ClipboardService);
                clip.PrimaryClip = ClipData.NewPlainText("Link", "https://movies123.pro" + activeLinks[flink[pos]]);
                ShowSnackBar("Copied " + activeLinksNames[flink[pos]] + " Link To Clipboard!", ax_links.View);
            }
            else if (id == 9) {
                if (!currentMain && movieProvider[moveSelectedID] == 4) {
                    HistoryPressTitle(movieTitles[moveSelectedID].Replace("B___", "").Replace(" (Bookmark)", "") + "|" + currentEpisodeName + "|" + activeLinksNames[flink[pos]]);
                }
                else {
                    HistoryPressTitle(movieTitles[moveSelectedID].Replace("B___", "").Replace(" (Bookmark)", "") + "|" + activeLinksNames[flink[pos]]);

                }

                UpdateList();
                _re.ScrollToPosition(pos);
            }
            else if (id == 10) {
                ClipboardManager clip = (ClipboardManager)Context.GetSystemService(Context.ClipboardService);
                clip.PrimaryClip = ClipData.NewPlainText("Link", activeSubtitles[currentActiveSubtitle]);
                ShowSnackBar("Copied " + activeSubtitlesNames[currentActiveSubtitle] + " Subtitle Link To Clipboard!", ax_links.View);
            }
            else if (id == 11) {

                StartNewDownload(activeSubtitles[currentActiveSubtitle], movieTitles[moveSelectedID].Replace("B___", "").Replace(" (Bookmark)", "") + " | " + activeLinksNames[flink[pos]] + " | " + activeSubtitlesNames[currentActiveSubtitle], "Subtitles");

            }
            else if (id == 12) {
                string subtitleURL = movieTitles[moveSelectedID].Replace("B___", "").Replace(" (Bookmark)", "") + " | " + activeLinksNames[flink[pos]] + " | " + activeSubtitlesNames[currentActiveSubtitle];
                DoLink(0, pos, null, subtitleURL);
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