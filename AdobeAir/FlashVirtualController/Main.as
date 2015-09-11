package
{
    import flash.display.MovieClip;
	import flash.events.Event;
	import tv.ouya.sdk.OuyaNativeInterface;

    public class Main extends MovieClip
    {
		var INTERVAL_MS_INPUT:Number = 10;
		
		var _mOuyaNativeInterface: OuyaNativeInterface;
		var _mVirtualController1: VirtualController;
		var _mVirtualController2: VirtualController;
		var _mVirtualController3: VirtualController;
		var _mVirtualController4: VirtualController;
		
		var _mInputTimer:Number = 0;
		
		private function fl_EnterFrameHandler_1(event:Event):void
		{
			var date:Date = new Date();
			if (_mInputTimer < date.getTime())
			{
				_mInputTimer = date.getTime() + INTERVAL_MS_INPUT;
			
				_mVirtualController1.Update();
				_mVirtualController2.Update();
				_mVirtualController3.Update();
				_mVirtualController4.Update();
				
				_mOuyaNativeInterface.ClearButtonStatesPressedReleased();
			}
		}
		
        public function Main()
        {
			_mOuyaNativeInterface = new OuyaNativeInterface();
			_mOuyaNativeInterface.OuyaInit();
			
			_mVirtualController1 = new VirtualController(this, _mOuyaNativeInterface, 0, 15.65, -75.1);
			_mVirtualController2 = new VirtualController(this, _mOuyaNativeInterface, 1, 1232.55, -75.1);
			_mVirtualController3 = new VirtualController(this, _mOuyaNativeInterface, 2, 15.65, 495.75);
			_mVirtualController4 = new VirtualController(this, _mOuyaNativeInterface, 3, 1232.55, 495.75);
			
			//_mOuyaNativeInterface.LogInfo("***** Add event listener...");			
			addEventListener(Event.ENTER_FRAME, fl_EnterFrameHandler_1);
        }
    }
}
