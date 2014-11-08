call init.cmd

rm -r -f ContentShell
"%JDK7%\bin\java.exe" -jar %APKTOOL% decode ContentShell.apk
if NOT "%1"=="1" pause