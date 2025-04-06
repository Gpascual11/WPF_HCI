using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

namespace WPF_HCI
{
    public class EmailViewModel : INotifyPropertyChanged
    {
        // Initialize these collections inline so they are never null.
        private ObservableCollection<Email> emails = new ObservableCollection<Email>();
        public ObservableCollection<Email> Emails
        {
            get => emails;
            set { emails = value; OnPropertyChanged(nameof(Emails)); }
        }

        private ObservableCollection<Email> filteredEmails = new ObservableCollection<Email>();
        public ObservableCollection<Email> FilteredEmails
        {
            get => filteredEmails;
            set { filteredEmails = value; OnPropertyChanged(nameof(FilteredEmails)); }
        }

        private string currentFolder = "Inbox1";  // default folder
        public string CurrentFolder
        {
            get => currentFolder;
            set
            {
                if (currentFolder != value)
                {
                    currentFolder = value;
                    OnPropertyChanged(nameof(CurrentFolder));
                    // Update the filtered emails based on the new folder.
                    FilteredEmails = new ObservableCollection<Email>(
                        Emails.Where(email => email.Folder == currentFolder)
                    );
                    OnPropertyChanged(nameof(FilteredEmails));
                }
            }
        }

        // Mark SelectedEmail as nullable since no email may be selected initially.
        private Email? selectedEmail;
        public Email? SelectedEmail
        {
            get => selectedEmail;
            set
            {
                if (selectedEmail != value)
                {
                    selectedEmail = value;
                    OnPropertyChanged(nameof(SelectedEmail));
                    CommandManager.InvalidateRequerySuggested();
                }
            }
        }

        // ICommand properties for adding, deleting, and editing emails.
        public ICommand AddStaticEmailCommand { get; }
        public ICommand DeleteEmailCommand { get; }
        public ICommand EditDraftEmailCommand { get; }

        public EmailViewModel()
        {
            // Collections are already initialized inline.
            AddStaticEmailCommand = new RelayCommand(AddStaticEmail);
            DeleteEmailCommand = new RelayCommand(DeleteEmail, CanDeleteEmail);
            EditDraftEmailCommand = new RelayCommand(EditDraftEmail, CanEditDraftEmail);
        }

