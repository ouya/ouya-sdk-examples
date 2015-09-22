package
{
	import flash.display.Bitmap;
	import flash.display.BitmapData;
    import flash.display.MovieClip;
	import flash.display.PixelSnapping;
	import flash.geom.Matrix;
	import tv.ouya.console.api.OuyaController;
	import tv.ouya.sdk.OuyaNativeInterface;

    public class VirtualController extends MovieClip
    {
		var _mMain: Main;
		var _mOuyaNativeInterface: OuyaNativeInterface;
		
		var DEADZONE:Number = 0.25;
		
		var AXIS_SCALAR:Number = 4;
		
		var _mPlayerNum:int = 0;
		
		var _mX:Number = 0;
		var _mY:Number = 0;
		
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
		
		var _mAxisLsX:Number = 0;
		var _mAxisLsY:Number = 0;
		var _mAxisRsX:Number = 0;
		var _mAxisRsY:Number = 0;
		var _mAxisL2:Number = 0;
		var _mAxisR2:Number = 0;
		
		var _mToggleButtonO:Boolean = false;
		var _mToggleButtonU:Boolean = false;
		var _mToggleButtonY:Boolean = false;
		var _mToggleButtonA:Boolean = false;
		var _mToggleButtonL1:Boolean = false;
		var _mToggleButtonL3:Boolean = false;
		var _mToggleButtonR1:Boolean = false;
		var _mToggleButtonR3:Boolean = false;
		var _mToggleButtonDpadDown:Boolean = false;
		var _mToggleButtonDpadLeft:Boolean = false;
		var _mToggleButtonDpadRight:Boolean = false;
		var _mToggleButtonDpadUp:Boolean = false;
		var _mToggleButtonMenu:Boolean = false;
		
		var _mMenuTimer:Number = 0;
		
		private function AddBitmap(bitmap : Bitmap) : Bitmap
		{
			var scale:Number = 2;
			var matrix:Matrix = new Matrix();
			matrix.scale(scale, scale);

			var resizedBitmapData:BitmapData = new BitmapData(bitmap.width * scale, bitmap.height * scale, true, 0x000000);
			resizedBitmapData.draw(bitmap, matrix, null, null, null, true);

			var resizedBitmap = new Bitmap(resizedBitmapData, PixelSnapping.NEVER, true);
			
			resizedBitmap.x = _mX;
			resizedBitmap.y = _mY;
			
			_mMain.addChild(resizedBitmap);
			return resizedBitmap;
		}
		
		public function VirtualController(main:Main, ane:OuyaNativeInterface, playerNum:int, x:Number, y:Number)
        {
			_mMain = main;
			_mOuyaNativeInterface = ane;
			_mPlayerNum = playerNum;
			_mX = x;
			_mY = y;
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
		
		private function MoveBitmap(bitmap : Bitmap, offsetX : Number, offsetY : Number) : void
		{
			bitmap.x = _mX + offsetX;
			bitmap.y = _mY + offsetY;
		}
		
		public function Axis(axis:int,val:Number) {
			//_mOuyaNativeInterface.LogInfo("Axis: axis:"+axis+" val:"+val);			
			if (axis == OuyaController.AXIS_LS_X) {
				_mAxisLsX = val;
			} else if (axis == OuyaController.AXIS_LS_Y) {
				_mAxisLsY = val;
			} else if (axis == OuyaController.AXIS_RS_X) {
				_mAxisRsX = val;
			} else if (axis == OuyaController.AXIS_RS_Y) {
				_mAxisRsY = val;
			} else if (axis == OuyaController.AXIS_L2) {
				_mAxisL2 = val;
			} else if (axis == OuyaController.AXIS_R2) {
				_mAxisR2 = val;
			}
		}
		
		public function ButtonDown(button:int) {
			//_mOuyaNativeInterface.LogInfo("ButtonDown: button:"+button);
			if (button == OuyaController.BUTTON_O) {
				_mToggleButtonO = true;
			} else if (button == OuyaController.BUTTON_U) {
				_mToggleButtonU = true;
			} else if (button == OuyaController.BUTTON_Y) {
				_mToggleButtonY = true;
			} else if (button == OuyaController.BUTTON_A) {
				_mToggleButtonA = true;
			} else if (button == OuyaController.BUTTON_L1) {
				_mToggleButtonL1 = true;
			} else if (button == OuyaController.BUTTON_L3) {
				_mToggleButtonL3 = true;
			} else if (button == OuyaController.BUTTON_R1) {
				_mToggleButtonR1 = true;
			} else if (button == OuyaController.BUTTON_R3) {
				_mToggleButtonR3 = true;
			} else if (button == OuyaController.BUTTON_DPAD_DOWN) {
				_mToggleButtonDpadDown = true;
			} else if (button == OuyaController.BUTTON_DPAD_LEFT) {
				_mToggleButtonDpadLeft = true;
			} else if (button == OuyaController.BUTTON_DPAD_RIGHT) {
				_mToggleButtonDpadRight = true;
			} else if (button == OuyaController.BUTTON_DPAD_UP) {
				_mToggleButtonDpadUp = true;
			} else if (button == OuyaController.BUTTON_MENU) {
				_mToggleButtonMenu = true;
				var date:Date = new Date();
				_mMenuTimer = date.getTime() + 1000;
			}
		}
		
		public function ButtonUp(button:int) {
			//_mOuyaNativeInterface.LogInfo("ButtonUp: button:"+button);
			if (button == OuyaController.BUTTON_O) {
				_mToggleButtonO = false;
			} else if (button == OuyaController.BUTTON_U) {
				_mToggleButtonU = false;
			} else if (button == OuyaController.BUTTON_Y) {
				_mToggleButtonY = false;
			} else if (button == OuyaController.BUTTON_A) {
				_mToggleButtonA = false;
			} else if (button == OuyaController.BUTTON_L1) {
				_mToggleButtonL1 = false;
			} else if (button == OuyaController.BUTTON_L3) {
				_mToggleButtonL3 = false;
			} else if (button == OuyaController.BUTTON_R1) {
				_mToggleButtonR1 = false;
			} else if (button == OuyaController.BUTTON_R3) {
				_mToggleButtonR3 = false;
			} else if (button == OuyaController.BUTTON_DPAD_DOWN) {
				_mToggleButtonDpadDown = false;
			} else if (button == OuyaController.BUTTON_DPAD_LEFT) {
				_mToggleButtonDpadLeft = false;
			} else if (button == OuyaController.BUTTON_DPAD_RIGHT) {
				_mToggleButtonDpadRight = false;
			} else if (button == OuyaController.BUTTON_DPAD_UP) {
				_mToggleButtonDpadUp = false;
			}
		}
		
		public function Update():void
		{
            UpdateVisibility(_mButtonO, _mToggleButtonO);
            UpdateVisibility(_mButtonU, _mToggleButtonU);
            UpdateVisibility(_mButtonY, _mToggleButtonY);
            UpdateVisibility(_mButtonA, _mToggleButtonA);
            
            UpdateVisibility(_mButtonL1, _mToggleButtonL1);
            
            UpdateVisibility(_mButtonL3, _mToggleButtonL3);
            UpdateVisibility(_mButtonLS, !_mToggleButtonL3);
            
            UpdateVisibility(_mButtonR1, _mToggleButtonR1);
            
            UpdateVisibility(_mButtonR3, _mToggleButtonR3);
            UpdateVisibility(_mButtonRS, !_mToggleButtonR3);
            
            UpdateVisibility(_mButtonDpadDown, _mToggleButtonDpadDown);
            UpdateVisibility(_mButtonDpadLeft, _mToggleButtonDpadLeft);
            UpdateVisibility(_mButtonDpadRight, _mToggleButtonDpadRight);
            UpdateVisibility(_mButtonDpadUp, _mToggleButtonDpadUp);

			var date:Date = new Date();
			UpdateVisibility(_mButtonMenu, date.getTime() < _mMenuTimer);
			
			var lsX = _mAxisLsX;
			var lsY = _mAxisLsY;
			var rsX = _mAxisRsX;
			var rsY = _mAxisRsY;
			var l2 = _mAxisL2;
			var r2 = _mAxisR2;
			
			UpdateVisibility(_mButtonL2, l2 > DEADZONE);
			UpdateVisibility(_mButtonR2, r2 > DEADZONE);
			
			//rotate input by N degrees to match image
			var degrees:Number = 135;
			var radians:Number = degrees / 180.0 * 3.14;
			var cos:Number = Math.cos(radians);
			var sin:Number = Math.sin(radians);
			
			MoveBitmap(_mButtonL3, AXIS_SCALAR * (lsX * cos - lsY * sin), AXIS_SCALAR * (lsX * sin + lsY * cos));
			MoveBitmap(_mButtonLS, AXIS_SCALAR * (lsX * cos - lsY * sin), AXIS_SCALAR * (lsX * sin + lsY * cos));
			
			MoveBitmap(_mButtonR3, AXIS_SCALAR * (rsX * cos - rsY * sin), AXIS_SCALAR * (rsX * sin + rsY * cos));
			MoveBitmap(_mButtonRS, AXIS_SCALAR * (rsX * cos - rsY * sin), AXIS_SCALAR * (rsX * sin + rsY * cos));
			
			//_mOuyaNativeInterface.LogInfo("***** LX:"+lx+" LY:"+ly+" RX:"+rx+" RY:"+ry+" L2:"+l2+" R2:"+r2);	
		}
    }
}
