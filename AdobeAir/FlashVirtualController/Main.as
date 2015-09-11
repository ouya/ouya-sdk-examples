package
{
    import flash.display.MovieClip;
	import flash.events.Event;
	import tv.ouya.console.api.OuyaController;
	import tv.ouya.sdk.OuyaNativeInterface;

    public class Main extends MovieClip
    {
		var _mOuyaNativeInterface: OuyaNativeInterface;
		
		private function fl_EnterFrameHandler_1(event:Event):void
		{
			var button:int = OuyaController.BUTTON_O;
			var pressed:Boolean = _mOuyaNativeInterface.GetAnyButton(button);
			if (pressed) {
				_mOuyaNativeInterface.LogInfo("*******Entered frame detected O button");
			}
		}
		
        public function Main()
        {
			var imageController: ImageController = new ImageController();
			imageController.x = 1000;
			imageController.y = 0;
			
			_mOuyaNativeInterface = new OuyaNativeInterface();
			_mOuyaNativeInterface.OuyaInit();
			
			//_mOuyaNativeInterface.LogInfo("***** Add event listener...");			
			addEventListener(Event.ENTER_FRAME, fl_EnterFrameHandler_1);
        }
    }
}
