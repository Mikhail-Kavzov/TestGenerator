using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestGeneratorLib.Interfaces
{
    public interface ICodeTestGenerator
    {
        Task<string[]> Generate(string text);
    }
}
