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

FILENAME_ROOT="${NUGET_FILE%.nupkg}"

if [ $REMOVE_SUFFIX_FLAG -eq 1 ]
then
  OUTPUT_FILENAME_ROOT="${FILENAME_ROOT%-*}"
fi

if [ ! -z $SUFFIX ]
then
  if [ $REPLACE_SUFFIX_FLAG -eq 1 ]
  then
    OUTPUT_FILENAME_ROOT="${FILENAME_ROOT%-*}-$SUFFIX"
  else
    OUTPUT_FILENAME_ROOT="$FILENAME_ROOT-$SUFFIX"
  fi
fi

eval "mv $FILENAME_ROOT.nupkg $OUTPUT_FILENAME_ROOT.nupkg"

if [ $REVERSION_SYMBOLS_FLAG -eq 1 ]
then
  eval "mv $FILENAME_ROOT.snupkg  $OUTPUT_FILENAME_ROOT.snupkg"
fi