using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

namespace WpfTreeView
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// 
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// When main window opens
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            foreach(var drive in Directory.GetLogicalDrives())
            {
                var item = new TreeViewItem()
                {
                    Header = drive,
                    Tag = drive
                };

                // Add dummy item to get expand icon
                item.Items.Add(null);

                // Listen for folder expanded
                item.Expanded += Folder_Expanded;

                // Add item to main treeview
                FolderView.Items.Add(item);
            }
        }

        private void Folder_Expanded(object sender, RoutedEventArgs e)
        {
            var item = (TreeViewItem)sender;

            // return if item already processed
            if (item.Items.Count != 1 || item.Items[0] != null) return;

            // clear dummy item
            item.Items.Clear();

            var fullPath = (string)item.Tag;

            var directories = new List<string>();

            try
            {
                var dirs = Directory.GetDirectories(fullPath);

                if (dirs.Length > 0)
                    directories.AddRange(dirs);
            }
            catch {  }

            // for each directory in list
            directories.ForEach(directoryPath =>
            {
                var subItem = new TreeViewItem()
                {
                    //////////////
                    // TODO : Create helper function to get file or folder name from full path
                    //
                    Header = directoryPath,
                    Tag = directoryPath
                };

                subItem.Items.Add(null);

                subItem.Expanded += Folder_Expanded;

                item.Items.Add(subItem);
            });



        }
    }
}
