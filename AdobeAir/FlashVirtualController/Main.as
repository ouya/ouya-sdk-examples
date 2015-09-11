package
{
	import flash.display.Bitmap;
    import flash.display.MovieClip;
	import flash.events.Event;
	import tv.ouya.console.api.OuyaController;
	import tv.ouya.sdk.OuyaNativeInterface;

    public class Main extends MovieClip
    {
		var _mOuyaNativeInterface: OuyaNativeInterface;
		
		var DEADZONE = 0.25;
		
		var _mController:Bitmap;
		var _mButtonO:Bitmap;
		var _mButtonU:Bitmap;
		var _mButtonY:Bitmap;
		var _mButtonA:Bitmap;
		var _mButtonL1:Bitmap;
		var _mButtonL2:Bitmap;
		var _mButtonL3:Bitmap;
		var _mButtonR1:Bitmap;
		var _mButtonR2:Bitmap;
		var _mButtonR3:Bitmap;
		var _mButtonLS:Bitmap;
		var _mButtonRS:Bitmap;
		var _mButtonDpadDown:Bitmap;
		var _mButtonDpadLeft:Bitmap;
		var _mButtonDpadRight:Bitmap;
		var _mButtonDpadUp:Bitmap;
		var _mButtonMenu:Bitmap;
		
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
		
		private function fl_EnterFrameHandler_1(event:Event):void
		{
			UpdateVisibility(_mButtonO, _mOuyaNativeInterface.GetAnyButton(OuyaController.BUTTON_O));
			UpdateVisibility(_mButtonU, _mOuyaNativeInterface.GetAnyButton(OuyaController.BUTTON_U));
			UpdateVisibility(_mButtonY, _mOuyaNativeInterface.GetAnyButton(OuyaController.BUTTON_Y));
			UpdateVisibility(_mButtonA, _mOuyaNativeInterface.GetAnyButton(OuyaController.BUTTON_A));
			
			UpdateVisibility(_mButtonL1, _mOuyaNativeInterface.GetAnyButton(OuyaController.BUTTON_L1));
			
			UpdateVisibility(_mButtonL3, _mOuyaNativeInterface.GetAnyButton(OuyaController.BUTTON_L3));
			UpdateVisibility(_mButtonLS, !_mOuyaNativeInterface.GetAnyButton(OuyaController.BUTTON_L3));
			
			UpdateVisibility(_mButtonR1, _mOuyaNativeInterface.GetAnyButton(OuyaController.BUTTON_R1));			
			
			UpdateVisibility(_mButtonR3, _mOuyaNativeInterface.GetAnyButton(OuyaController.BUTTON_R3));
			UpdateVisibility(_mButtonRS, !_mOuyaNativeInterface.GetAnyButton(OuyaController.BUTTON_R3));
			
			UpdateVisibility(_mButtonDpadDown, _mOuyaNativeInterface.GetAnyButton(OuyaController.BUTTON_DPAD_DOWN));
			UpdateVisibility(_mButtonDpadLeft, _mOuyaNativeInterface.GetAnyButton(OuyaController.BUTTON_DPAD_LEFT));
			UpdateVisibility(_mButtonDpadRight, _mOuyaNativeInterface.GetAnyButton(OuyaController.BUTTON_DPAD_RIGHT));
			UpdateVisibility(_mButtonDpadUp, _mOuyaNativeInterface.GetAnyButton(OuyaController.BUTTON_DPAD_UP));
			
			UpdateVisibility(_mButtonMenu, _mOuyaNativeInterface.GetAnyButtonUp(OuyaController.BUTTON_MENU));
			
			var lx = _mOuyaNativeInterface.GetAxis(0, OuyaController.AXIS_LS_X);
			var ly = _mOuyaNativeInterface.GetAxis(0, OuyaController.AXIS_LS_Y);
			var rx = _mOuyaNativeInterface.GetAxis(0, OuyaController.AXIS_RS_X);
			var ry = _mOuyaNativeInterface.GetAxis(0, OuyaController.AXIS_RS_Y);
			var l2 = _mOuyaNativeInterface.GetAxis(0, OuyaController.AXIS_L2);
			var r2 = _mOuyaNativeInterface.GetAxis(0, OuyaController.AXIS_R2);
			
			UpdateVisibility(_mButtonL2, l2 > DEADZONE);
			UpdateVisibility(_mButtonR2, r2 > DEADZONE);
			
			_mOuyaNativeInterface.LogInfo("***** LX:"+lx+" LY:"+ly+" RX:"+rx+" RY:"+ry+" L2:"+l2+" R2:"+r2);	
			
			_mOuyaNativeInterface.ClearButtonStatesPressedReleased();
		}
		
		private function AddBitmap(bitmap : Bitmap) : Bitmap
		{
			bitmap.x = 121.80;
			bitmap.y = 84.25;
			addChild(bitmap);
			return bitmap;
		}
		
        public function Main()
        {
			_mController = AddBitmap(new Bitmap(new ImageController()));
			_mButtonO = AddBitmap(new Bitmap(new ImageO()));
			_mButtonU = AddBitmap(new Bitmap(new ImageU()));
			_mButtonY = AddBitmap(new Bitmap(new ImageY()));
			_mButtonA = AddBitmap(new Bitmap(new ImageA()));
			
			_mButtonL1 = AddBitmap(new Bitmap(new ImageL1()));
			_mButtonL2 = AddBitmap(new Bitmap(new ImageL2()));
			_mButtonL3 = AddBitmap(new Bitmap(new ImageL3()));
			_mButtonR1 = AddBitmap(new Bitmap(new ImageR1()));
			_mButtonR2 = AddBitmap(new Bitmap(new ImageR2()));
			_mButtonR3 = AddBitmap(new Bitmap(new ImageR3()));
			_mButtonLS = AddBitmap(new Bitmap(new ImageLS()));
			_mButtonRS = AddBitmap(new Bitmap(new ImageRS()));
			_mButtonDpadDown = AddBitmap(new Bitmap(new ImageDpadDown()));
			_mButtonDpadLeft = AddBitmap(new Bitmap(new ImageDpadLeft()));
			_mButtonDpadRight = AddBitmap(new Bitmap(new ImageDpadRight()));
			_mButtonDpadUp = AddBitmap(new Bitmap(new ImageDpadUp()));
			_mButtonMenu = AddBitmap(new Bitmap(new ImageMenu()));
			
			_mOuyaNativeInterface = new OuyaNativeInterface();
			_mOuyaNativeInterface.OuyaInit();
			
			//_mOuyaNativeInterface.LogInfo("***** Add event listener...");			
			addEventListener(Event.ENTER_FRAME, fl_EnterFrameHandler_1);
        }
    }
}
