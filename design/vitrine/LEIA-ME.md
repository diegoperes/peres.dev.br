# Vitrine: telas do programa de verdade para os anúncios e o site

As telas de `design/minhagaragem/telas/` e `design/mywork/telas/` não são desenhadas à mão: este programa abre o
MyWork / a Minha Garagem sem janela (Avalonia Headless com Skia), numa pasta de dados temporária, cria uma base de
exemplo fictícia pelos próprios serviços do programa e fotografa as telas em 2x.

- `SementeGaragem.cs`: a garagem do "Ricardo Almeida" (Corolla, CB 500X, Maverick, Opala, Fusca e um Gol vendido),
  com manutenções, abastecimentos, gastos com prestadores, documentos, financiamento, projetos e peças guardadas.
  As datas são relativas ao dia em que roda.
- `SementeMyWork.cs`: a confeitaria "Doces da Ana", com ingredientes, receitas (uma dentro da outra) e o cardápio.
- `Roteiros.cs`: quais telas abrir e o nome de cada foto. Computador em 1250 x 775 (algumas mais altas) e celular em
  390 x 844.
- `Program.cs`: abre o programa, tira a marca "Base de teste" e troca o caminho do banco no rodapé por um caminho
  de exemplo do Windows.

Como gerar: veja o começo do `gerar.sh` (precisa de uma chave de cada produto para o CPF de teste, gerada na hora
com o GeradorChave do MyWork; a chave nunca fica gravada aqui, porque este repositório é público).
