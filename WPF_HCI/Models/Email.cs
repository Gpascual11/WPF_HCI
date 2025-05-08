// Email.cs
// ----------------------------------------------------------------------
// This file defines the Email class, which represents an email message.
// The class implements INotifyPropertyChanged to support data binding in
// WPF applications. Changes to any property will notify the UI, ensuring
// that data is always up-to-date.
// 
// Author: Gerard Pascual
// Date: 4/4/2025
// Version: 2.2
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel; // Required for INotifyPropertyChanged

namespace WPF_HCI
{
    /// <summary>
    /// Represents an email message with properties such as Sender, Recipients,
    /// Subject, Content, Attachments, DateSent, and Folder. Implements INotifyPropertyChanged
    /// to support data binding.
    /// </summary>
    public class Email : INotifyPropertyChanged
    {
        // Private backing fields for the properties.
        private string _sender = string.Empty;
        private List<string> _recipients = new List<string>();
        private string _subject = string.Empty;
        private string _content = string.Empty;
        private bool _isImportant;
        private List<string> _attachments = new List<string>();
        private DateTime _dateSent;
        private string _folder = string.Empty;

        /// <summary>
        /// Occurs when a property value changes.
        /// Marked as nullable to match INotifyPropertyChanged interface.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Gets or sets the sender of the email.
        /// </summary>
        public string Sender
        {
            get => _sender;
            set
            {
                _sender = value;
                OnPropertyChanged(nameof(Sender));
            }
        }

        /// <summary>
        /// Gets or sets the list of recipient email addresses.
        /// </summary>
        public List<string> Recipients
        {
            get => _recipients;
            set
            {
                _recipients = value;
                OnPropertyChanged(nameof(Recipients));
            }
        }

        /// <summary>
        /// Returns all recipients as a single comma-separated string for UI display.
        /// </summary>
        public string RecipientsAsString => string.Join(", ", Recipients);


        /// <summary>
        /// Gets or sets the subject of the email.
        /// </summary>
        public string Subject
        {
            get => _subject;
            set
            {
                _subject = value;
                OnPropertyChanged(nameof(Subject));
            }
        }

        /// <summary>
        /// Gets or sets the content (body) of the email.
        /// </summary>
        public string Content
        {
            get => _content;
            set
            {
                _content = value;
                OnPropertyChanged(nameof(Content));
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the email is marked as important.
        /// </summary>
        public bool IsImportant
        {
            get => _isImportant;
            set
            {
                _isImportant = value;
                OnPropertyChanged(nameof(IsImportant));
            }
        }

        /// <summary>
        /// Gets or sets the list of attachment file paths.
        /// </summary>
        public List<string> Attachments
        {
            get => _attachments;
            set
            {
                _attachments = value;
                OnPropertyChanged(nameof(Attachments));
            }
        }

        /// <summary>
        /// Gets or sets the date and time the email was sent.
        /// </summary>
        public DateTime DateSent
        {
            get => _dateSent;
            set
            {
                _dateSent = value;
                OnPropertyChanged(nameof(DateSent));
            }
        }

        /// <summary>
        /// Gets or sets the folder category of the email (e.g., Inbox, Sent, Drafts, Trash).
        /// </summary>
        public string Folder
        {
            get => _folder;
            set
            {
                _folder = value;
                OnPropertyChanged(nameof(Folder));
            }
        }

        /// <summary>
        /// Raises the PropertyChanged event for the specified property.
        /// </summary>
        /// <param name="propertyName">The name of the property that changed.</param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            // Notify any subscribers that the property value has changed.
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Initializes a new instance of the Email class with the specified parameters.
        /// </summary>
        /// <param name="sender">The email sender address.</param>
        /// <param name="recipients">A list of recipient email addresses.</param>
        /// <param name="subject">The subject of the email.</param>
        /// <param name="content">The content (body) of the email.</param>
        /// <param name="isImportant">A value indicating whether the email is marked as important.</param>
        /// <param name="attachments">A list of attachment file paths.</param>
        /// <param name="dateSent">The date and time the email was sent.</param>
        /// <param name="folder">The folder category for the email (e.g., Inbox, Sent, Drafts, Trash).</param>
        public Email(string sender, List<string> recipients, string subject, string content, bool isImportant, List<string> attachments, DateTime dateSent, string folder)
        {
            // Use the public setters to initialize properties so that change notifications occur.
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
