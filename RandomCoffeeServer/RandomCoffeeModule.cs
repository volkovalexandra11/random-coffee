using Autofac;
using RandomCoffee.Repositories;
using RandomCoffee.Services;
using RandomCoffeeServer.Services.YandexCloud.Lockbox;

namespace RandomCoffee;

public class RandomCoffeeModule : Module
{
    public RandomCoffeeModule(HostBuilderContext hostCtx)
    {
        this.hostCtx = hostCtx;
    }

    protected override void Load(ContainerBuilder builder)
    {
        RegisterJobs(builder);
        RegisterServices(builder);
        RegisterRepositories(builder);
    }

    private void RegisterJobs(ContainerBuilder builder)
    {
        builder.RegisterType<SchemeUpdater>().SingleInstance();
        builder.RegisterType<PopulateWithMockDataJob>().SingleInstance();
    }

    private void RegisterServices(ContainerBuilder builder)
    {
        builder.RegisterType<LockboxFactory>().SingleInstance();
        builder.Register(
                ctx => ctx.Resolve<LockboxFactory>().Create(hostCtx.HostingEnvironment))
            .SingleInstance();

        builder.RegisterType<YdbFactory>().SingleInstance();
        builder.Register(ctx =>
                ctx.Resolve<YdbFactory>().Create(hostCtx.HostingEnvironment, ctx.Resolve<ILoggerFactory>()))
            .SingleInstance();

        builder.RegisterType<GroupService>().SingleInstance(); // <=> .AsSelf()
        builder.RegisterType<UserService>().SingleInstance();
    }

    private void RegisterRepositories(ContainerBuilder builder)
    {
        builder.RegisterType<GroupRepository>().SingleInstance();
        builder.RegisterType<GroupUserRepository>().SingleInstance();
        builder.RegisterType<UserRepository>().SingleInstance();
    }

    private readonly HostBuilderContext hostCtx;
}