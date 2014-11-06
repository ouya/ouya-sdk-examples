cd Build
zip ../web_archive.jar *
move ..\web_archive.jar ..\..\..\Html5\WebArchive\ContentShell\res\raw\web_archive.jar
dir ..\..\..\Html5\WebArchive\ContentShell\res\raw\web_archive.jar
cd ..\..\..\Html5\WebArchive\
CALL repackage.cmd
