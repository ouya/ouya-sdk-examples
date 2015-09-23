package
{
    import flash.display.MovieClip;
	import flash.events.Event;
	import flash.events.StatusEvent;
	import tv.ouya.console.api.OuyaController;
	import tv.ouya.sdk.OuyaNativeInterface;

    public class Main extends MovieClip
    {
		static var DEVLEOPER_ID:String = "310a8f51-4d6e-4ae5-bda0-b93878e5f5d0";
		
		var INTERVAL_MS_INPUT:Number = 10;
		
		var _mOuyaNativeInterface: OuyaNativeInterface;
		
		var _mInputTimer:Number = 0;
		
		var _mInitialized:Boolean = false;
		
		private function fl_EnterFrameHandler_1(event:Event):void
		{
			var date:Date = new Date();
			if (_mInputTimer < date.getTime())
			{
				_mInputTimer = date.getTime() + INTERVAL_MS_INPUT;
			}
			
			if (!_mInitialized &&
				_mOuyaNativeInterface.IsInitialized()) {
				var deviceName:String = _mOuyaNativeInterface.GetDeviceHardwareName();
				_mOuyaNativeInterface.LogInfo("HardwareDevice: "+_mOuyaNativeInterface.GetDeviceHardwareName());
				LblHello.text = "Running on device: `"+deviceName+"`";
				
				_mOuyaNativeInterface.LogInfo("BUTTON_O Name: "+_mOuyaNativeInterface.GetButtonName(OuyaController.BUTTON_O));
				_mOuyaNativeInterface.LogInfo("BUTTON_U Name: "+_mOuyaNativeInterface.GetButtonName(OuyaController.BUTTON_U));
				_mOuyaNativeInterface.LogInfo("BUTTON_Y Name: "+_mOuyaNativeInterface.GetButtonName(OuyaController.BUTTON_Y));
				_mOuyaNativeInterface.LogInfo("BUTTON_A Name: "+_mOuyaNativeInterface.GetButtonName(OuyaController.BUTTON_A));
					
				_mInitialized = true;
			}
		}
		
		private function Axis(jsonData:String):void
		{
			var json:Object = JSON.parse(jsonData);
			var playerNum:int = json.playerNum;
			var axis:int = json.axis;
			var val:Number = json.value;
			//_mOuyaNativeInterface.LogInfo("Axis: playerNum:"+playerNum+" axis:"+axis+" value:"+val);
		}
		
		private function ButtonDown(jsonData:String):void
		{
			var json:Object = JSON.parse(jsonData);
			var playerNum:int = json.playerNum;
			var button:int = json.button;
			//_mOuyaNativeInterface.LogInfo("ButtonDown: playerNum:"+playerNum+" button:"+button);
		}
		
		private function ButtonUp(jsonData:String):void
		{
			var json:Object = JSON.parse(jsonData);
			var playerNum:int = json.playerNum;
			var button:int = json.button;
			//_mOuyaNativeInterface.LogInfo("ButtonUp: playerNum:"+playerNum+" button:"+button);
		}
		
		private function onStatusEvent( _event : StatusEvent ) : void 
		{
			//_mOuyaNativeInterface.LogInfo("Code: " + _event.code );
			//_mOuyaNativeInterface.LogInfo("Level: " + _event.level );
			
			if (_event.code == "Axis") {
				Axis(_event.level);
			} else if (_event.code == "ButtonDown") {
				ButtonDown(_event.level);
			} else if (_event.code == "ButtonUp") {
				ButtonUp(_event.level);
			}
		}
		
        public function Main()
        {
			_mOuyaNativeInterface = new OuyaNativeInterface();
			_mOuyaNativeInterface.OuyaInit(DEVLEOPER_ID);
			
			// put the label on top of the sprites
			setChildIndex(LblHello, numChildren-1);
			
			//_mOuyaNativeInterface.LogInfo("***** Add event listener...");			
			addEventListener(Event.ENTER_FRAME, fl_EnterFrameHandler_1);
			
			_mOuyaNativeInterface.GetExtensionContext().addEventListener( StatusEvent.STATUS, onStatusEvent );
        }
    }
}
