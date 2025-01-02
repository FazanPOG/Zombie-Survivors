using R3;

namespace _Project.API
{
    public interface ILocalizationProvider
    {
        ILocalizationAsset LocalizationAsset { get; }

        Observable<ILocalizationAsset> LoadLocalizationAsset();
    }
}