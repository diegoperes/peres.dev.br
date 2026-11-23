using System.Globalization;
using Microsoft.Extensions.DependencyInjection;
using MyWork.Common.Enums;
using MyWork.Common.Requests;
using MyWork.Core.Perfis;
using MyWork.Core.Servicos;

namespace Vitrine;

internal static class SementeGaragem
{
	static readonly DateOnly Hoje = DateOnly.FromDateTime(DateTime.Today);

	public static string D(int diasAtras) => Hoje.AddDays(-diasAtras).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

	public static string F(int diasAFrente) => D(-diasAFrente);

	public static void Criar(IServiceProvider s)
	{
		var empresa = s.GetRequiredService<IEmpresaServico>();
		empresa.AlterarRamo(Ramo.Garagem, new ModulosRequest(PerfisRamo.Garagem.ModulosSugeridos));
		s.GetRequiredService<IAreaServico>().CriarPadrao(PerfisRamo.Garagem);
		empresa.Salvar(EmpresaRequest.De(empresa.Ficha()) with { Nome = "Ricardo Almeida", Documento = "52998224725", Telefone = "(41) 99876-5432" });

		var carros = s.GetRequiredService<ICarroServico>();
		var man = s.GetRequiredService<IManutencaoServico>();
		var abast = s.GetRequiredService<IAbastecimentoServico>();
		var gastos = s.GetRequiredService<IGastoCarroServico>();
		var docs = s.GetRequiredService<IDocumentoCarroServico>();
		var fin = s.GetRequiredService<IFinanciamentoServico>();
		var proj = s.GetRequiredService<IProjetoCarroServico>();
		var guard = s.GetRequiredService<IItemGuardadoServico>();
		var cli = s.GetRequiredService<IClienteServico>();

		Guid Prestador(string nome, string tipo, string fone, int estrelas) =>
			cli.Salvar(new ClienteRequest(TipoPessoa.Juridica, nome, Categoria: tipo, TipoContato: TipoContato.Fornecedor,
				Telefones: [new TelefoneRequest("Celular", fone, Whatsapp: true, Principal: true)], Avaliacao: estrelas));

		var mecanica = Prestador("Auto Mecânica São Jorge", "Mecânica", "(41) 99712-3388", 5);
		var retifica = Prestador("Retífica Paraná", "Retífica de motores", "(41) 3342-1180", 4);
		var estetica = Prestador("Brilho Total Estética", "Estética automotiva", "(41) 99654-2210", 5);
		var eletrica = Prestador("Auto Elétrica Faísca", "Elétrica", "(41) 99803-4471", 4);
		var funilaria = Prestador("Funilaria do Zé", "Funilaria e pintura", "(41) 99130-5562", 5);
		var pneus = Prestador("Pneus Curitiba", "Pneus e alinhamento", "(41) 3029-8800", 4);

		// Veículos
		var corolla = carros.Salvar(new CarroRequest("Corolla", "Toyota", "Corolla XEi 2.0", 2022, 2022, "Prata", "AAA12X3",
			"9BRBL3HE5N0123456", "01234567890", "2023-05-15", 128900m, 8200, 41800, "Carro do dia a dia. Revisões na concessionária até 2025.", Tipo: TipoVeiculo.Carro));
		var opala = carros.Salvar(new CarroRequest("Opala Comodoro", "Chevrolet", "Opala Comodoro 4.1", 1979, 1979, "Preto", "BBB34Y5",
			"5J69CBR123456", "00123456789", "2019-03-10", 38000m, 86000, 89400, "Placa preta. Motor 4.1 original, câmbio de 4 marchas no assoalho. Dono anterior: só 2 donos desde 1979.", Tipo: TipoVeiculo.Carro));
		var maverick = carros.Salvar(new CarroRequest("Maverick GT", "Ford", "Maverick GT V8", 1977, 1977, "Amarelo", "CCC56Z7",
			"LA4BUS12345", "00234567891", "2021-08-21", 95000m, 58900, 61200, "V8 302 original, com documentação de época.", Tipo: TipoVeiculo.Esportivo));
		var fusca = carros.Salvar(new CarroRequest("Fusca azul", "Volkswagen", "Fusca 1300", 1972, 1972, "Azul", "DDD78W9",
			"BS123456", "00345678912", "2015-10-02", 14500m, 104000, 112300, "Primeiro carro do meu pai. Restaurado em 2018.", Tipo: TipoVeiculo.Carro));
		var moto = carros.Salvar(new CarroRequest("CB 500X", "Honda", "CB 500X", 2020, 2021, "Vermelho", "EEE90V1",
			"9C2PC4510MR012345", "01198765432", "2022-11-04", 31900m, 9800, 18900, Tipo: TipoVeiculo.Moto));
		carros.Salvar(new CarroRequest("Gol", "Volkswagen", "Gol 1.0", 2012, 2013, "Branco", "FFF21U3",
			null, null, "2014-02-10", 26000m, 15000, 98000, null, Status.Inativo, "2023-05-20", 21500m, Tipo: TipoVeiculo.Carro));

		// Planos de manutenção, com a última vez de cada item
		void Plano(Guid carro, int kmAtual, Dictionary<string, (int km, int dias)> ultimas, bool severo = false)
		{
			var linhas = new List<ManutencaoRequest>();
			foreach (var item in PlanosManutencao.Comum.Itens)
			{
				var (ik, im) = item.Intervalo(severo);
				var u = ultimas.FirstOrDefault(kv => item.Descricao.StartsWith(kv.Key, StringComparison.Ordinal));
				if (u.Key is null)
					continue;
				linhas.Add(new ManutencaoRequest(carro, item.Descricao, ik, u.Value.km, D(u.Value.dias), IntervaloMeses: im > 0 ? im : null));
			}
			man.SalvarPlano(carro, [], linhas, severo);
		}
		Plano(corolla, 41800, new()
		{
			["Troca de óleo"] = (32100, 300),
			["Filtro de ar"] = (32100, 300),
			["Filtro de combustível"] = (32100, 300),
			["Filtro de cabine"] = (32100, 300),
			["Correia do alternador"] = (8200, 920),
			["Fluido de freio"] = (24000, 560),
			["Líquido de arrefecimento"] = (24000, 560),
			["Velas"] = (24000, 560),
			["Alinhamento"] = (33500, 353),
			["Revisão geral"] = (32100, 300),
		});
		Plano(opala, 89400, new()
		{
			["Troca de óleo"] = (86500, 250),
			["Filtro de ar"] = (86500, 340),
			["Filtro de combustível"] = (86500, 340),
			["Fluido de freio"] = (85800, 690),
			["Líquido de arrefecimento"] = (86500, 340),
			["Velas"] = (84000, 700),
			["Alinhamento"] = (88100, 160),
			["Revisão geral"] = (86500, 250),
		});
		Plano(maverick, 61200, new()
		{
			["Troca de óleo"] = (59800, 200),
			["Filtro de ar"] = (59800, 200),
			["Fluido de freio"] = (58900, 600),
			["Velas"] = (58900, 600),
			["Revisão geral"] = (59800, 200),
		});
		Plano(fusca, 112300, new()
		{
			["Troca de óleo"] = (110400, 170),
			["Filtro de ar"] = (108000, 400),
			["Fluido de freio"] = (108000, 400),
			["Velas"] = (108000, 400),
			["Revisão geral"] = (110400, 170),
		});
		Plano(moto, 18900, new()
		{
			["Troca de óleo"] = (14200, 230),
			["Filtro de ar"] = (14200, 230),
			["Fluido de freio"] = (9800, 380),
			["Velas"] = (9800, 380),
			["Revisão geral"] = (14200, 230),
		});

		// Abastecimentos
		var km = 41800 - 22 * 480;
		for (var i = 22; i >= 0; i--)
		{
			km = 41800 - i * 480;
			var litros = 38m + (i % 4) * 2.3m;
			var preco = i % 5 == 0 ? 4.29m : 6.19m - (i % 3) * 0.08m;
			var comb = i % 5 == 0 ? Combustivel.Etanol : Combustivel.Gasolina;
			abast.Salvar(new AbastecimentoRequest(corolla, D(i * 15 + 2), litros, Math.Round(litros * preco, 2), km, comb, true,
				i % 2 == 0 ? "Posto Shell Batel" : "Posto Ipiranga Água Verde"));
		}
		km = 89400 - 8 * 380;
		for (var i = 8; i >= 0; i--)
		{
			km += 380;
			abast.Salvar(new AbastecimentoRequest(opala, D(i * 38 + 5), 52m + i % 3, Math.Round((52m + i % 3) * 6.49m, 2), km, Combustivel.GasolinaAditivada, true, "Posto Shell Batel"));
		}
		km = 61200 - 5 * 260;
		for (var i = 5; i >= 0; i--)
		{
			km += 260;
			abast.Salvar(new AbastecimentoRequest(maverick, D(i * 55 + 9), 60m, Math.Round(60m * 6.49m, 2), km, Combustivel.GasolinaAditivada, true, "Posto Shell Batel"));
		}
		km = 18900 - 10 * 290;
		for (var i = 10; i >= 0; i--)
		{
			km += 290;
			abast.Salvar(new AbastecimentoRequest(moto, D(i * 20 + 4), 13.5m, Math.Round(13.5m * 6.19m, 2), km, Combustivel.Gasolina, true, "Posto Ipiranga Água Verde"));
		}

		// Gastos (serviços, peças, estética)
		gastos.Salvar(new GastoCarroRequest(corolla, D(300), TipoGastoCarro.Servico, "Revisão dos 30.000 km", 690m, 32100, null, null,
			[new PecaRequest("Óleo 0W-20 sintético", 4.2m, 62m, "Toyota", "08880-83322"), new PecaRequest("Filtro de óleo", 1, 58m, "Toyota", "90915-YZZD4"), new PecaRequest("Filtro de ar", 1, 142m, "Toyota", "17801-0T050")],
			ContatoId: mecanica));
		gastos.Salvar(new GastoCarroRequest(corolla, D(353), TipoGastoCarro.Servico, "Alinhamento e balanceamento", 180m, 33500, ContatoId: pneus));
		gastos.Salvar(new GastoCarroRequest(corolla, D(120), TipoGastoCarro.Peca, "Palhetas do limpador", 0m, 38900, "Loja de autopeças",
			Pecas: [new PecaRequest("Palheta dianteira (par)", 1, 189.90m, "Bosch", "AF26")]));
		gastos.Salvar(new GastoCarroRequest(corolla, D(12), TipoGastoCarro.Estetica, "Lavagem detalhada e cera", 280m, 41500, ContatoId: estetica));
		gastos.Salvar(new GastoCarroRequest(corolla, D(60), TipoGastoCarro.Servico, "Troca das pastilhas de freio dianteiras", 160m, 40600, null, null,
			[new PecaRequest("Pastilha de freio dianteira", 1, 260m, "Cobreq", "N-1412")], ContatoId: mecanica));

		gastos.Salvar(new GastoCarroRequest(opala, D(340), TipoGastoCarro.Servico, "Troca de óleo e filtros", 220m, 86500, null, null,
			[new PecaRequest("Óleo 20W-50 mineral", 5, 38m, "Lubrax"), new PecaRequest("Filtro de óleo", 1, 45m, "Tecfil", "PSL 140"), new PecaRequest("Filtro de ar", 1, 68m, "Tecfil")], ContatoId: mecanica));
		gastos.Salvar(new GastoCarroRequest(opala, D(160), TipoGastoCarro.Servico, "Regulagem do carburador e ponto", 380m, 88100, ContatoId: mecanica));
		gastos.Salvar(new GastoCarroRequest(opala, D(95), TipoGastoCarro.Servico, "Revisão do alternador e chicote", 650m, 88600, ContatoId: eletrica));
		gastos.Salvar(new GastoCarroRequest(opala, D(18), TipoGastoCarro.Estetica, "Polimento e vitrificação", 1200m, 89300, ContatoId: estetica));
		gastos.Salvar(new GastoCarroRequest(opala, D(8), TipoGastoCarro.Peca, "Frisos cromados das portas", 0m, 89350, "Encontro de antigos",
			Pecas: [new PecaRequest("Friso cromado da porta (jogo)", 1, 690m)]));

		gastos.Salvar(new GastoCarroRequest(maverick, D(200), TipoGastoCarro.Servico, "Troca de óleo e revisão", 420m, 59800, null, null,
			[new PecaRequest("Óleo 20W-50", 5, 42m, "Castrol"), new PecaRequest("Filtro de óleo", 1, 52m, "Fram", "PH8A")], ContatoId: mecanica));
		gastos.Salvar(new GastoCarroRequest(maverick, D(70), TipoGastoCarro.Servico, "Retoque de pintura no para-lama", 1450m, 60700, ContatoId: funilaria));
		gastos.Salvar(new GastoCarroRequest(maverick, D(25), TipoGastoCarro.Acessorio, "Volante esportivo de época", 980m, 61000, "Encontro de antigos"));

		gastos.Salvar(new GastoCarroRequest(fusca, D(170), TipoGastoCarro.Servico, "Troca de óleo e regulagem", 290m, 110400, ContatoId: mecanica));
		gastos.Salvar(new GastoCarroRequest(fusca, D(40), TipoGastoCarro.Peca, "Lanterna traseira original", 0m, 112100, "Loja de peças antigas",
			Pecas: [new PecaRequest("Lanterna traseira (par)", 1, 340m, "Original VW")]));

		gastos.Salvar(new GastoCarroRequest(moto, D(230), TipoGastoCarro.Servico, "Revisão dos 14.000 km", 690m, 14200, ContatoId: mecanica));
		gastos.Salvar(new GastoCarroRequest(moto, D(30), TipoGastoCarro.Peca, "Kit relação (corrente, coroa e pinhão)", 0m, 18500, "Loja de motos",
			Pecas: [new PecaRequest("Kit relação", 1, 720m, "DID")]));

		gastos.Salvar(new GastoCarroRequest(opala, D(330), TipoGastoCarro.Documentacao, "Licenciamento 2026", 160.22m));
		gastos.Salvar(new GastoCarroRequest(opala, D(300), TipoGastoCarro.Seguro, "Seguro para antigos", 1680m));
		gastos.Salvar(new GastoCarroRequest(corolla, D(310), TipoGastoCarro.Documentacao, "IPVA 2026", 4520m));

		// Documentos e CNH
		docs.Salvar(new DocumentoCarroRequest(opala, TipoDocumentoCarro.Licenciamento, F(12), null, 160.22m));
		docs.Salvar(new DocumentoCarroRequest(corolla, TipoDocumentoCarro.Seguro, F(21), "Porto Seguro", 3480m));
		docs.Salvar(new DocumentoCarroRequest(corolla, TipoDocumentoCarro.Ipva, F(53), null, 4380m));
		docs.Salvar(new DocumentoCarroRequest(corolla, TipoDocumentoCarro.Licenciamento, F(130), null, 160.22m));
		docs.Salvar(new DocumentoCarroRequest(corolla, TipoDocumentoCarro.Outro, F(75), "Rastreador (anuidade)", 418.80m));
		docs.Salvar(new DocumentoCarroRequest(corolla, TipoDocumentoCarro.Outro, F(200), "Garantia estendida", null, false, "Termina em junho de 2027."));
		docs.Salvar(new DocumentoCarroRequest(maverick, TipoDocumentoCarro.Seguro, F(108), "Seguro para antigos", 2150m));
		docs.Salvar(new DocumentoCarroRequest(maverick, TipoDocumentoCarro.Licenciamento, F(160), null, 160.22m));
		docs.Salvar(new DocumentoCarroRequest(fusca, TipoDocumentoCarro.Licenciamento, F(190), null, 160.22m));
		docs.Salvar(new DocumentoCarroRequest(moto, TipoDocumentoCarro.Ipva, F(55), null, 1180m));
		docs.GravarValidadeCnh(F(27));

		// Financiamento do Corolla
		var f = fin.Salvar(new FinanciamentoRequest(corolla, 48, 1890m, "2023-06-15", "Banco Toyota", 40000m, 88900m));
		var venc = new DateOnly(2023, 6, 15);
		while (venc < Hoje.AddDays(-5))
		{
			fin.PagarParcela(new PagamentoParcelaRequest(f, venc.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture), 1890m));
			venc = venc.AddMonths(1);
		}

