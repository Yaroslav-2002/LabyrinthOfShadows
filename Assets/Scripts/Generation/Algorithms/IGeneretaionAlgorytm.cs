using System.Collections.Generic;

namespace Generation.Algorithms
{
    public interface IGenerationAlgorithm
    {
        IEnumerator<bool[,]> GenerateRow();
    }
}