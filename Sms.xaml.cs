using Microsoft.Maui.Controls;
using Microsoft.Maui.ApplicationModel.Communication;
using Microsoft.Maui.Media;
using Microsoft.Maui.Storage;
using System.IO;
using System.Collections.Generic;

namespace TARpv23_Mobiile_App
{
    public partial class Sms : ContentPage
    {
        private TableView contactsTableView;
        private Button addContactBTN;

        public Sms()
        {

            addContactBTN = new Button
            {
                Text = "Lisada uus kontakt",
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.End,
                BackgroundColor = Colors.LightGray,
                TextColor = Colors.Black
            };
            addContactBTN.Clicked += AddContactBTN_Clicked;

            contactsTableView = new TableView
            {
                Intent = TableIntent.Data,
                Root = new TableRoot("kontaktid")
            };

            AddContactTB("Roma Pro", "123456789", "romapro@mail.com", null);

            var layout = new VerticalStackLayout
            {
                Children = { addContactBTN, contactsTableView }
            };

            Content = layout;
        }

        private void AddContactTB(string name, string phone, string email, ImageSource photo)
        {
            var contactSection = new TableSection(name)
            {
                new TextCell { Text = "nimi", Detail = name },
                new TextCell { Text = "telefon", Detail = phone },
                new TextCell { Text = "Email", Detail = email }
            };

            if (photo != null)
            {
                contactSection.Add(new ImageCell { Text = "foto", ImageSource = photo });
            }

            contactSection.Add(new ViewCell
            {
                View = new HorizontalStackLayout
                {
                    Children =
                    {
                        new Button
                        {
                            Text = "SMS",
                            BackgroundColor = Colors.LightBlue,
                            TextColor = Colors.White,
                            WidthRequest = 130,
                            Command = new Command(async () =>
                            {
                                try
                                {
                                    var smsMessage = new SmsMessage("", phone);
                                    await Microsoft.Maui.ApplicationModel.Communication.Sms.Default.ComposeAsync(smsMessage);
                                }
                                catch (Exception ex)
                                {
                                    await DisplayAlert("viga", $"ei saanud avada SMS: {ex.Message}", "OK");
                                }
                            })
                        },
                        new Button
                        {
                            Text = "Email",
                            BackgroundColor = Colors.LightGreen,
                            TextColor = Colors.White,
                            WidthRequest = 130,
                            Command = new Command(async () =>
                            {
                                try
                                {
                                    var emailMessage = new EmailMessage
                                    {
                                        To = new List<string> { email },
                                        Subject = "",
                                        Body = ""
                                    };
                                    await Email.Default.ComposeAsync(emailMessage);
                                }
                                catch (Exception ex)
                                {
                                    await DisplayAlert("viga", $"ei saanud avada email: {ex.Message}", "OK");
                                }
                            })
                        },
                        new Button
                        {
                            Text = "helista",
                            BackgroundColor = Colors.LightCoral,
                            TextColor = Colors.White,
                            WidthRequest = 130,
                            Command = new Command(() =>
                            {
                                try
                                {
                                    if (PhoneDialer.IsSupported)
                                    {
                                        PhoneDialer.Open(phone);
                                    }
                                    else
                                    {
                                        DisplayAlert("viga", "ei saanud helistada", "OK");
                                    }
                                }
                                catch (Exception ex)
                                {
                                    DisplayAlert("viga", $"mingi pribleem {ex.Message}", "OK");
                                }
                            })
                        }
                    }
                }
            }); 

            contactsTableView.Root.Add(contactSection);
        }

        private async void AddContactBTN_Clicked(object sender, EventArgs e)
        {
            var nameEntry = new Entry { Placeholder = "Nimi" };
            var phoneEntry = new Entry { Placeholder = "telefon", Keyboard = Keyboard.Telephone };
            var emailEntry = new Entry { Placeholder = "email", Keyboard = Keyboard.Email };

            var takePhotoBTN = new Button
            {
                Text = "teha foto",
                BackgroundColor = Colors.LightGray,
                TextColor = Colors.White
            };

            Image contactPhoto = new Image
            {
                HeightRequest = 100,
                WidthRequest = 100,
                HorizontalOptions = LayoutOptions.Center
            };

            byte[] photoBytes = null;

            takePhotoBTN.Clicked += async (sender, e) =>
            {
                try
                {
                    var status = await Permissions.RequestAsync<Permissions.Camera>();
                    if (status != PermissionStatus.Granted)
                    {
                        await DisplayAlert("viga", "ei ole õigust", "OK");
                        return;
                    }

                    var photo = await MediaPicker.CapturePhotoAsync();
                    if (photo != null)
                    {
                        using (var stream = await photo.OpenReadAsync())
                        using (var memoryStream = new MemoryStream())
                        {
                            await stream.CopyToAsync(memoryStream);
                            photoBytes = memoryStream.ToArray();
                        }

                        contactPhoto.Source = ImageSource.FromStream(() => new MemoryStream(photoBytes));
                    }
                }
                catch (Exception ex)
                {
                    await DisplayAlert("viga", $"ei saanud teha foto {ex.Message}", "OK");
                }
            };

            var saveBTN = new Button
            {
                Text = "salvestama",
                VerticalOptions = LayoutOptions.EndAndExpand
            };

            saveBTN.Clicked += async (s, args) =>
            {
                string name = nameEntry.Text;
                string phone = phoneEntry.Text;
                string email = emailEntry.Text;

                if (photoBytes != null)
                {
                    AddContactTB(name, phone, email, ImageSource.FromStream(() => new MemoryStream(photoBytes)));
                }
                else
                {
                    AddContactTB(name, phone, email, null);
                }

                await DisplayAlert("edukas", "kontakt on lisanud!", "OK");
            };

            var contactFormLayout = new VerticalStackLayout
            {
                Children = { nameEntry, phoneEntry, emailEntry, takePhotoBTN, contactPhoto, saveBTN }
            };

            await Navigation.PushModalAsync(new ContentPage
            {
                Title = "lisada kontakt",
                Content = new ScrollView { Content = contactFormLayout }
            });
        }
    }
}