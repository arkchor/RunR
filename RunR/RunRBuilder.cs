using System;
using System.Collections.Generic;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;

namespace Archersoft.RunR
{
    public class RunRBuilder
    {
        private static IServiceProvider _serviceProvider;

        private readonly IServiceCollection _serviceCollection;
        private readonly RunRControl _runRControl;

        private Action<string, Exception> _onUnhandledExceptionAction;

        private RunRBuilder(IServiceCollection serviceCollection, RunRControl runRControl)
        {
            _serviceCollection = serviceCollection;
            _runRControl = runRControl;
        }

        public static RunRBuilder Configure(RunRControl runRControl)
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddSingleton(Application.Current.Dispatcher);
            serviceCollection.AddSingleton(runRControl);

            serviceCollection.AddTransient<RunRUserInterfaceGateway>();

            return new RunRBuilder(serviceCollection, runRControl);
        }

        public RunRBuilder AddCommand<TCommand>(string uniqueName, string displayName, string description)
            where TCommand : class, ICommand
        {
            var commandDefinition = new CommandDefinition(
                uniqueName,
                displayName,
                description,
                serviceProvider => serviceProvider.GetService<TCommand>());

            _serviceCollection.AddSingleton(commandDefinition);
            _serviceCollection.AddTransient<TCommand>();

            return this;
        }

        public RunRBuilder AddCustomServices(Action<IServiceCollection> setupAction)
        {
            setupAction(_serviceCollection);

            return this;
        }

        public RunRBuilder OnUnhandledException(Action<string, Exception> onUnhandledExceptionAction)
        {
            _onUnhandledExceptionAction = onUnhandledExceptionAction;

            return this;
        }

        public void Build()
        {
            _serviceProvider = new DefaultServiceProviderFactory()
                .CreateServiceProvider(_serviceCollection);

            var serviceScopeFactory = _serviceProvider
                .GetService<IServiceScopeFactory>();

            var commandDefinitions = _serviceProvider
                .GetService<IEnumerable<CommandDefinition>>();

            _runRControl.SetDependencies(serviceScopeFactory, commandDefinitions, _onUnhandledExceptionAction);
        }
    }
}