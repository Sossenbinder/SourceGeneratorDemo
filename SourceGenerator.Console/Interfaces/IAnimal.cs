namespace SourceGenerator.Console.Interfaces
{
    [GenerateDecorator]
    public interface IAnimal
    {
	    void MakeNoise();
	    void MakeNoise(string withNoise);
    }
}
