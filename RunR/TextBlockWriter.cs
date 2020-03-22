using System.IO;
using System.Text;
using System.Windows.Controls;

namespace Archersoft.RunR
{
    public class TextBlockWriter : TextWriter
    {
        private readonly TextBox _textBox;

        public TextBlockWriter(TextBox textBox)
        {
            _textBox = textBox;
        }

        public override void Write(char value)
        {
            _textBox.Text += value;
            _textBox.ScrollToEnd();

        }

        public override void Write(string? value)
        {
            _textBox.Text += value;
            _textBox.ScrollToEnd();

        }

        public override Encoding Encoding
            => Encoding.ASCII;
    }
}