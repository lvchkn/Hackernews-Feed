#!/bin/bash

set -euo pipefail

CMD="$1"
PROFILE="${2-}" # empty if not provided

case "$1" in
    up|down)
        if [[ -n "$PROFILE" ]]; then
          docker compose \
            -f "docker-compose.local.yml" \
            --profile "$PROFILE" \
            "$CMD"
        else
          docker compose \
            -f "docker-compose.local.yml" \
            "$CMD"
        fi
        ;;
    *)
      echo "Unknown command: $CMD"
      echo "Usage: $0 {up|down}"
      exit 1
      ;;
esac