		// Projetos
		proj.Salvar(new ProjetoCarroRequest(opala, "Motor 4.1 retificado",
			"Retífica completa mantendo as peças originais sempre que possível.",
			[
				new PecaProjetoRequest("Jogo de pistões e anéis", 1, 1850m, "Metal Leve", Comprada: true, DataCompra: D(70)),
				new PecaProjetoRequest("Jogo de juntas do motor", 1, 420m, "Sabó", Comprada: true, DataCompra: D(70)),
				new PecaProjetoRequest("Bomba d'água", 1, 380m, "Urba"),
				new PecaProjetoRequest("Carburador DFV 446 revisado", 1, 950m),
				new PecaProjetoRequest("Bronzinas de biela e mancal", 1, 0m),
			],
			[
				new EtapaProjetoRequest("Desmontar o motor", 0, D(80), true),
				new EtapaProjetoRequest("Retífica do bloco e do cabeçote", 3200m, D(45), true, retifica),
				new EtapaProjetoRequest("Montagem do motor", 1800m, null, false, mecanica),
				new EtapaProjetoRequest("Pintura do motor na cor original", 0, null, false),
			]));
		proj.Salvar(new ProjetoCarroRequest(opala, "Bancos em couro na cor original",
			"Tapeceiro indicado no encontro de Opalas.",
			[new PecaProjetoRequest("Couro natural preto (m²)", 6, 210m), new PecaProjetoRequest("Espuma D33 dos bancos", 1, 380m, Comprada: true, DataCompra: D(20))],
			[new EtapaProjetoRequest("Orçamento com o tapeceiro", 0, D(25), true), new EtapaProjetoRequest("Reforma dos bancos", 2600m)]));
		proj.Salvar(new ProjetoCarroRequest(opala, "Som de época",
			null,
			[new PecaProjetoRequest("Toca-fitas Motoradio original", 1, 650m, Comprada: true, DataCompra: D(150)), new PecaProjetoRequest("Alto-falantes 6x9\"", 2, 120m, Comprada: true, DataCompra: D(150))],
			[new EtapaProjetoRequest("Instalação e chicote", 250m, D(140), true, eletrica)]));
		proj.Salvar(new ProjetoCarroRequest(maverick, "Rodas e pneus de época",
			null,
			[
				new PecaProjetoRequest("Roda de liga 14\" original (jogo)", 1, 4200m),
				new PecaProjetoRequest("Pneu faixa branca 215/70 R14", 4, 690m),
			],
			[new EtapaProjetoRequest("Montagem e balanceamento", 240m, null, false, pneus)]));

