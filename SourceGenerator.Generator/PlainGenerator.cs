using System.Diagnostics;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace SourceGenerator.Generator
{
	[Generator]
	public class PlainGenerator : ISourceGenerator
	{
		public void Initialize(GeneratorInitializationContext context)
		{
		}

		public void Execute(GeneratorExecutionContext context)
		{
			var callingEntrypoint = context.Compilation.GetEntryPoint(context.CancellationToken);

			var sb = new StringBuilder();

			sb.Append(@$"
	using SourceGenerator.Console.Interfaces;

	namespace {$"{callingEntrypoint!.ContainingNamespace.ContainingNamespace.Name}.{callingEntrypoint!.ContainingNamespace.Name}"}
	{{
	public class CustomDuck : IAnimal
	{{
		public void MakeNoise()
		{{
			System.Console.WriteLine(""Hello from custom duck"");
		}}

		public void MakeNoise(string withNoise)
		{{
			System.Console.WriteLine($""Hello from custom duck with noise: {{withNoise}}"");
		}}
	}}
	}}
			");

			context.AddSource("CustomDuck", SourceText.From(sb.ToString(), Encoding.UTF8));
		}
	}
}
