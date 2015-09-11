package
{
	import flash.display.Bitmap;
    import flash.display.MovieClip;
	import flash.events.Event;
	import tv.ouya.sdk.OuyaNativeInterface;

    public class Main extends MovieClip
    {
		var _mOuyaNativeInterface: OuyaNativeInterface;
		var _mVirtualController1: VirtualController;
		var _mVirtualController2: VirtualController;
		var _mVirtualController3: VirtualController;
		var _mVirtualController4: VirtualController;
		
		private function fl_EnterFrameHandler_1(event:Event):void
		{
			_mVirtualController1.Update();
			_mVirtualController2.Update();
			_mVirtualController3.Update();
			_mVirtualController4.Update();
			_mOuyaNativeInterface.ClearButtonStatesPressedReleased();
		}
		
        public function Main()
        {
			_mOuyaNativeInterface = new OuyaNativeInterface();
			_mOuyaNativeInterface.OuyaInit();
			
			_mVirtualController1 = new VirtualController(this, _mOuyaNativeInterface, 0, 288.9, 93.05);
			_mVirtualController2 = new VirtualController(this, _mOuyaNativeInterface, 1, 1253.55, 93.05);
			_mVirtualController3 = new VirtualController(this, _mOuyaNativeInterface, 2, 288.9, 585.95);
			_mVirtualController4 = new VirtualController(this, _mOuyaNativeInterface, 3, 1253.55, 585.95);
			
			//_mOuyaNativeInterface.LogInfo("***** Add event listener...");			
			addEventListener(Event.ENTER_FRAME, fl_EnterFrameHandler_1);
        }
    }
}
