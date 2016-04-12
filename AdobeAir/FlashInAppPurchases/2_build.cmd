CALL init.cmd
rm -r -f %PROJNAME%New
MOVE %PROJNAME% %PROJNAME%New
COPY /Y AndroidManifest.xml %PROJNAME%New\AndroidManifest.xml
COPY /Y icons\leanback_icon.png %PROJNAME%New\res\drawable
IF EXIST %PROJNAME%New.apk DEL %PROJNAME%New.apk
"%JDK7%\bin\java.exe" -jar %APKTOOL% build %PROJNAME%New %PROJNAME%New.apk
IF EXIST %PROJNAME%New\dist\%PROJNAME%.apk COPY %PROJNAME%New\dist\%PROJNAME%.apk %PROJNAME%New.apk
IF NOT "%1"=="1" PAUSE
