using NUnit.Framework;
using TestGeneratorLib;
using TestGeneratorLib.Implementation;

namespace TestGeneratorTests.Tests
{
    [TestFixture]
    public class GeneratorTests
    {
        private readonly TestGenerator _defaultTestGenerator;
        private readonly GeneratorConfig _defaultGeneratorConfig = new();
        private const string writeFolder = @"..\..\..\..\TestGenerator\FolderToWrite\";
        private const string path = @"..\..\..\..\ClassesFolder\TestClass";
        private readonly string[] _files = new string[12];

        public GeneratorTests()
        {
            _defaultTestGenerator = new(new NUnitCodeTestGenerator(), _defaultGeneratorConfig);
            for (int i = 0; i < _files.Length; i++)
            {
                var fileName = path + i.ToString() + ".cs";
                _files[i] = fileName;
            }
        }

        /// <summary>
        /// Try to read a file that doesn't exist
        /// </summary>
        [Test]
        public void FileNotFoundTest()
        {
            Assert.Throws<FileNotFoundException>(() =>
            _defaultTestGenerator.Generate(new string[] { "someFile.cs" }, writeFolder));
        }

        /// <summary>
        /// Test normal work with one degree
        /// </summary>
        [Test]
        public void CorrectTestWithOneDegree()
        {
            var oneGeneratorConfig = new GeneratorConfig(1, 1, 1);
            var testOneGenerator = new TestGenerator(new NUnitCodeTestGenerator(), oneGeneratorConfig);
            var task = testOneGenerator.Generate(_files, writeFolder);
            Assert.That(task, Is.Not.Null);
            task.Wait();
        }

        /// <summary>
        /// Test normal work with many degree
        /// </summary>
        [Test]
        public void CorrectTestWithManyDegree()
        {
            var task = _defaultTestGenerator.Generate(_files, writeFolder);
            Assert.That(task, Is.Not.Null);
            task.Wait();
        }
    }
}