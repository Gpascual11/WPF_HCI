using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace WPF_HCI
{
    public class Email : INotifyPropertyChanged
    {
        private string _sender;
        private List<string> _recipients;
        private string _subject;
        private string _content;
        private bool _isImportant;
        private List<string> _attachments;
        private DateTime _dateSent;
        private string _folder;  // New Folder property to categorize emails

        public event PropertyChangedEventHandler PropertyChanged;

        public string Sender
        {
            get { return _sender; }
            set { _sender = value; OnPropertyChanged(nameof(Sender)); }
        }

        public List<string> Recipients
        {
            get { return _recipients; }
            set { _recipients = value; OnPropertyChanged(nameof(Recipients)); }
        }

        public string Subject
        {
            get { return _subject; }
            set { _subject = value; OnPropertyChanged(nameof(Subject)); }
        }

        public string Content
        {
            get { return _content; }
            set { _content = value; OnPropertyChanged(nameof(Content)); }
        }

        public bool IsImportant
        {
            get { return _isImportant; }
            set { _isImportant = value; OnPropertyChanged(nameof(IsImportant)); }
        }

        public List<string> Attachments
        {
            get { return _attachments; }
            set { _attachments = value; OnPropertyChanged(nameof(Attachments)); }
        }

        public DateTime DateSent
        {
            get { return _dateSent; }
            set { _dateSent = value; OnPropertyChanged(nameof(DateSent)); }
        }

        public string Folder
        {
            get { return _folder; }
            set { _folder = value; OnPropertyChanged(nameof(Folder)); }  // Notify of Folder change
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Constructor with Folder parameter to categorize emails
        public Email(string sender, List<string> recipients, string subject, string content, bool isImportant, List<string> attachments, DateTime dateSent, string folder)
        {
            Sender = sender;
            Recipients = recipients;
            Subject = subject;
            Content = content;
            IsImportant = isImportant;
            Attachments = attachments;
            DateSent = dateSent;
            Folder = folder;  // Set the folder category for the email
        }
    }
}
