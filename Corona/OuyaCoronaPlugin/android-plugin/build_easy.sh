rm -r -f bin
rm -r -f gen
./build.plugin.sh ~/android/android-sdk-macosx/ /Applications/CoronaEnterprise/
./install_easy.sh
cp bin/plugin.ouya.jar ../../InAppPurchasesPlugin/android/libs/
