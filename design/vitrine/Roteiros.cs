using Microsoft.Extensions.DependencyInjection;
using MyWork.Core.Servicos;
using static Vitrine.Program;

namespace Vitrine;

internal static class RoteiroGaragem
{
	public static void Executar()
	{
		Ativar();
		var carros = Servicos.GetRequiredService<ICarroServico>().Listar();
		Guid Id(string nome) => carros.First(c => c.Descricao == nome).Id;
		var opala = Id("Opala Comodoro");
		var corolla = Id("Corolla");
		Tamanho(Largura, 1100);
		Esperar(Vm.IrInicio());
		Foto("inicio-alta");
		Tamanho(Largura, 775);
		Esperar(Vm.IrInicio());
		Foto("inicio");
		Esperar(Vm.IrCarros());
		Foto("garagem");
		Esperar(Vm.IrLista());
		Foto("contatos");
		Esperar(Vm.AbrirCarro(opala, "resumo"));
		Foto("resumo");
		Esperar(Vm.AbrirCarro(corolla, "manutencoes"));
		Foto("manutencoes");
		Esperar(Vm.AbrirCarro(opala, "historico"));
		Foto("historico");
		var c = (MyWork.App.ViewModels.Garagem.CarroViewModel)Vm.TelaAtual!;
		c.AbrirGastoCommand.Execute(c.Gastos.First(g => g.Gasto.Descricao.Contains("Regulagem")));
		Bombear();
		Foto("gasto");
		Esperar(Vm.AbrirCarro(corolla, "abastecimentos"));
		Foto("abastecimentos");
		Esperar(Vm.AbrirCarro(corolla, "documentos"));
		Foto("documentos");
		Esperar(Vm.AbrirCarro(corolla, "financiamento"));
		Foto("financiamento");
		Esperar(Vm.AbrirCarro(opala, "projetos"));
		Foto("projetos");
		c = (MyWork.App.ViewModels.Garagem.CarroViewModel)Vm.TelaAtual!;
		c.Projetos.AbrirCommand.Execute(c.Projetos.Projetos.First(p => p.Nome.Contains("Motor")));
		Bombear();
		Tamanho(Largura, 1100);
		Foto("projeto");
		Tamanho(Largura, 775);
		Esperar(Vm.AbrirCarro(opala, "dados"));
		Foto("dados");
		Esperar(Vm.IrPecas(null));
		Foto("pecas");
		var mec = Servicos.GetRequiredService<IClienteServico>().Listar(new MyWork.Common.Requests.ClienteFiltroRequest("sao jorge")).Itens.First().Id;
		Tamanho(Largura, 1100);
		Esperar(Vm.AbrirCliente(mec));
		Foto("contato");
		Esperar(Vm.IrAjustes(null));
		Foto("ajustes");
		Tamanho(Largura, 775);
		if (Environment.GetEnvironmentVariable("SO_DESKTOP") == "1") return;
		((PlataformaVitrine)MyWork.App.Servicos.Plataforma.Atual).Celular = true;
		Tamanho(390, 844);
		Esperar(Vm.IrInicio());
		Foto("celular-inicio");
		Esperar(Vm.AbrirCarro(opala, "resumo"));
		Foto("celular-resumo");
		Esperar(Vm.AbrirCarro(corolla, "abastecimentos"));
		Foto("celular-abastecimentos");
		Esperar(Vm.AbrirCarro(opala, "historico"));
		Foto("celular-historico");
	}

	static string Texto(object o) => string.Join(" ", o.GetType().GetProperties().Where(p => p.PropertyType == typeof(string)).Select(p => p.GetValue(o) as string));

	public static void Ativar()
	{
		var b = Servicos.GetRequiredService<IBackupRapidoServico>();
		if (b.Ultimo is null)
		{
			var pasta = Path.Combine(Path.GetTempPath(), "vitrine-backup");
			Directory.CreateDirectory(pasta);
			b.DefinirPasta(pasta);
			Esperar(b.Fazer());
			Vm.AtualizarSituacaoBackup();
		}
		Bombear();
	}
}

internal static class RoteiroMyWork
{
	public static void Executar(string roteiro)
	{
		Rodape = @"C:\Users\Ana\AppData\Roaming\MyWork 2027\dados\MyWork.db";
		RoteiroGaragem.Ativar();
		var rec = Servicos.GetRequiredService<IReceitaServico>().Listar();
		Esperar(Vm.IrInicio());
		Foto("mw-inicio");
		Esperar(Vm.IrReceitas(null));
		Foto("mw-receitas");
		Esperar(Vm.IrReceitas("ingredientes"));
		Foto("mw-ingredientes");
		Tamanho(Largura, 1300);
		Esperar(Vm.AbrirReceita(rec.First(r => r.Nome.StartsWith("Bolo de pote")).Id));
		Foto("mw-receita-bolo");
		Esperar(Vm.AbrirReceita(rec.First(r => r.Nome.StartsWith("Brigadeiro")).Id));
		Foto("mw-receita");
		Tamanho(Largura, 775);
		((PlataformaVitrine)MyWork.App.Servicos.Plataforma.Atual).Celular = true;
		Tamanho(390, 844);
		Esperar(Vm.AbrirReceita(rec.First(r => r.Nome.StartsWith("Brigadeiro")).Id));
		Foto("mw-celular-receita");
		Esperar(Vm.IrReceitas(null));
		Foto("mw-celular-receitas");
	}
}
