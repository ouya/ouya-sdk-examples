SET PATH=C:\NVPACK\jdk1.7.0_71\bin;%PATH%

SET AIR_SDK=%USERPROFILE%\Downloads\AIRSDK_Compiler

IF NOT EXIST %AIR_SDK% ECHO AdobeAIRSDK was not found!
IF NOT EXIST %AIR_SDK% PAUSE
IF NOT EXIST %AIR_SDK% EXIT

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
jar xf *.swc
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
IF EXIST Combined.jar DEL Combined.jar
CALL jar -cvf Combined.jar .
COPY /Y Combined.jar ..\..\%TEMP%\ane\Android-ARM\AirOuyaPlugin.jar
cd ..\..

DIR %TEMP%\ane\extension.xml
DIR %TEMP%\ane\*.swc
DIR %TEMP%\ane\Android-ARM

CALL %ADT% -package -storetype PKCS12 -keystore cert.p12 -storepass password -tsa none -target ane %ANE_NAME% %TEMP%\ane\extension.xml -swc %TEMP%\ane\*.swc  -platform Android-ARM -C %TEMP%\ane\Android-ARM .
		
PAUSE