// MainWindow.xaml.cs
// ----------------------------------------------------------------------
// This file defines the code-behind for the main window of the email client
// application. It sets up the DataContext with the EmailViewModel and attaches
// event handlers for UI interactions such as search, folder selection, and toggling
// email importance. This file uses inline comments and XML documentation for clarity.
// 
// Author: Gerard Pascual
// Date: 5/4/2025
// Version: 3.1
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

            // Load the predefined list of emails.
            ViewModel.LoadEmails();

            // Attach event handler for the FilterBox SelectionChanged event.
            FilterBox.SelectionChanged += FilterBox_SelectionChanged;
        }

        /// <summary>
        /// Event handler for the Exit menu item click.
        /// Shuts down the application.
        /// </summary>
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        /// <summary>
        /// Event handler for double-clicking an email in the ListView.
        /// Displays a message box showing the subject of the selected email.
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
        /// Event handler for when the SearchBox gains focus.
        /// Clears the default text ("Search...") and changes the foreground color to black.
        /// </summary>
        private void SearchBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (SearchBox.Text == "Search...")
            {
                SearchBox.Text = "";
                SearchBox.Foreground = System.Windows.Media.Brushes.Black;
            }
        }

        /// <summary>
        /// Event handler for when the SearchBox loses focus.
        /// Restores the default text ("Search...") and sets the foreground color to gray if empty.
        /// </summary>
        private void SearchBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SearchBox.Text))
            {
                SearchBox.Text = "Search...";
                SearchBox.Foreground = System.Windows.Media.Brushes.Gray;
            }
        }

        /// <summary>
        /// Event handler for changes in the SearchBox text.
        /// Filters emails based on the entered query.
        /// </summary>
        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Do not filter if the text is still the default prompt.
            if (SearchBox.Text == "Search...") return;
            ViewModel.FilterEmails(SearchBox.Text);
        }

        /// <summary>
        /// Event handler for changes in the FilterBox selection.
        /// Retrieves the selected filter option and calls the ViewModel's FilterByOption method.
        /// Defaults to "All" if no selection is found.
        /// </summary>
        private void FilterBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Ensure a non-null value by defaulting to "All" if null.
            string selectedFilter = ((ComboBoxItem?)FilterBox.SelectedItem)?.Content?.ToString() ?? "All";
            ViewModel.FilterByOption(selectedFilter);
        }

        /// <summary>
        /// Event handler for the Toggle Important button click.
        /// Toggles the important flag of the selected email.
        /// </summary>
        private void ToggleImportant_Click(object sender, RoutedEventArgs e)
        {
            if (EmailList.SelectedItem is Email selectedEmail)
            {
                ViewModel.ToggleImportant(selectedEmail);
            }
        }

        /// <summary>
        /// Event handler for when the selected folder in the TreeView changes.
        /// Retrieves the folder tag from the selected TreeViewItem and updates the ViewModel.
        /// </summary>
        private void FolderTree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (FolderTree.SelectedItem is TreeViewItem selectedFolder)
            {
                // Convert the Tag to a string; default to empty string if null.
                string folderTag = selectedFolder.Tag?.ToString() ?? "";
                if (!string.IsNullOrEmpty(folderTag))
                {
                    ViewModel.FilterEmailsByFolder(folderTag);
                }
            }
        }
    }
}
