copy /Y customization\AndroidManifest.xml ..\WebArchive\ContentShell\AndroidManifest.xml
cd web_archive
zip ../web_archive.jar *
move ..\web_archive.jar ..\..\WebArchive\ContentShell\res\raw\web_archive.jar
dir ..\..\WebArchive\ContentShell\res\raw\web_archive.jar
cd ..\..\WebArchive\
CALL repackage.cmd
