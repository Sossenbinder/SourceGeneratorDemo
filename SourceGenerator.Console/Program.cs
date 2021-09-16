using System;
using SourceGenerator.Console.Interfaces;
using SourceGenerator.Console.Types;
using SourceGenerator.Console.Decorators;

Console.WriteLine("Running application");
Console.WriteLine("---");

IAnimal duck = new Duck();

Console.WriteLine("Regular duck:");
duck.MakeNoise();
duck.MakeNoise("additional noise");

Console.WriteLine("Decorated duck:");
duck = new DecoratedAnimal(duck);
duck.MakeNoise();
duck.MakeNoise("additional noise");


ICar car = new Toyota();
Console.WriteLine("Regular car:");
car.Drive();
Console.WriteLine("Decorated car:");
car = new DecoratedCar(car);
car.Drive();

