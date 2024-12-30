using R3;

namespace _Project.Data
{
    public class GameDataProxy
    {
        public ReactiveProperty<float> MusicVolume = new ReactiveProperty<float>();
        public ReactiveProperty<float> SoundVolume = new ReactiveProperty<float>();
        
        public GameDataProxy(GameData data)
        {
            MusicVolume.Value = data.MusicVolume;
            SoundVolume.Value = data.SoundVolume;

            MusicVolume.Subscribe(value => data.MusicVolume = value);
            SoundVolume.Subscribe(value => data.SoundVolume = value);
        }
    }
}