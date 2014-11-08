rm ContentShellSigned.apk
cp ContentShellAligned.apk ContentShellSigned.apk
export JARSIGNER=/Library/Java/JavaVirtualMachines/1.6.0_51-b11-457.jdk/Contents/Home/bin/jarsigner
#export JARSIGNER=/Library/Java/JavaVirtualMachines/jdk1.7.0_67.jdk/Contents/Home/bin/jarsigner
$JARSIGNER -tsa http://timestamp.digicert.com -keystore ./debug.keystore -storepass android -keypass android ContentShellSigned.apk androiddebugkey
$JARSIGNER -verify -certs -verbose ContentShellSigned.apk
sleep 1
