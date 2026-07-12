#!/bin/bash

set -euo pipefail

app="${1:?Usage: $0 path/to/NBTExplorer.app}"

if [[ ! -d "$app" ]]; then
	printf 'App bundle not found: %s\n' "$app" >&2
	exit 1
fi

# Files copied from Downloads can carry extended attributes that invalidate a
# macOS code signature. Sign nested native libraries before the app bundle.
xattr -cr "$app"

while IFS= read -r -d '' binary; do
	if file "$binary" | grep -q 'Mach-O'; then
		codesign --remove-signature "$binary" 2>/dev/null || true
		codesign --force --sign - --timestamp=none "$binary"
	fi
done < <(find "$app/Contents/MonoBundle" -type f -print0)

main="$app/Contents/MacOS/NBTExplorer"
codesign --remove-signature "$main" 2>/dev/null || true
codesign --force --sign - --timestamp=none "$main"
codesign --force --sign - --timestamp=none "$app"
codesign --verify --deep --strict "$app"

printf 'Signed and verified: %s\n' "$app"
