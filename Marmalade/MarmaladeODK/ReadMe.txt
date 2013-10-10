
======================================================================================
bluescrn's Marmalade wrapper for the Ouya ODK
(v0.01 - Only supports controllers for now, not fully tested, only just got it working!)
======================================================================================


To use it
--------------------------------------------------------------------------------------

- Put a copy of ouya-sdk.jar, from the ODD in ODK\libs
- Make sure you're set up to build Marmalade EDK extensions for Android (see the Marmalade docs)
- Build ODK_android.mkb and ODK_android_java.mkb (Marmalade context menu items)
- Add the ODK to your project's .mkb file, e.g.:

subprojects
{
   path/to/ODK
}

- Use the ODKAvailable() function to check the extension is present/loaded
- Use one of the 'selectController' functions to make a controller active for button queries
- Call the getButton/getAxisValue functions as required!

if ( ODKAvailable() )
{
   if ( OuyaController_selectControllerByPlayer(0) )
   {				
	bOButton = OuyaController_getButton( OuyaController_BUTTON_O );
   }
}

See h/ODK.h for list of wrapped functions and axis/button defines.

Have fun! :)



Implementation notes
--------------------------------------------------------------------------------------

Getting the EDK extension class to work as a custom activity was a bit of a pain, 
but essentially there's not much to it.

The extension was created from ODK.s4e, as a regular extension, then modified to be an Activity.

source/android/ODK.java
- added the package name com.ODK, derived the class from LoaderActivity
- And all the simple wrappers for the OuyaController functions

ODK.mkf
- added android-custom-activity ='com.ODK.ODK', so the activity is created

source/android/ODK_platform.cpp
- Messed with the JNI code in here, to get the existing instance of com.ODK.ODK from it's static m_Activity field
  (instead of the default extension behaviour of creating an instance of a non-activity class)

(Also note that the ouya-sdk.jar is added to ODK.mkf and ODK_android_java.mkb)

		