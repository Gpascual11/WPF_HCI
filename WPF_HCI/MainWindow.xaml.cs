using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WPF_HCI;

namespace WPF_HCI
{
    public partial class MainWindow : Window
    {
        private List<Email> emails = new List<Email>();  // Email storage list

        public MainWindow()
        {
            InitializeComponent();
            this.WindowState = WindowState.Normal; // Ensure window is visible
            LoadEmails();

            // Attach the event handler after data is loaded to avoid null reference errors
            FilterBox.SelectionChanged += FilterBox_SelectionChanged;
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void LoadEmails()
        {
            emails = new List<Email>
    {
        // Emails for Mailbox 1
        // Inbox1
        new Email("boss@example.com", new List<string> { "employee1@example.com" },
            "Meeting Reminder", "Don't forget the meeting!", true,
            new List<string> { "attachment1.pdf" }, DateTime.Parse("2025-03-25"), "Inbox1"),

        new Email("manager@example.com", new List<string> { "employee1@example.com" },
            "Quarterly Report", "The quarterly report is due by next week.", false,
            new List<string>(), DateTime.Parse("2025-03-26"), "Inbox1"),

        // Sent1
        new Email("client@example.com", new List<string> { "me@example.com" },
            "Project Update", "Please review the attached document.", false,
            new List<string> { "project.pdf" }, DateTime.Parse("2025-03-24"), "Sent1"),

        new Email("boss@example.com", new List<string> { "employee1@example.com" },
            "Follow Up", "Just following up on the meeting.", true,
            new List<string>(), DateTime.Parse("2025-03-27"), "Sent1"),

        // Drafts1
        new Email("me@example.com", new List<string> { "client@example.com" },
            "Subject for Review", "Here's the draft for your review.", false,
            new List<string>(), DateTime.Parse("2025-03-28"), "Drafts1"),

        new Email("me@example.com", new List<string> { "team@example.com" },
            "Important Discussion", "Let's discuss the quarterly strategies.", true,
            new List<string>(), DateTime.Parse("2025-03-29"), "Drafts1"),

        // Trash1
        new Email("spam@example.com", new List<string> { "me@example.com" },
            "Limited Time Offer", "Hurry! Act Now!", false,
            new List<string>(), DateTime.Parse("2025-03-30"), "Trash1"),

        new Email("marketing@example.com", new List<string> { "me@example.com" },
            "Exclusive Deal!", "Get 50% off on all items!", false,
            new List<string>(), DateTime.Parse("2025-03-31"), "Trash1"),

        // Emails for Mailbox 2
        // Inbox2
        new Email("friend@example.com", new List<string> { "me@example.com" },
            "Weekend Plans", "Are we still on for the weekend?", false,
            new List<string>(), DateTime.Parse("2025-03-23"), "Inbox2"),

        new Email("billing@company.com", new List<string> { "me@example.com" },
            "Invoice #12345", "Please find your invoice attached.", false,
            new List<string> { "invoice12345.pdf" }, DateTime.Parse("2025-03-22"), "Inbox2"),

        // Sent2
        new Email("me@example.com", new List<string> { "friend@example.com" },
            "Re: Weekend Plans", "Yes, looking forward to it!", false,
            new List<string>(), DateTime.Parse("2025-03-24"), "Sent2"),

        new Email("me@example.com", new List<string> { "client@example.com" },
            "Re: Project Update", "I've reviewed the document. Let's meet next week.", false,
            new List<string>(), DateTime.Parse("2025-03-25"), "Sent2"),

        // Drafts2
        new Email("me@example.com", new List<string> { "team@example.com" },
            "Upcoming Team Meeting", "Here's the agenda for the upcoming meeting.", true,
            new List<string>(), DateTime.Parse("2025-03-26"), "Drafts2"),

        new Email("me@example.com", new List<string> { "boss@example.com" },
            "Quarterly Review", "Please find the details for the quarterly review.", false,
            new List<string>(), DateTime.Parse("2025-03-27"), "Drafts2"),

        // Trash2
        new Email("newsletter@example.com", new List<string> { "me@example.com" },
            "Monthly Newsletter", "Check out the latest news!", false,
            new List<string>(), DateTime.Parse("2025-03-28"), "Trash2"),

        new Email("promotions@example.com", new List<string> { "me@example.com" },
            "Exclusive Offer Just for You!", "Test Trash", false,
            new List<string>(), DateTime.Parse("2025-03-29"), "Trash2"),
    };

            // Default: Show emails from Inbox1
            EmailList.ItemsSource = emails.Where(email => email.Folder == "Inbox1").ToList();
        }



        private void EmailList_DoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (EmailList.SelectedItem is Email selectedEmail)
            {
                // EmailContent.Text = $"From: {selectedEmail.Sender}\n\nSubject: {selectedEmail.Subject}\n\n{selectedEmail.Content}";
            }
        }



        private void SearchBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (SearchBox.Text == "Search...")
            {
                SearchBox.Text = "";
                SearchBox.Foreground = System.Windows.Media.Brushes.Black;
            }
        }

        private void SearchBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SearchBox.Text))
            {
                SearchBox.Text = "Search...";
                SearchBox.Foreground = System.Windows.Media.Brushes.Gray;
            }
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SearchBox.Text == "Search...") return;

            string query = SearchBox.Text.ToLower();
            EmailList.ItemsSource = emails.Where(email =>
                email.Subject.ToLower().Contains(query) || email.Sender.ToLower().Contains(query)).ToList();
        }

        private void FilterBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (emails == null || EmailList == null || FilterBox?.SelectedItem == null)
                return;

            string selectedFilter = (FilterBox.SelectedItem as ComboBoxItem)?.Content?.ToString();

            if (selectedFilter == "All")
            {
                EmailList.ItemsSource = emails;
            }
            else if (selectedFilter == "Unread") 
            {
                EmailList.ItemsSource = emails.Where(email => !email.IsImportant).ToList();
            }
            else if (selectedFilter == "Important")
            {
                EmailList.ItemsSource = emails.Where(email => email.IsImportant).ToList();
            }
        }


        private void ToggleImportant_Click(object sender, RoutedEventArgs e)
        {
            if (EmailList.SelectedItem is Email selectedEmail)
            {
                selectedEmail.IsImportant = !selectedEmail.IsImportant;
                // Refresh the ListView to reflect the change
                EmailList.Items.Refresh();
            }
        }


        private void FolderTree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (FolderTree.SelectedItem is TreeViewItem selectedFolder)
            {
                string folderTag = selectedFolder.Tag?.ToString();

                if (!string.IsNullOrEmpty(folderTag))
                {
                    // Filter emails based on the selected mailbox and folder
                    List<Email> filteredEmails = emails.Where(email => email.Folder == folderTag).ToList();
                    EmailList.ItemsSource = filteredEmails;
                }
            }
        }

        private void EmailList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Implement your logic here, or leave it empty if not needed.
        }

    }
}
