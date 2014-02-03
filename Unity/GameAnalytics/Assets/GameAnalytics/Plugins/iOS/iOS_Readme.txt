In order to follow Apple's terms the GameAnalytics Unity SDK will only submit data on iOS if the following steps are completed.

How to enabled advertiser ID for iOS:

1) Uncomment this section at the top of the GA_Settings class (GameAnalytics > Plugins > Framework > Scripts):

	#define IOS_ID

2) Move the GA_UserID.mm file from the GameAnalytics > Plugins > iOS folder to a new folder called Plugins > iOS in your Assets folder. This will cause Unity to include the file in your compiled XCode project when you build for iOS.

3) When you build your project for iOS, XCode will complain about some missing imports in the GA_UserID.mm file. To fix this go to the Build Phases tab of your Unity-iPhone project inside XCode, and fold out the Link Binary With Libraries section. Click the (+) button and add the AdSupport.framework.