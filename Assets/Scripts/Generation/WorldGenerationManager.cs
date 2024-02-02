namespace Generation
{
    public class WorldGenerationManager : IWorldGenerationManager
    {
        private readonly WorldGeneratorBase _worldGenerator;
        
        public WorldGenerationManager(WorldGeneratorBase generator)
        {
            _worldGenerator = generator;
        }

        public void InitWorld()
        {
            _worldGenerator.Generate();
        }
    }

    public interface IWorldGenerationManager
    {
        void InitWorld();
    }
}