#!/usr/bin/env python3
"""Gera o sitemap.xml do site (rodado pelo workflow de publicação; dá para rodar local também).

Uso: python3 scripts/sitemap.py [pasta_de_saida]   (padrão: imprime na tela)

O lastmod de cada página é a data do último commit que mexeu nela (o HTML, o CSS e as imagens que ela usa), então o
checkout precisa do histórico completo (fetch-depth: 0 no workflow). Página nova entra aqui, na lista ROTAS.
"""
import subprocess
import sys
from datetime import datetime, timezone
from pathlib import Path
from xml.sax.saxutils import escape

# Domínio que o GitHub Pages serve (Settings > Pages). O mesmo dos canonical e og:url das páginas.
SITE = "https://www.peres.dev.br"

# rota, prioridade, arquivos que, mudando, mudam a página
ROTAS = [
    ("/", "1.0", ["index.html", "css/home.css", "css/style.css", "img"]),
    ("/mywork/", "0.9", ["mywork/index.html", "css/produto.css", "css/style.css", "mywork/img"]),
    ("/meuscarros/", "0.9", ["meuscarros/index.html", "meuscarros/meuscarros.css", "css/produto.css", "css/style.css", "meuscarros/img"]),
]

RAIZ = Path(__file__).resolve().parent.parent


def ultima_mudanca(arquivos):
    data = subprocess.run(
        ["git", "log", "-1", "--format=%cI", "--", *arquivos],
        cwd=RAIZ, capture_output=True, text=True, check=True,
    ).stdout.strip()
    if not data:  # fora do git ou sem histórico: a data de hoje
        return datetime.now(timezone.utc).strftime("%Y-%m-%d")
    return datetime.fromisoformat(data).astimezone(timezone.utc).strftime("%Y-%m-%d")


def gerar():
    linhas = ['<?xml version="1.0" encoding="UTF-8"?>',
              '<urlset xmlns="http://www.sitemaps.org/schemas/sitemap/0.9">']
    for rota, prioridade, arquivos in ROTAS:
        if not (RAIZ / arquivos[0]).is_file():
            raise SystemExit(f"Página da rota {rota} não encontrada: {arquivos[0]}")
        linhas += [
            "  <url>",
            f"    <loc>{escape(SITE + rota)}</loc>",
            f"    <lastmod>{ultima_mudanca(arquivos)}</lastmod>",
            f"    <priority>{prioridade}</priority>",
            "  </url>",
        ]
    linhas.append("</urlset>")
    return "\n".join(linhas) + "\n"


if __name__ == "__main__":
    xml = gerar()
    if len(sys.argv) > 1:
        destino = Path(sys.argv[1]) / "sitemap.xml"
        destino.write_text(xml, encoding="utf-8")
        print(f"Gerado {destino}")
    else:
        sys.stdout.write(xml)
