namespace _Project.Data
{
    public interface IGameDataProvider
    {
        GameDataProxy GameDataProxy { get; }

        void SaveGameData();
        GameDataProxy LoadGameData();
    }
}