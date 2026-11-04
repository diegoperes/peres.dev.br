# peres.dev.br

Site publicado via GitHub Pages em https://peres.dev.br.

- `index.html`: redireciona peres.dev.br para a página do MyWork (`/mywork/`).
- `css/style.css`: estilo base do site.

## MyWork (`mywork/`)

Página do programa em https://peres.dev.br/mywork/.

- `mywork/index.html` e `mywork/mywork.css`: página (usa `css/style.css` e troca o destaque pelo azul do app). Procure por `EDITAR`.
- `mywork/img/`: prints e ícone.
- `mywork/versao.json`: o programa consulta este arquivo para avisar que saiu versão nova.
  Formato: `{"versao": "26.11.3.1", "link": "https://peres.dev.br/mywork/#baixar"}`.
  A cada versão nova, troque só o número (o mesmo do instalador). O link aponta para a seção de download,
  que serve para Windows e Mac.
- Instaladores: os botões de download apontam para
  `https://github.com/diegoperes/peres.github.io/releases/latest/download/<arquivo>`, com os nomes
  `MyWork-Windows.exe`, `MyWork-Mac-AppleSilicon.dmg` e `MyWork-Mac-Intel.dmg`. Ao publicar uma versão, crie um
  Release novo e anexe os três arquivos com esses nomes (sem o número da versão): os links nunca precisam mudar.
