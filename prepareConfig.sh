#!/bin/bash

variable="$1"
value="$2"
filename="$3"

if [ ! -f "${filename}" ]; then
    echo "File not found."
    exit 1
fi

# Replace occurrences of %<variable>% with the provided value
sed -i "s/%${variable}%/${value}/g" "${filename}"

echo "Values replaced successfully."