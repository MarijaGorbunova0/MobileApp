using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TARpv23_Mobiile_App
{

    public partial class Sms : ContentPage
    {
        TableView tabelview;
        SwitchCell sc;
        ImageCell ic;
        TableSection fotosection;
        private Button  EmailBTN;
        TableSection kontaktSection;

        public Sms()
        {
            sc = new SwitchCell
            {
                Text = "Näita veel"
            };
            sc.OnChanged += Sc_OnChanged;

            ic = new ImageCell
            {
                ImageSource = ImageSource.FromFile("bob.jpg"), 
                Text = "Foto nimetus",
                Detail = "Foto kirjeldus"
       
            };


            fotosection = new TableSection("Fotosektsioon")
            {
            sc  
            };
            kontaktSection = new TableSection("Kontaktandmed")
            {

            };

            tabelview = new TableView
            {
                Intent = TableIntent.Form, 
                Root = new TableRoot("Andmete sisestamine")
            {
                new TableSection("Põhiandmed:")
                {
                    new EntryCell
                    {
                        Label = "Nimi:",
                        Placeholder = "Sisesta oma sõbra nimi",
                        Keyboard = Keyboard.Default
                    }
                },

                new TableSection("Kontaktandmed:")
                {
                    new EntryCell
                    {
                        Label = "Telefon",
                        Placeholder = "Sisesta tel. number",
                        Keyboard = Keyboard.Telephone
                    },
                    new EntryCell
                    {
                        Label = "Email",
                        Placeholder = "Sisesta email",
                        Keyboard = Keyboard.Email
                    }
                },
                fotosection,
                kontaktSection
            }
            };
            Content = tabelview;
        }
        private void Sc_OnChanged(object sender, ToggledEventArgs e)
        {
            if (e.Value)
            {
                fotosection.Title = "Foto:";
                fotosection.Add(ic); 
                sc.Text = "Peida";  
            }
            else
            {
                fotosection.Title = ""; 
                fotosection.Remove(ic);
                sc.Text = "Näita veel"; 
            }
        }
        private void ShowKontakt(object sender, ToggledEventArgs e)
        {
            if (e.Value)
            {
                string phone = "123456789";  
                string email = "test@example.com"; 


                kontaktSection.Clear(); 
                kontaktSection.Add(new ViewCell
                {
                    View = new Label
                    {
                        Text = $"Telefon: {phone}",
                        FontSize = 18,
                        HorizontalOptions = LayoutOptions.Start
                    }
                });
                kontaktSection.Add(new ViewCell
                {
                    View = new Label
                    {
                        Text = $"Email: {email}",
                        FontSize = 18,
                        HorizontalOptions = LayoutOptions.Start
                    }
                });
                kontaktSection.Add(new ViewCell
                {
                    View = new Button
                    {
                        Text = "Saada SMS",
                        VerticalOptions = LayoutOptions.CenterAndExpand,
                        Command = new Command(() => SendSms(phone))
                    }
                });
            }
            else
            {

                kontaktSection.Clear();
            }
        }

        private async void SendSms(string phone)
        {
            var message = "Tere, see on test SMS!";
            SmsMessage sms = new SmsMessage(message, phone);


            if (Sms.Default.IsComposeSupported)
            {
                await Sms.Default.ComposeAsync(sms);
            }
            else
            {
                await DisplayAlert("Viga", "SMS saatmine ei ole toetatud sellel seadmel.", "OK");
            }
        }
    }
}