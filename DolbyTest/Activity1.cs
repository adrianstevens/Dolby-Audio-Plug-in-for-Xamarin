using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
//using Com.Dolby.Dap;
using Android.Media;
using Com.Dolby.Dap;

namespace DolbyTest
{
    [Activity(Label = "DolbyAPI Test", MainLauncher = true, Icon = "@drawable/icon")]
    public class Activity1 : Activity, MediaPlayer.IOnCompletionListener, IOnDolbyAudioProcessingEventListener
    {
        MediaPlayer mPlayer;
        DolbyAudioProcessing mDolbyAudioProcessing;

        //controls
        Button btnPlay;
        Button btnDolby;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            //super simple
            SetContentView(Resource.Layout.main);

            InitAudio();
            InitControls();
        }

        void InitAudio()
        {
            if (mDolbyAudioProcessing != null)
                return;

            mDolbyAudioProcessing = DolbyAudioProcessing.GetDolbyAudioProcessing(this, DolbyAudioProcessing.PROFILE.Music, this);

            if (mDolbyAudioProcessing == null)
            {
                Toast.MakeText(this, "Dolby Audio Processing failed", ToastLength.Short).Show();
                return;
            }
        }

        void InitControls()
        {
            btnPlay = FindViewById<Button>(Resource.Id.btnPlayStop);
            btnPlay.Click += btnPlay_Click;

            btnDolby = FindViewById<Button>(Resource.Id.btnDolby);
            btnDolby.Click += btnDolby_Click;
        }

        void btnPlay_Click(object sender, EventArgs e)
        {
            if (mPlayer == null)
            {
                try
                {
                    mPlayer = MediaPlayer.Create(this, Resource.Raw.sage);
                    if (mPlayer != null)
                    {
                        mPlayer.Start();

                        btnPlay.Text = "Stop";
                    }
                }
                catch
                { 
                
                }
            }
            else
            {
                mPlayer.Stop();
                mPlayer.Release();
                mPlayer = null;

                btnPlay.Text = "Play";
            }
        }

        void btnDolby_Click(object sender, EventArgs e)
        {
            if (mDolbyAudioProcessing != null)
                mDolbyAudioProcessing.Enabled = !mDolbyAudioProcessing.Enabled;
            UpdateDolbyBtnText();
        }

        public void OnCompletion (MediaPlayer mp)
        {
            //cleanup
            mPlayer.Release();
            mPlayer = null;
        }

        void UpdateDolbyBtnText()
        {
            if (mDolbyAudioProcessing.Enabled == true)
                btnDolby.Text = "Disable Dolby";
            else
                btnDolby.Text = "Enable Dolby";
        }

        public void OnDolbyAudioProcessingClientConnected()
        {
            if (mDolbyAudioProcessing != null)
            {
                UpdateDolbyBtnText();
            }
        }

        public void OnDolbyAudioProcessingClientDisconnected()
        {

        }

        public void OnDolbyAudioProcessingEnabled(bool p0)
        {

        }

        public void OnDolbyAudioProcessingProfileSelected(DolbyAudioProcessing.PROFILE p0)
        {

        }
    }
}

