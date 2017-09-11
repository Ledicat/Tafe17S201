﻿// **************************************************************************
//Start Finance - An to manage your personal finances.

//Start Finance is free software: you can redistribute it and/or modify
//it under the terms of the GNU General Public License as published by
//the Free Software Foundation, either version 3 of the License, or
//(at your option) any later version.

//Start Finance is distributed in the hope that it will be useful,
//but WITHOUT ANY WARRANTY; without even the implied warranty of
//MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
//GNU General Public License for more details.

//You should have received a copy of the GNU General Public License
//along with Start Finance.If not, see<http://www.gnu.org/licenses/>.
// ***************************************************************************

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using SQLite;
using StartFinance.Models;
using Windows.UI.Popups;
using SQLite.Net;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace StartFinance.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PersonalDetailsPage : Page
    {
            SQLiteConnection conn; // adding an SQLite connection
            string path = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "Findata.sqlite");

        public PersonalDetailsPage()
        {
            this.InitializeComponent();
            NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;
            /// Initializing a database
            conn = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), path);
            // Creating table
            Results();
        }

        public void Results()
        {
            conn.CreateTable<PersonalDetails>();
            var query1 = conn.Table<PersonalDetails>();
            PersonalDetailsView.ItemsSource = query1.ToList();
        }

        private async void AddPersonalDetails_Click(object sender, RoutedEventArgs e)
        {
            try
                {
                     //Data Validation
                    if (_FirstName.Text.ToString() == "")
                    {
                        MessageDialog dialog = new MessageDialog("No first name entered", "Oops..!");
                        await dialog.ShowAsync();
                    }
                    else if (_LastName.Text.ToString() == "")
                    {
                        MessageDialog dialog = new MessageDialog("No last name entered", "Oops..!");
                        await dialog.ShowAsync();
                    }
                    else if (_DateOfBirth.ToString() == "")
                    {
                        MessageDialog dialog = new MessageDialog("No date of birth entered", "Oops..!");
                        await dialog.ShowAsync();
                    }
                    else if (_Gender.SelectedValue.ToString() == "")
                    {
                        MessageDialog dialog = new MessageDialog("No gender entered", "Oops..!");
                        await dialog.ShowAsync();
                    }
                    else if (_EmailAddress.Text.ToString() == "")
                    {
                        MessageDialog dialog = new MessageDialog("No email entered", "Oops..!");
                        await dialog.ShowAsync();
                    }



                else
                {
                   
                    conn.CreateTable<PersonalDetails>();
                    conn.Insert(new PersonalDetails
                    {
                        FirstName = _FirstName.Text.ToString(),
                    });
                    // Creating table
                    Results();
                }
            }
                catch (Exception ex)
                {
                    if (ex is FormatException)
                   {
                        MessageDialog dialog = new MessageDialog("You forgot to enter the Amount or entered an invalid Amount", "Oops..!");
                        await dialog.ShowAsync();
                    }
                else if (ex is SQLiteException)
                {
                    MessageDialog dialog = new MessageDialog("Wish Name already exist, Try Different Name", "Oops..!");
                    await dialog.ShowAsync();
                }
                else
                {
                    /// no idea
                }
            }
        }

            private async void DeleteItem_Click(object sender, RoutedEventArgs e)
            {
            try
            {
                string AccSelection = ((WishList)PersonalDetailsView.SelectedItem).WishName;
                if (AccSelection == "")
                {
                    MessageDialog dialog = new MessageDialog("Not selected the Item", "Oops..!");
                    await dialog.ShowAsync();
                }
                else
                {
                    conn.CreateTable<WishList>();
                    var query1 = conn.Table<WishList>();
                    var query3 = conn.Query<WishList>("DELETE FROM WishList WHERE WishName ='" + AccSelection + "'");
                    PersonalDetailsView.ItemsSource = query1.ToList();
                }
            }
            catch (NullReferenceException)
                {
                    MessageDialog dialog = new MessageDialog("Not selected the Item", "Oops..!");
                    await dialog.ShowAsync();
                }
            }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Results();
        }

    }
}
