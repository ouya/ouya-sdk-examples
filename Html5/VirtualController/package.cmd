copy /Y customization\AndroidManifest.xml ..\WebArchive\ContentShell\AndroidManifest.xml
copy /Y customization\res\drawable\app_icon.png ..\WebArchive\ContentShell\res\drawable\app_icon.png
copy /Y customization\res\drawable\splash_screen.png ..\WebArchive\ContentShell\res\drawable\splash_screen.png
copy /Y customization\res\drawable-xhdpi\ouya_icon.png ..\WebArchive\ContentShell\res\drawable-xhdpi\ouya_icon.png
copy /Y customization\res\values\strings.xml ..\WebArchive\ContentShell\res\values\strings.xml
cd web_archive
zip ../web_archive.jar *
move ..\web_archive.jar ..\..\WebArchive\ContentShell\res\raw\web_archive.jar
dir ..\..\WebArchive\ContentShell\res\raw\web_archive.jar
cd ..\..\WebArchive\
CALL repackage.cmd
