
using System;
using System.Windows.Documents;
using System.Windows.Media;
using XTC.oelMVCS;

namespace app
{
    class ConsoleLogger : Logger
    {
        public System.Windows.Controls.RichTextBox rtbLog { get; set; }
        protected override void trace(string _categoray, string _message)
        {
            this.appendTextColorful(string.Format("TRACE | {0} > {1}", _categoray, _message), Colors.Gray);
        }

        protected override void debug(string _categoray, string _message)
        {
            this.appendTextColorful(string.Format("DEBUG | {0} > {1}", _categoray, _message), Colors.Blue);
        }

        protected override void info(string _categoray, string _message)
        {
            this.appendTextColorful(string.Format("INFO | {0} > {1}", _categoray, _message), Colors.Green);
        }

        protected override void warning(string _categoray, string _message)
        {
            this.appendTextColorful(string.Format("WARN | {0} > {1}", _categoray, _message), Colors.Orange);
        }

        protected override void error(string _categoray, string _message)
        {
            this.appendTextColorful(string.Format("ERROR | {0} > {1}", _categoray, _message), Colors.Red);
        }

        protected override void exception(Exception _exception)
        {
            this.appendTextColorful(string.Format("EXCEPT | > {0}", _exception.ToString()), Colors.Purple);
        }

        private void appendTextColorful(string addtext, Color color)
        {
            var p = new Paragraph();
            var r = new Run(addtext);
            p.Inlines.Add(r);
            p.Foreground = new SolidColorBrush(color);
            rtbLog.Document.Blocks.Add(p);
        }
    }//class
}//namespace

