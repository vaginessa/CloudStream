using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;
using static CloudStream.MainActivity;
using SupportFragment = Android.Support.V4.App.Fragment;
using SupportToolbar = Android.Support.V7.Widget.Toolbar;

namespace CloudStream.Fragments
{
    public class ax_Settings : SupportFragment
    {

        const bool haveAnimeEnabled = true;
        const bool searchForUpdates = true;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        void SaveBool(string inp, bool on)
        {
            var _set = Application.Context.GetSharedPreferences("Settings", FileCreationMode.Private);
            var edit = _set.Edit();
            edit.PutBoolean(inp, on);
            edit.Commit();
        }

        void SaveInt(string inp, int saveState)
        {
           // print("________________ SaveInt" + inp + "|" + saveState + "___________");
            var _set = Application.Context.GetSharedPreferences("Settings", FileCreationMode.Private);
            var edit = _set.Edit();
            edit.PutInt(inp, saveState);
            edit.Commit();
        }

        /// <summary>
        /// 0 = msearch 1 = hdasearch, 2 = asearch, 3 = basearch, 4 = savelinks, 5 = savetitles, 6 = tvasearch, 7 = bmsearch, 8 = htvasearch, 9 = hdbasearch, 10 = savem3u
        /// </summary>
        /// <returns></returns>
        public static bool SettingsGetChecked(int i)
        {
            try {
                bool[] bools = { msearch.Checked, hdasearch.Checked, asearch.Checked, basearch.Checked, savelinks.Checked, savetitles.Checked, tvasearch.Checked, bmsearch.Checked, htvasearch.Checked, hdbasearch.Checked, savem3u.Checked };
                return bools[i];
            }
            catch (Exception) {
                var set = Application.Context.GetSharedPreferences("Settings", FileCreationMode.Private);
                string[] boolset = { "msearch", "hdasearch", "asearch", "basearch", "savelinks", "savetitles", "tvasearch", "bmsearch", "htvasearch", "hdbasearch", "savem3u" };
                bool[] defVal = { true, haveAnimeEnabled, false, false, true, true, false, false, true, false, false };
                return set.GetBoolean(boolset[i], defVal[i]);
            }
        }

        /// <summary>
        /// 0 = def actions; 1 = sec action
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public static int SettingsGetDef(int i, bool axLinkSett = false)
        {
            int r = defActions[i];
            if (r == -1) {
                var set = Application.Context.GetSharedPreferences("Settings", FileCreationMode.Private);
                string[] keys = { "defAct", "secAct" };
                int[] ret = { 0, 1 };
                r = set.GetInt(keys[i], ret[i]);
                if (r == -1) {
                    r = ret[i];
                }
            }
            if (axLinkSett) {
                r = CheckToReal(r, true);
            }

            return r;
        }

        public static int[] defActions = { -1, -1 };
        static CheckBox msearch, hdasearch, asearch, basearch, savelinks, savetitles, tvasearch, bmsearch, htvasearch, hdbasearch, savem3u;
        static string[] pingLinks = { "movies123.pro", "www3.gogoanime.io", "ww1.kuroani.me", "ww.9animes.net", "grabthebeast.com", "zmovies.me", "movies123.pro", "gogoanime.cool" };
        const int providersCount = 8;
        static string[] searchTxts = new string[providersCount];
        static List<CheckBox> seachBoxes = new List<CheckBox>();
        static bool showSites = false;

        static string version = "";
        Java.Lang.Thread sThred;

        static string updateTo = "-1";
        public static ax_Settings ax_settings;

        static void Ping(int providerID)
        {
            WebClient client = new WebClient();
            var sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            bool error = false;
            try {
                string d = client.DownloadString("https://" + pingLinks[providerID]);

            }
            catch (Exception) {
                error = true;
                seachBoxes[providerID].Text = searchTxts[providerID] + (showSites ? (" (" + pingLinks[providerID] + ")") : "") + " (ERROR)";
            }
            if (!error) {
                long elapsedMilliseconds = sw.ElapsedMilliseconds;
                seachBoxes[providerID].Text = searchTxts[providerID] + (showSites ? (" (" + pingLinks[providerID] + ")") : "") + " (" + elapsedMilliseconds + "ms)";
            }

        }


        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);
            View view = inflater.Inflate(Resource.Layout.ax_Settings, container, false);

            ax_settings = this;

            // --------- SET VARS ---------

            var cbookmarks = view.FindViewById(Resource.Id.clearbookmarks);
            var chistory = view.FindViewById(Resource.Id.clearhistory);

