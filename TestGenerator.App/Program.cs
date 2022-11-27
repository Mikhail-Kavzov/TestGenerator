using TestGeneratorLib.Implementation;
using TestGeneratorLib.Interfaces;

internal class Program
{
    private static async Task Main()
    {
        string res;
        using StreamReader sr = new("C:\\Users\\User\\source\\repos\\TestGenerator\\TestGeneratorLib\\Implementation\\NUnitCodeTestGenerator.cs");
        res = await sr.ReadToEndAsync();
        ICodeTestGenerator generator = new NUnitCodeTestGenerator();
        var result = generator.Generate(res);
        foreach (var r in result)
        {
            Console.WriteLine(r);
        }
    }
}