using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Java.Lang;
using static CloudStream.MainActivity;
using SupportFragment = Android.Support.V4.App.Fragment;

namespace CloudStream.Fragments
{
    public class ax_Info : SupportFragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }
        List<Button> btts = new List<Button>();

        public void AddTabs(int _seasons)
        {
            seasons = _seasons;
            Runnable m = new Runnable(_AddTabs);
            try {
                Activity.RunOnUiThread(m);
            }
            catch (System.Exception) {
            }

           
        }
        int seasons = 0;
        void _AddTabs()
        {

            if(seasons > 0) { 
                btts[0].Text = "Show All";

                for (int i = 1; i <= seasons; i++) {
                    btts[i].Text = "Only Season " + i;
                }
                for (int i = 0; i <= seasons; i++) {
                    btts[i].Visibility = ViewStates.Visible;
                }
            }
        }
        public static ax_Info ax_info;
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);
            View view = inflater.Inflate(Resource.Layout.ax_Info, container, false);
            ax_info = this;
            var m_title = view.FindViewById<TextView>(Resource.Id.moveTitle);

            
            currentMovie = moviesActive[moveSelectedID];
            m_title.Text = movieTitles[moveSelectedID].Replace("B___","");


            string add = "";
            if(currentMovie.imdbRating != null) {
                add += currentMovie.imdbRating + "\n"; 
            }
            if (currentMovie.year != null) {
                add += currentMovie.year + "\n";
            }
            if (currentMovie.runtime != null) {
                add += currentMovie.runtime + "\n";
            }
            if (currentMovie.plot != null) {
                add += currentMovie.plot + "\n";
            }

            
           
            var m_dec = view.FindViewById<TextView>(Resource.Id.movieInfo);
            m_dec.Text = add;

            if (movieProvider[moveSelectedID] == 3) {
                m_dec.SetLines(2);
            }

            var buttons = view.FindViewById<LinearLayout>(Resource.Id.Buttons);

            LinearLayout linearLayout = buttons; //new LinearLayout(this);
            linearLayout.Orientation = (Orientation.Vertical);

            for (int i = 0; i < 100; i++) {
                var button = new Button(this.Context);
                button.Text = "Button " + i;
                button.LayoutParameters = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);
                button.Visibility = ViewStates.Gone;
                button.Click += (o, e) =>
                {
                    string txt = button.Text;
                    txt = txt.Replace("Only Season ", "");
                    if(txt == "Show All") {
                        __selSeason = 0;
                    }
                    else {
                        __selSeason = int.Parse(txt);
                    }
                    ax_Links.ax_links.UpdateList();


                };
                linearLayout.AddView(button);
                btts.Add(button);
            }


            return view;
        }
    }
}