. ./init.sh
echo APKTOOL=$APKTOOL
"$JDK7/bin/java" -jar $APKTOOL decode ContentShell.apk
sleep 1
