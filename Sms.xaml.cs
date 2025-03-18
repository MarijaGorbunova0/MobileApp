using Microsoft.Maui.Controls;
using Microsoft.Maui.ApplicationModel.Communication;
using Microsoft.Maui.Media;
using Microsoft.Maui.Storage;
using System.IO;
using Microsoft.Maui.Layouts;

namespace TARpv23_Mobiile_App
{
    public partial class Sms : ContentPage
    {

        private VerticalStackLayout kontaktLayout;

        private Button addContactButton;

        public Sms()
        {

            addContactButton = new Button
            {
                Text = "Добавить новый контакт",
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.End,
                BackgroundColor = Colors.LightGray,
                TextColor = Colors.Black
            };
            addContactButton.Clicked += AddContactButton_Clicked;

            kontaktLayout = new VerticalStackLayout();


            AddContactToLayout("Иван Иванов", "123-456-789", "ivan@mail.com", null);

            var layout = new AbsoluteLayout
            {
                Children =
                {
                    addContactButton
                }
            };

            AbsoluteLayout.SetLayoutBounds(addContactButton, new Rect(1, 0, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
            AbsoluteLayout.SetLayoutFlags(addContactButton, AbsoluteLayoutFlags.PositionProportional);

            var scrollView = new ScrollView
            {
                Content = new VerticalStackLayout
                {
                    Children = { kontaktLayout }
                }
            };

            layout.Children.Add(scrollView);

            Content = layout;
        }

        private void AddContactToLayout(string name, string phone, string email, ImageSource photo)
        {
            var nameLabel = new Label
            {
                Text = $"Имя: {name}",
                FontSize = 18,
                HorizontalOptions = LayoutOptions.Start
            };

            var phoneLabel = new Label
            {
                Text = $"Телефон: {phone}",
                FontSize = 18,
                HorizontalOptions = LayoutOptions.Start
            };

            var emailLabel = new Label
            {
                Text = $"Email: {email}",
                FontSize = 18,
                HorizontalOptions = LayoutOptions.Start
            };

            var contactImage = new Image
            {
                Source = photo,
                HeightRequest = 100,
                WidthRequest = 100,
                HorizontalOptions = LayoutOptions.Center
            };

            var smsButton = new Button
            {
                Text = "Написать SMS",
                BackgroundColor = Colors.LightBlue,
                TextColor = Colors.White
            };
            smsButton.Clicked += async (sender, e) =>
            {
                try
                {
                    var smsMessage = new SmsMessage("", phone); 
                    await Microsoft.Maui.ApplicationModel.Communication.Sms.Default.ComposeAsync(smsMessage);
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Ошибка", $"Не удалось открыть SMS: {ex.Message}", "OK");
                }
            };

            var emailButton = new Button
            {
                Text = "Написать на почту",
                BackgroundColor = Colors.LightGreen,
                TextColor = Colors.White
            };
            emailButton.Clicked += async (sender, e) =>
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
                    await DisplayAlert("Ошибка", $"Не удалось открыть почту: {ex.Message}", "OK");
                }
            };

            var callButton = new Button
            {
                Text = "Позвонить",
                BackgroundColor = Colors.LightCoral,
                TextColor = Colors.White
            };
            callButton.Clicked += (sender, e) =>
            {
                try
                {
                    if (PhoneDialer.IsSupported)
                    {
                        PhoneDialer.Open(phone);
                    }
                    else
                    {
                        DisplayAlert("Ошибка", "Звонки не поддерживаются на этом устройстве.", "OK");
                    }
                }
                catch (Exception ex)
                {
                    DisplayAlert("Ошибка", $"Не удалось совершить звонок: {ex.Message}", "OK");
                }
            };

            var buttonsLayout = new HorizontalStackLayout
            {
                Children = { smsButton, emailButton, callButton }
            };

            var contactLayout = new VerticalStackLayout
            {
                Children = { contactImage, nameLabel, phoneLabel, emailLabel, buttonsLayout },
                BackgroundColor = Colors.LightSkyBlue, 
                Padding = new Thickness(10),
                Spacing = 10
            };

            kontaktLayout.Children.Add(contactLayout);
        }

        private async void AddContactButton_Clicked(object sender, EventArgs e)
        {

            var nameEntry = new Entry { Placeholder = "Введите имя" };
            var phoneEntry = new Entry { Placeholder = "Введите телефон", Keyboard = Keyboard.Telephone };
            var emailEntry = new Entry { Placeholder = "Введите email", Keyboard = Keyboard.Email };

            var takePhotoButton = new Button
            {
                Text = "Сделать фото",
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

            takePhotoButton.Clicked += async (sender, e) =>
            {
                try
                {

                    var status = await Permissions.RequestAsync<Permissions.Camera>();
                    if (status != PermissionStatus.Granted)
                    {
                        await DisplayAlert("Ошибка", "Разрешение на использование камеры не предоставлено.", "OK");
                        return;
                    }

                    // Создание фото
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
                    await DisplayAlert("Ошибка", $"Не удалось сделать фото: {ex.Message}", "OK");
                }
            };

            var saveButton = new Button
            {
                Text = "Сохранить",
                VerticalOptions = LayoutOptions.EndAndExpand
            };

            saveButton.Clicked += async (s, args) =>
            {
                string name = nameEntry.Text;
                string phone = phoneEntry.Text;
                string email = emailEntry.Text;

                if (photoBytes != null)
                {
                    AddContactToLayout(name, phone, email, ImageSource.FromStream(() => new MemoryStream(photoBytes)));
                }
                else
                {
                    AddContactToLayout(name, phone, email, null);
                }

                await DisplayAlert("Успех", "Контакт добавлен!", "OK");
            };

            var contactFormLayout = new VerticalStackLayout
            {
                Children = { nameEntry, phoneEntry, emailEntry, takePhotoButton, contactPhoto, saveButton }
            };


            await Navigation.PushModalAsync(new ContentPage
            {
                Title = "Добавить контакт",
                Content = new ScrollView { Content = contactFormLayout }
            });
        }
    }
}
