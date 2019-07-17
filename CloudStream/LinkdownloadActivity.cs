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
        static EditText title, link;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            //Window.RequestFeature(WindowFeatures.NoTitle);
            SetContentView(Resource.Layout.Activity_Linkdownload);

            Button btnDownload = FindViewById<Button>(Resource.Id.btnDownload_2);
            Button bttPaste = FindViewById<Button>(Resource.Id.pasteLink);

            title = FindViewById<EditText>(Resource.Id.txtTitle);
            link = FindViewById<EditText>(Resource.Id.txtLink);

            //  TextInputLayout passwordWrapper = FindViewById<TextInputLayout>(Resource.Id.txt);
            //string txtPassword = passwordWrapper.EditText.Text;

            bttPaste.Click += (o, e) =>
            {
                var clipboard = (ClipboardManager)GetSystemService(ClipboardService);

                string pasteData = "";

                if (!(clipboard.HasPrimaryClip)) {
                    // If it does contain data, decide if you can handle the data.
                }
                else if (!(clipboard.PrimaryClipDescription.HasMimeType(ClipDescription.MimetypeTextPlain))) {
                    // since the clipboard has data but it is not plain text
                }
                else {
                    //since the clipboard contains plain text.
                    var item = clipboard.PrimaryClip.GetItemAt(0);

                    // Gets the clipboard as text.
                    pasteData = item.Text;
                }

                if (pasteData != "") {
                    Paste(pasteData);
                }
            };

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

        static async void Paste(string pasteData)
        {
            link.Text = pasteData;
            string _title = await MainActivity.GetYTVideo(pasteData);
            if (_title != "") {
                title.Text = _title;
            }
        }
    }
}