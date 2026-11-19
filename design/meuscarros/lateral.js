// Menu lateral e barra de status do Meus Carros 2027, iguais em todas as telas. Uso: <body data-ativo="carros">.
// Itens do menu do ramo "Meus carros" (OS, equipamentos, vendas, produtos e MEI desligados), na ordem do programa.
// Peças/ferramentas (F5) vem logo depois de Meus carros, desde a versão de 18/11/2026.
// Sem Análises desde 19/11/2026 (o Meus Carros não tem finanças de empresa): Relatórios é F6.
(function () {
  const ic = {
    inicio: 'M8 1.5 1 7.2V15h5v-4.5h4V15h5V7.2z',
    financeiro: 'M1 3.5h14v9H1zM3 6h4v1.6H3zM11 8a1.4 1.4 0 1 0 0 .01z',
    contatos: 'M5.5 7.5a2.6 2.6 0 1 0 0-5.2 2.6 2.6 0 0 0 0 5.2zM0.8 14c0-2.8 2.1-4.6 4.7-4.6s4.7 1.8 4.7 4.6zM11.3 7.3a2.1 2.1 0 1 0 0-4.2 2.1 2.1 0 0 0 0 4.2zM11.2 8.8c2.3 0 4 1.5 4 4.1h-3.8c0-1.7-.5-3-1.5-4 .4-.1.8-.1 1.3-.1z',
    carros: 'M3.2 3.2 2 7H1v4.5h1.5V13h2v-1.5h7V13h2v-1.5H15V7h-1l-1.2-3.8zM4.3 4.7h7.4l.8 2.3h-9zM4 8.3a1 1 0 1 1 0 2 1 1 0 0 1 0-2zm8 0a1 1 0 1 1 0 2 1 1 0 0 1 0-2z',
    relatorios: 'M3 1h6.5L13 4.5V15H3zM5.2 7.3v1.3h5.6V7.3zm0 2.6v1.3h5.6V9.9z',
    ajustes: 'M8 5a3 3 0 1 0 0 6 3 3 0 0 0 0-6zm0 1.6a1.4 1.4 0 1 1 0 2.8 1.4 1.4 0 0 1 0-2.8z',
    ativacao: 'M5 5a3 3 0 1 0 2.8 4H11v1.6h1.6V9H14V7.4H7.8A3 3 0 0 0 5 5zm0 1.6a1.4 1.4 0 1 1 0 2.8 1.4 1.4 0 0 1 0-2.8z',
    ferramenta: 'M22.7 19l-9.1-9.1c.9-2.3.4-5-1.5-6.9-2-2-5-2.4-7.4-1.3L9 6 6 9 1.6 4.7C.4 7.1.9 10.1 2.9 12.1c1.9 1.9 4.6 2.4 6.9 1.5l9.1 9.1c.4.4 1 .4 1.4 0l2.3-2.3c.5-.4.5-1.1.1-1.4z',
    backup: 'M8 1.5c3.3 0 6 1 6 2.3v8.4c0 1.3-2.7 2.3-6 2.3s-6-1-6-2.3V3.8C2 2.5 4.7 1.5 8 1.5z',
  };
  const svg = n => `<svg viewBox="${n === 'ferramenta' ? '0 0 24 24' : '0 0 16 16'}"><path d="${ic[n]}"/></svg>`;
  const item = (id, texto, tecla) =>
    `<div class="item${document.body.dataset.ativo === id ? ' ativo' : ''}">${svg(id)}<span>${texto}</span>${tecla ? `<kbd>${tecla}</kbd>` : ''}</div>`;

  document.querySelector('.lateral').innerHTML = `
    <div class="marca"><img src="icone-app.png" alt=""><div><b>Meus Carros 2027</b><small>seus carros em dia</small></div></div>
    ${item('inicio', 'Início', 'F1')}
    <div class="sep"></div>
    ${item('financeiro', 'Financeiro', 'F2')}
    <div class="sep"></div>
    ${item('contatos', 'Contatos', 'F3')}
    ${item('carros', 'Meus carros', 'F4')}
    ${item('ferramenta', 'Peças/ferramentas', 'F5')}
    <div class="sep"></div>
    ${item('relatorios', 'Relatórios', 'F6')}
    <div class="sep"></div>
    ${item('ajustes', 'Ajustes')}
    ${item('ativacao', 'Ativação')}
    <div class="espaco"></div>
    <div class="backup">${item('backup', 'Fazer backup')}<small>Último backup: hoje às 14:32</small></div>
    <div class="licenca"><b>Licença ativa</b><br>Ricardo Almeida<br>CPF 529.***.***-**</div>
    <div class="sobre">ⓘ&nbsp; Sobre</div>`;

  document.querySelector('.status').innerHTML =
    '<span>▤&nbsp; Banco: C:\\Users\\Ricardo\\AppData\\Roaming\\Meus Carros 2027\\dados</span><span>Versão 26.11.19.2</span>';
})();
