namespace Generation
{
    public interface IGenerationAlgorithm
    {
        void Init();
        void Generate(ref bool[,] row);
    }
}