#!/bin/bash

COVERAGE_TYPE=$1
COVERAGE_FILE=$2

SEARCH_RESULT=$(grep "$COVERAGE_TYPE coverage" $COVERAGE_FILE)

COVERAGE_PERCENTAGE="$(echo $SEARCH_RESULT | grep -Eo -m 1 '[0-9]{1,3}(\.[0-9]+)?' | head -1)"

# Calulate Red Value
if [ $(printf "%1.f" "$COVERAGE_PERCENTAGE") -ge 50 ]; then
  COLOR_PERCENTAGE=$(echo "scale=2; 1 - ($COVERAGE_PERCENTAGE / 100)" | bc)
  RED=$(printf "%1.f" $(echo "scale=0;($COLOR_PERCENTAGE * 2 * 255)" | bc))
else
  RED=255
fi

# Calulate Green Value
if [ $(printf "%1.f" "$COVERAGE_PERCENTAGE") -le 50 ]; then
  COLOR_PERCENTAGE=$(echo "scale=2; $COVERAGE_PERCENTAGE / 100" | bc)
  GREEN=$(printf "%1.f" $(echo "scale=0;($COLOR_PERCENTAGE * 2 * 255)" | bc))
else
  GREEN=255
fi

echo "https://img.shields.io/badge/$1%20Code%20Coverage-$COVERAGE_PERCENTAGE%25-rgb%28$RED%2c$GREEN%2c0%29"
