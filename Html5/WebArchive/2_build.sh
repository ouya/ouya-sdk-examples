. ./init.sh
"$JDK7/bin/java" -jar $APKTOOL build ContentShell
rm ContentShellNew.apk
mv ContentShell/dist/ContentShell.apk ContentShellNew.apk
sleep 1
