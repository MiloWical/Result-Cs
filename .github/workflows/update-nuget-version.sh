#! /bin/bash

# --- Positional parameters ---

NUGET_FILE=$1
shift

# --- Flag defaults ---

REMOVE_SUFFIX_FLAG=0
REVERSION_SYMBOLS_FLAG=0

# --- Process arguments ---

while [[ $# -gt 0 ]]; do
  case $1 in

    -r|--remove-suffix)
      REMOVE_SUFFIX_FLAG=1
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

if [ $REMOVE_SUFFIX_FLAG -eq 1 ] && [ ! -z $SUFFIX ]
then
  echo "Cannot provide a suffix and request to remove one in the same invocation."
  exit 1
fi