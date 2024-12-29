namespace _Project.Gameplay
{
    public interface IZombieSpawnerService
    {
        void StartSpawning(float minSpawnDelay, float maxSpawnDelay, Player zombieTarget, int maxZombies);
        void StopSpawning();
    }
}