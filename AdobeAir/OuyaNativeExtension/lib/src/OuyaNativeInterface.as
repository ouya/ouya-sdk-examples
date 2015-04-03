package
{
	import flash.external.ExtensionContext;
	
	public class OuyaNativeInterface
	{
		private var context:ExtensionContext;
		
		public function OuyaNativeInterface()
		{
			if (!context) {
				context = ExtensionContext.createExtensionContext("tv.ouya.sdk.ouyanativecontext", null);
			}
		}
		
		public function OuyaToggleInput(toggle:Boolean):void {
			context.call("ouyaToggleInput", toggle);
		}
	}
}