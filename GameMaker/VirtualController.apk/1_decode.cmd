call init.cmd

rm -r -f %PROJNAME%
"%JDK7%\bin\java.exe" -jar %APKTOOL% decode %PROJNAME%.apk
if NOT "%1"=="1" pause
