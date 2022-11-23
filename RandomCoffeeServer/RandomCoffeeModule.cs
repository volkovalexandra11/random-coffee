using Autofac;
using RandomCoffee.CredentialProviders;
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
        builder.RegisterType<GroupService>().AsSelf().SingleInstance();
        builder.RegisterType<UserService>().AsSelf().SingleInstance();
        builder.RegisterType<YdbService>().AsSelf().SingleInstance();
        base.Load(builder);
    }

    private readonly HostBuilderContext hostCtx;
}