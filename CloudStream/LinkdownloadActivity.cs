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
    public class LinkdownloadActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {

            base.OnCreate(savedInstanceState);
            //Window.RequestFeature(WindowFeatures.NoTitle);
            SetContentView(Resource.Layout.Activity_Linkdownload);

            Button btnDownload = FindViewById<Button>(Resource.Id.btnDownload_2);

            EditText title = FindViewById<EditText>(Resource.Id.txtTitle);
            EditText link = FindViewById<EditText>(Resource.Id.txtLink);

            //  TextInputLayout passwordWrapper = FindViewById<TextInputLayout>(Resource.Id.txt);
            //string txtPassword = passwordWrapper.EditText.Text;

            btnDownload.Click += (o, e) =>
            {

                if (title.Text != "" && link.Text != "") {
                    mainActivity.DownloadLink(title.Text, link.Text);
                    Finish();
                }
                else {
                    ShowSnackBar("Make sure to fill both fields", o);
                }
            };

        }

    }
}