#! /bin/bash

# --- Positional parameters ---

NUGET_FILE=$1
shift

# --- Flag defaults ---

REMOVE_SUFFIX_FLAG=0
REPLACE_SUFFIX_FLAG=0
REVERSION_SYMBOLS_FLAG=0

# --- Process arguments ---

while [[ $# -gt 0 ]]; do
  case $1 in

    --rem|--remove-suffix)
      REMOVE_SUFFIX_FLAG=1
      shift
      ;;  

    --rep|--replace-suffix)
      REPLACE_SUFFIX_FLAG=1
      shift
      ;;

    -s|--suffix)
      SUFFIX=$2
      shift
      shift
      ;;

    -v|--version)
      VERSION=$2
      shift
      shift
      ;;

    --sym)
      REVERSION_SYMBOLS_FLAG=1
      shift
      ;;

    *)
      echo "Unknown argument: $1"
      exit 1
      ;;
  esac
done

# --- Validate arguments ---

if [ -z $NUGET_FILE ]
then
  echo "You must specify a file to reversion."
  exit 1
fi

if [ $REMOVE_SUFFIX_FLAG -eq 1 ] && [ ! -z $SUFFIX ]
then
  echo "Cannot provide a suffix and request to remove one in the same invocation."
  exit 1
fi

if [ $REPLACE_SUFFIX_FLAG -eq 1 ] && [ -z $SUFFIX ]
then
  echo "Cannot replace a suffix without one supplied."
  exit 1
fi

NUGET_PATH=$(dirname "$NUGET_FILE")
NUGET_FILENAME=$(basename "$NUGET_FILE")

FILENAME_ROOT="${NUGET_FILENAME%.nupkg}"

if [ ! -z $VERSION ]
then
  OUTPUT_FILENAME_ROOT="${NUGET_FILENAME%%.[0-9]*}.$VERSION"
else
  OUTPUT_FILENAME_ROOT="${NUGET_FILENAME%.nupkg}"
fi

if [ $REMOVE_SUFFIX_FLAG -eq 1 ]
then
  OUTPUT_FILENAME_ROOT="${OUTPUT_FILENAME_ROOT%-*}"
fi

if [ ! -z $SUFFIX ]
then
  if [ $REPLACE_SUFFIX_FLAG -eq 1 ]
  then
    OUTPUT_FILENAME_ROOT="${OUTPUT_FILENAME_ROOT%-*}-$SUFFIX"
  else
    OUTPUT_FILENAME_ROOT="$OUTPUT_FILENAME_ROOT-$SUFFIX"
  fi
fi

NEW_VERSION=$(sed -rn 's/.*([0-9]+\.[0-9]+\.[0-9]+.*)/\1/p' <<< "$OUTPUT_FILENAME_ROOT")
NUGET_PACKAGE=$(sed -rn 's/(.*).([0-9]+\.[0-9]+\.[0-9]+.*)/\1/p' <<< "$FILENAME_ROOT")

eval "unzip '$NUGET_FILE' -d '$NUGET_PATH/$FILENAME_ROOT'"
eval "sed -i 's/<version>.*<\/version>/<version>$NEW_VERSION<\/version>/' '$NUGET_PATH/$FILENAME_ROOT/$NUGET_PACKAGE.nuspec'"

pushd "$NUGET_PATH/$FILENAME_ROOT/"
eval "zip -r '$NUGET_PATH/$OUTPUT_FILENAME_ROOT.nupkg' *"
popd

eval "rm '$NUGET_PATH/$FILENAME_ROOT.nupkg'"
eval "rm -rf '$NUGET_PATH/$FILENAME_ROOT'"

if [ $REVERSION_SYMBOLS_FLAG -eq 1 ]
then
  eval "unzip '$NUGET_PATH/$FILENAME_ROOT.snupkg' -d '$NUGET_PATH/$FILENAME_ROOT-sym'"
  eval "sed -i 's/<version>.*<\/version>/<version>$NEW_VERSION<\/version>/' '$NUGET_PATH/$FILENAME_ROOT-sym/$NUGET_PACKAGE.nuspec'"

  pushd "$NUGET_PATH/$FILENAME_ROOT-sym/"
  eval "zip -r '$NUGET_PATH/$OUTPUT_FILENAME_ROOT.snupkg' *"
  popd

  eval "rm '$NUGET_PATH/$FILENAME_ROOT.snupkg'"
  eval "rm -rf '$NUGET_PATH/$FILENAME_ROOT-sym'"
fi