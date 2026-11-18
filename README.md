# peres.dev.br

Site publicado via GitHub Pages em https://www.peres.dev.br.

- `index.html` e `css/home.css`: página principal (www.peres.dev.br), com um cartão para cada programa (MyWork e Meus
  Carros). Links antigos com âncora (`www.peres.dev.br/#baixar`) vão para a seção certa do MyWork. Ícone e imagem de
  compartilhamento em `img/`, gerados por `assets/home/gerar.sh`.
- `css/style.css`: estilo base do site.

## MyWork (`mywork/`)

Página do programa em https://www.peres.dev.br/mywork/.

- `mywork/index.html` e `mywork/mywork.css`: página (usa `css/style.css` e troca o destaque pelo azul do app). Procure por `EDITAR`.
- `mywork/img/`: prints e ícone.
- O MyWork é vendido por edição anual. Foi lançado direto na edição 2027 (a 2026 não chegou a ser vendida); o
  MyWork 2028 será uma compra nova. A compra inclui só as atualizações de correção e ajuste da edição; a página e o
  anúncio dizem isso.
- `mywork/2027/versao.json`: o MyWork 2027 consulta este arquivo para avisar que saiu versão nova (e, assinado, se
  atualizar sozinho). Cada edição tem o seu, e o programa ignora o de outra edição. Quem grava é o
  `scripts/publicar-versao.sh` do MyWork, depois de subir os instaladores. `mywork/2026/versao.json` e
  `mywork/versao.json` ficam para quem instalou as versões de teste de antes (2026 e sem edição).
- Instaladores: os botões de download apontam para
  `https://github.com/diegoperes/peres.dev.br/releases/latest/download/<arquivo>`, com os nomes da edição:
  `MyWork-2027-Windows.exe`, `MyWork-2027-Mac-AppleSilicon.dmg` e `MyWork-2027-Mac-Intel.dmg`. Cada versão é um
  Release `mywork-2027-v<versão>` com esses três arquivos, publicado como *latest*. Quando sair o MyWork 2028,
  troque os botões pelos nomes da 2028.

## Meus Carros (`meuscarros/`)

Página em https://www.peres.dev.br/meuscarros/ (o programa abre esta página no Sobre e no aviso de versão nova).
O Meus Carros 2027 é o ramo "Meus carros" do MyWork vendido à parte, gerado do mesmo código com
`PRODUTO=MeusCarros` (veja o README do MyWork).

- `meuscarros/index.html` e `meuscarros/meuscarros.css`: página (usa `css/style.css` e `mywork/mywork.css`, com o
  destaque laranja). Procure por `EDITAR`: o link do anúncio no Mercado Livre. Enquanto estiver vazio, os botões
  Comprar ficam escondidos.
- `meuscarros/img/`: prints, imagem de compartilhar e ícones. Saem de `Anuncio/MeusCarros/fonte/gerar.sh` (não edite
  à mão).
- `meuscarros/2027/versao.json`: gravado por `PRODUTO=MeusCarros scripts/publicar-versao.sh`. O Meus Carros sobe sem
  *latest* no GitHub (o *latest* é do MyWork), então os botões de download leem os endereços deste arquivo. Enquanto
  ele não existe, os botões ficam apagados com o aviso "sai em breve".
- Android: o botão "Celular Android" usa o arquivo de sistema `android` do mesmo `versao.json`
  (`MeusCarros-2027-Android.apk`, que o `publicar-versao.sh` sobe quando o `scripts/gerar-android.sh` gerou o APK).
  Se a versão publicada não tiver APK, só esse botão fica apagado.
- O e-mail de suporte vem do mesmo `mywork/site-config.js`.

## Anúncios (`Anuncio/`, fora do site)

- `Anuncio/`: MyWork (título, descrição e imagens; veja o `LEIA-ME.txt`).
- `Anuncio/MeusCarros/`: Meus Carros 2027. As telas do programa são recriadas em HTML em `fonte/` e
  `fonte/gerar.sh` gera as imagens do anúncio e as do site (precisa do Google Chrome).