        // Loads the predefined list of emails.
        public void LoadEmails()
        {
            var list = new List<Email>
            {
            // Mailbox 1 - Inbox1 emails.
            new Email("ceo@company.com", new List<string> { "employee1@company.com", "employee2@company.com" },
                "Quarterly Earnings Report",
                "Dear Team,\n\nPlease review the attached quarterly earnings report. This report includes comprehensive financial performance data, key performance indicators, and strategic recommendations for improvement. Your feedback is essential as we plan our next steps.\n\nBest regards,\nCEO",
                true, new List<string> { "Q1_Earnings.pdf" }, DateTime.Parse("2025-04-01"), "Inbox1"),
            new Email("hr@company.com", new List<string> { "employee1@company.com" },
                "Updated Leave Policy Notification",
                "Hello,\n\nWe have updated our leave policy to better align with current workforce needs. Kindly read the attached document thoroughly and reach out with any questions.\n\nThank you,\nHR Department",
                false, new List<string> { "LeavePolicy.docx" }, DateTime.Parse("2025-04-02"), "Inbox1"),
            new Email("support@company.com", new List<string> { "employee1@company.com" },
                "Scheduled IT Maintenance Downtime",
                "Attention,\n\nPlease note that due to scheduled maintenance, our IT systems will be offline from 2 AM to 4 AM tomorrow. Ensure that all your work is saved and plan accordingly.\n\nRegards,\nIT Support",
                false, new List<string>(), DateTime.Parse("2025-04-03"), "Inbox1"),
            // Mailbox 1 - Sent1 emails.
            new Email("employee1@company.com", new List<string> { "manager@company.com" },
                "Re: Quarterly Earnings Report",
                "Hi Manager,\n\nAttached are my detailed notes and suggestions regarding the quarterly earnings report. I look forward to discussing these points in our upcoming meeting.\n\nBest,\nEmployee1",
                false, new List<string> { "Notes.pdf" }, DateTime.Parse("2025-04-03"), "Sent1"),
            new Email("employee1@company.com", new List<string> { "hr@company.com" },
                "Feedback on Updated Leave Policy",
                "Dear HR,\n\nI have reviewed the new leave policy. I have a few concerns regarding the implementation timeline and the leave accrual process. Please see my detailed feedback below.\n\nRegards,\nEmployee1",
                true, new List<string>(), DateTime.Parse("2025-04-04"), "Sent1"),
            // Mailbox 1 - Drafts1 emails.
            new Email("employee1@company.com", new List<string> { "team@company.com" },
                "Team Outing Proposal",
                "Hello Team,\n\nI am proposing that we plan a team outing next month. I suggest a visit to the lakeside park for a picnic and team-building activities. Please share your thoughts and availability.\n\nBest,\nEmployee1",
                false, new List<string>(), DateTime.Parse("2025-04-05"), "Drafts1"),
            new Email("employee1@company.com", new List<string> { "employee1@company.com" },
                "Self Reminder: Project Update Deadline",
                "Reminder:\n\nPlease ensure that the project update is submitted by Friday. Also, review your performance metrics and schedule a feedback session with your supervisor.\n\nThanks,\nEmployee1",
                false, new List<string>(), DateTime.Parse("2025-04-06"), "Drafts1"),
            // Mailbox 1 - Trash1 emails.
            new Email("spam@promo.com", new List<string> { "employee1@company.com" },
                "Buy One Get One Free!",
                "Special Offer:\n\nTake advantage of our buy one, get one free offer! Visit our website to claim your discount. Limited time only!",
                false, new List<string>(), DateTime.Parse("2025-04-07"), "Trash1"),
            new Email("newsletter@ads.com", new List<string> { "employee1@company.com" },
                "Monthly Deals You Can't Miss",
                "Dear Subscriber,\n\nDiscover our monthly deals with up to 50% off on selected items. Hurry, these deals won’t last long!",
                false, new List<string>(), DateTime.Parse("2025-04-08"), "Trash1"),
            // Mailbox 2 - Inbox2 emails.
            new Email("friend@example.com", new List<string> { "me@example.com" },
                "Road Trip Invitation",
                "Hey,\n\nI'm planning a cross-country road trip and would love for you to join! Let's explore scenic routes and make unforgettable memories. Let me know if you're in.",
                false, new List<string>(), DateTime.Parse("2025-03-30"), "Inbox2"),
            new Email("news@daily.com", new List<string> { "me@example.com" },
                "Breaking News: Market Volatility",
                "Dear Reader,\n\nToday's market witnessed significant volatility due to global economic uncertainties. Stay tuned for in-depth analysis and expert commentary.",
                false, new List<string>(), DateTime.Parse("2025-03-29"), "Inbox2"),
            new Email("alerts@weather.com", new List<string> { "me@example.com" },
                "Severe Weather Warning",
                "Attention:\n\nA severe weather warning has been issued for your area. Please take all necessary precautions and stay updated with local emergency services.",
                true, new List<string>(), DateTime.Parse("2025-03-28"), "Inbox2"),
            // Mailbox 2 - Sent2 emails.
            new Email("me@example.com", new List<string> { "friend@example.com" },
                "Re: Road Trip Invitation",
                "Hi,\n\nCount me in for the road trip! I'm excited about the adventure and the chance to catch up. Let's finalize the details soon.",
                false, new List<string>(), DateTime.Parse("2025-03-31"), "Sent2"),
            new Email("me@example.com", new List<string> { "news@daily.com" },
                "Re: Market Volatility",
                "Thank you for the update on the market trends. I will keep a close eye on the developments and adjust my strategies accordingly.",
                false, new List<string>(), DateTime.Parse("2025-03-30"), "Sent2"),
            // Mailbox 2 - Drafts2 emails.
            new Email("me@example.com", new List<string> { "colleague@example.com" },
                "Brainstorming Session for New Project",
                "Hello Team,\n\nI have compiled a list of ideas for our new project. I suggest we hold a brainstorming session to discuss potential strategies, timelines, and deliverables. Please review the attached ideas and come prepared for discussion.",
                true, new List<string>(), DateTime.Parse("2025-03-28"), "Drafts2"),
            new Email("me@example.com", new List<string> { "me@example.com" },
                "Personal To-Do List",
                "Reminder:\n\n1. Buy groceries (milk, eggs, bread, and cheese)\n2. Schedule doctor's appointment\n3. Prepare the presentation for Monday's meeting\n4. Review and update the project documentation",
                false, new List<string>(), DateTime.Parse("2025-03-27"), "Drafts2"),
            // Mailbox 2 - Trash2 emails.
            new Email("spam@offer.com", new List<string> { "me@example.com" },
                "Limited Time Discount Offer",
                "Act Now:\n\nHurry and grab your discount before time runs out. Visit our site to benefit from exclusive deals available only for a limited period.",
                false, new List<string>(), DateTime.Parse("2025-03-26"), "Trash2"),
            new Email("promo@store.com", new List<string> { "me@example.com" },
                "Exclusive Offer Just for You!",
                "Dear Customer,\n\nWe are excited to offer you an exclusive discount on our latest products. This special offer is available for a short time only—don’t miss out on these savings!",
                false, new List<string>(), DateTime.Parse("2025-03-25"), "Trash2")            };

            Emails = new ObservableCollection<Email>(list);
            FilteredEmails = new ObservableCollection<Email>(
                Emails.Where(email => email.Folder == CurrentFolder)
            );
            OnPropertyChanged(nameof(FilteredEmails));
        }

