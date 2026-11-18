#!/usr/bin/env bash
# Gera o ícone (img/icon.png, img/favicon.png) e a imagem de compartilhamento (img/compartilhar.png) da página principal.
set -euo pipefail
cd "$(dirname "$0")"
CHROME="${CHROME:-/Applications/Google Chrome.app/Contents/MacOS/Google Chrome}"
foto() { "$CHROME" --headless=new --disable-gpu --hide-scrollbars --no-first-run --allow-file-access-from-files \
  --default-background-color=00000000 --virtual-time-budget=3000 --force-device-scale-factor="$3" --window-size="$2" \
  --screenshot="$4" "file://$PWD/$1" >/dev/null 2>&1; }
TMP="$(mktemp -d)"
foto icone.html 500,256 2 "$TMP/icone.png"
foto compartilhar.html 1200,630 1 "../../img/compartilhar.png"
python3 - "$TMP/icone.png" <<'PY'
import sys
from PIL import Image
im = Image.open(sys.argv[1]).crop((0, 0, 512, 512))
im.resize((256, 256), Image.LANCZOS).save('../../img/icon.png', optimize=True)
im.resize((64, 64), Image.LANCZOS).save('../../img/favicon.png', optimize=True)
PY
rm -rf "$TMP"
ls -l ../../img
