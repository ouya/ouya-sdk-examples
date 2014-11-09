copy /Y customization\AndroidManifest.xml ..\..\Html5\WebArchive\ContentShell\AndroidManifest.xml
copy /Y customization\assets\key.der ..\..\Html5\WebArchive\ContentShell\assets\key.der
copy /Y customization\res\drawable\app_icon.png ..\..\Html5\WebArchive\ContentShell\res\drawable\app_icon.png
copy /Y customization\res\drawable\splash_screen.png ..\..\Html5\WebArchive\ContentShell\res\drawable\splash_screen.png
copy /Y customization\res\drawable-xhdpi\ouya_icon.png ..\..\Html5\WebArchive\ContentShell\res\drawable-xhdpi\ouya_icon.png
copy /Y customization\res\values\strings.xml ..\..\Html5\WebArchive\ContentShell\res\values\strings.xml
cd Build
zip -r ../web_archive.jar *
move ..\web_archive.jar ..\..\..\Html5\WebArchive\ContentShell\res\raw\web_archive.jar
dir ..\..\..\Html5\WebArchive\ContentShell\res\raw\web_archive.jar
cd ..\..\..\Html5\WebArchive\
CALL repackage.cmd
