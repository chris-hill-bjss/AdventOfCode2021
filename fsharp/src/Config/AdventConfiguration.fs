namespace AdventOfCode2021.Config

module AdventConfiguration =

    open Microsoft.Extensions.Configuration

    [<CLIMutable>]
    type AdventClientConfig = {
        Cookie: string
    }

    [<CLIMutable>]
    type Configuration = {
        AdventClient: AdventClientConfig
    }

    let configuration = 
        (new ConfigurationBuilder())
            .AddJsonFile("./appsettings.json", false, true)
            .AddJsonFile("./appsettings.local.json", true, true)
            .Build()
            .Get<Configuration>()
    
