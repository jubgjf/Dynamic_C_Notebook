using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

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

                // 未打开文件夹时，显示提示信息，提示打开文件夹
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
    }
}
