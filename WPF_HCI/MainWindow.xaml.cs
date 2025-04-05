using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WPF_HCI;

namespace WPF_HCI
{
    // MainWindow class serves as the primary window of the email client.
    public partial class MainWindow : Window
    {
        // List to store Email objects. This list is used as the data source for the email ListView.
        private List<Email> emails = new List<Email>();

        // Constructor for the MainWindow.
        public MainWindow()
        {
            InitializeComponent(); // Initialize all components defined in the XAML.
            this.WindowState = WindowState.Normal; // Ensure the window is in its normal (non-minimized/maximized) state.
            LoadEmails(); // Load the predefined list of emails into the application.

            // Attach the event handler for the FilterBox selection change.
            // This is done after loading emails to avoid potential null reference errors.
            FilterBox.SelectionChanged += FilterBox_SelectionChanged;
        }

        // Event handler for the "Exit" menu item.
        // When clicked, the application will close.
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown(); // Shuts down the application.
        }

        // Method to load a predefined list of emails.
        // Emails are added for different mailboxes/folders such as Inbox, Sent, Drafts, and Trash.
        private void LoadEmails()
        {
            emails = new List<Email>
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

            // Set the default view of the EmailList ListView to show emails from the "Inbox1" folder.
            EmailList.ItemsSource = emails.Where(email => email.Folder == "Inbox1").ToList();
        }

        // Event handler for double-clicking an email in the EmailList ListView.
        // Displays a message box showing the "Subject" of the selected email.
        private void EmailList_DoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // Check if an email is selected in the ListView.
            if (EmailList.SelectedItem is Email selectedEmail)
            {
                // Show a message box with the email's subject.
                MessageBox.Show("Subject: " + selectedEmail.Subject, "Email Subject", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        // Event handler for when the SearchBox gains focus.
        // Clears the default "Search..." text and changes the text color for user input.
        private void SearchBox_GotFocus(object sender, RoutedEventArgs e)
        {
            // If the TextBox contains the default text, clear it.
            if (SearchBox.Text == "Search...")
            {
                SearchBox.Text = "";
                SearchBox.Foreground = System.Windows.Media.Brushes.Black; // Set text color to black for input visibility.
            }
        }

        // Event handler for when the SearchBox loses focus.
        // If the TextBox is empty, restores the default "Search..." text and color.
        private void SearchBox_LostFocus(object sender, RoutedEventArgs e)
        {
            // If no text is entered, reset the TextBox.
            if (string.IsNullOrWhiteSpace(SearchBox.Text))
            {
                SearchBox.Text = "Search...";
                SearchBox.Foreground = System.Windows.Media.Brushes.Gray; // Set text color to gray to indicate placeholder text.
            }
        }

        // Event handler for text changes in the SearchBox.
        // Filters the EmailList based on the user's query (searches in the email subject and sender).
        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Do nothing if the TextBox still contains the default text.
            if (SearchBox.Text == "Search...") return;

            // Convert the search query to lowercase for a case-insensitive search.
            string query = SearchBox.Text.ToLower();

            // Filter the emails list and update the EmailList ItemsSource accordingly.
            EmailList.ItemsSource = emails.Where(email =>
                email.Subject.ToLower().Contains(query) || email.Sender.ToLower().Contains(query)).ToList();
        }

        // Event handler for selection changes in the FilterBox ComboBox.
        // Filters the EmailList based on the selected filter option (All, Unread, or Important).
        private void FilterBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Verify that emails, EmailList, and FilterBox's selected item are not null.
            if (emails == null || EmailList == null || FilterBox?.SelectedItem == null)
                return;

            // Retrieve the selected filter option.
            string selectedFilter = (FilterBox.SelectedItem as ComboBoxItem)?.Content?.ToString();

            // Apply the appropriate filter to the EmailList.
            if (selectedFilter == "All")
            {
                EmailList.ItemsSource = emails; // Show all emails.
            }
            else if (selectedFilter == "Unread")
            {
                // Filter emails that are not marked as important (assuming "important" here means the email is read).
                EmailList.ItemsSource = emails.Where(email => !email.IsImportant).ToList();
            }
            else if (selectedFilter == "Important")
            {
                // Show only emails marked as important.
                EmailList.ItemsSource = emails.Where(email => email.IsImportant).ToList();
            }
        }

        // Event handler for the "Toggle Important" button click.
        // Toggles the importance flag for the selected email and refreshes the ListView.
        private void ToggleImportant_Click(object sender, RoutedEventArgs e)
        {
            // Check if an email is selected in the EmailList.
            if (EmailList.SelectedItem is Email selectedEmail)
            {
                // Toggle the IsImportant property.
                selectedEmail.IsImportant = !selectedEmail.IsImportant;
                // Refresh the ListView to update the display (for example, background color changes for important emails).
                EmailList.Items.Refresh();
            }
        }

        // Event handler that is triggered when the selected item in the FolderTree changes.
        private void FolderTree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            // Check if the newly selected item is a TreeViewItem.
            if (FolderTree.SelectedItem is TreeViewItem selectedFolder)
            {
                // Retrieve the folder identifier from the TreeViewItem's Tag property.
                // This identifier is used to determine which folder (e.g., Inbox1, Sent1, etc.) is selected.
                string folderTag = selectedFolder.Tag?.ToString();

                // If the folderTag is not null or empty, proceed to filter the emails.
                if (!string.IsNullOrEmpty(folderTag))
                {
                    // Use LINQ to filter the 'emails' list so that only emails belonging to the selected folder are included.
                    // The filtered list is created by checking each email's Folder property against the folderTag.
                    List<Email> filteredEmails = emails.Where(email => email.Folder == folderTag).ToList();

                    // Update the ItemsSource property of the EmailList ListView.
                    // Data binding is used here so that the ListView displays the filtered list of emails.
                    EmailList.ItemsSource = filteredEmails;
                }
            }
        }


        // Event handler for selection changes in the EmailList ListView.
        // Currently a placeholder for additional logic, such as updating the email details view.
        private void EmailList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Additional logic can be implemented here if needed.
        }
    }
}
