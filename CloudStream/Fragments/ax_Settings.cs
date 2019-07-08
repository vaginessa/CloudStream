using System;
using System.Collections.Generic;
using System.Linq;
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

        readonly static bool haveAnimeEnabled = true;
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
            print("________________ SaveInt" + inp + "|" + saveState + "___________");
            var _set = Application.Context.GetSharedPreferences("Settings", FileCreationMode.Private);
            var edit = _set.Edit();
            edit.PutInt(inp, saveState);
            edit.Commit();
        }

        /// <summary>
        /// 0 = msearch 1 = hdasearch, 2 = asearch, 3 = basearch, 4 = savelinks, 5 = savetitles, 6 = tvasearch, 7 = bmsearch, 8 = htvasearch
        /// </summary>
        /// <returns></returns>
        public static bool SettingsGetChecked(int i)
        {
            try {
                bool[] bools = { msearch.Checked, hdasearch.Checked, asearch.Checked, basearch.Checked, savelinks.Checked, savetitles.Checked, tvasearch.Checked, bmsearch.Checked, htvasearch.Checked };
                return bools[i];
            }
            catch (Exception) {
                var set = Application.Context.GetSharedPreferences("Settings", FileCreationMode.Private);
                bool[] bools = { set.GetBoolean("msearch", true), set.GetBoolean("hdasearch", haveAnimeEnabled), set.GetBoolean("asearch", false), set.GetBoolean("basearch", false), set.GetBoolean("savelinks", true), set.GetBoolean("savetitles", true), set.GetBoolean("tvasearch", true), set.GetBoolean("bmsearch", false), set.GetBoolean("htvasearch", false) };
                return bools[i];
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
        static CheckBox msearch, hdasearch, asearch, basearch, savelinks, savetitles, tvasearch, bmsearch, htvasearch;
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);
            View view = inflater.Inflate(Resource.Layout.ax_Settings, container, false);

            var cbookmarks = view.FindViewById(Resource.Id.clearbookmarks);
            var chistory = view.FindViewById(Resource.Id.clearhistory);

            msearch = view.FindViewById<CheckBox>(Resource.Id.msearch);
            hdasearch = view.FindViewById<CheckBox>(Resource.Id.hdasearch);
            asearch = view.FindViewById<CheckBox>(Resource.Id.asearch);
            basearch = view.FindViewById<CheckBox>(Resource.Id.basearch);
            tvasearch = view.FindViewById<CheckBox>(Resource.Id.tvasearch);
            htvasearch = view.FindViewById<CheckBox>(Resource.Id.htvasearch);
            bmsearch = view.FindViewById<CheckBox>(Resource.Id.bmsearch);

            savelinks = view.FindViewById<CheckBox>(Resource.Id.savelinks);
            savetitles = view.FindViewById<CheckBox>(Resource.Id.savetitles);


            //  var sdownloads =  view.FindViewById(Resource.Id.showdownloads);

            // sdownloads.Click += (o,e) => MainActivity.mainActivity.ShowDownloads();


            var set = Application.Context.GetSharedPreferences("Settings", FileCreationMode.Private);

            msearch.Checked = set.GetBoolean("msearch", true);
            hdasearch.Checked = set.GetBoolean("hdasearch", haveAnimeEnabled);
            asearch.Checked = set.GetBoolean("asearch", false);
            basearch.Checked = set.GetBoolean("basearch", false);
            tvasearch.Checked = set.GetBoolean("tvasearch", false);
            bmsearch.Checked = set.GetBoolean("bmsearch", false);
            htvasearch.Checked = set.GetBoolean("bmsearch", true);

            if (!haveAnimeEnabled) {
                hdasearch.Visibility = ViewStates.Gone;
                asearch.Visibility = ViewStates.Gone;
                basearch.Visibility = ViewStates.Gone;
            }




            savelinks.Checked = set.GetBoolean("savelinks", true);
            savetitles.Checked = set.GetBoolean("savetitles", true);

            msearch.Click += (o, e) => SaveBool("msearch", msearch.Checked);
            hdasearch.Click += (o, e) => SaveBool("hdasearch", hdasearch.Checked);
            asearch.Click += (o, e) => SaveBool("asearch", asearch.Checked);
            basearch.Click += (o, e) => SaveBool("basearch", basearch.Checked);
            tvasearch.Click += (o, e) => SaveBool("tvasearch", tvasearch.Checked);
            bmsearch.Click += (o, e) => SaveBool("bmsearch", bmsearch.Checked);
            htvasearch.Click += (o, e) => SaveBool("htvasearch", htvasearch.Checked);

            savelinks.Click += (o, e) => SaveBool("savelinks", savelinks.Checked);
            savetitles.Click += (o, e) => SaveBool("savetitles", savetitles.Checked);


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

            // fab.Visibility = ViewStates.Gone;


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