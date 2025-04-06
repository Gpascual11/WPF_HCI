using System;
using System.Collections.Generic;
using System.ComponentModel; // Required for INotifyPropertyChanged

namespace WPF_HCI
{
    // The Email class represents an email message.
    // It implements INotifyPropertyChanged to support data binding by notifying the UI
    // whenever a property value changes.
    public class Email : INotifyPropertyChanged
    {
        // Private backing fields for properties.
        private string _sender = string.Empty;
        private List<string> _recipients = new List<string>();
        private string _subject = string.Empty;
        private string _content = string.Empty;
        private bool _isImportant;
        private List<string> _attachments = new List<string>();
        private DateTime _dateSent;
        private string _folder = string.Empty;

        // Event required by INotifyPropertyChanged.
        // Marked as nullable to match the interface.
        public event PropertyChangedEventHandler? PropertyChanged;

        // Public property for the email sender.
        public string Sender
        {
            get => _sender;
            set
            {
                _sender = value;
                OnPropertyChanged(nameof(Sender));
            }
        }

        // Public property for the list of recipients.
        public List<string> Recipients
        {
            get => _recipients;
            set
            {
                _recipients = value;
                OnPropertyChanged(nameof(Recipients));
            }
        }

        // Public property for the email subject.
        public string Subject
        {
            get => _subject;
            set
            {
                _subject = value;
                OnPropertyChanged(nameof(Subject));
            }
        }

        // Public property for the email content (body).
        public string Content
        {
            get => _content;
            set
            {
                _content = value;
                OnPropertyChanged(nameof(Content));
            }
        }

        // Public property to indicate if the email is marked as important.
        public bool IsImportant
        {
            get => _isImportant;
            set
            {
                _isImportant = value;
                OnPropertyChanged(nameof(IsImportant));
            }
        }

        // Public property for storing attachments.
        public List<string> Attachments
        {
            get => _attachments;
            set
            {
                _attachments = value;
                OnPropertyChanged(nameof(Attachments));
            }
        }

        // Public property for the date the email was sent.
        public DateTime DateSent
        {
            get => _dateSent;
            set
            {
                _dateSent = value;
                OnPropertyChanged(nameof(DateSent));
            }
        }

        // Public property for the folder category of the email.
        public string Folder
        {
            get => _folder;
            set
            {
                _folder = value;
                OnPropertyChanged(nameof(Folder));
            }
        }

        // The OnPropertyChanged method raises the PropertyChanged event.
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Constructor for the Email class.
        public Email(string sender, List<string> recipients, string subject, string content, bool isImportant, List<string> attachments, DateTime dateSent, string folder)
        {
            // Use public setters to initialize properties.
            Sender = sender;
            Recipients = recipients;
            Subject = subject;
            Content = content;
            IsImportant = isImportant;
            Attachments = attachments;
            DateSent = dateSent;
            Folder = folder;
        }
    }
}
