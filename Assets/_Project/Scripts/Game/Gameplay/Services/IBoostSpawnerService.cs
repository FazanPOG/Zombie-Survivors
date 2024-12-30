namespace _Project.Gameplay
{
    public interface IBoostSpawnerService
    {
        void StartSpawning(float minSpawnDelay, float maxSpawnDelay, int maxBoosts);
        void StopSpawning();
    }
}