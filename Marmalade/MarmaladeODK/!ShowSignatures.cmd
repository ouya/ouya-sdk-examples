REM
REM Copyright (C) 2012, 2013 OUYA, Inc.
REM
REM  Licensed under the Apache License, Version 2.0 (the "License");
REM  you may not use this file except in compliance with the License.
REM  You may obtain a copy of the License at
REM 
REM      http://www.apache.org/licenses/LICENSE-2.0
REM
REM  Unless required by applicable law or agreed to in writing, software
REM  distributed under the License is distributed on an "AS IS" BASIS,
REM  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
REM  See the License for the specific language governing permissions and
REM  limitations under the License.
REM

pushd
cd build_odk_android_java\classes
javap -s tv.ouya.sdk.marmalade.AsyncCppInitOuyaPlugin
javap -s tv.ouya.sdk.marmalade.AsyncCppOuyaRequestGamerInfo
javap -s tv.ouya.sdk.marmalade.AsyncCppOuyaRequestProducts
javap -s tv.ouya.sdk.marmalade.AsyncCppOuyaRequestPurchase
javap -s tv.ouya.sdk.marmalade.AsyncCppOuyaRequestReceipts
javap -s tv.ouya.sdk.marmalade.CallbacksInitOuyaPlugin
javap -s tv.ouya.sdk.marmalade.CallbacksRequestGamerInfo
javap -s tv.ouya.sdk.marmalade.CallbacksRequestProducts
javap -s tv.ouya.sdk.marmalade.CallbacksRequestPurchase
javap -s tv.ouya.sdk.marmalade.CallbacksRequestReceipts
javap -s tv.ouya.sdk.marmalade.IMarmaladeOuyaActivity
popd
pause
