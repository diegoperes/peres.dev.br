# peres.dev.br

Site publicado via GitHub Pages em https://www.peres.dev.br.

## Pastas

```
index.html            página principal: um cartão para cada programa
css/                  style.css (base do site), home.css (página principal), produto.css (molde das páginas dos produtos)
js/site-config.js     e-mail de suporte (vazio no git; o workflow preenche na publicação)
robots.txt            libera o rastreamento e aponta para o sitemap
img/                  ícone e imagem de compartilhamento da página principal
mywork/               página do MyWork, prints (img/) e versao.json de cada edição
minhagaragem/           página da Minha Garagem, prints (img/) e versao.json de cada edição
privacidade/          aviso de privacidade (LGPD) dos dois programas; link no rodapé de todas as páginas

anuncios/             fora do site: o que vai para o Mercado Livre (imagens, título e descrição de cada produto)
scripts/sitemap.py    fora do site: gera o sitemap.xml na publicação
design/               fora do site: HTML de onde saem as imagens (anúncios, prints recriados e ícones)
  comum/              fontes Inter e o estilo das imagens de anúncio
  home/               ícone e imagem de compartilhamento da página principal
  mywork/             imagem 07 do anúncio do MyWork
  minhagaragem/         telas do programa recriadas em HTML e as 11 imagens do anúncio
```

O workflow (`.github/workflows/deploy-pages.yml`) publica tudo, menos `anuncios/`, `design/`, `scripts/`, o README
e os arquivos do git, e confere se as páginas e os `versao.json` estão lá.

## SEO e Google Search Console

- Domínio do site: **https://www.peres.dev.br** (configurado em Settings > Pages do GitHub; `peres.dev.br` sem www
  redireciona para ele). Canonical, `og:url`, `og:image`, o sitemap e o robots.txt usam todos esse endereço.
- `sitemap.xml`: gerado na publicação por `scripts/sitemap.py`, em https://www.peres.dev.br/sitemap.xml. O `lastmod`
  é a data do último commit de cada página (HTML, CSS e imagens). **Página nova: inclua na lista `ROTAS`** do script
  (e ponha canonical, title, description e og:* nela, como nas outras).
- `robots.txt`: fixo na raiz.
- Para ver o sitemap localmente: `python3 scripts/sitemap.py`.

**Não mude de lugar** os `versao.json` (`mywork/<edição>/versao.json`, `minhagaragem/<edição>/versao.json`): os programas instalados consultam esses endereços, e o
`scripts/publicar-versao.sh` do MyWork grava neles. Os endereços `/mywork/` e `/minhagaragem/` também ficam: os
programas abrem essas páginas no Sobre e no aviso de versão nova.

## Página principal

`index.html` e `css/home.css`. Ícone e imagem de compartilhamento: `design/home/gerar.sh`.

## MyWork (`mywork/`)

Página em https://www.peres.dev.br/mywork/.

- `mywork/index.html`: página (usa `css/style.css` e `css/produto.css`). Procure por `EDITAR`.
- `mywork/img/`: prints e ícone.
- O MyWork é vendido por edição anual. A edição atual é a 2027; o
  MyWork 2028 será uma compra nova. A compra inclui só as atualizações de correção e ajuste da edição; a página e o
  anúncio dizem isso.
- `mywork/2027/versao.json`: o MyWork 2027 consulta este arquivo para avisar que saiu versão nova (e, assinado, se
  atualizar sozinho). Cada edição tem o seu, e o programa ignora o de outra edição. Quem grava é o
  `scripts/publicar-versao.sh` do MyWork, depois de subir os instaladores.
- Instaladores: os botões de download apontam para
  `https://github.com/diegoperes/peres.dev.br/releases/latest/download/<arquivo>`, com os nomes da edição:
  `MyWork-2027-Windows.exe`, `MyWork-2027-Mac-AppleSilicon.dmg`, `MyWork-2027-Mac-Intel.dmg` e
  `MyWork-2027-Android.apk`. Cada versão é um Release `mywork-2027-v<versão>` com esses arquivos, publicado como
  *latest*; a publicação deixa só os 5 Releases mais novos de cada produto e edição. O botão do Android fica apagado até o `mywork/2027/versao.json` trazer o sistema `android`, para não levar
  a um arquivo que ainda não existe. Quando sair o MyWork 2028, troque os botões pelos nomes da 2028.

## Minha Garagem (`minhagaragem/`)

Página em https://www.peres.dev.br/minhagaragem/. A Minha Garagem 2027 é o ramo "Meus carros" do MyWork vendido à parte,
gerado do mesmo código com `PRODUTO=MinhaGaragem` (veja o README do MyWork).

- `minhagaragem/index.html` e `minhagaragem/minhagaragem.css`: página (usa `css/style.css` e `css/produto.css`, com o
  destaque laranja). Procure por `EDITAR`: o link do anúncio no Mercado Livre (se ficar vazio, os botões Comprar
  somem).
- `minhagaragem/img/`: prints, imagem de compartilhar e ícones. Saem de `design/minhagaragem/gerar.sh` (não edite à mão).
- `minhagaragem/2027/versao.json`: gravado por `PRODUTO=MinhaGaragem scripts/publicar-versao.sh`. A Minha Garagem sobe sem
  *latest* no GitHub (o *latest* é do MyWork), então os botões de download leem os endereços deste arquivo. Sistema
  sem arquivo na versão publicada (por exemplo, o APK) fica com o botão apagado.

## Anúncios (`anuncios/`) e imagens (`design/`)

- `anuncios/mywork/` e `anuncios/minhagaragem/`: imagens 1200 x 1200 na ordem (a 01 é a capa), `titulo.txt` e
  `descricao.txt`. Veja `anuncios/LEIA-ME.txt`.
- `design/minhagaragem/gerar.sh`: gera as telas recriadas, as 11 imagens do anúncio da Minha Garagem e os prints e ícones
  de `minhagaragem/img/` (`gerar.sh telas` gera só as telas).
- `design/mywork/gerar.sh`: gera a imagem 07 do anúncio do MyWork. As outras imagens do MyWork são prints editados e
  não têm HTML.
- `design/home/gerar.sh`: ícone e imagem de compartilhamento da página principal.
- Os scripts usam o Google Chrome sem janela e o Python com Pillow.
