using Autofac;
using Microsoft.AspNetCore.Identity;
using RandomCoffeeServer.Jobs;
using RandomCoffeeServer.Repositories;
using RandomCoffeeServer.Repositories.IdentityStorageProvider;
using RandomCoffeeServer.Services.Coffee;
using RandomCoffeeServer.Services.YandexCloud.Lockbox;
using RandomCoffeeServer.Services.YandexCloud.Ydb.YdbFactory;

namespace RandomCoffeeServer;

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
        builder.RegisterType<SchemeUpdateJob>().SingleInstance();
        builder.RegisterType<PopulateWithMockDataJob>().SingleInstance();
    }

    private void RegisterAuth(ContainerBuilder builder)
    {
        builder.RegisterType<CoffeeUserStore>().As<IUserStore<User>>().SingleInstance();
        builder.RegisterType<CoffeeRoleStore>().As<IRoleStore<Role>>().SingleInstance();
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