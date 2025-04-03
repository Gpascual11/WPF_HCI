using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace EmailClient
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
                new Email { Subject = "Meeting Reminder", Sender = "boss@example.com", Date = "2025-03-25", Category = "Important" },
                new Email { Subject = "Weekend Plans", Sender = "friend@example.com", Date = "2025-03-24", Category = "Unread" },
                new Email { Subject = "Invoice #12345", Sender = "billing@company.com", Date = "2025-03-23", Category = "All" }
            };

            EmailList.ItemsSource = emails;
        }

        private void EmailList_DoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (EmailList.SelectedItem is Email selectedEmail)
            {
                MessageBox.Show($"Subject: {selectedEmail.Subject}", "Email Selected", MessageBoxButton.OK, MessageBoxImage.Information);
                EmailContent.Text = $"From: {selectedEmail.Sender}\n\n{selectedEmail.Subject}";  // Display email content
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
                return; // Prevent crashes due to uninitialized objects

            string selectedFilter = (FilterBox.SelectedItem as ComboBoxItem)?.Content?.ToString();

            if (selectedFilter == "All")
            {
                EmailList.ItemsSource = emails;
            }
            else
            {
                EmailList.ItemsSource = emails.Where(email => email.Category == selectedFilter).ToList();
            }
        }


    }

    public class Email
    {
        public string Subject { get; set; }
        public string Sender { get; set; }
        public string Date { get; set; }
        public string Category { get; set; }
    }
}
