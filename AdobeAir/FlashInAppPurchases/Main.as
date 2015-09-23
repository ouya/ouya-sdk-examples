package
{
	import flash.display.Bitmap;
	import flash.display.BitmapData;
    import flash.display.MovieClip;
	import flash.display.PixelSnapping;
	import flash.events.Event;
	import flash.events.StatusEvent;
	import flash.geom.Matrix;
	import tv.ouya.console.api.OuyaController;
	import tv.ouya.sdk.OuyaNativeInterface;

    public class Main extends MovieClip
    {
		static var DEVLEOPER_ID:String = "310a8f51-4d6e-4ae5-bda0-b93878e5f5d0";
		
		var INTERVAL_MS_INPUT:Number = 10;
		
		var _mOuyaNativeInterface: OuyaNativeInterface;
		
		var _mInputTimer:Number = 0;
		
		var _mInitialized:Boolean = false;
		
		var _mBtnRequestProductList:Bitmap;
		var _mBtnRequestPurchase:Bitmap;
		var _mBtnRequestReceipts:Bitmap;
		var _mBtnRequestGamerInfo:Bitmap;
		var _mBtnExit:Bitmap;
		var _mBtnPause:Bitmap;
		
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
			_mOuyaNativeInterface.LogInfo("Axis: playerNum:"+playerNum+" axis:"+axis+" value:"+val);
		}
		
		private function ButtonDown(jsonData:String):void
		{
			var json:Object = JSON.parse(jsonData);
			var playerNum:int = json.playerNum;
			var button:int = json.button;
			_mOuyaNativeInterface.LogInfo("ButtonDown: playerNum:"+playerNum+" button:"+button);
		}
		
		private function ButtonUp(jsonData:String):void
		{
			var json:Object = JSON.parse(jsonData);
			var playerNum:int = json.playerNum;
			var button:int = json.button;
			_mOuyaNativeInterface.LogInfo("ButtonUp: playerNum:"+playerNum+" button:"+button);
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
		
		private function AddBitmap(textField : TextField, bitmap : Bitmap) : Bitmap
		{
			var matrix:Matrix = new Matrix();
			matrix.scale(new Number(textField.width)/bitmap.width, new Number(textField.height)/bitmap.height);

			var resizedBitmapData:BitmapData = new BitmapData(textField.width, textField.height, true, 0x000000);
			resizedBitmapData.draw(bitmap, matrix, null, null, null, true);

			var resizedBitmap = new Bitmap(resizedBitmapData, PixelSnapping.NEVER, true);
			
			resizedBitmap.x = textField.x;
			resizedBitmap.y = textField.y;
			
			addChild(resizedBitmap);
			return resizedBitmap;
		}
		
		private function CreateButton(textField : TextField) : Bitmap
		{
			AddBitmap(textField, new Bitmap(new ImageButtonInactive()));
			var result:Bitmap = AddBitmap(textField, new Bitmap(new ImageButtonActive()));
			
			// put the label on top of the sprites
			setChildIndex(textField, numChildren-1);
			
			return result;
		}
		
		private function UpdateVisibility(bitmap:Bitmap, show:Boolean) : void
		{
			if (show)
			{
				bitmap.alpha = 1;
			}
			else
			{
				bitmap.alpha = 0;
			}
		}
		
        public function Main()
        {
			_mOuyaNativeInterface = new OuyaNativeInterface();
			_mOuyaNativeInterface.OuyaInit(DEVLEOPER_ID);
			
			_mBtnRequestProductList = CreateButton(BtnRequestProductList);
			_mBtnRequestPurchase = CreateButton(BtnRequestPurchase);
			_mBtnRequestReceipts = CreateButton(BtnRequestReceipts);
			_mBtnRequestGamerInfo = CreateButton(BtnRequestGamerInfo);
			_mBtnExit = CreateButton(BtnExit);
			_mBtnPause = CreateButton(BtnPause);
			
			UpdateVisibility(_mBtnRequestProductList, true);
			UpdateVisibility(_mBtnRequestPurchase, false);
			UpdateVisibility(_mBtnRequestReceipts, false);
			UpdateVisibility(_mBtnRequestGamerInfo, false);
			UpdateVisibility(_mBtnExit, false);
			UpdateVisibility(_mBtnPause, false);
			
			//_mOuyaNativeInterface.LogInfo("***** Add event listener...");			
			addEventListener(Event.ENTER_FRAME, fl_EnterFrameHandler_1);
			
			_mOuyaNativeInterface.GetExtensionContext().addEventListener( StatusEvent.STATUS, onStatusEvent );
        }
    }
}
