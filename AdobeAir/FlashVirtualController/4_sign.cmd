IF EXIST FlashVirtualControllerSigned.apk DEL FlashVirtualControllerSigned.apk
IF EXIST FlashVirtualControllerSigned.apk.orig del FlashVirtualControllerSigned.apk.orig
IF EXIST FlashVirtualControllerSigned.apk.sig del FlashVirtualControllerSigned.apk.sig
copy FlashVirtualControllerNew.apk FlashVirtualControllerSigned.apk
jarsigner -keystore debug.keystore -storepass android -keypass android FlashVirtualControllerSigned.apk androiddebugkey
jarsigner -verify -certs -verbose FlashVirtualControllerSigned.apk
if NOT "%1"=="1" pause