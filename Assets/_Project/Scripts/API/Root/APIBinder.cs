using Zenject;

namespace _Project.API
{
    public class APIBinder
    {
        private readonly DiContainer _container;

        public APIBinder(DiContainer container)
        {
            _container = container;
        }
        
        public void Bind()
        {
            _container.Bind<IAPIEnvironmentService>().To<YandexGamesEnvironmentService>().FromNew().AsSingle().NonLazy();
            _container.Bind<IADService>().To<YandexGamesADService>().FromNew().AsSingle().NonLazy();
            _container.Bind<IDeviceProvider>().To<YandexGamesDeviceProvider>().FromNew().AsSingle().NonLazy();
            _container.Bind<ILocalizationProvider>().To<YandexGamesLocalizationProvider>().FromNew().AsSingle().NonLazy();
            _container.Bind<ILeaderBoardService>().To<YandexGamesLeaderBoardService>().FromNew().AsSingle().NonLazy();
        }
    }
}