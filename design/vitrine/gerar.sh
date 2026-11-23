#!/usr/bin/env bash
# Gera as telas dos anúncios e do site com o programa de verdade (sem janela), numa base de exemplo fictícia.
#   Minha Garagem: design/minhagaragem/telas/   ·   MyWork (confeitaria): design/mywork/telas/
# Precisa do repositório MyWork ao lado deste (../MyWork) e de uma chave de ativação para o CPF de teste
# 529.982.247-25 de cada produto (nunca grave a chave aqui: o site é público):
#   VITRINE_CHAVE_MG=$(cd ../MyWork && dotnet run --project tools/GeradorChave --property:Produto=MinhaGaragem -- 52998224725 | grep '^ey')
#   VITRINE_CHAVE_MW=$(cd ../MyWork && dotnet run --project tools/GeradorChave -- 52998224725 | grep '^ey')
# Uso: VITRINE_CHAVE_MG=... VITRINE_CHAVE_MW=... design/vitrine/gerar.sh   (depois design/minhagaragem/gerar.sh e design/mywork/gerar.sh)
set -euo pipefail
cd "$(dirname "$0")"
TMP=$(mktemp -d)
trap 'rm -rf "$TMP"' EXIT

if [ -n "${VITRINE_CHAVE_MG:-}" ]; then
  dotnet build -c Release -p:Produto=MinhaGaragem -v q -nologo >/dev/null
  VITRINE_CHAVE="$VITRINE_CHAVE_MG" dotnet bin/Release/net10.0/Vitrine.dll "$TMP/mg/dados" "$TMP/out-mg" garagem
  # Só as que os anúncios e o site usam.
  for n in inicio garagem resumo manutencoes documentos abastecimentos gasto projeto pecas celular-inicio; do
    cp "$TMP/out-mg/$n.png" ../minhagaragem/telas/
  done
fi
if [ -n "${VITRINE_CHAVE_MW:-}" ]; then
  # Projeto à parte (no temporário) com os mesmos fontes: o MyWork e a Minha Garagem não dividem bin/obj.
  # (Não use -p:OutDir/IntermediateOutputPath: vale para os projetos do MyWork também e bagunça as pastas de lá.)
  mkdir -p "$TMP/mwsrc"
  sed -e "s#<NoWarn>#<EnableDefaultCompileItems>false</EnableDefaultCompileItems><NoWarn>#" \
      -e "s#../../../MyWork/#$(cd ../../../MyWork && pwd)/#g" \
      -e "s#</Project>#<ItemGroup><Compile Include=\"$PWD/*.cs\" /></ItemGroup></Project>#" Vitrine.csproj > "$TMP/mwsrc/Vitrine.csproj"
  dotnet build "$TMP/mwsrc/Vitrine.csproj" -c Release -p:Produto=MyWork -v q -nologo >/dev/null
  VITRINE_CHAVE="$VITRINE_CHAVE_MW" dotnet "$TMP/mwsrc/bin/Release/net10.0/Vitrine.dll" "$TMP/mw/dados" "$TMP/out-mw" confeitaria
  mkdir -p ../mywork/telas && cp "$TMP/out-mw/mw-receita.png" ../mywork/telas/
fi
