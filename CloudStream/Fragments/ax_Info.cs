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

            if (seasons > 0) {
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
        public Spinner subtitleSpinner;
        public TextView subtitleText;
        static List<string> subtitleFill = new List<string>();

        public void SetSpinner(List<string> fill)
        {
            subtitleFill = fill;
            try {


                Runnable r = new Runnable(_SetSpinner);
                Activity.RunOnUiThread(r);
            }
            catch (System.Exception) {

            }

        }

        public void _SetSpinner()
        {
            List<string> fill = new List<string>();
            for (int i = 0; i < subtitleFill.Count; i++) {
                fill.Add(subtitleFill[i]);
            }
            if (!useSubtitles) {
                fill = new List<string>();
            }
            fill.Insert(0, "None");
            bool gone = fill.Count <= 1;

            if (!SHOW_INFO_SUBTITLES) {
                gone = true;
            }

            subtitleSpinner.Visibility = gone ? ViewStates.Gone : ViewStates.Visible;
            subtitleText.Visibility = gone ? ViewStates.Gone : ViewStates.Visible;
            if (!gone) {
                var adapter = new ArrayAdapter<string>(Context, Android.Resource.Layout.SimpleSpinnerItem, fill);
                subtitleSpinner.Adapter = adapter;
            }
        }


        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);
            View view = inflater.Inflate(Resource.Layout.ax_Info, container, false);
            ax_info = this;
            var m_title = view.FindViewById<TextView>(Resource.Id.moveTitle);
            subtitleSpinner = view.FindViewById<Spinner>(Resource.Id.subtitleSpinner);
            subtitleText = view.FindViewById<TextView>(Resource.Id.subtitleText);

            SetSpinner(new List<string>());

            subtitleSpinner.ItemSelected += SubtitleSpinner_ItemSelected;

            currentMovie = moviesActive[moveSelectedID];
            m_title.Text = movieTitles[moveSelectedID].Replace("B___", "").Replace(" (Bookmark)", "");


            string add = "";
            if (currentMovie.imdbRating != null) {
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

            ImageButton castBtt = view.FindViewById<ImageButton>(Resource.Id.imageButton1);
            castBtt.Visibility = ChromechastExists() ? ViewStates.Visible : ViewStates.Invisible;
            if (!ChromechastExists()) {
                m_title.TranslationX /= 4;
            }
            castBtt.Click += (o, e) =>
            {
                PopupMenu menu = new PopupMenu(ax_Info.ax_info.Context, view);
                menu.MenuInflater.Inflate(Resource.Menu.menu1, menu.Menu);

                for (int i = 0; i < allChromeCasts.Count; i++) {
                    menu.Menu.Add(allChromeCasts[i].FriendlyName);
                }

                menu.MenuItemClick += (s, arg) =>
                {
                    // Toast.MakeText(mainActivity, string.Format("Menu {0} clicked", arg.Item.TitleFormatted), ToastLength.Short).Show();
                    print("DA: " + arg.Item.TitleFormatted);
                    print("Pressed cast:");
                    SetCaster(arg.Item.TitleFormatted.ToString());
                };

                menu.DismissEvent += (s, arg) =>
                {
                    //Toast.MakeText(mainActivity, string.Format("Menu dissmissed"), ToastLength.Short).Show();

                };

                menu.Show();
            };

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
                    if (txt == "Show All") {
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
        private void SubtitleSpinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            currentActiveSubtitle = e.Position - 1;
        }

    }
}