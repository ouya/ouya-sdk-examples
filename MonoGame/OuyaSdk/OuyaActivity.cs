using Android.OS;
using Android.Views;

namespace OuyaSdk
{
    public class OuyaActivity : Microsoft.Xna.Framework.AndroidGameActivity
    {
        private const string TAG = "OuyaActivity";

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            OuyaInputMapper.init(this.Handle);
        }

        protected override void OnDestroy()
        {
            OuyaInputMapper.shutdown(this.Handle);
            base.OnDestroy();
        }

        public override bool DispatchGenericMotionEvent(MotionEvent motionEvent)
        {
            if (OuyaInputMapper.shouldHandleInputEvent(motionEvent.Handle))
            {
                return OuyaInputMapper.DispatchGenericMotionEvent(this.Handle, motionEvent);
            }
            else
            {
                return base.DispatchGenericMotionEvent(motionEvent);
            }
        }

        public override bool DispatchKeyEvent(KeyEvent keyEvent)
        {
            if (OuyaInputMapper.shouldHandleInputEvent(keyEvent.Handle))
            {
                return OuyaInputMapper.DispatchKeyEvent(this.Handle, keyEvent);
            }
            else
            {
                if (keyEvent.Action == KeyEventActions.Down)
                {
                    return OnKeyDown(keyEvent.KeyCode, keyEvent);
                }
                else if (keyEvent.Action == KeyEventActions.Up)
                {
                    return OnKeyUp(keyEvent.KeyCode, keyEvent);
                }
                else
                {
                    return base.DispatchKeyEvent(keyEvent);
                }
            }
        }
    }
}

