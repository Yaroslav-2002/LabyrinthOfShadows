namespace Entities.States
{
    public static class EnemyStateFactory
    {
        public static T GetState<T>() where T : ICreatureState, new()
        {
            return new T();
        }
    }
}