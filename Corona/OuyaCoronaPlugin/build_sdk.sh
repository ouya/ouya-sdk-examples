#!/bin/bash

path=`dirname $0`

BUILD_DIR=$1
PLUGIN_NAME=$2
BUILD_NUM=$3
PRODUCT=sdk

if [ ! "$BUILD_NUM" ]
then
	BUILD_NUM=2013.1133
fi

#
# Checks exit value for error
# 
checkError() {
    if [ $? -ne 0 ]
    then
        echo "Exiting due to errors (above)"
        exit -1
    fi
}

# 
# Canonicalize relative paths to absolute paths
# 
pushd $path > /dev/null
dir=`pwd`
path=$dir
popd > /dev/null


#
# OUTPUT_DIR
# 
OUTPUT_DIR=$BUILD_DIR/$PRODUCT

# Clean build
if [ -e "$OUTPUT_DIR" ]
then
	rm -rf "$OUTPUT_DIR"
fi

# Plugins
OUTPUT_PLUGINS_DIR=$OUTPUT_DIR/plugins/$BUILD_NUM
OUTPUT_DIR_MAC=$OUTPUT_PLUGINS_DIR/mac-sim
OUTPUT_DIR_ANDROID=$OUTPUT_PLUGINS_DIR/android
OUTPUT_DIR_WIN32=$OUTPUT_PLUGINS_DIR/win32-sim

# Docs
OUTPUT_DIR_DOCS=$OUTPUT_DIR/docs

# Samples
OUTPUT_DIR_SAMPLES=$OUTPUT_DIR/samples

# Create directories
mkdir "$OUTPUT_DIR"
checkError
mkdir -p "$OUTPUT_DIR_ANDROID"
checkError
mkdir -p "$OUTPUT_DIR_MAC"
checkError
mkdir -p "$OUTPUT_DIR_WIN32"
checkError
mkdir -p "$OUTPUT_DIR_SAMPLES"
checkError


#
# Build
#

echo "------------------------------------------------------------------------"
echo "[android]"
cd "$path/android-plugin"
	export OUTPUT_PLUGIN_DIR_ANDROID="$OUTPUT_DIR_ANDROID"
	./build.plugin.sh
	checkError
	cp -v bin/plugin.${PLUGIN_NAME}.jar "$OUTPUT_DIR_ANDROID"
	checkError
	cp -v metadata.lua "$OUTPUT_DIR_ANDROID"
	checkError
	# Note: You'll need to copy library dependencies too, such as the "ouya-sdk.jar" file.
cd -

echo "------------------------------------------------------------------------"
echo "[mac]"
cp -v "$path"/simulator-plugin/*.lua "$OUTPUT_DIR_MAC"
checkError

echo "------------------------------------------------------------------------"
echo "[win32]"
cp -v "$path"/simulator-plugin/*.lua "$OUTPUT_DIR_WIN32"
checkError

echo "------------------------------------------------------------------------"
echo "[metadata]"
cp -v "$path/metadata.json" "$OUTPUT_DIR"
checkError

echo "------------------------------------------------------------------------"
echo "[docs]"
cp -vrf "$path/docs" "$OUTPUT_DIR"
checkError

echo "------------------------------------------------------------------------"
echo "[samples]"
cp -vrf "$path/samples/" "$OUTPUT_DIR_SAMPLES"
checkError

echo "------------------------------------------------------------------------"
echo "Generating plugin zip"
ZIP_FILE=$BUILD_DIR/${PRODUCT}-${PLUGIN_NAME}.zip
cd "$OUTPUT_DIR"
	zip -rv "$ZIP_FILE" *
cd -

echo "------------------------------------------------------------------------"
echo "Plugin build succeeded."
echo "Zip file located at: '$ZIP_FILE'"
