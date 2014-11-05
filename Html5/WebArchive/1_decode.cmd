call init.cmd

"%JDK7%\bin\java.exe" -jar %APKTOOL% decode ContentShell.apk
if NOT "%1"=="1" pause