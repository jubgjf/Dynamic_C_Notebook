using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Highlighting;

namespace Dynamic_C_Notebook
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            CheckOpenFolder();
            AddCodeCellButton_Click(new object(), new RoutedEventArgs());
        }

        /// <summary>
        /// 检查未打开文件夹时，将显示提示信息，提示用户打开文件夹
        /// </summary>
        private void CheckOpenFolder()
        {
            if (FilesSidePanel.Children.Count == 0)
            {
                Label label = new Label { Content = "尚未打开文件夹", Height = 30 };
                Button button = new Button
                {
                    Content = "打开文件夹",
                    Width = 100,
                    Height = 25,
                    Background = Brushes.White
                };
                button.Click += OpenFolder_Click;
                FilesSidePanel.Children.Add(label);
                FilesSidePanel.Children.Add(button);
            }
        }

        /// <summary>
        /// 打开/关闭“文件”侧边栏
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FilesButton_Click(object sender, RoutedEventArgs e)
        {
            if (FilesSidePanel.Visibility == Visibility.Visible)
            {
                FilesSidePanel.Visibility = Visibility.Collapsed;
                FilesCloseButton.Visibility = Visibility.Visible;
                FilesOpenButton.Visibility = Visibility.Collapsed;
                SidePanelSplitter.Visibility = Visibility.Collapsed;
                SidePanelAndCodeGrid.ColumnDefinitions[0].Width = new GridLength(0, GridUnitType.Pixel);
                SidePanelAndCodeGrid.ColumnDefinitions[1].Width = new GridLength(0, GridUnitType.Pixel);
            }
            else
            {
                FilesSidePanel.Visibility = Visibility.Visible;
                FilesCloseButton.Visibility = Visibility.Collapsed;
                FilesOpenButton.Visibility = Visibility.Visible;
                SidePanelSplitter.Visibility = Visibility.Visible;
                SidePanelAndCodeGrid.ColumnDefinitions[0].Width = new GridLength(150, GridUnitType.Pixel);
                SidePanelAndCodeGrid.ColumnDefinitions[1].Width = new GridLength(3, GridUnitType.Pixel);

                CheckOpenFolder();
            }
        }

        /// <summary>
        /// 打开文件夹
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenFolder_Click(object sender, RoutedEventArgs e)
        {
            // “打开文件夹”对话框
            System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
            var result = dialog.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.Cancel) { return; }
            string dir = dialog.SelectedPath.Trim();

            // 清空原有文件名
            FilesSidePanel.Children.Clear();

            // 获取所有文件及文件夹名
            DirectoryInfo folder = new DirectoryInfo(dir);

            foreach (DirectoryInfo directory in folder.GetDirectories())
            {
                Button button = new Button
                {
                    Content = directory.Name,
                    Background = Brushes.White,
                    BorderThickness = new Thickness(0),
                    HorizontalContentAlignment = HorizontalAlignment.Left
                };
                FilesSidePanel.Children.Add(button);
            }

            foreach (FileInfo file in folder.GetFiles())
            {
                Button button = new Button
                {
                    Content = file.Name,
                    Background = Brushes.White,
                    BorderThickness = new Thickness(0),
                    HorizontalContentAlignment = HorizontalAlignment.Left
                };
                FilesSidePanel.Children.Add(button);
            }
        }

        /// <summary>
        /// 代码片段序号
        /// </summary>
        private static int _codeCellIndex;

        /// <summary>
        /// 添加代码片段
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddCodeCellButton_Click(object sender, RoutedEventArgs e)
        {
            Border outsideBorder = new Border { BorderThickness = new Thickness(30), BorderBrush = Brushes.White, Uid = _codeCellIndex + "border" };
            Border roundBorder = new Border { BorderThickness = new Thickness(1), BorderBrush = Brushes.Black, CornerRadius = new CornerRadius(5) };

            Grid grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(30) });
            grid.ColumnDefinitions.Add(new ColumnDefinition());

            StackPanel buttonsPanel = new StackPanel();
            grid.Children.Add(buttonsPanel);
            Grid.SetColumn(buttonsPanel, 0);

            // “运行”按钮
            ImageBrush runImage = new ImageBrush();
            string runUrl = @"E:\Code\C#\Dynamic_C_Notebook\Resources\start.png";
            runImage.ImageSource = new BitmapImage(new Uri(runUrl, UriKind.Absolute));
            Button runButton = new Button { Height = 30, Width = 30, BorderBrush = null, Background = runImage, Uid = _codeCellIndex + "run" };
            runButton.Click += RunButton_Click;

            // “删除”按钮
            ImageBrush deleteImage = new ImageBrush();
            string deleteUrl = @"E:\Code\C#\Dynamic_C_Notebook\Resources\delete.png";
            deleteImage.ImageSource = new BitmapImage(new Uri(deleteUrl, UriKind.Absolute));
            Button deleteButton = new Button { Height = 30, Width = 30, BorderBrush = null, Background = deleteImage, Uid = _codeCellIndex + "delete" };
            deleteButton.Click += DeleteButton_Click;

            buttonsPanel.Children.Add(runButton);
            buttonsPanel.Children.Add(deleteButton);

            Border codeBorder = new Border { BorderThickness = new Thickness(5) };
            grid.Children.Add(codeBorder);
            Grid.SetColumn(codeBorder, 1);

            // 编辑器
            TextEditor textEditor = new TextEditor
            {
                ShowLineNumbers = true,
                SyntaxHighlighting = HighlightingManager.Instance.GetDefinition("C++"),
                Options = { ShowSpaces = true },
                FontSize = 16,
                HorizontalScrollBarVisibility = ScrollBarVisibility.Auto,
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                Uid = _codeCellIndex + "text",
                MinHeight = 100
            };
            textEditor.TextArea.TextEntered += TextArea_TextEntered;

            codeBorder.Child = textEditor;

            roundBorder.Child = grid;
            outsideBorder.Child = roundBorder;

            CodeCellsPanel.Children.Add(outsideBorder);

            _codeCellIndex += 1;
        }

        /// <summary>
        /// 删除代码片段
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            string uid = ((Button)sender).Uid.Replace("delete", "");
            foreach (Border border in CodeCellsPanel.Children)
            {
                if (border.Uid == uid + "border")
                {
                    CodeCellsPanel.Children.Remove(border);
                    break;
                }
            }
        }

        /// <summary>
        /// 从CodeCellsPanel中分离出用户输入的代码
        /// </summary>
        /// <param name="uidToFind">对应代码片段按钮的Uid</param>
        /// <returns>用户输入的代码</returns>
        private string GetTextEditorTextFromCodeCellsPanel(string uidToFind)
        {
            string code = "";
            foreach (Border outsideBorder in CodeCellsPanel.Children)
            {
                if (outsideBorder.Uid != uidToFind + "border") { continue; }

                Border roundBorder = (Border)outsideBorder.Child;
                Grid grid = (Grid)roundBorder.Child;
                foreach (UIElement element in grid.Children)
                {
                    if (element.GetType() == typeof(Border))
                    {
                        Border codeBorder = (Border)element;
                        TextEditor textEditor = (TextEditor)codeBorder.Child;
                        code = textEditor.Text;
                        break;
                    }
                }

                if (code != "") { break; }
            }

            return code;
        }

        /// <summary>
        /// 运行代码片段中的代码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RunButton_Click(object sender, RoutedEventArgs e)
        {
            string buttonUid = ((Button)sender).Uid.Replace("run", "");

            // TODO 代码分离
            Console.WriteLine(GetTextEditorTextFromCodeCellsPanel(buttonUid));
        }
    }
}
