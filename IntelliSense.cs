using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Editing;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using System.Windows.Media;

namespace Dynamic_C_Notebook
{
    public partial class MainWindow
    {
        /// <summary>
        /// 随用户输入进行代码提示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextArea_TextEntered(object sender, TextCompositionEventArgs e)
        {
            _completionWindow = new CompletionWindow((TextArea)sender);
            IList<ICompletionData> completionData = _completionWindow.CompletionList.CompletionData;

            // TODO 代码提示完善
            if (e.Text == "c")
            {
                completionData.Add(new CompletionData("char"));
                _completionWindow.Show();
            }
            else if (e.Text == "s")
            {
                completionData.Add(new CompletionData("signed"));
                completionData.Add(new CompletionData("struct"));
                _completionWindow.Show();
            }

            try
            {
                _completionWindow.Closed += (o, args) => _completionWindow = null;
            }
            catch (NullReferenceException)
            {
                // ignored
            }
        }

        /// <summary>
        /// 代码提示窗口
        /// </summary>
        private CompletionWindow _completionWindow;
    }

    public class CompletionData : ICompletionData
    {
        public CompletionData(string text) { Text = text; }

        public ImageSource Image => null;

        public string Text { get; }

        public object Content => Text;

        public object Description
        {
            get
            {
                return "Description for " + Text;
            }
        }

        public double Priority { get; }

        public void Complete(TextArea textArea, ISegment completionSegment, EventArgs insertionRequestEventArgs)
        {
            textArea.Document.Replace(completionSegment, Text.Remove(0,1));
        }
    }
}
