using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace WPF_HCI
{
    public class EmailViewModel
    {
        public ObservableCollection<Email> Emails { get; set; }  // Collection of all emails
        public ObservableCollection<Email> FilteredEmails { get; set; }  // Collection for filtered emails (based on folder or filter)
        public List<string> Folders { get; set; }  // List of available folders (Inbox, Sent, etc.)

        public EmailViewModel()
        {
            // Initialize collections
            Emails = new ObservableCollection<Email>
            {
                // Add default emails with folder property
                new Email("john.doe@example.com", new List<string> { "alice@example.com" }, "Meeting Update", "We have a meeting at 10 AM tomorrow.", false, new List<string> { "attachment1.pdf" }, DateTime.Now, "Inbox"),
                new Email("jane.doe@example.com", new List<string> { "bob@example.com" }, "Important Notice", "This is an important notice.", true, new List<string>(), DateTime.Now, "Sent"),
                new Email("mark.smith@example.com", new List<string> { "susan@example.com" }, "Invoice #456", "Here is your invoice.", false, new List<string> { "invoice.pdf" }, DateTime.Now, "Inbox")
            };

            // Initialize filtered emails collection to show all emails initially
            FilteredEmails = new ObservableCollection<Email>(Emails);

            // Initialize folders list (you can expand this as needed)
            Folders = new List<string> { "Inbox", "Sent", "Drafts", "Trash" };
        }

        // Method to filter emails based on folder selection
        public void FilterEmailsByFolder(string folder)
        {
            if (folder == "All")
            {
                FilteredEmails = new ObservableCollection<Email>(Emails);  // Show all emails if "All" is selected
            }
            else
            {
                // Filter emails by folder
                var filtered = Emails.Where(email => email.Folder == folder).ToList();
                FilteredEmails = new ObservableCollection<Email>(filtered);
            }

            // Notify any listeners that the FilteredEmails collection has changed
            OnPropertyChanged(nameof(FilteredEmails));
        }

        // Property change notification method (for data binding)
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
        }
    }
}