        public void FilterEmailsByFolder(string folder)
        {
            CurrentFolder = folder;
            FilteredEmails = new ObservableCollection<Email>(
                Emails.Where(email => email.Folder == folder)
            );
            OnPropertyChanged(nameof(FilteredEmails));
        }

        public void FilterEmails(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                FilteredEmails = new ObservableCollection<Email>(Emails);
            }
            else
            {
                string lowerQuery = query.ToLower();
                var filtered = Emails.Where(email =>
                    email.Subject.ToLower().Contains(lowerQuery) ||
                    email.Sender.ToLower().Contains(lowerQuery)).ToList();
                FilteredEmails = new ObservableCollection<Email>(filtered);
            }
            OnPropertyChanged(nameof(FilteredEmails));
        }

        public void FilterByOption(string option)
        {
            if (option == "All")
            {
                FilteredEmails = new ObservableCollection<Email>(Emails);
            }
            else if (option == "Unread")
            {
                var filtered = Emails.Where(email => !email.IsImportant).ToList();
                FilteredEmails = new ObservableCollection<Email>(filtered);
            }
            else if (option == "Important")
            {
                var filtered = Emails.Where(email => email.IsImportant).ToList();
                FilteredEmails = new ObservableCollection<Email>(filtered);
            }
            OnPropertyChanged(nameof(FilteredEmails));
        }

        public void ToggleImportant(Email email)
        {
            email.IsImportant = !email.IsImportant;
            OnPropertyChanged(nameof(FilteredEmails));
        }

        // Adds a static email to the currently active folder.
        private void AddStaticEmail()
        {
            var newEmail = new Email(
                "static@company.com",
                new List<string> { "recipient@example.com" },
                "Static New Email",
                "This is a statically added email message.",
                false,
                new List<string>(),
                DateTime.Now,
                CurrentFolder  // use the current folder
            );

            Emails.Add(newEmail);
            if (newEmail.Folder == CurrentFolder)
            {
                FilteredEmails.Add(newEmail);
            }

            OnPropertyChanged(nameof(Emails));
            OnPropertyChanged(nameof(FilteredEmails));
        }

        // Delete the currently selected email.
        private void DeleteEmail()
        {
            if (SelectedEmail != null)
            {
                Emails.Remove(SelectedEmail);
                FilteredEmails.Remove(SelectedEmail);
                SelectedEmail = null;
                OnPropertyChanged(nameof(Emails));
                OnPropertyChanged(nameof(FilteredEmails));
            }
        }

        private bool CanDeleteEmail()
        {
            return SelectedEmail != null;
        }

        // Edit the selected draft email.
        private bool CanEditDraftEmail()
        {
            return SelectedEmail != null && SelectedEmail.Folder.StartsWith("Drafts");
        }

        private void EditDraftEmail()
        {
            if (SelectedEmail != null)
            {
                SelectedEmail.Content = "This content has been updated via the Edit command.";
                OnPropertyChanged(nameof(SelectedEmail));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
