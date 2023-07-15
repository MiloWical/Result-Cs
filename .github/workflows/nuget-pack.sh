#! /bin/bash

# --- Flag defaults ---

INCLUDE_SOURCE_FLAG=0
INCLUDE_SYMBOLS_FLAG=0

# --- Parameter defaults ---

BUILD_CONFIGURATION="Release"
OUTPUT_PATH="./pkg"
VERSION="0.0.1"

while [[ $# -gt 0 ]]; do
  case $1 in

    -p|--project)
      PROJECT=$2
      shift
      shift
      ;; 

    -v|--version)
      VERSION=$2
      shift
      shift
      ;;

    -o|--output)
      OUTPUT_PATH=$2
      shift
      shift
      ;;

    -c|--configuration)
      BUILD_CONFIGURATION=$2
      shift
      shift
      ;;

    --src)
      INCLUDE_SOURCE_FLAG=1
      shift
      ;;

    --sym)
      INCLUDE_SYMBOLS_FLAG=1
      shift
      ;;

    *)
      echo "Unknown argument: $1"
      exit 1
      ;;
  esac
done

if [ -z $PROJECT ]
then
  echo "Must specify a project to pack."
  exit 1
fi

DOTNET_PACK_CMD="pack \"$PROJECT\""

DOTNET_PACK_CMD="$DOTNET_PACK_CMD --configuration $BUILD_CONFIGURATION"

if [ $INCLUDE_SOURCE_FLAG -eq 1 ]
then
  DOTNET_PACK_CMD="$DOTNET_PACK_CMD --include-source"
fi

if [ $INCLUDE_SYMBOLS_FLAG -eq 1 ]
then
  DOTNET_PACK_CMD="$DOTNET_PACK_CMD --include-symbols"
fi

DOTNET_PACK_CMD="$DOTNET_PACK_CMD --output \"$OUTPUT_PATH\""

DOTNET_PACK_CMD="$DOTNET_PACK_CMD -p:PackageVersion=\"$VERSION\""

eval "dotnet $DOTNET_PACK_CMD"