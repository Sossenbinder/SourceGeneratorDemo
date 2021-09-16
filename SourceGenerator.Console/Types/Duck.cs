using System.Threading;
using SourceGenerator.Console.Interfaces;

namespace SourceGenerator.Console.Types
{
    public class Duck : IAnimal
    {
	    public void MakeNoise()
	    {
		    System.Console.WriteLine("Quack");
			Thread.Sleep(20);
	    }

	    public void MakeNoise(string withNoise)
		{
			System.Console.WriteLine($"Quack with {withNoise}");
			Thread.Sleep(50);
		}
    }
}
