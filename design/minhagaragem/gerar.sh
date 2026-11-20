#!/usr/bin/env bash
# Gera as imagens da Minha Garagem 2027 a partir destes HTML, com o Chrome sem janela.
#   1) tela-*.html (1000 x 620, telas do programa) -> telas/*.png, em 2x
#   2) anuncio-*.html (1200 x 1200) -> anuncios/minhagaragem/NN-*.png, as imagens do anúncio
#   3) copia para o site: minhagaragem/img/ (prints, imagem de compartilhar e ícones)
#   O ícone é o icone-marca.png (o ícone definitivo do programa, 512 px); as imagens do anúncio usam ele.
# Uso: design/minhagaragem/gerar.sh   (ou só as telas: design/minhagaragem/gerar.sh telas)
set -euo pipefail
cd "$(dirname "$0")"
CHROME="${CHROME:-/Applications/Google Chrome.app/Contents/MacOS/Google Chrome}"
SITE=../../minhagaragem/img
ANUNCIO=../../anuncios/minhagaragem

foto() { # arquivo.html largura altura escala saida.png
  "$CHROME" --headless=new --disable-gpu --hide-scrollbars --no-first-run --no-default-browser-check \
    --allow-file-access-from-files --default-background-color=00000000 --force-device-scale-factor="$4" --window-size="$2,$3" \
    --virtual-time-budget=3000 --screenshot="$PWD/$5" "file://$PWD/$1" >/dev/null 2>&1
  echo "  $5"
}

# Ícone: icone-marca.png (512 px) é o ícone definitivo do programa (MyWork, src/.../meus-carros), copiado aqui.
# O icone.html era o provisório e não é mais usado.

mkdir -p telas
echo "Telas do programa:"
for f in tela-*.html; do
  n="${f#tela-}"; n="${n%.html}"
  foto "$f" 1000 620 2 "telas/$n.png"
done
[ "${1:-}" = telas ] && exit 0

echo "Anúncio:"
for f in anuncio-*.html; do
  n="${f#anuncio-}"; n="${n%.html}"
  foto "$f" 1200 1200 1 "$ANUNCIO/$n.png"
done

echo "Site:"
mkdir -p "$SITE"
for n in inicio garagem resumo manutencoes documentos abastecimentos historico pecas; do
  [ -f "telas/$n.png" ] && cp "telas/$n.png" "$SITE/$n.png" && echo "  $SITE/$n.png"
done
cp "$ANUNCIO/01-capa.png" "$SITE/compartilhar.png" && echo "  $SITE/compartilhar.png"
python3 - "$SITE" <<'PY'
import sys
from PIL import Image
site = sys.argv[1]
im = Image.open('icone-marca.png').convert('RGBA')  # 512 x 512
im.save(f'{site}/icon-512.png')
im.resize((256, 256), Image.LANCZOS).save(f'{site}/icon.png')
im.resize((64, 64), Image.LANCZOS).save(f'{site}/favicon.png')
PY
echo "  $SITE/icon.png, icon-512.png, favicon.png"
