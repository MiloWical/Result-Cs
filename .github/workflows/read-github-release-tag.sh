#! /bin/bash

GH_RELEASE=$(gh release list --repo "$1" --limit 1)
RELEASE_TYPE=$(sed -rn 's/^\S+\s(\S+)\s.*/\1/p' <<< "$GH_RELEASE")
CURRENT_RELEASE_TAG=$(sed -rn 's/(v[0-9]\.[0-9]\.[0-9](-\S+)?).*/\1/p' <<< "$GH_RELEASE")

if [ -z $CURRENT_RELEASE_TAG ]
then
  echo "v0.0.0"
else
  echo "$CURRENT_RELEASE_TAG"
fi