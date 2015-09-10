package
{
    import flash.display.MovieClip;
	import tv.ouya.console.api.OuyaController;
	import tv.ouya.sdk.OuyaNativeInterface;
    public class Main extends MovieClip
    {
        public function Main()
        {
			trace("***** Start of InitOuyaPlugin...");
			var imageController: ImageController = new ImageController();
			imageController.x = 1000;
			imageController.y = 0;

			trace("***** Initialize OUYA Extension...");
			var ouyaNativeInterface: OuyaNativeInterface = new OuyaNativeInterface();
			ouyaNativeInterface.OuyaInit();
        }
    }
}
