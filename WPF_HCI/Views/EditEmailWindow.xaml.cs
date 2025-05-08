using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;

namespace WPF_HCI
{
    public partial class EditEmailWindow : Window
    {
        // Reference to the shared ViewModel containing all emails and selection
        private readonly EmailViewModel _viewModel;

        // Reference to the currently selected email, used for editing/viewing
        private Email? _currentEmail;

        // List of attachment file paths for display and editing
        private List<string> attachmentPaths = new();

        public EditEmailWindow(EmailViewModel vm)
        {
            InitializeComponent();
            _viewModel = vm;

            // (2f) Ensure this window stays above the main window
            Owner = Application.Current.MainWindow;

            // (2d) Link DataContext for binding and listening to changes
            DataContext = _viewModel;

            // (2b) Load selected email into fields on open
            UpdateUIFromEmail();

            // (2d) React to changes in the selected email while the window is open
            _viewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        // (2d) Handle property changes in the ViewModel
        private void ViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(EmailViewModel.SelectedEmail))
            {
                // When the selected email changes, refresh the UI
                UpdateUIFromEmail();
            }
        }

        /// <summary>
        /// (2b, 2d, 2g) Populate UI fields with the selected email's data.
        /// Disable editing if the email is not a draft.
        /// </summary>
        private void UpdateUIFromEmail()
        {
            _currentEmail = _viewModel.SelectedEmail;

            // If there's no email selected, clear the fields
            if (_currentEmail == null)
            {
                ClearFields();
                return;
            }

            // (2b) Populate fields with data from the selected email
            SenderBox.Text = _currentEmail.Sender;
            RecipientsBox.Text = string.Join(", ", _currentEmail.Recipients);
            SubjectBox.Text = _currentEmail.Subject;
            ContentBox.Text = _currentEmail.Content;

            // Load attachments
            attachmentPaths = new List<string>(_currentEmail.Attachments);
            AttachmentsList.ItemsSource = null;
            AttachmentsList.ItemsSource = attachmentPaths;

            // (2g) Disable editing if email is not in the "Drafts" folder
            bool isEditable = _currentEmail.Folder.StartsWith("Drafts");
            SenderBox.IsReadOnly = !isEditable;
            RecipientsBox.IsReadOnly = !isEditable;
            SubjectBox.IsReadOnly = !isEditable;
            ContentBox.IsReadOnly = !isEditable;
            AddAttachmentBtn.IsEnabled = isEditable;
            SaveButton.IsEnabled = isEditable;
            SendButton.IsEnabled = isEditable; 
        }

        // Clear all input fields and reset attachments
        private void ClearFields()
        {
            SenderBox.Text = "";
            RecipientsBox.Text = "";
            SubjectBox.Text = "";
            ContentBox.Text = "";
            AttachmentsList.ItemsSource = null;
            attachmentPaths.Clear();
        }

        // Handle the click to add attachments (opens file dialog)
        private void AddAttachments_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Multiselect = true,
                Filter = "All files (*.*)|*.*"
            };

            if (dialog.ShowDialog() == true)
            {
                foreach (var file in dialog.FileNames)
                {
                    if (!attachmentPaths.Contains(file))
                        attachmentPaths.Add(file);
                }

                // Refresh the attachments list UI
                AttachmentsList.ItemsSource = null;
                AttachmentsList.ItemsSource = attachmentPaths;
            }
        }

        // Handle removal of an attachment from the list
        private void RemoveAttachment_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is string file)
            {
                attachmentPaths.Remove(file);
                AttachmentsList.ItemsSource = null;
                AttachmentsList.ItemsSource = attachmentPaths;
            }
        }

        // Save changes made to the email back to the model
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentEmail == null)
                return;

            // (2b) Save updated fields into the selected email object
            _currentEmail.Sender = SenderBox.Text.Trim();
            _currentEmail.Recipients = new List<string>(
                RecipientsBox.Text.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            );
            _currentEmail.Subject = SubjectBox.Text.Trim();
            _currentEmail.Content = ContentBox.Text;
            _currentEmail.Attachments = new List<string>(attachmentPaths);

            // Close the window after saving
            this.Close();
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentEmail == null)
                return;

            // Validate inputs
            var senderAddr = SenderBox.Text.Trim();
            var subject = SubjectBox.Text.Trim();
            var body = ContentBox.Text;
            var recipients = RecipientsBox.Text
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(r => r.Trim())
                .ToList();

            if (string.IsNullOrEmpty(senderAddr) || !IsValidEmail(senderAddr))
            {
                MessageBox.Show("Invalid sender address.\nPlease enter a valid email (e.g. user@domain.com).",
                                "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (recipients.Count == 0 || recipients.Any(r => !IsValidEmail(r)))
            {
                MessageBox.Show("Invalid recipient list.\nEnter one or more valid emails, separated by commas.",
                                "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrEmpty(subject))
            {
                MessageBox.Show("Subject cannot be empty.",
                                "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // All valid → update email and move to "SentX" folder
            _currentEmail.Sender = senderAddr;
            _currentEmail.Recipients = recipients;
            _currentEmail.Subject = subject;
            _currentEmail.Content = body;
            _currentEmail.Attachments = new List<string>(attachmentPaths);

            // Replace "DraftsX" with "SentX"
            string suffix = new string(_currentEmail.Folder.Where(char.IsDigit).ToArray());
            string sentFolder = $"Sent{suffix}";
            _currentEmail.Folder = sentFolder;

            // Refresh filtered list if currently in the Sent folder
            if (_viewModel.CurrentFolder == sentFolder && !_viewModel.FilteredEmails.Contains(_currentEmail))
                _viewModel.FilteredEmails.Add(_currentEmail);
            else if (_viewModel.CurrentFolder.StartsWith("Drafts"))
                _viewModel.FilterEmailsByFolder(_viewModel.CurrentFolder); // Refresh list if removing it from view

            this.Close();
        }

        private static bool IsValidEmail(string email)
        {
            try { _ = new System.Net.Mail.MailAddress(email); return true; }
            catch { return false; }
        }


        // Close the window without saving
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
