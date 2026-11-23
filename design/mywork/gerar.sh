#!/usr/bin/env bash
# Gera as imagens 06 a 08 do anúncio do MyWork (anuncios/mywork/) a partir dos HTML, com o Chrome sem janela.
# As imagens 01 a 05 não têm fonte aqui: são prints editados. As telas de telas/ saem do programa de verdade
# (design/vitrine, veja o LEIA-ME de lá).
set -euo pipefail
cd "$(dirname "$0")"
CHROME="${CHROME:-/Applications/Google Chrome.app/Contents/MacOS/Google Chrome}"
for f in anuncio-*.html; do
  n="${f#anuncio-}"; n="${n%.html}"
  "$CHROME" --headless=new --disable-gpu --hide-scrollbars --no-first-run --allow-file-access-from-files \
    --force-device-scale-factor=1 --window-size=1200,1200 --virtual-time-budget=3000 \
    --screenshot="$PWD/../../anuncios/mywork/$n.png" "file://$PWD/$f" >/dev/null 2>&1
  echo "anuncios/mywork/$n.png"
done
