namespace Generation.Algorithms
{
    public interface IGenerationAlgorithm
    {
        void Init();
        void Generate(ref bool[,,] walls);
    }
}