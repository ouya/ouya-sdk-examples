#!/bin/bash

path=`dirname $0`

BUILD_NUM=$1
PLUGIN_NAME=ouya

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
OUTPUT_DIR=$path/bin

# Clean build
if [ -e "$OUTPUT_DIR" ]
then
	rm -rf "$OUTPUT_DIR"
fi

# Create dst dir
mkdir "$OUTPUT_DIR"
checkError


#
# Build
#

cd "$path"
	echo "========================================================================"
	echo "Packaging plugin for SDK..."
	./build_sdk.sh "$OUTPUT_DIR" $PLUGIN_NAME $BUILD_NUM
	checkError
	echo "Done."

	# echo "========================================================================"
	# echo "Packaging plugin for Enterprise"
	# ./build_enterprise.sh "$OUTPUT_DIR" $PLUGIN_NAME
	# checkError
	# echo "Done."

	echo "========================================================================"
	echo "Build successful."
	echo "Plugin ZIP files available at: '$OUTPUT_DIR'"
cd -
