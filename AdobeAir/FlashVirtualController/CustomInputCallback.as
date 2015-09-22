package
{
	import tv.ouya.sdk.OuyaInputCallback;
	import tv.ouya.sdk.OuyaNativeInterface;

    public class CustomInputCallback extends OuyaInputCallback
    {
		var _mOuyaNativeInterface: OuyaNativeInterface;
		
		public function CustomInputCallback(ane:OuyaNativeInterface)
		{
			_mOuyaNativeInterface = ane;
		}
		
		public override function Axis(playerNum:int, axis:int, value:Number) : void
		{
			_mOuyaNativeInterface.LogInfo("Axis playerNum:"+playerNum+" axis:"+axis+" value:"+value);
		}
		
		public override function ButtonDown(playerNum:int, button:int) : void
		{
			_mOuyaNativeInterface.LogInfo("ButtonDown playerNum:"+playerNum+" button:"+button);
		}
		
		public override function ButtonUp(playerNum:int, button:int) : void
		{
			_mOuyaNativeInterface.LogInfo("ButtonUp playerNum:"+playerNum+" button:"+button);
		}
    }
}
