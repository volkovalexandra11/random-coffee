using Autofac;
using RandomCoffee.CredentialProviders;
using RandomCoffee.Repositories;
using Ydb.Sdk.Auth;

namespace RandomCoffee;

public class RandomCoffeeModule : Module
{
    public RandomCoffeeModule(HostBuilderContext hostCtx)
    {
        this.hostCtx = hostCtx;
    }

    protected override void Load(ContainerBuilder builder)
    {
        builder.Register((ctx) => CredentialProviderFactory.GetSa().GetAwaiter().GetResult()).As<ICredentialsProvider>().SingleInstance();

        RegisterServices(builder);
        RegisterRepositories(builder);
    }

    private void RegisterServices(ContainerBuilder builder)
    {
        builder.RegisterType<GroupService>().SingleInstance(); // <=> .AsSelf()
        builder.RegisterType<UserService>().SingleInstance();
        
        builder.RegisterType<YdbService>().SingleInstance();
    }

    private void RegisterRepositories(ContainerBuilder builder)
    {
        builder.RegisterType<GroupRepository>().SingleInstance();
        builder.RegisterType<GroupUserRepository>().SingleInstance();
        builder.RegisterType<UserRepository>().SingleInstance();
    }

    private readonly HostBuilderContext hostCtx;
}