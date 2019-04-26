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

                // Add dummy item to get expand icon to show
                item.Items.Add(null);

                // Listen for folder expanded
                item.Expanded += Folder_Expanded;

                // Add item to main treeview
                FolderView.Items.Add(item);
            }
        }

        /// <summary>
        /// When a folder is expanded, find the sub folders and files
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Folder_Expanded(object sender, RoutedEventArgs e)
        {
            // cast sender to TreeViewItem
            var item = (TreeViewItem)sender;

            // return if item already processed
            if (item.Items.Count != 1 || item.Items[0] != null) return;

            // clear dummy item
            item.Items.Clear();

            // get full path from tag
            var fullPath = (string)item.Tag;

            #region Get Directories

            // create empty list
            var directories = new List<string>();

            try
            {
                // try to get list of directories
                var dirs = Directory.GetDirectories(fullPath);

                // transfer to working list 
                if (dirs.Length > 0)
                    directories.AddRange(dirs);
            }
            catch {  }

            // for each directory in list...
            directories.ForEach(directoryPath =>
            {
                // create subitem
                var subItem = new TreeViewItem()
                {
                    // use helper function to get directory name from full path
                    Header = GetFileFolderName(directoryPath),
                    Tag = directoryPath
                };

                // add dummy item to subitem
                subItem.Items.Add(null);

                // listen for expand
                subItem.Expanded += Folder_Expanded;

                // add subitem to parent
                item.Items.Add(subItem);
            });

            #endregion

            #region Get Files

            // create empty list
            var fileList = new List<string>();

            try
            {
                // try to get list of directories
                var files = Directory.GetFiles(fullPath);

                // transfer to working list 
                if (files.Length > 0)
                    fileList.AddRange(files);
            }
            catch { }

            // for each directory in list...
            fileList.ForEach(filename =>
            {
                // create subitem
                var subItem = new TreeViewItem()
                {
                    // use helper function to get directory name from full path
                    Header = GetFileFolderName(filename),
                    Tag = filename
                };

                // add subitem to parent
                item.Items.Add(subItem);
            });

            #endregion
        }

        /// <summary>
        /// Get directory name from full path
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetFileFolderName(string path)
        {
            // return empty string if null or empty passed
            if (string.IsNullOrEmpty(path)) return string.Empty;

            // replace fs with bs 
            var normalizedPath = path.Replace('/', '\\');

            // look for last occurance of bs
            var index = normalizedPath.LastIndexOf('\\');

            // if no slash, must be a file name
            if (index == -1)
                return path;

            // return everything after last bs
            return path.Substring(index + 1);
        }

    }
}
