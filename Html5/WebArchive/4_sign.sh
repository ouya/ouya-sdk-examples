rm ContentShellSigned.apk
cp ContentShellAligned.apk ContentShellSigned.apk
jarsigner -tsa http://timestamp.digicert.com -keystore debug.keystore -storepass android -keypass android ContentShellSigned.apk androiddebugkey
jarsigner -verify -certs -verbose ContentShellSigned.apk
sleep 1
