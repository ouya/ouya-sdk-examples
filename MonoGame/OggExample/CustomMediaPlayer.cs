using System;
using Android.Media;

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
    }
}