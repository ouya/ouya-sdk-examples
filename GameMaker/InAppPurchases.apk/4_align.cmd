CALL init.cmd
IF EXIST %PROJNAME%Aligned.apk DEL %PROJNAME%Aligned.apk
%ZIPALN% -f -v 4 %PROJNAME%Signed.apk %PROJNAME%Aligned.apk
if NOT "%1"=="1" pause