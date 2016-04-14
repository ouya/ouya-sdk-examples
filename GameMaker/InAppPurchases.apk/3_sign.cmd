CALL init.cmd
IF EXIST %PROJNAME%Signed.apk DEL %PROJNAME%Signed.apk
IF EXIST %PROJNAME%Signed.apk.orig del %PROJNAME%Signed.apk.orig
IF EXIST %PROJNAME%Signed.apk.sig del %PROJNAME%Signed.apk.sig
copy %PROJNAME%New.apk %PROJNAME%Signed.apk
SET JARSNGR="c:\Program Files (x86)\Java\jdk1.6.0_45\bin\jarsigner.exe"
%JARSNGR% -keystore debug.keystore -storepass android -keypass android %PROJNAME%Signed.apk androiddebugkey
%JARSNGR% -verify -certs -verbose %PROJNAME%Signed.apk
if NOT "%1"=="1" pause
