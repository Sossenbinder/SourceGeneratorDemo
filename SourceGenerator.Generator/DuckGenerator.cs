using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace SourceGenerator.Generator
{
	[Generator]
    public class DuckGenerator : ISourceGenerator
    {
	    internal class SyntaxReceiver : ISyntaxReceiver
	    {
		    public List<InterfaceDeclarationSyntax> DecoratorRequestingInterfaces { get; } =
			    new List<InterfaceDeclarationSyntax>();
			
			public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
			{
				// Get all interfaces with any attributes
				if (!(syntaxNode is InterfaceDeclarationSyntax ids))
			    {
				    return;
			    }

			    if (!ids.AttributeLists.Any())
			    {
				    return;
			    }

			    var requiresGeneration = ids.AttributeLists
				    .Select(x => x.Attributes)
				    .SelectMany(x => x)
				    .Select(x => x.Name)
				    .OfType<IdentifierNameSyntax>()
				    .Any(x => x.Identifier.ValueText == "GenerateDecorator");
				
				if (requiresGeneration)
				{
					DecoratorRequestingInterfaces.Add(ids);
				}
			}
	    }

	    public void Initialize(GeneratorInitializationContext context)
	    {
			context.RegisterForSyntaxNotifications(() => new SyntaxReceiver());
	    }

	    public void Execute(GeneratorExecutionContext context)
	    {
			// Get our SyntaxReceiver back
			if (!(context.SyntaxReceiver is SyntaxReceiver receiver))
		    {
			    throw new ArgumentException("Received invalid receiver in Execute step");
		    }

		    foreach (var ids in receiver.DecoratorRequestingInterfaces)
		    {
			    var methods = ids.DescendantNodes().OfType<MethodDeclarationSyntax>();

				var sourceBuilder = new StringBuilder();

				var interfaceName = ids.Identifier.ValueText;
				var className = $"Decorated{ids.Identifier.ValueText.Substring(1)}";
				
				sourceBuilder.Append("using System.Diagnostics;\n");
				sourceBuilder.Append("using SourceGenerator.Console.Interfaces;\n");
				sourceBuilder.Append("\n");
				sourceBuilder.Append("namespace SourceGenerator.Console.Decorators\n");
				sourceBuilder.Append("{\n");
				sourceBuilder.Append($"\tpublic class {className} : {interfaceName}\n");
				sourceBuilder.Append("\t{\n");
				sourceBuilder.Append($"\t\tprivate {interfaceName} _decorated;\n");
				sourceBuilder.Append("\n");
				sourceBuilder.Append($"\t\tpublic {className}({interfaceName} decorated)\n");
				sourceBuilder.Append("\t\t{\n");
				sourceBuilder.Append("\t\t\t_decorated = decorated;\n");
				sourceBuilder.Append("\t\t}\n");
				sourceBuilder.Append("\n");

				foreach (var method in methods)
				{
					var methodName = method.Identifier.Text;

					sourceBuilder.Append($"\t\tpublic {((PredefinedTypeSyntax) method.ReturnType).Keyword.Text} {methodName}(");

					var parameterIdentifiers = new List<string>();

					for (var i = 0; i < method.ParameterList.Parameters.Count; i++)
					{
						var parameter = method.ParameterList.Parameters[i];
						if (!(parameter.Type is PredefinedTypeSyntax typeSyntax))
						{
							return;
						}

						var type = typeSyntax.Keyword.Text;
						var name = parameter.Identifier.ValueText;

						sourceBuilder.Append($"{type} {name}");

						parameterIdentifiers.Add(name);

						if (i != method.ParameterList.Parameters.Count - 1)
						{
							sourceBuilder.Append(",");
						}
					}
					sourceBuilder.Append(")\n");
					sourceBuilder.Append("\t\t{\n");

					sourceBuilder.Append("\t\t\tvar sw = Stopwatch.StartNew();\n");
					sourceBuilder.Append($"\t\t\t_decorated.{methodName}({string.Join(",", parameterIdentifiers)});\n");
					sourceBuilder.Append("\t\t\tsw.Stop();\n");
					sourceBuilder.Append("\t\t\tSystem.Console.WriteLine($\"Elapsed time: {sw.Elapsed}\");\n");
					sourceBuilder.Append("\t\t}\n");
					sourceBuilder.Append("\n");
				}

				sourceBuilder.Append("\t}\n");
				sourceBuilder.Append("}");

				context.AddSource(ids.Identifier.ValueText, SourceText.From(sourceBuilder.ToString(), Encoding.UTF8));
		    }
	    }
    }
}
