// MainWindow.xaml.cs
// ----------------------------------------------------------------------
// This file defines the code-behind for the main window of the email client
// application. It sets up the DataContext with the EmailViewModel and attaches
// event handlers for UI interactions such as search, folder selection, and toggling
// email importance.
// 
// Author: Gerard Pascual
// Date: 7/5/2025
// Version: 4.6
// ----------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WPF_HCI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml.
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Gets or sets the EmailViewModel used as the DataContext for data binding.
        /// Initialized inline so that it is never null.
        /// </summary>
        public EmailViewModel ViewModel { get; set; } = new EmailViewModel();

        // (2e) Only allow one instance of the EditEmailWindow at a time
        private EditEmailWindow? editWindow;

        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// Sets the DataContext, loads emails, and attaches event handlers.
        /// </summary>
        public MainWindow()
        {
            // Set the DataContext before InitializeComponent so that bindings have access to the ViewModel.
            DataContext = ViewModel;
            InitializeComponent();

            // Set window state to normal.
            this.WindowState = WindowState.Normal;

            // Load the predefined list of emails from data source or mock.
            ViewModel.LoadEmails();
        }

        /// <summary>
        /// Event handler for the Exit menu item click.
        /// (File → Exit) — Shuts down the application.
        /// </summary>
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        /// <summary>
        /// Event handler for double-clicking an email in the ListView.
        /// (Not part of spec 2, just UI info preview.)
        /// </summary>
        private void EmailList_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (EmailList.SelectedItem is Email selectedEmail)
            {
                MessageBox.Show("Subject: " + selectedEmail.Subject,
                                "Email Subject",
                                MessageBoxButton.OK,
                                MessageBoxImage.Information);
            }
        }

        /// <summary>
        /// (2a) Enable or disable the "Edit" menu item based on selection.
        /// Triggered when the email selection changes.
        /// </summary>
        private void EmailList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EditMenuItem.IsEnabled = ViewModel.SelectedEmail != null;
        }

        /// <summary>
        /// (Optional) Marks or unmarks the selected email as important.
        /// Uses the ViewModel method to toggle the flag.
        /// </summary>
        private void ToggleImportant_Click(object sender, RoutedEventArgs e)
        {
            if (EmailList.SelectedItem is Email selectedEmail)
            {
                ViewModel.ToggleImportant(selectedEmail);
            }
        }

        /// <summary>
        /// (Folder Sidebar) Triggered when a folder is selected in the TreeView.
        /// Updates the ViewModel's current folder and filters emails accordingly.
        /// </summary>
        private void FolderTree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (FolderTree.SelectedItem is TreeViewItem selectedFolder)
            {
                // Use the Tag to get the folder key (Inbox1, Sent1, etc.)
                string folderTag = selectedFolder.Tag?.ToString() ?? "";
                if (!string.IsNullOrEmpty(folderTag))
                {
                    ViewModel.FilterEmailsByFolder(folderTag);
                }
            }
        }

        /// <summary>
        /// Opens the NewEmailWindow in modal mode for composing.
        /// Not related to spec 2 (editing), just for creating.
        /// </summary>
        private void NewEmail_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new NewEmailWindow(ViewModel);
            dlg.ShowDialog();   // modal
        }

        /// <summary>
        /// (2c, 2e, 2f) Opens the EditEmailWindow non-modally.
        /// - Only allows one open window
        /// - Brings it to front if already open
        /// - Opens the editor in non-modal mode using Show()
        /// </summary>
        private void EditMenuItem_Click(object sender, RoutedEventArgs e)
        {
            // (2e) Prevent multiple edit windows
            if (editWindow == null || !editWindow.IsLoaded)
            {
                // (2c) Use Show() for non-modal window
                // (2f) Ownership ensures it stays above main window
                editWindow = new EditEmailWindow(ViewModel);
                editWindow.Show();
            }
            else
            {
                // Bring existing window to front if already open
                editWindow.Activate();
            }
        }

        /// <summary>
        /// (SearchBarControl) Executes a filtered search by query and category.
        /// </summary>
        private void SearchControl_SearchChanged(object sender, SearchChangedEventArgs e)
        {
            ViewModel.FilterEmailsByCategory(e.Query, e.Category);
        }

        /// <summary>
        /// (SearchBarControl) Resets search and shows all emails for the current folder.
        /// </summary>
        private void SearchControl_SearchReset(object sender, EventArgs e)
        {
            ViewModel.FilterEmailsByFolder(ViewModel.CurrentFolder); // Resets to current folder
        }

        /// <summary>
        /// (SearchBarControl) Applies additional UI-level filters (e.g., unread, starred).
        /// </summary>
        private void SearchControl_FilterChanged(object sender, string filter)
        {
            ViewModel.FilterByOption(filter);
        }
    }
}
