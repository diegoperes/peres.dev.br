using Microsoft.Extensions.DependencyInjection;
using MyWork.Common.Enums;
using MyWork.Common.Requests;
using MyWork.Core.Perfis;
using MyWork.Core.Servicos;

namespace Vitrine;

internal static class SementeMyWork
{
	public static void Criar(IServiceProvider s, string roteiro)
	{
		var empresa = s.GetRequiredService<IEmpresaServico>();
		var p = PerfisRamo.Confeitaria;
		empresa.AlterarRamo(Ramo.Confeitaria, new ModulosRequest(p.ModulosSugeridos));
		s.GetRequiredService<IAreaServico>().CriarPadrao(p);
		empresa.Salvar(EmpresaRequest.De(empresa.Ficha()) with { Nome = "Doces da Ana", Documento = "52998224725", Telefone = "(41) 99876-5432" });

		var prod = s.GetRequiredService<IProdutoServico>();
		var cento = prod.Salvar(new ProdutoRequest(TipoItem.Produto, "Brigadeiro gourmet (cento)", PrecoVenda: 350m));
		var pote = prod.Salvar(new ProdutoRequest(TipoItem.Produto, "Bolo de pote de brigadeiro", PrecoVenda: 17m));
		prod.Salvar(new ProdutoRequest(TipoItem.Produto, "Bolo de aniversário (kg)", PrecoVenda: 95m));

		var ing = s.GetRequiredService<IIngredienteServico>();
		Guid I(string nome, decimal emb, string un, decimal preco, decimal estoque, string? marca = null) =>
			ing.Salvar(new IngredienteRequest(nome, emb, un, preco, true, estoque, marca));
		var lc = I("Leite condensado", 395, "g", 7.49m, 4740, "Moça");
		var creme = I("Creme de leite", 200, "g", 3.29m, 1600, "Nestlé");
		var choc = I("Chocolate em pó 50%", 1, "kg", 42.90m, 1.4m, "Melken");
		var mant = I("Manteiga sem sal", 500, "g", 24.90m, 750, "Tirolez");
		var gran = I("Granulado de chocolate", 1, "kg", 29.90m, 0.6m, "Melken");
		var acucar = I("Açúcar refinado", 1, "kg", 4.99m, 5, "União");
		var farinha = I("Farinha de trigo", 1, "kg", 5.49m, 4, "Dona Benta");
		var ovos = I("Ovos", 1, "dz", 13.90m, 3);
		var leite = I("Leite integral", 1, "L", 5.29m, 6);
		var forma = I("Forminha nº 4", 100, "un", 9.90m, 300);
		var potes = I("Pote 250 ml com tampa", 50, "un", 34.90m, 40);
		var fermento = I("Fermento em pó", 100, "g", 4.59m, 200, "Royal");

		var rec = s.GetRequiredService<IReceitaServico>();
		rec.ValorHora = 25m;
		var brig = rec.Salvar(new ReceitaRequest("Brigadeiro gourmet", 50, "un",
			[new(790, "g", lc), new(200, "g", creme), new(80, "g", choc), new(30, "g", mant), new(150, "g", gran), new(50, "un", forma)],
			[new ReceitaCustoRequest("Embalagem e etiqueta", 4m)],
			MinutosTrabalho: 90, PercentualGas: 10, Margem: 120, ProdutoId: cento, PorcoesPorItem: 100,
			ModoPreparo: "Leve ao fogo baixo o leite condensado, o creme de leite, o chocolate e a manteiga, mexendo até desgrudar do fundo. Deixe esfriar, enrole e passe no granulado."));
		var recheio = rec.Salvar(new ReceitaRequest("Recheio de brigadeiro de colher", 900, "g",
			[new(790, "g", lc), new(200, "g", creme), new(60, "g", choc), new(20, "g", mant)],
			MinutosTrabalho: 30, PercentualGas: 10, Margem: 100));
		var massa = rec.Salvar(new ReceitaRequest("Massa de chocolate", 1200, "g",
			[new(400, "g", farinha), new(300, "g", acucar), new(80, "g", choc), new(4, "un", ovos), new(240, "ml", leite), new(15, "g", fermento), new(100, "g", mant)],
			MinutosTrabalho: 40, PercentualGas: 15, Margem: 100));
		rec.Salvar(new ReceitaRequest("Bolo de pote de brigadeiro", 10, "un",
			[new(600, "g", null, massa), new(900, "g", null, recheio), new(10, "un", potes), new(50, "g", gran)],
			[new ReceitaCustoRequest("Colherzinha e etiqueta", 3.5m)],
			MinutosTrabalho: 30, PercentualGas: 0, Margem: 110, ProdutoId: pote,
			ModoPreparo: "Monte em camadas: massa, recheio, massa e recheio. Finalize com granulado e tampe."));
		rec.Salvar(new ReceitaRequest("Beijinho", 50, "un",
			[new(790, "g", lc), new(30, "g", mant), new(50, "un", forma)],
			[new ReceitaCustoRequest("Coco ralado", 9.8m)],
			MinutosTrabalho: 80, PercentualGas: 10, Margem: 120));
	}
}