            msearch = view.FindViewById<CheckBox>(Resource.Id.msearch);
            hdasearch = view.FindViewById<CheckBox>(Resource.Id.hdasearch);
            asearch = view.FindViewById<CheckBox>(Resource.Id.asearch);
            basearch = view.FindViewById<CheckBox>(Resource.Id.basearch);
            tvasearch = view.FindViewById<CheckBox>(Resource.Id.tvasearch);
            htvasearch = view.FindViewById<CheckBox>(Resource.Id.htvasearch);
            bmsearch = view.FindViewById<CheckBox>(Resource.Id.bmsearch);
            hdbasearch = view.FindViewById<CheckBox>(Resource.Id.hdbasearch);

            savelinks = view.FindViewById<CheckBox>(Resource.Id.savelinks);
            savetitles = view.FindViewById<CheckBox>(Resource.Id.savetitles);
            savem3u = view.FindViewById<CheckBox>(Resource.Id.savem3u);

            Button updatebtt = view.FindViewById<Button>(Resource.Id.update);
            updatebtt.Visibility = ViewStates.Gone;


            // --------- PING DATA ---------

            seachBoxes = new List<CheckBox> { msearch, hdasearch, asearch, basearch, tvasearch, bmsearch, htvasearch, hdbasearch };
            for (int i = 0; i < seachBoxes.Count; i++) {
                searchTxts[i] = seachBoxes[i].Text;
            }

            Button pingBtt = view.FindViewById<Button>(Resource.Id.pingProviders);
            pingBtt.Click += (o, e) =>
            {
                for (int i = 0; i < providersCount; i++) {
                    DelegateWithParameters_int ping =
         new DelegateWithParameters_int(Ping);

                    IAsyncResult tag =
                        ping.BeginInvoke(i, null, null);

                }
            };

            Button showBtt = view.FindViewById<Button>(Resource.Id.showProviders);
            showBtt.Click += (o, e) =>
            {
                showSites = !showSites;
                for (int i = 0; i < providersCount; i++) {
                    seachBoxes[i].Text = searchTxts[i] + (showSites ? (" (" + pingLinks[i] + ")") : "");

                }
            };

            // --------- AUTO UPDATE ---------

            version = Context.PackageManager.GetPackageInfo(Context.PackageName, 0).VersionName;
            TextView versionTxt = view.FindViewById<TextView>(Resource.Id.versionTxt);
            versionTxt.Text = "v" + version;
            sThred = new Java.Lang.Thread(() =>
            {
                try {
                    WebClient client = new WebClient();
                    string d = client.DownloadString("https://github.com/LagradOst/CloudStream/releases");
                    string look = "/LagradOst/CloudStream/releases/tag/";
                    //   float bigf = -1;
                    //     string bigUpdTxt = "";
                    // while (d.Contains(look)) {
                    string tag = FindHTML(d, look, "\"");
                    string updText = FindHTML(d, look + tag + "\">", "<");
                    /*
                        try {
                            float tagf = float.Parse(tag.Replace("v", ""));
                            print("" + tagf);
                            if(tagf > bigf) {
                                bigf = tagf;
                                bigUpdTxt = updText;
                            }
                        }
                        catch (Exception) {

                        }
                        d = d.Substring(d.IndexOf(look) + 1, d.Length - d.IndexOf(look) - 1);
                        */
                    updatebtt.Click += (o, e) =>
                    {
                        if (version != updateTo && updateTo != "-1") {
                            string downloadLink = "https://github.com/LagradOst/CloudStream/releases/download/" + updateTo + "/CloudStream.CloudStream.apk";
                            DownloadFromLink(downloadLink, "CloudStream-" + updateTo, "Downloading APK", this, ".apk", true);
                            updatebtt.Visibility = ViewStates.Gone;
                        }
                    };

                    if (tag != "") {
                        Activity.RunOnUiThread(new Action(() =>
                        {
                            updateTo = tag;
                            if (tag == "v" + version) {
                                versionTxt.Text = "v" + version + " (Up to date)";
                                updatebtt.Visibility = ViewStates.Gone;
                            }
                            else {
                                versionTxt.Text = "v" + version + " -> " + tag + " (" + updText + ")";
                                updatebtt.Visibility = ViewStates.Visible;
                                updatebtt.Text = "Update to " + tag;
                            }

                        }));
                    }

                }
                finally {
                    sThred.Join();
                }
            });
            if (searchForUpdates) {
                sThred.Start();
            }
            //  var sdownloads =  view.FindViewById(Resource.Id.showdownloads);

            // sdownloads.Click += (o,e) => MainActivity.mainActivity.ShowDownloads();



            // --------- DEF DATA ---------

            var set = Application.Context.GetSharedPreferences("Settings", FileCreationMode.Private);

