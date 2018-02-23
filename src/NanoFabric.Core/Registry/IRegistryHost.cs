namespace NanoFabric.Core
{
    public interface IRegistryHost : IManageServiceInstances, 
        IManageHealthChecks,
        IResolveServiceInstances
    {
    }
}
