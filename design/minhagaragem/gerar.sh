#!/usr/bin/env bash
# Gera as imagens da Minha Garagem 2027 a partir destes HTML, com o Chrome sem janela.
#   1) telas/*.png: telas do programa de verdade (1250 x 775 em 2x; celular 390 x 844), geradas pelo design/vitrine
#      com dados de exemplo (veja design/vitrine/LEIA-ME.md). Este script não mexe nelas.
#   2) anuncio-*.html (1200 x 1200) -> anuncios/minhagaragem/NN-*.png, as imagens do anúncio
#   3) copia para o site: minhagaragem/img/ (prints, imagem de compartilhar e ícones)
#   O ícone é o icone-marca.png (o ícone definitivo do programa, 512 px); as imagens do anúncio usam ele.
# Uso: design/minhagaragem/gerar.sh
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


echo "Anúncio:"
for f in anuncio-*.html; do
  n="${f#anuncio-}"; n="${n%.html}"
  foto "$f" 1200 1200 1 "$ANUNCIO/$n.png"
done

echo "Site:"
mkdir -p "$SITE"
# Prints do site: as telas do computador cortadas na proporção da janela (1000 x 620) e com 2000 px de largura.
python3 - "$SITE" <<'PY'
import sys
from PIL import Image
site = sys.argv[1]
for n in ['inicio', 'garagem', 'resumo', 'manutencoes', 'documentos', 'abastecimentos', 'gasto', 'projeto', 'pecas']:
    im = Image.open(f'telas/{n}.png').convert('RGB')
    w = im.width
    im = im.crop((0, 0, w, round(w * 620 / 1000))).resize((2000, 1240), Image.LANCZOS)
    im.save(f'{site}/{n}.png', optimize=True)
    print(f'  {site}/{n}.png')
PY
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
