rm -r -f bin
rm -r -f gen
./build.sh ~/android/android-sdk-macosx/ /Applications/CoronaEnterprise/
./install_easy.sh
adb shell am start tv.ouya.examples.corona.inapppurchases/com.ansca.corona.CoronaActivity

