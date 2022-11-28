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

        /// <summary>
        /// Generate source test code
        /// </summary>
        /// <param name="files">files array to read</param>
        /// <param name="writeFolder">folder to write</param>
        /// <param name="maxDegreeOfParallelism">parallelism degree</param>
        /// <returns></returns>
        /// <exception cref="FileNotFoundException"></exception>
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
                    text => _generator.Generate(text),
                    exDataFlowOptions);

            var writeFileBlock = new ActionBlock<string>(
                async text => await WriteFileAsync(text, writeFolder),
                exDataFlowOptions);

            var linkOptions = new DataflowLinkOptions { PropagateCompletion = true };
            readFileBlock.LinkTo(generateCodeBlock, linkOptions);
            generateCodeBlock.LinkTo(writeFileBlock, linkOptions);

            if (!Directory.Exists(writeFolder))
            {
                Directory.CreateDirectory(writeFolder);
            }

            foreach (var file in files)
            {
                if (!File.Exists(file))
                {
                    throw new FileNotFoundException($"File not found: {file}");
                }
                readFileBlock.Post(file);
            }
            readFileBlock.Complete();
            return writeFileBlock.Completion;
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
            var path = writeFolder + classDeclaration.Identifier.Text + "_" + Guid.NewGuid().ToString() + ".cs";
            using StreamWriter sr = new(path);
            await sr.WriteAsync(text);
        }
    }
}