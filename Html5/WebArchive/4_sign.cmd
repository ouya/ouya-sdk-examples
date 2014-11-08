IF EXIST ContentShellSigned.apk del ContentShellSigned.apk
IF EXIST ContentShellSigned.apk.orig del ContentShellSigned.apk.orig
IF EXIST ContentShellSigned.apk.sig del ContentShellSigned.apk.sig
copy ContentShellAligned.apk ContentShellSigned.apk
jarsigner -keystore debug.keystore -storepass android -keypass android ContentShellSigned.apk androiddebugkey
jarsigner -verify -certs -verbose ContentShellSigned.apk
if NOT "%1"=="1" pause