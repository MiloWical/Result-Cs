#! /bin/bash

# --- Process command-line arguments ---

REPO=$1
BRANCH=$2

shift
shift

PRERELEASE_FLAG=0
RELEASE_FILES_ARR=()

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
    
    -f|--force)
      FORCE_FLAG=1
      shift
      ;;

    -pre|--prerelease)
      PRERELEASE_FLAG=1
      shift
      ;;

    -*|--*)
      echo "Unknown option $1"
      exit 1
      ;;
      
    *)
      RELEASE_FILES_ARR+=("$1") # save positional arg
      shift # past argument
      ;;
  esac
done

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

VERSION_TAG=v$(eval "$GENERATE_VERSION_CMD")

# --- Read additional release files ---

RELEASE_FILES=$( IFS=' '; echo "${RELEASE_FILES_ARR[*]}" )

# --- Create the GitHub release ---

GH_RELEASE_PARAMS="release create"
GH_RELEASE_PARAMS="$GH_RELEASE_PARAMS $VERSION_TAG --generate-notes"

if [ $PRERELEASE_FLAG -eq 1 ]
then
  GH_RELEASE_PARAMS="$GH_RELEASE_PARAMS --prerelease"
fi

GH_RELEASE_PARAMS="$GH_RELEASE_PARAMS $RELEASE_FILES"

eval "gh $GH_RELEASE_PARAMS"