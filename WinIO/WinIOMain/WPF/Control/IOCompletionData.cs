using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Editing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace WinIO.WPF.Control
{
    public class IOCompletionData : ICompletionData
    {
		public IOCompletionData(string text)
		{
			this.Text = text;
		}

		public ImageSource Image
		{
			get { return null; }
		}

		public string Text { get; private set; }

		public object Content
		{
			get { return this.Text; }
		}

		public object Description
		{
			get { return null; }
		}

		public double Priority { get { return 0; } }

		public void Complete(TextArea textArea, ISegment completionSegment, EventArgs insertionRequestEventArgs)
		{
			textArea.Document.Replace(completionSegment, this.Text);
		}
	}
}
