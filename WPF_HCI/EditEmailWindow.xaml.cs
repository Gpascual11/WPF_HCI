using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;

namespace WPF_HCI
{
    /// <summary>
    /// Interaction logic for EditEmailWindow.xaml
    /// </summary>
    public partial class EditEmailWindow : Window
    {
        private readonly EmailViewModel _viewModel;
        private Email? _currentEmail;
        private List<string> attachmentPaths = new();

        public EditEmailWindow(EmailViewModel vm)
        {
            InitializeComponent();
            _viewModel = vm;
            Owner = Application.Current.MainWindow;
            DataContext = _viewModel;

            // Initial email
            UpdateUIFromEmail();

            // Track changes to SelectedEmail
            _viewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        private void ViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(EmailViewModel.SelectedEmail))
            {
                UpdateUIFromEmail();
            }
        }

        /// <summary>
        /// Loads the current selected email into the input fields.
        /// Disables editing if the message is not a draft.
        /// </summary>
        private void UpdateUIFromEmail()
        {
            _currentEmail = _viewModel.SelectedEmail;

            if (_currentEmail == null)
            {
                ClearFields();
                return;
            }

            SenderBox.Text = _currentEmail.Sender;
            RecipientsBox.Text = string.Join(", ", _currentEmail.Recipients);
            SubjectBox.Text = _currentEmail.Subject;
            ContentBox.Text = _currentEmail.Content;
            attachmentPaths = new List<string>(_currentEmail.Attachments);
            AttachmentsList.ItemsSource = null;
            AttachmentsList.ItemsSource = attachmentPaths;

            // Disable editing if not a draft
            bool isEditable = _currentEmail.Folder.StartsWith("Drafts");
            SenderBox.IsReadOnly = !isEditable;
            RecipientsBox.IsReadOnly = !isEditable;
            SubjectBox.IsReadOnly = !isEditable;
            ContentBox.IsReadOnly = !isEditable;
            AddAttachmentBtn.IsEnabled = isEditable;
            SaveButton.IsEnabled = isEditable;
        }

        private void ClearFields()
        {
            SenderBox.Text = "";
            RecipientsBox.Text = "";
            SubjectBox.Text = "";
            ContentBox.Text = "";
            AttachmentsList.ItemsSource = null;
            attachmentPaths.Clear();
        }

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

                AttachmentsList.ItemsSource = null;
                AttachmentsList.ItemsSource = attachmentPaths;
            }
        }

        private void RemoveAttachment_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is string file)
            {
                attachmentPaths.Remove(file);
                AttachmentsList.ItemsSource = null;
                AttachmentsList.ItemsSource = attachmentPaths;
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentEmail == null)
                return;

            _currentEmail.Sender = SenderBox.Text.Trim();
            _currentEmail.Recipients = new List<string>(RecipientsBox.Text.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries));
            _currentEmail.Subject = SubjectBox.Text.Trim();
            _currentEmail.Content = ContentBox.Text;
            _currentEmail.Attachments = new List<string>(attachmentPaths);

            this.Close();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
