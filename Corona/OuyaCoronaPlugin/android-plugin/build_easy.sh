rm -r -f bin
rm -r -f gen
./build.plugin.sh ~/android/android-sdk-macosx/ /Applications/CoronaEnterprise/
cp bin/plugin.ouya.jar ../../InAppPurchasesPlugin/android/libs/
cp bin/plugin.ouya.jar ../../VirtualControllerPlugin/android/libs/
