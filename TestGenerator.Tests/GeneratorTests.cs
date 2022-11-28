using NUnit.Framework;
using TestGeneratorLib.Implementation;
using TestGeneratorLib.Interfaces;

namespace TestGenerator.Tests
{
    [TestFixture]
    public class GeneratorTests
    {
        private readonly ICodeTestGenerator _generator; 

        public GeneratorTests()
        {
            _generator = new NUnitCodeTestGenerator();
        }

        [Test]
        public void FileNotFoundTest()
        {
           
        }
    }
}