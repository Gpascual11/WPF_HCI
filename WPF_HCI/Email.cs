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
        private string _sender;
        private List<string> _recipients;
        private string _subject;
        private string _content;
        private bool _isImportant;
        private List<string> _attachments;
        private DateTime _dateSent;
        private string _folder;  // Used to categorize emails (e.g., Inbox, Sent, etc.)

        // Event required by the INotifyPropertyChanged interface.
        // This event is raised whenever a property value changes.
        public event PropertyChangedEventHandler PropertyChanged;

        // Public property for the email sender.
        // When the value is set, it calls OnPropertyChanged to notify subscribers.
        public string Sender
        {
            get { return _sender; }
            set
            {
                _sender = value;
                OnPropertyChanged(nameof(Sender)); // Notify that Sender has changed.
            }
        }

        // Public property for the list of recipients.
        // Recipients are stored as a List of strings.
        public List<string> Recipients
        {
            get { return _recipients; }
            set
            {
                _recipients = value;
                OnPropertyChanged(nameof(Recipients)); // Notify that Recipients has changed.
            }
        }

        // Public property for the email subject.
        public string Subject
        {
            get { return _subject; }
            set
            {
                _subject = value;
                OnPropertyChanged(nameof(Subject)); // Notify that Subject has changed.
            }
        }

        // Public property for the email content (body).
        public string Content
        {
            get { return _content; }
            set
            {
                _content = value;
                OnPropertyChanged(nameof(Content)); // Notify that Content has changed.
            }
        }

        // Public property to indicate if the email is marked as important.
        public bool IsImportant
        {
            get { return _isImportant; }
            set
            {
                _isImportant = value;
                OnPropertyChanged(nameof(IsImportant)); // Notify that IsImportant has changed.
            }
        }

        // Public property for storing attachments.
        // Only the file paths of the attachments are saved.
        public List<string> Attachments
        {
            get { return _attachments; }
            set
            {
                _attachments = value;
                OnPropertyChanged(nameof(Attachments)); // Notify that Attachments has changed.
            }
        }

        // Public property for the date the email was sent.
        public DateTime DateSent
        {
            get { return _dateSent; }
            set
            {
                _dateSent = value;
                OnPropertyChanged(nameof(DateSent)); // Notify that DateSent has changed.
            }
        }

        // Public property for the folder category of the email.
        // This property can be used to organize emails (e.g., Inbox, Sent, Drafts, Trash).
        public string Folder
        {
            get { return _folder; }
            set
            {
                _folder = value;
                OnPropertyChanged(nameof(Folder)); // Notify that Folder has changed.
            }
        }

        // The OnPropertyChanged method is a helper that raises the PropertyChanged event.
        // It takes the name of the property that changed as a parameter.
        protected virtual void OnPropertyChanged(string propertyName)
        {
            // If there are any subscribers to the PropertyChanged event, raise the event.
            // This notifies the UI (or any other subscribers) to update based on the new property value.
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Constructor for the Email class.
        // Initializes all properties of the email, ensuring that the PropertyChanged event is raised.
        public Email(string sender, List<string> recipients, string subject, string content, bool isImportant, List<string> attachments, DateTime dateSent, string folder)
        {
            // Set each property using the public setters so that OnPropertyChanged is invoked.
            Sender = sender;
            Recipients = recipients;
            Subject = subject;
            Content = content;
            IsImportant = isImportant;
            Attachments = attachments;
            DateSent = dateSent;
            Folder = folder;  // Categorize the email by folder (e.g., Inbox, Sent)
        }
    }
}
