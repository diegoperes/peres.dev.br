#!/usr/bin/env bash
# Gera a imagem 07 do anúncio do MyWork (anuncios/mywork/07-como-funciona.png) a partir do HTML.
# As outras imagens do MyWork não têm fonte aqui: são prints editados.
set -euo pipefail
cd "$(dirname "$0")"
CHROME="${CHROME:-/Applications/Google Chrome.app/Contents/MacOS/Google Chrome}"
"$CHROME" --headless=new --disable-gpu --hide-scrollbars --no-first-run --allow-file-access-from-files \
  --force-device-scale-factor=1 --window-size=1200,1200 --virtual-time-budget=3000 \
  --screenshot="$PWD/../../anuncios/mywork/07-como-funciona.png" "file://$PWD/anuncio-07-como-funciona.html" >/dev/null 2>&1
echo "anuncios/mywork/07-como-funciona.png"
