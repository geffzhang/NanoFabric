namespace CFT.NanoFabric.Core
{
    public interface IRegistryHost : IManageServiceInstances, 
        IManageHealthChecks,
        IResolveServiceInstances,
        IHaveKeyValues
    {
    }
}
