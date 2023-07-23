using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RankedDasEstrelas.Domain.Interfaces.Repositories;
using RankedDasEstrelas.Domain.Interfaces.Services;
using RankedDasEstrelas.Domain.Services;
using RankedDasEstrelas.Domain.Validations;
using RankedDasEstrelas.Infra.DbContext;
using RankedDasEstrelas.Infra.Repositories;
using RankedDasEstrelas.Selenium.Interfaces;
using RankedDasEstrelas.Selenium.Services;
using System;
using System.Threading.Tasks;

namespace RankDasEstrelas.Bot
{
    public class Program
    {
        public MongoDBContext MongoDbContext { get; private set; }
        protected IConfiguration Configuration { get; private set; }
        public Program()
        {
            Configuration = BuildConfiguration();
            MongoDbContext = new MongoDBContext(Configuration.GetConnectionString("MongoConnection"));
        }

        private static void Main() => new Program().RodarBotAsync().GetAwaiter().GetResult();
        private static IConfiguration BuildConfiguration()
        {
#if DEBUG
            var environment = "Development";
#else
            var environment = "Production";
#endif

            return new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables().Build();
        }
        private async Task RodarBotAsync()
        {
            await new Bot(Configuration.GetSection("appSettings")["Token"], BuildServices()).ConectarAsync();
            await Task.Delay(-1);
        }
        private ServiceProvider BuildServices()
        {
            return new ServiceCollection()
                .AddSingleton(MongoDbContext)
                .AddScoped<ISeleniumService, SeleniumService>()
                .AddScoped<IPlayerRepository, PlayerRepository>()
                .AddScoped<IMatchRepository, MatchRepository>()
                .AddScoped<IRankingTableRepository, RankingTableRepository>()
                .AddScoped<IMatchResultService, MatchResultService>()
                .AddScoped<IRankingTableService, RankingTableService>()
                .AddScoped<IValidations, Validations>()
                .BuildServiceProvider();
        }
    }
}