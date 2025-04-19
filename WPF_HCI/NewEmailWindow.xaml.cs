using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Windows;
using System.Windows.Controls;

namespace WPF_HCI
{
    public partial class NewEmailWindow : Window
    {
        private readonly EmailViewModel _vm;
        private readonly string _currentFolder;  // e.g. "Inbox1" or "Inbox2"
        private readonly List<string> _attachments = new();

        public NewEmailWindow(EmailViewModel vm)
        {
            InitializeComponent();
            Owner = Application.Current.MainWindow;
            _vm = vm;
            _currentFolder = vm.CurrentFolder;

            AttachmentList.ItemsSource = _attachments;
        }

        private void Send_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateInputs(out var senderAddr, out var recipients, out var subject, out var body))
                return;

            var email = new Email(
                senderAddr,
                recipients,
                subject,
                body,
                false,
                new List<string>(_attachments),
                DateTime.Now,
                _currentFolder
            );

            _vm.Emails.Add(email);
            if (email.Folder == _currentFolder) _vm.FilteredEmails.Add(email);
            Close();
        }

        private void SaveDraft_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateInputs(out var senderAddr, out var recipients, out var subject, out var body))
                return;

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
            {
                _vm.FilteredEmails.Add(email);
            }

            Close();
        }

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
                    {
                        _attachments.Add(file);
                    }
                }

                AttachmentList.ItemsSource = null;
                AttachmentList.ItemsSource = _attachments;
            }
        }

        private void RemoveAttachment_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is string file)
            {
                _attachments.Remove(file);
                AttachmentList.ItemsSource = null;
                AttachmentList.ItemsSource = _attachments;
            }
        }

        private bool ValidateInputs(out string senderAddr, out List<string> recipients,
                                    out string subject, out string body)
        {
            senderAddr = SenderBox.Text.Trim();
            subject = SubjectBox.Text.Trim();
            body = ContentBox.Text;
            recipients = new List<string>();

            if (string.IsNullOrEmpty(senderAddr) || !IsValidEmail(senderAddr))
            {
                MessageBox.Show("Invalid sender address.\nPlease enter a valid email (e.g. user@domain.com).",
                                "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

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

            if (string.IsNullOrEmpty(subject))
            {
                MessageBox.Show("Subject cannot be empty.",
                                "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }

        private static bool IsValidEmail(string email)
        {
            try { _ = new MailAddress(email); return true; }
            catch { return false; }
        }
    }
}
