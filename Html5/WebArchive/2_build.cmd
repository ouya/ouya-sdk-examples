call init.cmd
"%JDK7%\bin\java.exe" -jar %APKTOOL% build ContentShell
del ContentShellNew.apk
move ContentShell\dist\ContentShell.apk ContentShellNew.apk
if NOT "%1"=="1" pause