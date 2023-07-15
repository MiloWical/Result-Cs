#! /bin/bash

# --- Flag defaults ---

INCLUDE_SOURCE_FLAG=0
INCLUDE_SYMBOLS_FLAG=0

# --- Parameter defaults ---

BUILD_CONFIGURATION="Release"
OUTPUT_PATH="./pkg"

# --- Process command-line arguments ---

REPO=$1
BRANCH=$2

shift
shift

while [[ $# -gt 0 ]]; do
  case $1 in

    -t|--tag)
      TAG=$2
      shift
      shift
      ;;  

    -s|--suffix)
      SUFFIX=$2
      shift
      shift
      ;;

    -M|--major)
      MAJOR_MASK=$2
      shift
      shift
      ;;
    
    -m|--minor)
      MINOR_MASK=$2
      shift
      shift
      ;;

    -p|--patch)
      PATCH_MASK=$2
      shift
      shift
      ;;
    
    -f|--force-version-update)
      FORCE_FLAG=1
      shift
      ;;

    -p|--project)
      PROJECT=$2
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

    -*|--*)
      echo "Unknown option $1"
      exit 1
      ;;
  esac
done

# --- Validate command-line parameters ---

if [ -z $PROJECT ]
then
  echo "Must specify a project to pack."
  exit 1
fi

# --- Process commit comments for automated versioning ---

COMMIT=$(git log -1 --format='%B%n%N' "$BRANCH")
RELEASE_TYPE=$(sed -rn 's/.*\(RELEASE_TYPE:([a-zA-Z]+)\).*/\1/p' <<< "$COMMIT")

if [ -z $RELEASE_TYPE ]
then
  RELEASE_TYPE="patch"
fi

# --- Read current release tag from GitHub if it wasn't passed --- 

if [ -z $TAG ]
then
  SCRIPT_PATH=$(dirname "$0")

  READ_RELEASE_CMD="$SCRIPT_PATH/read-github-release-tag.sh"
  READ_RELEASE_CMD="$READ_RELEASE_CMD $REPO"

  RELEASE_TAG=$(eval "$READ_RELEASE_CMD")
else
  RELEASE_TAG="$TAG"
fi

# --- Generate the new version tag ---

GENERATE_VERSION_CMD="$SCRIPT_PATH/generate-version.sh"
GENERATE_VERSION_CMD="$GENERATE_VERSION_CMD $RELEASE_TAG"
GENERATE_VERSION_CMD="$GENERATE_VERSION_CMD --increment-position $RELEASE_TYPE"

if [ ! -z $SUFFIX ]
then
  GENERATE_VERSION_CMD="$GENERATE_VERSION_CMD  --suffix $SUFFIX"
fi

if [ ! -z $MAJOR_MASK ]
then
  GENERATE_VERSION_CMD="$GENERATE_VERSION_CMD  --major $MAJOR_MASK"
fi

if [ ! -z $MINOR_MASK ]
then
  GENERATE_VERSION_CMD="$GENERATE_VERSION_CMD  --minor $MINOR_MASK"
fi

if [ ! -z $PATCH_MASK ]
then
  GENERATE_VERSION_CMD="$GENERATE_VERSION_CMD  --patch $PATCH_MASK"
fi

if [ ! -z $FORCE_FLAG ]
then
  GENERATE_VERSION_CMD="$GENERATE_VERSION_CMD  --force"
fi

VERSION=$(eval "$GENERATE_VERSION_CMD")


NUGET_PACK_CMD="--project '$PROJECT'"
NUGET_PACK_CMD="$NUGET_PACK_CMD --configuration '$BUILD_CONFIGURATION'"
NUGET_PACK_CMD="$NUGET_PACK_CMD --output '$OUTPUT_PATH'"
NUGET_PACK_CMD="$NUGET_PACK_CMD --version '$VERSION'"

if [ $INCLUDE_SOURCE_FLAG -eq 1 ]
then
  DOTNET_PACK_CMD="$DOTNET_PACK_CMD --src"
fi

if [ $INCLUDE_SYMBOLS_FLAG -eq 1 ]
then
  DOTNET_PACK_CMD="$DOTNET_PACK_CMD --sym"
fi

eval "./nuget-pack.sh $DOTNET_PACK_CMD"