using TestGeneratorLib;
using TestGeneratorLib.Implementation;

internal class Program
{
    private static void Main()
    {
        List<string> files = new(12);
        const string writeFolder = @"..\..\..\..\FolderToWrite\";
        const string path = @"..\..\..\..\ClassesFolder\TestClass";

        for (int i=0; i<files.Capacity; i++)
        {
            var fileName = path + i.ToString() + ".cs";
            files.Add(fileName);
        }

        var testGenerator = new TestGenerator(new NUnitCodeTestGenerator());
        var task=testGenerator.Generate(files.ToArray(), writeFolder);
        task.Wait();
    }
}