using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestGeneratorLib
{
    public sealed class GeneratorConfig
    {
        public int MaxDegreeOfRead { get; } = 5;
        public int MaxDegreeOfGenerate { get; } = 5;
        public int MaxDegreeOfWrite { get; } = 5;

        public GeneratorConfig(int maxDegreeOfRead, int maxDegreeOfGenerate, int maxDegreeOfWrite)
        {
            MaxDegreeOfRead = maxDegreeOfRead;
            MaxDegreeOfGenerate = maxDegreeOfGenerate;
            MaxDegreeOfWrite = maxDegreeOfWrite;
        }

        public GeneratorConfig() {}
    }
}
