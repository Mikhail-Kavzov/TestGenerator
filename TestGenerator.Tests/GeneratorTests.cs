using NUnit.Framework;
using TestGeneratorLib;
using TestGeneratorLib.Implementation;
using TestGeneratorLib.Interfaces;

namespace TestGeneratorTests.Tests
{
    [TestFixture]
    public class GeneratorTests
    {
        private readonly TestGenerator _testGenerator;
        private const string writeFolder = @"..\..\..\..\TestGenerator\FolderToWrite\";
        private const string path = @"..\..\..\..\ClassesFolder\TestClass";
        private readonly List<string> _files = new(12);

        public GeneratorTests()
        {
            _testGenerator = new(new NUnitCodeTestGenerator());
            for (int i = 0; i < _files.Capacity; i++)
            {
                var fileName = path + i.ToString() + ".cs";
                _files.Add(fileName);
            }
        }

        /// <summary>
        /// Try to read a file that doesn't exist
        /// </summary>
        [Test]
        public void FileNotFoundTest()
        {
            Assert.Throws<FileNotFoundException>(() =>
            _testGenerator.Generate(new string[] { "someFile.cs" }, writeFolder));
        }

        /// <summary>
        /// Test normal work with one degree
        /// </summary>
        [Test]
        public void CorrectTestWithOneDegree()
        {
            var task = _testGenerator.Generate(_files.ToArray(), writeFolder, 1);
            Assert.NotNull(task);
            task.Wait();
        }

        /// <summary>
        /// Test normal work with many degree
        /// </summary>
        [Test]
        public void CorrectTestWithManyDegree()
        {
            var task = _testGenerator.Generate(_files.ToArray(), writeFolder, 4);
            Assert.NotNull(task);
            task.Wait();
        }


    }
}