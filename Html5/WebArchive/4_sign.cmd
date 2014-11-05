del ContentShellSigned.apk
copy ContentShellAligned.apk ContentShellSigned.apk
jarsigner -keystore debug.keystore -storepass android -keypass android ContentShellSigned.apk androiddebugkey
jarsigner -verify -certs -verbose ContentShellSigned.apk
if NOT "%1"=="1" pause