CALL init.cmd
rm -r -f %PROJNAME%New
MOVE %PROJNAME% %PROJNAME%New
COPY /Y AndroidManifest.xml %PROJNAME%New\AndroidManifest.xml
COPY /Y "..\InAppPurchases.gmx\extensions\OuyaSDK\AndroidSource\res\drawable\app_icon.png" %PROJNAME%New\res\drawable
COPY /Y "..\InAppPurchases.gmx\extensions\OuyaSDK\AndroidSource\res\drawable\banner.png" %PROJNAME%New\res\drawable
COPY /Y "..\InAppPurchases.gmx\extensions\OuyaSDK\AndroidSource\res\drawable-xhdpi\ouya_icon.png" %PROJNAME%New\res\drawable-xhdpi
COPY /Y apktool.yml %PROJNAME%New\apktool.yml
IF EXIST %PROJNAME%New.apk DEL %PROJNAME%New.apk
"%JDK7%\bin\java.exe" -jar %APKTOOL% build %PROJNAME%New %PROJNAME%New.apk
IF EXIST %PROJNAME%New\dist\%PROJNAME%.apk COPY %PROJNAME%New\dist\%PROJNAME%.apk %PROJNAME%New.apk
IF NOT "%1"=="1" PAUSE
