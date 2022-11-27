﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
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

        public async Task Generate(string[] files, string writeFolder, int maxDegreeOfParallelism = 5)
        {
        }

        private async Task<string> ReadFileAsync(string fileName)
        {
            using StreamReader sr = new(fileName);
            return await sr.ReadToEndAsync();
        }
    }
}