How to enabled android ID for Android:

1) Move the AndroidManifest.xml and GA_Android.jar files from the GameAnalytics > Plugins > Android folder to a new folder called Plugins > Android in your Assets folder. This will cause Unity to include the file in your compiled project when you build for Android.

2) Edit the package="com.yourcompany.yourgame" in the AndroidManifest.xml to your own Android package name.

3) In the GA_Settings class file (found at GameAnalytics > Plugins > Framework > Scripts > GA_Settings) uncomment the following code at the top:

		//#define ANDROID_ID