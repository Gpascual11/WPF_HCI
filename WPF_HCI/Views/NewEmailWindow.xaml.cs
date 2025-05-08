using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Windows;
using System.Windows.Controls;

namespace WPF_HCI
{
    /// <summary>
    /// Logic for composing and sending a new email (requirement 1).
    /// </summary>
    public partial class NewEmailWindow : Window
    {
        private readonly EmailViewModel _vm;
        private readonly string _currentFolder; // (1c) Stores the currently selected folder
        private readonly List<string> _attachments = new(); // (1a) Holds attachment paths

        /// <summary>
        /// Constructor initializes DataContext and attachment list.
        /// (1a) Setup for entering all fields.
        /// (1b) This window is shown modally from the main window.
        /// </summary>
        public NewEmailWindow(EmailViewModel vm)
        {
            InitializeComponent();

            // Ensures window appears centered over the main app window
            Owner = Application.Current.MainWindow;

            _vm = vm;
            _currentFolder = vm.CurrentFolder;

            // (1a) Attachments displayed in a list control
            AttachmentList.ItemsSource = _attachments;
        }

        /// <summary>
        /// (1c) Handler for sending the email — adds it to the current folder.
        /// Validates inputs before adding.
        /// </summary>
        private void Send_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateInputs(out var senderAddr, out var recipients, out var subject, out var body))
                return;

            // Map current folder to the corresponding Sent folder (e.g., Inbox1 → Sent1)
            string suffix = new string(_currentFolder.Where(char.IsDigit).ToArray());
            var sentFolder = $"Sent{suffix}";

            var email = new Email(
                senderAddr,
                recipients,
                subject,
                body,
                false,
                new List<string>(_attachments),
                DateTime.Now,
                sentFolder
            );

            _vm.Emails.Add(email);

            // Show in view only if we're already in the Sent folder
            if (_vm.CurrentFolder == sentFolder)
                _vm.FilteredEmails.Add(email);

            Close(); // Exit after sending
        }


        /// <summary>
        /// (1c) Handler for saving as draft — adds email to a Drafts folder.
        /// </summary>
        private void SaveDraft_Click(object sender, RoutedEventArgs e)
        {
            string senderAddr = SenderBox.Text.Trim();
            List<string> recipients = RecipientsBox.Text
                .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Trim())
                .ToList();
            string subject = SubjectBox.Text.Trim();
            string body = ContentBox.Text;

            // If all fields are empty and there are no attachments, don't save
            if (string.IsNullOrEmpty(senderAddr) &&
                recipients.Count == 0 &&
                string.IsNullOrEmpty(subject) &&
                string.IsNullOrEmpty(body) &&
                _attachments.Count == 0)
            {
                MessageBox.Show("Draft not saved because it is completely empty.",
                                "Empty Draft", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            // Map current folder to appropriate Drafts folder (e.g., Inbox1 → Drafts1)
            string suffix = new string(_currentFolder.Where(char.IsDigit).ToArray());
            var draftsFolder = $"Drafts{suffix}";

            var email = new Email(
                senderAddr,
                recipients,
                subject,
                body,
                false,
                new List<string>(_attachments),
                DateTime.Now,
                draftsFolder
            );

            _vm.Emails.Add(email);

            if (_vm.CurrentFolder == draftsFolder)
                _vm.FilteredEmails.Add(email);

            Close(); // Exit after saving
        }


        /// <summary>
        /// (1a) Adds files to the attachment list using OpenFileDialog.
        /// </summary>
        private void AddAttachment_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                Multiselect = true,
                Title = "Select attachment(s)"
            };

            if (dialog.ShowDialog() == true)
            {
                foreach (var file in dialog.FileNames)
                {
                    if (!_attachments.Contains(file))
                        _attachments.Add(file);
                }

                // Refresh the ItemsControl
                AttachmentList.ItemsSource = null;
                AttachmentList.ItemsSource = _attachments;
            }
        }

        /// <summary>
        /// (1a) Removes an individual attachment when delete button is clicked.
        /// </summary>
        private void RemoveAttachment_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is string file)
            {
                _attachments.Remove(file);
                AttachmentList.ItemsSource = null;
                AttachmentList.ItemsSource = _attachments;
            }
        }

        /// <summary>
        /// (1d) Validates the form inputs: sender, recipients, and subject.
        /// Shows a message box on error.
        /// </summary>
        private bool ValidateInputs(out string senderAddr, out List<string> recipients,
                                    out string subject, out string body)
        {
            senderAddr = SenderBox.Text.Trim();
            subject = SubjectBox.Text.Trim();
            body = ContentBox.Text;
            recipients = new List<string>();

            // Validate sender email
            if (string.IsNullOrEmpty(senderAddr) || !IsValidEmail(senderAddr))
            {
                MessageBox.Show("Invalid sender address.\nPlease enter a valid email (e.g. user@domain.com).",
                                "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            // Validate recipients: at least one, all valid
            var parts = RecipientsBox.Text
                          .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                          .Select(s => s.Trim())
                          .ToList();

            if (parts.Count == 0 || parts.Any(r => !IsValidEmail(r)))
            {
                MessageBox.Show("Invalid recipient list.\nEnter one or more valid emails, separated by commas.",
                                "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            recipients = parts;

            // Validate subject
            if (string.IsNullOrEmpty(subject))
            {
                MessageBox.Show("Subject cannot be empty.",
                                "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }

        /// <summary>
        /// (1d) Utility to verify email format using .NET MailAddress.
        /// </summary>
        private static bool IsValidEmail(string email)
        {
            try { _ = new MailAddress(email); return true; }
            catch { return false; }
        }
    }
}
