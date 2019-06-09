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

        /// <summary>
        /// 0 = msearch 1 = hdasearch, 2 = asearch, 3 = basearch, 4 = savelinks, 5 = savetitles, 6 = tvasearch
        /// </summary>
        /// <returns></returns>
        public static bool SettingsGetChecked(int i)
        {
            try {
                bool[] bools = { msearch.Checked, hdasearch.Checked, asearch.Checked, basearch.Checked, savelinks.Checked, savetitles.Checked, tvasearch.Checked };
                return bools[i];
            }
            catch (Exception) {
                var set = Application.Context.GetSharedPreferences("Settings", FileCreationMode.Private);
                bool[] bools = { set.GetBoolean("msearch", true), set.GetBoolean("hdasearch", true), set.GetBoolean("asearch", false), set.GetBoolean("basearch", false), set.GetBoolean("savelinks", true), set.GetBoolean("savetitles", true),set.GetBoolean("tvasearch",true) };
                return bools[i];
            }
        }

        static CheckBox msearch, hdasearch, asearch, basearch, savelinks, savetitles, tvasearch;
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);
            View view = inflater.Inflate(Resource.Layout.ax_Settings, container, false);

            var cbookmarks =  view.FindViewById(Resource.Id.clearbookmarks);
            var chistory=  view.FindViewById(Resource.Id.clearhistory);

            msearch =  view.FindViewById<CheckBox>(Resource.Id.msearch);
            hdasearch =  view.FindViewById<CheckBox>(Resource.Id.hdasearch);
            asearch =  view.FindViewById<CheckBox>(Resource.Id.asearch);
            basearch =  view.FindViewById<CheckBox>(Resource.Id.basearch);
            tvasearch =  view.FindViewById<CheckBox>(Resource.Id.tvasearch);

            savelinks =  view.FindViewById<CheckBox>(Resource.Id.savelinks);
            savetitles =  view.FindViewById<CheckBox>(Resource.Id.savetitles);

          //  var sdownloads =  view.FindViewById(Resource.Id.showdownloads);

           // sdownloads.Click += (o,e) => MainActivity.mainActivity.ShowDownloads();


            var set = Application.Context.GetSharedPreferences("Settings", FileCreationMode.Private);

            msearch.Checked = set.GetBoolean("msearch", true);
            hdasearch.Checked = set.GetBoolean("hdasearch", true);
            asearch.Checked = set.GetBoolean("asearch", false);
            basearch.Checked = set.GetBoolean("basearch", false);
            tvasearch.Checked = set.GetBoolean("tvasearch", true);

            savelinks.Checked = set.GetBoolean("savelinks", true);
            savetitles.Checked = set.GetBoolean("savetitles", true);

            msearch.Click += (o, e) => SaveBool("msearch", msearch.Checked);
            hdasearch.Click += (o, e) => SaveBool("hdasearch", hdasearch.Checked);
            asearch.Click += (o, e) => SaveBool("asearch", asearch.Checked);
            basearch.Click += (o, e) => SaveBool("basearch", basearch.Checked);
            tvasearch.Click += (o, e) => SaveBool("tvasearch", tvasearch.Checked);

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


            // fab.Visibility = ViewStates.Gone;


            return view;
        }
    }
}