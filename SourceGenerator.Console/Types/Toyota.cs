using System.Threading;
using SourceGenerator.Console.Interfaces;

namespace SourceGenerator.Console.Types
{
    public class Toyota : ICar
    {
	    public void Drive()
	    {
		    System.Console.WriteLine("Driving");
		    Thread.Sleep(100);
	    }
    }
}
