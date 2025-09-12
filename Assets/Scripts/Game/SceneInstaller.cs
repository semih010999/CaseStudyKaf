using Zenject;

public class SceneInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<PlayerMovement>().FromComponentInHierarchy().AsSingle();
        Container.Bind<PlayerAttack>().FromComponentInHierarchy().AsSingle();
        Container.Bind<BulletObjectPool>().FromComponentInHierarchy().AsSingle();
        Container.Bind<GameManager>().FromComponentInHierarchy().AsSingle();
        Container.Bind<AddressablesManager>().FromComponentInHierarchy().AsSingle();
    }
}
