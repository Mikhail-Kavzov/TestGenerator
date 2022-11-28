using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Threading.Tasks.Dataflow;
using TestGeneratorLib.Interfaces;

namespace TestGeneratorLib
{
    public sealed class TestGenerator
    {
        private readonly ICodeTestGenerator _generator;

        public TestGenerator(ICodeTestGenerator generator)
        {
            _generator = generator;
        }

        public Task Generate(string[] files, string writeFolder, int maxDegreeOfParallelism = 5)
        {
            var exDataFlowOptions = new ExecutionDataflowBlockOptions()
            {
                MaxDegreeOfParallelism = maxDegreeOfParallelism,
            };
            var readFileBlock = new TransformBlock<string, string>(
                async fileName => await ReadFileAsync(fileName),
                exDataFlowOptions);

            var generateCodeBlock = new TransformManyBlock<string, string>(
                    async text => await _generator.Generate(text),
                    exDataFlowOptions);

            var writeFileBlock = new ActionBlock<string>(
                async text => await WriteFileAsync(text, writeFolder),
                exDataFlowOptions);

            var linkOptions = new DataflowLinkOptions { PropagateCompletion = true };
            readFileBlock.LinkTo(generateCodeBlock, linkOptions);
            generateCodeBlock.LinkTo(writeFileBlock, linkOptions);

            foreach (var file in files)
            {
                readFileBlock.Post(file);
            }
            readFileBlock.Complete();
            return readFileBlock.Completion;
        }

        private static async Task<string> ReadFileAsync(string fileName)
        {
            using StreamReader sr = new(fileName);
            return await sr.ReadToEndAsync();
        }

        private static async Task WriteFileAsync(string text, string writeFolder)
        {
            var root = await CSharpSyntaxTree.ParseText(text).GetRootAsync();
            var classDeclaration = root.DescendantNodes().OfType<ClassDeclarationSyntax>().First();
            var fileName = classDeclaration.Identifier.Text + ".cs";
            using StreamWriter sr = new(writeFolder + fileName);
            await sr.WriteAsync(text);
        }
    }
}