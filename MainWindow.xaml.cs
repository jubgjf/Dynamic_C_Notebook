using System.Windows;

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
            }
        }
    }
}
