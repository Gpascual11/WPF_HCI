﻿// EmailViewModel.cs
// ----------------------------------------------------------------------
// This file defines the EmailViewModel class, which serves as the ViewModel
// for the email client. It implements INotifyPropertyChanged to support data
// binding between the UI and the email data. The class manages a collection of
// Email objects and provides commands for adding, deleting, editing, and filtering
// emails based on various criteria.
// 
// Author: Gerard Pascual
// Date: 5/4/2025]
// Version: 3.3
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

namespace WPF_HCI
{
    /// <summary>
    /// Represents the ViewModel for managing email messages.
    /// Implements INotifyPropertyChanged to support UI data binding.
    /// </summary>
    public class EmailViewModel : INotifyPropertyChanged
    {
        // Initialize these collections inline so they are never null.

        /// <summary>
        /// Backing field for the Emails collection.
        /// </summary>
        private ObservableCollection<Email> emails = new ObservableCollection<Email>();

        /// <summary>
        /// Gets or sets the collection of all emails.
        /// </summary>
        public ObservableCollection<Email> Emails
        {
            get => emails;
            set { emails = value; OnPropertyChanged(nameof(Emails)); }
        }

        /// <summary>
        /// Backing field for the filtered emails collection.
        /// </summary>
        private ObservableCollection<Email> filteredEmails = new ObservableCollection<Email>();

        /// <summary>
        /// Gets or sets the collection of emails filtered by the current folder or search criteria.
        /// </summary>
        public ObservableCollection<Email> FilteredEmails
        {
            get => filteredEmails;
            set { filteredEmails = value; OnPropertyChanged(nameof(FilteredEmails)); }
        }

        /// <summary>
        /// Backing field for the current folder.
        /// </summary>
        private string currentFolder = "Inbox1";  // Default folder

        /// <summary>
        /// Gets or sets the current folder being viewed.
        /// When set, it updates the FilteredEmails collection to include only emails in this folder.
        /// </summary>
        public string CurrentFolder
        {
            get => currentFolder;
            set
            {
                if (currentFolder != value)
                {
                    currentFolder = value;
                    OnPropertyChanged(nameof(CurrentFolder));
                    // Update FilteredEmails based on the new folder.
                    FilteredEmails = new ObservableCollection<Email>(
                        Emails.Where(email => email.Folder == currentFolder)
                    );
                    OnPropertyChanged(nameof(FilteredEmails));
                }
            }
        }

        /// <summary>
        /// Backing field for the currently selected email.
        /// Marked as nullable because no email may be selected initially.
        /// </summary>
        private Email? selectedEmail;

        /// <summary>
        /// Gets or sets the currently selected email.
        /// </summary>
        public Email? SelectedEmail
        {
            get => selectedEmail;
            set
            {
                if (selectedEmail != value)
                {
                    selectedEmail = value;
                    OnPropertyChanged(nameof(SelectedEmail));
                    // Update command availability based on the new selection.
                    CommandManager.InvalidateRequerySuggested();
                }
            }
        }

        // ICommand properties for adding, deleting, and editing emails.

        /// <summary>
        /// Gets the command for adding a static email.
        /// </summary>
        public ICommand AddStaticEmailCommand { get; }

        /// <summary>
        /// Gets the command for deleting the currently selected email.
        /// </summary>
        public ICommand DeleteEmailCommand { get; }

        /// <summary>
        /// Gets the command for editing the currently selected draft email.
        /// </summary>
        public ICommand EditDraftEmailCommand { get; }

        /// <summary>
        /// Initializes a new instance of the EmailViewModel class.
        /// </summary>
        public EmailViewModel()
        {
            // Collections are already initialized inline.
            AddStaticEmailCommand = new RelayCommand(AddStaticEmail);
            DeleteEmailCommand = new RelayCommand(DeleteEmail, CanDeleteEmail);
            EditDraftEmailCommand = new RelayCommand(EditDraftEmail, CanEditDraftEmail);
        }

        /// <summary>
        /// Loads a predefined list of emails into the Emails collection and updates FilteredEmails.
        /// </summary>
        public void LoadEmails()
        {
            // Create a list of predefined emails.
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
                    false, new List<string>(), DateTime.Parse("2025-03-25"), "Trash2")
            };

