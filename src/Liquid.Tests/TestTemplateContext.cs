using System;
using Liquid.Core.Configuration;
using Liquid.Core.Context;
using Liquid.Core.Telemetry;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;
using Microsoft.Extensions.Logging.Console;

namespace Liquid.Tests
{
    /// <summary>
    /// Test Template Context Class.
    /// </summary>
    /// <typeparam name="TContext">The type of the context.</typeparam>
    [TestFixture]
    public abstract class TestTemplateContext<TContext>
    {
        /// <summary>
        /// Gets the subject under test.
        /// </summary>
        /// <value>
        /// The subject under test.
        /// </value>
        protected virtual TContext Sut => ServiceProvider.GetRequiredService<TContext>();

        /// <summary>
        /// Gets the service provider.
        /// </summary>
        /// <value>
        /// The service provider.
        /// </value>
        protected virtual IServiceProvider ServiceProvider { get; private set; }

        /// <summary>
        /// Executes the main setup before each test.
        /// </summary>
        [SetUp]
        protected virtual void MainSetup()
        {
            var services = new ServiceCollection();
            AddServices(services);
            ServiceProvider = services.BuildServiceProvider();
        }

        /// <summary>
        /// Teardown the context.
        /// </summary>
        [TearDown]
        public void Teardown()
        {
            TestCleanup();
            ServiceProvider = null;
        }

        /// <summary>
        /// Connect the Liquid basic services to the service collection.
        /// </summary>
        /// <param name="services">The services.</param>
        protected virtual void AddServices(IServiceCollection services)
        {
            services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, ConsoleLoggerProvider>());
            LoggerProviderOptions.RegisterProviderOptions<ConsoleLoggerOptions, ConsoleLoggerProvider>(services);
            services.Configure(new Action<ConsoleLoggerOptions>(options => options.DisableColors = false));
            services.AddSingleton(LoggerFactory.Create(builder => { builder.AddConsole(); }));
            IConfiguration configurationRoot = new ConfigurationBuilder().AddLightConfigurationFile().Build();
            services.AddSingleton(configurationRoot);
            services.AddDefaultTelemetry();
            services.AddDefaultContext();

            ConfigureServices(services);
        }

        /// <summary>
        /// Configure custom services.
        /// </summary>
        /// <returns></returns>
        protected abstract void ConfigureServices(IServiceCollection services);

        /// <summary>
        /// Cleans up the test after execution.
        /// </summary>
        protected abstract void TestCleanup();
    }

}