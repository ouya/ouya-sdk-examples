SET PATH=c:\Program Files\Java\jdk1.8.0_40\bin;%PATH%

SET AIR_SDK=%USERPROFILE%\Downloads\AdobeAIRSDK

REM path to the ADT tool in Flash Builder sdks
SET ADT=%AIR_SDK%\bin\adt.bat

REM name of ANE file
SET ANE_NAME=OuyaNativeExtension.ane

REM AS lib folder
SET LIB_FOLDER=lib

SET TEMP=temp

REM grab the extension descriptor and SWC library 
echo on

rm -r -f %TEMP%
IF NOT EXIST %TEMP% md %TEMP%
IF NOT EXIST %TEMP%\ane md %TEMP%\ane
IF NOT EXIST %TEMP%\ane\Android-ARM md %TEMP%\ane\Android-ARM

COPY %LIB_FOLDER%\src\extension.xml %TEMP%\ane
COPY %LIB_FOLDER%\bin\*.swc %TEMP%\ane
CD %TEMP%\ane\
unzip *.swc
MOVE library.swf Android-ARM
cd ..\..\

REM COPY jar\AirOuyaPlugin.jar %TEMP%\ane\Android-ARM
REM COPY jar\ouya-sdk.jar %TEMP%\ane\Android-ARM

echo Combine JARs
cd jar
rm -r -f tmp
mkdir tmp
cd tmp
CALL jar -xvf ../AirOuyaPlugin.jar
CALL jar -xvf ../ouya-sdk.jar
del Combined.jar
CALL jar -cvf Combined.jar .
COPY /Y Combined.jar ..\..\%TEMP%\ane\Android-ARM\AirOuyaPlugin.jar
cd ..\..

DIR %TEMP%\ane\extension.xml
DIR %TEMP%\ane\*.swc
DIR %TEMP%\ane\Android-ARM

CALL %ADT% -package -storetype PKCS12 -keystore cert.p12 -storepass password -tsa none -target ane %ANE_NAME% %TEMP%\ane\extension.xml -swc %TEMP%\ane\*.swc  -platform Android-ARM -C %TEMP%\ane\Android-ARM .
		
PAUSE