            // Assign the loaded list to the Emails collection.
            Emails = new ObservableCollection<Email>(list);
            // Set FilteredEmails based on the current folder.
            FilteredEmails = new ObservableCollection<Email>(
                Emails.Where(email => email.Folder == CurrentFolder)
            );
            OnPropertyChanged(nameof(FilteredEmails));
        }

        /// <summary>
        /// Filters the emails based on the specified folder.
        /// Updates the current folder and the FilteredEmails collection.
        /// </summary>
        /// <param name="folder">The folder to filter emails by (e.g., Inbox1, Sent1, etc.).</param>
        public void FilterEmailsByFolder(string folder)
        {
            CurrentFolder = folder;
            FilteredEmails = new ObservableCollection<Email>(
                Emails.Where(email => email.Folder == folder)
            );
            OnPropertyChanged(nameof(FilteredEmails));
        }

        /// <summary>
        /// Filters emails based on the given option.
        /// Options include "All", "Unread", and "Important".
        /// </summary>
        /// <param name="option">The filter option.</param>
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

        /// <summary>
        /// Toggles the 'important' status of the specified email.
        /// </summary>
        /// <param name="email">The email whose importance is toggled.</param>
        public void ToggleImportant(Email email)
        {
            email.IsImportant = !email.IsImportant;
            OnPropertyChanged(nameof(FilteredEmails));
        }

        /// <summary>
        /// Adds a static email to the currently active folder.
        /// The email's folder is set to the current folder.
        /// </summary>
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
                CurrentFolder  // Use the current folder
            );

            // Add the new email to the master collection.
            Emails.Add(newEmail);
            // If the new email belongs to the current folder, add it to the filtered collection.
            if (newEmail.Folder == CurrentFolder)
            {
                FilteredEmails.Add(newEmail);
            }

            OnPropertyChanged(nameof(Emails));
            OnPropertyChanged(nameof(FilteredEmails));
        }

        /// <summary>
        /// Deletes the currently selected email.
        /// </summary>
        private void DeleteEmail()
        {
            if (SelectedEmail == null)
                return;

            string currentFolder = SelectedEmail.Folder;

            if (currentFolder.StartsWith("Trash"))
            {
                // Permanently delete the email
                Emails.Remove(SelectedEmail);
                FilteredEmails.Remove(SelectedEmail);
            }
            else
            {
                // Move to TrashX (preserving mailbox number)
                string suffix = new string(currentFolder.Where(char.IsDigit).ToArray());
                string trashFolder = $"Trash{suffix}";

                SelectedEmail.Folder = trashFolder;

                // Remove from current filtered view
                FilteredEmails.Remove(SelectedEmail);

                // Only show in Trash if that's the current view
                if (CurrentFolder == trashFolder)
                {
                    FilteredEmails.Add(SelectedEmail);
                }
            }

            SelectedEmail = null;

            OnPropertyChanged(nameof(Emails));
            OnPropertyChanged(nameof(FilteredEmails));
        }


        /// <summary>
        /// Determines whether the delete command can execute.
        /// Returns true if an email is currently selected.
        /// </summary>
        /// <returns>True if an email is selected; otherwise, false.</returns>
        private bool CanDeleteEmail()
        {
            return SelectedEmail != null;
        }

        /// <summary>
        /// Determines whether the edit command can execute.
        /// Returns true if a draft email is selected.
        /// </summary>
        /// <returns>True if the selected email is in a Draft folder; otherwise, false.</returns>
        private bool CanEditDraftEmail()
        {
            return SelectedEmail != null && SelectedEmail.Folder.StartsWith("Drafts");
        }

        /// <summary>
        /// Edits the content of the selected draft email.
        /// This method updates the content property to a hardcoded value for testing.
        /// </summary>
        private void EditDraftEmail()
        {
            if (SelectedEmail != null)
            {
                SelectedEmail.Content = "This content has been updated via the Edit command.";
                OnPropertyChanged(nameof(SelectedEmail));
            }
        }

        /// <summary>
        /// Filters emails based on a query and category (e.g., Subject, Sender, Recipient).
        /// </summary>
        public void FilterEmailsByCategory(string query, string category)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                FilteredEmails = new ObservableCollection<Email>(
                    Emails.Where(e => e.Folder == CurrentFolder));
                return;
            }

            string lowerQuery = query.ToLower();
            IEnumerable<Email> filtered = Emails.Where(e => e.Folder == CurrentFolder);

            switch (category)
            {
                case "Subject":
                    filtered = filtered.Where(e => e.Subject.ToLower().Contains(lowerQuery));
                    break;
                case "Sender":
                    filtered = filtered.Where(e => e.Sender.ToLower().Contains(lowerQuery));
                    break;
                case "Recipient":
                    filtered = filtered.Where(e =>
                        e.Recipients.Any(r => r.ToLower().Contains(lowerQuery)));
                    break;
            }

            FilteredEmails = new ObservableCollection<Email>(filtered);
        }


        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Raises the PropertyChanged event for the specified property.
        /// </summary>
        /// <param name="propertyName">The name of the property that changed.</param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
