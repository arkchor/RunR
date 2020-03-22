using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;

namespace Archersoft.RunR
{
    public partial class RunRControl : UserControl
    {
        private IServiceScopeFactory _serviceScopeFactory;
        private Action<string, Exception> _onUnhandledExceptionAction;

        public RunRControl()
        {
            InitializeComponent();
        }

        internal void SetDependencies(
            IServiceScopeFactory serviceScopeFactory,
            IEnumerable<CommandDefinition> commandDefinitions,
            Action<string, Exception> onUnhandledExceptionAction)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _onUnhandledExceptionAction = onUnhandledExceptionAction;

            CommandComboBox.ItemsSource = commandDefinitions;
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            Console.SetOut(new TextBlockWriter(ConsoleOutputTextBox));

            CommandComboBox.SelectedIndex = 0;
        }

        private async void RunButton_Click(object sender, RoutedEventArgs e)
        {
            var commandDefinition = (CommandDefinition)CommandComboBox.SelectionBoxItem;
            var commandDisplayName = commandDefinition.DisplayName;
            var stopwatch = new Stopwatch();

            try
            {
                Reset();
                BlockUserInterface();

                Console.WriteLine($"Running {commandDisplayName}...");
                Console.WriteLine();

                stopwatch.Start();

                using var scope = _serviceScopeFactory.CreateScope();
                var command = commandDefinition.CommandFactoryMethod(scope.ServiceProvider);
                await command.Execute();

                stopwatch.Stop();

                Console.WriteLine();
                Console.WriteLine($"Succeed {commandDisplayName}. Duration: {stopwatch.Elapsed:g}");
            }
            catch (Exception exception)
            {
                stopwatch.Stop();

                Console.WriteLine($"Failed {commandDisplayName} with unexpected error:");
                Console.WriteLine(exception.ToString());
                Console.WriteLine($"Duration: {stopwatch.Elapsed:g}");

                _onUnhandledExceptionAction?.Invoke(commandDisplayName, exception);
            }

            ReleaseUserInterface();
        }

        private void CommandComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Reset();

            var commandDefinition = e.AddedItems.Cast<CommandDefinition>().First();
            DescriptionTextBox.Text = commandDefinition.Description;
        }

        private void Reset()
        {
            ConsoleOutputTextBox.Text = string.Empty;
            CommandProgressBar.Value = 0;
        }

        private void BlockUserInterface()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            CommandComboBox.IsEnabled = false;
            RunButton.IsEnabled = false;
        }

        private void ReleaseUserInterface()
        {
            RunButton.IsEnabled = true;
            CommandComboBox.IsEnabled = true;
            Mouse.OverrideCursor = null;
        }
    }
}
