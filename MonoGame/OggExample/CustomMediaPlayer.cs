using System;
using Android.App;
using Android.Content.Res;
using Android.Media;
using Java.IO;

namespace OggExample
{
    class CustomMediaPlayer : MediaPlayer
    {
        public enum States
        {
            None,
            FadeIn,
            FadeOut,
        }
        public States _State = States.None;
        public DateTime _Timer = DateTime.MinValue;
        public void Update()
        {
            if (_Timer == DateTime.MinValue)
            {
                return;
            }
            switch (_State)
            {
                case States.FadeIn:
                    if (_Timer < DateTime.Now)
                    {
                        SetVolume(1f, 1f);
                        _Timer = DateTime.MinValue;
                    }
                    else
                    {
                        float volume = 1f - (float)(_Timer - DateTime.Now).TotalSeconds;
                        //Log.i(TAG, "Volume: "+volume);
                        SetVolume(volume, volume);
                    }
                    break;
                case States.FadeOut:
                    if (_Timer < DateTime.Now)
                    {
                        if (IsPlaying)
                        {
                            Stop();
                        }
                        SetVolume(0f, 0f);
                        _Timer = DateTime.MinValue;
                    }
                    else
                    {
                        float volume = (float)(_Timer - DateTime.Now).TotalSeconds;
                        //Log.i(TAG, "Volume: "+volume);
                        SetVolume(volume, volume);
                    }
                    break;
                default:
                    break;
            }
        }

        public static void FadeIn(CustomMediaPlayer fadeInSound, String soundFile)
        {
            if (null != fadeInSound)
            {
                if (fadeInSound.IsPlaying)
                {
                    fadeInSound.Stop();
                }
                fadeInSound._State = CustomMediaPlayer.States.FadeIn;
                fadeInSound._Timer = DateTime.Now + TimeSpan.FromSeconds(1);
                fadeInSound.SetVolume(0f, 0f);
                PlaySound(fadeInSound, soundFile);
            }
        }

        public static void FadeOut(CustomMediaPlayer fadeOutSound)
        {
            if (null != fadeOutSound)
            {
                fadeOutSound._State = CustomMediaPlayer.States.FadeOut;
                fadeOutSound._Timer = DateTime.Now + TimeSpan.FromSeconds(1);
                fadeOutSound.SetVolume(1f, 1f);
            }
        }

        public static void CrossFade(CustomMediaPlayer fadeOutSound, CustomMediaPlayer fadeInSound, String soundFile)
        {
            if (null != fadeOutSound)
            {
                fadeOutSound._State = CustomMediaPlayer.States.FadeOut;
                fadeOutSound._Timer = DateTime.Now + TimeSpan.FromSeconds(1);
                fadeOutSound.SetVolume(1f, 1f);
            }

            if (null != fadeInSound)
            {
                fadeInSound._State = CustomMediaPlayer.States.FadeIn;
                fadeInSound._Timer = DateTime.Now + TimeSpan.FromSeconds(1);
                fadeInSound.SetVolume(0f, 0f);
                if (!fadeInSound.IsPlaying)
                {
                    PlaySound(fadeInSound, soundFile);
                }
            }
        }

        public static void PlaySound(CustomMediaPlayer mp, String asset)
        {
            if (null == mp)
            {
                return;
            }

            AssetManager assetManager = Game1.Activity.ApplicationContext.Assets;

            AssetFileDescriptor afd = null;
            try
            {
                afd = assetManager.OpenFd(asset);
            }
            catch (IOException e)
            {
                // TODO Auto-generated catch block
                e.PrintStackTrace();
            }

            if (null == afd)
            {
                return;
            }

            mp.Reset();

            mp.SetDataSource(afd.FileDescriptor,
                afd.StartOffset,
                afd.Length);

            mp.Prepare();

            afd.Close();

            mp.Start();
        }
    }
}