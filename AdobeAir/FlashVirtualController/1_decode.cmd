call init.cmd

rm -r -f FlashVirtualController
"%JDK7%\bin\java.exe" -jar %APKTOOL% decode FlashVirtualController.apk
if NOT "%1"=="1" pause
