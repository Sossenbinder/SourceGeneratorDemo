using SourceGenerator.Console.Interfaces;
using SourceGenerator.Console.Types;
using SourceGenerator.Console.Decorators;

namespace SourceGenerator.Console
{
	public class Program
	{
		public static void Main(string[] args)
		{
			System.Console.WriteLine("Running application");
			System.Console.WriteLine("---");

			IAnimal duck = new Duck();

			System.Console.WriteLine("Regular duck:");
			duck.MakeNoise();
			duck.MakeNoise("additional noise");

			System.Console.WriteLine("Decorated duck:");
			duck = new DecoratedAnimal(duck);
			duck.MakeNoise();
			duck.MakeNoise("additional noise");

			System.Console.WriteLine("Custom duck:");
			var customDuck = new CustomDuck.Namespace.CustomDuck();
			customDuck.MakeNoise();
			customDuck.MakeNoise("Additional noise");

			ICar car = new Toyota();
			System.Console.WriteLine("Regular car:");
			car.Drive();
			System.Console.WriteLine("Decorated car:");
			car = new DecoratedCar(car);
			car.Drive();
		}
	}
}