		// Peças e ferramentas guardadas
		guard.Salvar(new ItemGuardadoRequest(TipoItemGuardado.Peca, "Filtro de óleo", 2, "Tecfil", "PSL 140", "Armário da garagem", opala, 45m));
		guard.Salvar(new ItemGuardadoRequest(TipoItemGuardado.Peca, "Jogo de velas de ignição", 1, "NGK", "BP5ES", "Armário da garagem", opala, 96m));
		guard.Salvar(new ItemGuardadoRequest(TipoItemGuardado.Peca, "Filtro de óleo", 1, "Toyota", "90915-YZZD4", "Prateleira 2", corolla, 58m));
		guard.Salvar(new ItemGuardadoRequest(TipoItemGuardado.Peca, "Platinado e condensador", 2, "Bosch", null, "Caixa do Fusca", fusca, 38m));
		guard.Salvar(new ItemGuardadoRequest(TipoItemGuardado.Peca, "Óleo 20W-50 mineral (litro)", 6, "Lubrax", null, "Prateleira 1", null, 38m));
		guard.Salvar(new ItemGuardadoRequest(TipoItemGuardado.Peca, "Emblema Maverick GT da grade", 1, null, null, "Gaveta das peças raras", maverick, 350m));
		guard.Salvar(new ItemGuardadoRequest(TipoItemGuardado.Ferramenta, "Torquímetro 1/2\"", 1, "Gedore", null, "Bancada", null, 620m, "Marcos (vizinho)", D(9)));
		guard.Salvar(new ItemGuardadoRequest(TipoItemGuardado.Ferramenta, "Macaco jacaré 3 t", 1, "Bovenau", null, "Fundo da garagem", null, 480m));
		guard.Salvar(new ItemGuardadoRequest(TipoItemGuardado.Ferramenta, "Chave de vela 16 mm", 1, "Tramontina", null, "Bancada"));
		guard.Salvar(new ItemGuardadoRequest(TipoItemGuardado.Ferramenta, "Lâmpada de ponto", 1, null, null, "Bancada", opala));
	}
}
