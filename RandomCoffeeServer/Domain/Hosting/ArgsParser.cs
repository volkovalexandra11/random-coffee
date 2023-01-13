namespace RandomCoffeeServer.Domain.Hosting;

public static class ArgsParser
{
    public static ApplicationMode GetApplicationModes()
    {
        var modesArg = Environment.GetEnvironmentVariable("APPLICATION_MODES");
        if (modesArg is null)
            throw new ArgumentException("No env var");

        var modes = modesArg.Split(',');
        var allApplicationModes = ApplicationMode.None;
        foreach (var cliMode in modes)
        {
            var mode = Enum.GetValues<ApplicationMode>()
                .FirstOrDefault(
                    mode => string.Equals(cliMode, mode.ToString(), StringComparison.InvariantCultureIgnoreCase));
            if (mode.Equals(ApplicationMode.None))
            {
                throw new ArgumentException($"Unknown cli argument: {cliMode}");
            }

            allApplicationModes |= mode;
        }

        if (allApplicationModes.Equals(ApplicationMode.None))
        {
            throw new ArgumentException("No application modes provided");
        }

        return allApplicationModes;
    }
}