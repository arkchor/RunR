using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Archersoft.RunR
{
    public class TextBlockWriter : TextWriter
    {
        private static readonly Dispatcher Dispatcher = Application.Current.Dispatcher;

        private readonly TextBox _textBox;

        public TextBlockWriter(TextBox textBox)
        {
            _textBox = textBox;
        }

        public override void Write(char value)
        {
            Dispatcher.Invoke(() =>
            {
                _textBox.Text += value;
                _textBox.ScrollToEnd();
            });
        }

        public override void Write(string value)
        {
            Dispatcher.Invoke(() =>
            {
                _textBox.Text += value;
                _textBox.ScrollToEnd();
            });
        }

        public override Encoding Encoding
            => Encoding.ASCII;
    }
}