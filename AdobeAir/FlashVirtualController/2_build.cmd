CALL init.cmd
COPY /Y AndroidManifest.xml FlashVirtualController\AndroidManifest.xml
COPY /Y icons\leanback_icon.png FlashVirtualController\res\drawable
IF EXIST FlashVirtualControllerOld.apk DEL FlashVirtualControllerOld.apk
IF EXIST FlashVirtualController.apk COPY FlashVirtualController.apk FlashVirtualControllerOld.apk
IF EXIST FlashVirtualControllerNew.apk DEL FlashVirtualControllerNew.apk
"%JDK7%\bin\java.exe" -jar %APKTOOL% build FlashVirtualController FlashVirtualController.apk
IF EXIST FlashVirtualController\dist\FlashVirtualController.apk COPY FlashVirtualController\dist\FlashVirtualController.apk FlashVirtualControllerNew.apk
IF NOT "%1"=="1" PAUSE
