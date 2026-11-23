using Avalonia;
using Avalonia.Controls;
using Avalonia.Headless;
using Avalonia.Media.Imaging;
using Avalonia.Threading;
using Avalonia.VisualTree;
using Microsoft.Extensions.DependencyInjection;
using MyWork.App.DependencyInjection;
using MyWork.App.Servicos;
using MyWork.App.ViewModels.Comum;
using MyWork.App.Views.Comum;

namespace Vitrine;

// Gera telas do programa de verdade (sem janela), com dados fictícios, para os anúncios.
// Uso: Vitrine <pasta-dados> <pasta-saida> [roteiro]
internal static class Program
{
	public static ServiceProvider Servicos = null!;
	public static PrincipalViewModel Vm = null!;
	public static Window Janela = null!;
	public static string Saida = "";

	[STAThread]
	public static int Main(string[] args)
	{
		var pasta = Path.GetFullPath(args[0]);
		Saida = Path.GetFullPath(args[1]);
		var roteiro = args.Length > 2 ? args[2] : "garagem";
		Directory.CreateDirectory(Saida);
		var novo = !File.Exists(Path.Combine(pasta, "MinhaGaragem.db")) && !File.Exists(Path.Combine(pasta, "MyWork.db"));
		Environment.SetEnvironmentVariable("MYWORK_PASTA_DADOS", pasta);
		Plataforma.Atual = new PlataformaVitrine();
		AppBuilder
			.Configure<MyWork.App.App>()
			.UseSkia()
			.UseHeadless(new AvaloniaHeadlessPlatformOptions { UseHeadlessDrawing = false })
			.WithInterFont()
			.SetupWithoutStarting();

		Servicos = ContainerPrograma.Criar();
		if (novo)
		{
			var chave = Environment.GetEnvironmentVariable("VITRINE_CHAVE");
			if (!string.IsNullOrEmpty(chave))
			{
				var emp = Servicos.GetRequiredService<MyWork.Core.Servicos.IEmpresaServico>();
				emp.Salvar(MyWork.Common.Requests.EmpresaRequest.De(emp.Ficha()) with { Documento = Environment.GetEnvironmentVariable("VITRINE_DOC") ?? "52998224725" });
				Console.WriteLine(Servicos.GetRequiredService<MyWork.Core.Servicos.ILicencaServico>().Ativar(chave, Environment.GetEnvironmentVariable("VITRINE_DOC") ?? "52998224725"));
			}
			if (roteiro.StartsWith("garagem"))
				SementeGaragem.Criar(Servicos);
			else
				SementeMyWork.Criar(Servicos, roteiro);
		}
		Vm = Servicos.GetRequiredService<PrincipalViewModel>();
		Janela = new Window
		{
			Width = Largura * Escala,
			Height = Altura * Escala,
			Content = new LayoutTransformControl
			{
				LayoutTransform = new Avalonia.Media.ScaleTransform(Escala, Escala),
				Child = new PrincipalView { DataContext = Vm },
			},
		};
		Janela.Show();
		Bombear();
		if (roteiro.StartsWith("garagem"))
			RoteiroGaragem.Executar();
		else
			RoteiroMyWork.Executar(roteiro);
		return 0;
	}

	public static void Bombear(int vezes = 6)
	{
		for (var i = 0; i < vezes; i++)
		{
			Dispatcher.UIThread.RunJobs();
			AvaloniaHeadlessPlatform.ForceRenderTimerTick();
		}
	}

	public static void Esperar(Task t)
	{
		var limite = DateTime.Now.AddSeconds(20);
		while (!t.IsCompleted && DateTime.Now < limite)
		{
			Dispatcher.UIThread.RunJobs();
			Thread.Sleep(10);
		}
		if (t.IsFaulted)
			throw t.Exception!;
		Bombear();
	}

	public static double Largura = 1250, Altura = 775, Escala = 2;

	public static void Tamanho(double w, double h)
	{
		Largura = w; Altura = h;
		Janela.Width = w * Escala;
		Janela.Height = h * Escala;
		Bombear(10);
	}

	/// <summary>Tira a marca "Base de teste" e troca o caminho do banco no rodapé.</summary>
	public static void Maquiar()
	{
		foreach (var tb in Janela.GetVisualDescendants().OfType<TextBlock>())
		{
			if (tb.Text == "Base de teste" && tb.Parent is Border b)
				b.IsVisible = false;
			if (tb.Text is { } t && (t.EndsWith("MinhaGaragem.db") || t.EndsWith("MyWork.db")))
				tb.Text = (t.StartsWith("Banco: ") ? "Banco: " : "") + Rodape;
		}
		foreach (var tb in Janela.GetVisualDescendants().OfType<SelectableTextBlock>())
			if (tb.Text is { } t && (t.EndsWith("MinhaGaragem.db") || t.EndsWith("MyWork.db")))
				tb.Text = (t.StartsWith("Banco: ") ? "Banco: " : "") + Rodape;
		Bombear();
	}

	public static string Rodape = @"C:\Users\Ricardo\AppData\Roaming\Minha Garagem 2027\dados\MinhaGaragem.db";

	public static void Foto(string nome, double escala = 2)
	{
		Bombear(4);
		Maquiar();
		Bombear(4);
		using var frame = Janela.CaptureRenderedFrame();
		if (frame is null)
			throw new InvalidOperationException("sem frame");
		var arq = Path.Combine(Saida, nome + ".png");
		frame.Save(arq);
		Console.WriteLine($"ok {arq} {frame.PixelSize}");
	}
}

internal sealed class PlataformaVitrine : IPlataforma
{
	public IAtualizacaoPrograma Atualizacao { get; } = new SemAtualizacao();

	public void Abrir(string caminhoOuLink) { }

	public IReadOnlyList<(string Nome, string Pasta)> PastasNuvem() => [];

	public void AoAbrir() { }

	public bool Celular { get; set; }
}

internal sealed class SemAtualizacao : IAtualizacaoPrograma
{
	public string? Sistema => null;

	public string? MotivoParaNaoAtualizar() => "vitrine";

	public Task<string> Baixar(
		string versao,
		MyWork.Core.Atualizacoes.ArquivoAtualizacao arquivo,
		IProgress<int> progresso,
		CancellationToken ct = default
	) => throw new NotSupportedException();

	public void Agendar(string instalador) { }

	public void AplicarSePendente() { }
}
