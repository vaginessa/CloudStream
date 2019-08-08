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
    public class ChromeCastActivity : AppCompatActivity
    {
        public static ChromeCastActivity chromeCastActivity;

        FloatingActionButton fab;
        static int offset = 0;
        static ImageView poster;
        static ImageButton pauseBtt, backSecBtt, stopBtt;
        static SeekBar seekBar;
        static TextView leftCastTxt, rightCastTxt, titleTxt;
        static bool setPoster = false;
        static long posterIDRaw = 0;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            //Window.RequestFeature(WindowFeatures.NoTitle);
            chromeCastActivity = this;

            SetContentView(Resource.Layout.Activity_ChromeCast);
            poster = FindViewById<ImageView>(Resource.Id.posterID);

            pauseBtt = FindViewById<ImageButton>(Resource.Id.CastPauseBtt);
            backSecBtt = FindViewById<ImageButton>(Resource.Id.CastBackBtt);
            stopBtt = FindViewById<ImageButton>(Resource.Id.CastStopBtt);

            seekBar = FindViewById<SeekBar>(Resource.Id.seekBar);

            leftCastTxt = FindViewById<TextView>(Resource.Id.castTimeleft);
            rightCastTxt = FindViewById<TextView>(Resource.Id.castTimeright);
            titleTxt = FindViewById<TextView>(Resource.Id.posterText);


            titleTxt.Text = castingTitle;
            pauseBtt.SetBackgroundResource(castingPaused ? Resource.Drawable.ic_media_play_dark : Resource.Drawable.ic_media_pause_dark);


            print("POSTER: " + castingPosterId);
            setPoster = true;

            if (castingPosterId != null && castingPosterId != "") {
                if (CheckIfURLIsValid(castingPosterId)) {
                    if (!firstCast) {
                        string rootPath = Android.OS.Environment.ExternalStorageDirectory + "/" + Android.OS.Environment.DirectoryDownloads + "/" + "TempPoster";
                        poster.SetImageURI(Android.Net.Uri.Parse(rootPath));
                        setPoster = true;
                        poster.SetBackgroundResource(Resource.Menu.border);
                    }
                    else {
                        try {
                            posterIDRaw = DownloadRaw(castingPosterId, "TempPoster");
                            setPoster = false;
                            firstCast = false;
                        }
                        catch (Exception) {

                        }
                    }
                }
            }



            try {
                //   poster.SetImageURI(Android.Net.Uri.Parse(castingPosterId));
            }
            catch (Exception) {

            }

            backSecBtt.Click += (o, e) =>
            {
                try {
                    SeekMedia(-30);
                }
                catch (Exception) {

                }
            };

            pauseBtt.Click += (o, e) =>
            {
                castingPaused = !castingPaused;
                pauseBtt.SetBackgroundResource(castingPaused ? Resource.Drawable.ic_media_play_dark : Resource.Drawable.ic_media_pause_dark);
                try {
                    ChromeSetPauseState(castingPaused);

                }
                catch (Exception) {
                }
            };

            stopBtt.Click += (o, e) =>
            {
                try {

                    StopMovieCast();
                }
                catch (Exception) {

                }
                castingVideo = false;
                chromeStart.Visibility = ViewStates.Gone;
                Finish();
            };

            seekBar.ProgressChanged += (o, e) =>
            {
                if (e.FromUser) {
                    try {
                        SetChromeTime(e.Progress * (castingDuration / 100));
                    }
                    catch (Exception) {

                    }
                }
            };


            //print(poster.);

            // SupportToolbar toolBar = FindViewById<SupportToolbar>(Resource.Id.toolBar);
            //  SetSupportActionBar(toolBar);
            //  toolBar.Visibility = ViewStates.Gone;


            // TabLayout tabs = FindViewById<TabLayout>(Resource.Id.tabs);

            // ViewPager viewPager = FindViewById<ViewPager>(Resource.Id.viewpager);

            //SetUpViewPager(viewPager);

            //tabs.SetupWithViewPager(viewPager);

            //    fab = FindViewById<FloatingActionButton>(Resource.Id.fab);
            //   fab.Visibility = ViewStates.Gone;
            //fab.Visibility = ViewStates.Gone;

        }

        public void CastVideoStart()
        {
            print("START");

            updateThred = new Java.Lang.Thread(() =>
            {
                try {
                    while (castingVideo) {
                        Java.Lang.Thread.Sleep(1000);
                        RunOnUiThread(() => UpdateTime());
                    }
                }
                finally {
                    updateThred.Join();
                }
            });
            updateThred.Start();
        }


        static Java.Lang.Thread updateThred;

        static string ForceLetters(int inp, int letters = 2)
        {
            int added = letters - inp.ToString().Length;
            if (added > 0) {
                return MultiplyString("0", added) + inp.ToString();
            }
            else {
                return inp.ToString();
            }
        }
        public static string MultiplyString(string s, int times)
        {
            return String.Concat(Enumerable.Repeat(s, times));
        }

        static string ConvertTimeToString(double time)
        {
            int sec = (int)Math.Round(time);
            int rsec = (sec % 60);
            int min = (int)Math.Ceiling((sec - rsec) / 60f);
            int rmin = min % 60;
            int h = (int)Math.Ceiling((min - rmin) / 60f);
            int rh = h;// h % 24;
            return (h > 0 ? ForceLetters(h) + ":" : "") + ((rmin >= 0 || h >= 0) ? ForceLetters(rmin) + ":" : "") + ForceLetters(rsec);
        }

        void UpdateTime()
        {
            //  CastUpdateStatus();

            if (!castingPlaying) {
                castUpdatedNow = DateTime.Now;
            }

            double currentTime = GetChromeTime();
            if (!setPoster) {
                DownloadManager manager;
                manager = (DownloadManager)MainActivity.mainActivity.GetSystemService(Context.DownloadService);
                try {
                    Android.Net.Uri uri = manager.GetUriForDownloadedFile(posterIDRaw); // ERROR IF FILE DOSENT EXIST
                                                                                        // print("POSTERURI: " + uri);
                    string rootPath = Android.OS.Environment.ExternalStorageDirectory + "/" + Android.OS.Environment.DirectoryDownloads + "/" + "TempPoster";
                    poster.SetImageURI(Android.Net.Uri.Parse(rootPath));
                    setPoster = true;
                    poster.SetBackgroundResource(Resource.Menu.border);
                }
                catch (Exception) {


                }
            }


            if (castingDuration - currentTime <= 4) {
                MovieEnd();
            }

            leftCastTxt.Text = ConvertTimeToString(currentTime);
            rightCastTxt.Text = ConvertTimeToString(castingDuration - currentTime);
            seekBar.Progress = (int)Math.Round((currentTime * 100) / castingDuration);
        }


        public void MovieEnd()
        {
            castingVideo = false;
            chromeStart.Visibility = ViewStates.Gone;
            Finish();
        }

        public static void ChangePauseBtt()
        {
            pauseBtt.SetBackgroundResource(castingPaused ? Resource.Drawable.ic_media_play_dark : Resource.Drawable.ic_media_pause_dark);
        }


        /*
        private void SetUpViewPager(ViewPager viewPager)
        {
            TabAdapter adapter = new TabAdapter(SupportFragmentManager);

           // adapter.AddFragment(new ax_ChromeCastInfo(), "Chromecast");


            // adapter.Fragments[1].set = "currentMain";

            viewPager.Adapter = adapter;
        }
        public class MyBottomSheetCallBack : BottomSheetBehavior.BottomSheetCallback
        {
            public override void OnSlide(View bottomSheet, float slideOffset)
            {
                //Sliding
            }

            public override void OnStateChanged(View bottomSheet, int newState)
            {
                //State changed
            }
        }
        */
    }
}