            msearch.Checked = set.GetBoolean("msearch", true);
            hdasearch.Checked = set.GetBoolean("hdasearch", haveAnimeEnabled);
            asearch.Checked = set.GetBoolean("asearch", false);
            basearch.Checked = set.GetBoolean("basearch", false);
            tvasearch.Checked = set.GetBoolean("tvasearch", false);
            bmsearch.Checked = set.GetBoolean("bmsearch", false);
            htvasearch.Checked = set.GetBoolean("htvasearch", true);
            hdbasearch.Checked = set.GetBoolean("hdbasearch", false);
            savem3u.Checked = set.GetBoolean("savem3u", false);

            if (!haveAnimeEnabled) {
                hdasearch.Visibility = ViewStates.Gone;
                asearch.Visibility = ViewStates.Gone;
                basearch.Visibility = ViewStates.Gone;
                hdbasearch.Visibility = ViewStates.Gone;
            }


            // --------- SAVE DATA ---------

            savelinks.Checked = set.GetBoolean("savelinks", true);
            savetitles.Checked = set.GetBoolean("savetitles", true);

            msearch.Click += (o, e) => SaveBool("msearch", msearch.Checked);
            hdasearch.Click += (o, e) => SaveBool("hdasearch", hdasearch.Checked);
            asearch.Click += (o, e) => SaveBool("asearch", asearch.Checked);
            basearch.Click += (o, e) => SaveBool("basearch", basearch.Checked);
            tvasearch.Click += (o, e) => SaveBool("tvasearch", tvasearch.Checked);
            bmsearch.Click += (o, e) => SaveBool("bmsearch", bmsearch.Checked);
            htvasearch.Click += (o, e) => SaveBool("htvasearch", htvasearch.Checked);
            hdbasearch.Click += (o, e) => SaveBool("hdbasearch", hdbasearch.Checked);

            savelinks.Click += (o, e) => SaveBool("savelinks", savelinks.Checked);
            savetitles.Click += (o, e) => SaveBool("savetitles", savetitles.Checked);
            savem3u.Click += (o, e) => SaveBool("savem3u", savem3u.Checked); // M3U FILE

            // --------- HISTORY --------- 

            chistory.SetBackgroundColor(Color.AliceBlue);
            cbookmarks.SetBackgroundColor(Color.AliceBlue);
            //  sdownloads.SetBackgroundColor(Color.AliceBlue);

            chistory.LongClickable = true;
            cbookmarks.LongClickable = true;

            chistory.LongClick += (o, e) =>
            {
                ShowSnackBar("Removed All Viewhistory", o);
                var localC = Application.Context.GetSharedPreferences("History", FileCreationMode.Private);
                var edit = localC.Edit();
                edit.Clear();
                edit.Commit();
            };
            cbookmarks.LongClick += (o, e) =>
            {
                ShowSnackBar("Removed All Bookmarks", o);
                var localC = Application.Context.GetSharedPreferences("Bookmarks", FileCreationMode.Private);
                var edit = localC.Edit();
                edit.Clear();
                edit.Commit();
                ax_Bookmarks.ax_bookmarks.UpdateList();
            };
            chistory.Click += (o, e) =>
            {
                ShowSnackBar("Hold To Remove History", o);
            };
            cbookmarks.Click += (o, e) =>
            {
                ShowSnackBar("Hold To Remove Bookmarks", o);

            };

            // --------- ACTIONS --------- 

            var defAct = view.FindViewById<Spinner>(Resource.Id.DefSpinner);
            var secAct = view.FindViewById<Spinner>(Resource.Id.SecSpinner);


            var adapter = new ArrayAdapter<string>(Context, Android.Resource.Layout.SimpleSpinnerItem, defChecks);
            defAct.Adapter = adapter;
            secAct.Adapter = adapter;

            defActions[0] = SettingsGetDef(0); // set.GetInt("defAct", 0);
            defActions[1] = SettingsGetDef(1); // set.GetInt("secAct", 1);

            defAct.SetSelection(defActions[0]);
            secAct.SetSelection(defActions[1]);

            defAct.ItemSelected += (o, e) => { SaveInt("defAct", e.Position); defActions[0] = e.Position; };
            secAct.ItemSelected += (o, e) => { SaveInt("secAct", e.Position); defActions[1] = e.Position; };

            return view;
        }
        static string[] defChecks = { ax_Links.checks[0], ax_Links.checks[1], ax_Links.checks[5], ax_Links.checks[2], ax_Links.checks[3], ax_Links.checks[4] };

        static int CheckToReal(int c, bool return_q = false)
        {
            if (return_q) {
                for (int q = 0; q < ax_Links.checks.Length; q++) {
                    if (defChecks[c] == ax_Links.checks[q]) {
                        return q;
                    }
                }
            }
            else {
                for (int i = 0; i < defChecks.Length; i++) {
                    if (defChecks[i] == ax_Links.checks[c]) {
                        return i;
                    }
                }
            }


            return -1;
        }
    }
}