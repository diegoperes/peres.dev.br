# peres.dev.br

Site publicado via GitHub Pages em https://www.peres.dev.br.

- `index.html`: redireciona www.peres.dev.br para a página do MyWork (`/mywork/`).
- `css/style.css`: estilo base do site.

## MyWork (`mywork/`)

Página do programa em https://www.peres.dev.br/mywork/.

- `mywork/index.html` e `mywork/mywork.css`: página (usa `css/style.css` e troca o destaque pelo azul do app). Procure por `EDITAR`.
- `mywork/img/`: prints e ícone.
- O MyWork é vendido por edição anual (MyWork 2026 agora, MyWork 2027 como compra nova). A compra inclui só as
  atualizações de correção e ajuste da edição; a página e o anúncio dizem isso.
- `mywork/2026/versao.json`: o MyWork 2026 consulta este arquivo para avisar que saiu versão nova (e, assinado, se
  atualizar sozinho). Cada edição tem o seu (`mywork/2027/versao.json` para a 2027), e o programa ignora o de outra
  edição. Quem grava é o `scripts/publicar-versao.sh` do MyWork, depois de subir os instaladores.
  `mywork/versao.json` é o endereço das versões de antes das edições; o script também o atualiza na edição 2026.
- Instaladores: os botões de download apontam para
  `https://github.com/diegoperes/peres.dev.br/releases/latest/download/<arquivo>`, com os nomes da edição:
  `MyWork-2026-Windows.exe`, `MyWork-2026-Mac-AppleSilicon.dmg` e `MyWork-2026-Mac-Intel.dmg`. Cada versão é um
  Release `mywork-2026-v<versão>` com esses três arquivos. Quando sair o MyWork 2027, troque os botões pelos nomes da
  2027 (e, se quiser manter o download da 2026, aponte para o Release dela).
