using Microsoft.Maui.Controls;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;

namespace TARpv23_Mobiile_App
{
    public class Riigid : ContentPage
    {
        private ObservableCollection<Country> _countries;
        private ListView _listView;

        public Riigid()
        {
            Title = "Riigid";
            _countries = new ObservableCollection<Country>();

            var addButton = new Button
            {
                Text = "Lisa riik",
                Margin = new Thickness(10)
            };
            addButton.Clicked += AddCountryClicked;

            _listView = new ListView
            {
                ItemsSource = _countries,
                HasUnevenRows = true,
                ItemTemplate = new DataTemplate(() =>
                {
                    var flagImage = new Image { HeightRequest = 50, WidthRequest = 75 };
                    flagImage.SetBinding(Image.SourceProperty, "FlagImage");

                    var nameLabel = new Label { FontSize = 16, FontAttributes = FontAttributes.Bold };
                    nameLabel.SetBinding(Label.TextProperty, "Name");

                    var capitalLabel = new Label { FontSize = 14 };
                    capitalLabel.SetBinding(Label.TextProperty, "Capital");

                    var populationLabel = new Label { FontSize = 14 };
                    populationLabel.SetBinding(Label.TextProperty, new Binding("Population", stringFormat: "Rahvaarv: {0:N0}"));

                    var stack = new StackLayout
                    {
                        Orientation = StackOrientation.Horizontal,
                        Padding = 10,
                        Children = { flagImage, new StackLayout { Children = { nameLabel, capitalLabel, populationLabel } } }
                    };

                    return new ViewCell { View = stack };
                })
            };

            _listView.ItemSelected += OnCountrySelected;

            LoadCountries();

            Content = new StackLayout
            {
                Children = { addButton, _listView }
            };
        }

        private void LoadCountries()
        {
            _countries.Add(new Country { Name = "Eesti", Capital = "Tallinn", Population = 1330000, FlagImage = "flag_of_estonia.png" });
            _countries.Add(new Country { Name = "Soome", Capital = "Helsingi", Population = 5536000, FlagImage = "flag_of_filland.JPG" });
            _countries.Add(new Country { Name = "Saksamaa", Capital = "Berliin", Population = 83200000, FlagImage = "flag_of_germany.png" });
        }

        private async void AddCountryClicked(object sender, EventArgs e)
        {
            string name = await DisplayPromptAsync("Lisada riik", "Sisestage nimetus:");
            if (string.IsNullOrWhiteSpace(name)) return;

            string capital = await DisplayPromptAsync("Pealinn", "Sisestage pealinn:");
            if (string.IsNullOrWhiteSpace(capital)) return;

            string populationStr = await DisplayPromptAsync("Rahvastik", "Sisesta number:", keyboard: Keyboard.Numeric);
            if (!int.TryParse(populationStr, out int population)) return;

            string imagePath = await PickImageFromGallery();

            _countries.Add(new Country { Name = name, Capital = capital, Population = population, FlagImage = imagePath ?? "noflag.png" });
        }

        private async Task<string> PickImageFromGallery()
        {
            try
            {
                var result = await FilePicker.PickAsync(new PickOptions
                {
                    PickerTitle = "vali pilt",
                    FileTypes = FilePickerFileType.Images
                });

                return result?.FullPath;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private async void OnCountrySelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is Country selectedCountry)
            {
                ((ListView)sender).SelectedItem = null;

                await Navigation.PushAsync(new CountryDetailPage(selectedCountry));
            }
        }
    }

    public class Country : INotifyPropertyChanged
    {
        private string _name;
        private string _capital;
        private int _population;
        private string _flagImage;

        public string Name { get => _name; set { _name = value; OnPropertyChanged(nameof(Name)); } }
        public string Capital { get => _capital; set { _capital = value; OnPropertyChanged(nameof(Capital)); } }
        public int Population { get => _population; set { _population = value; OnPropertyChanged(nameof(Population)); } }
        public string FlagImage { get => _flagImage; set { _flagImage = value; OnPropertyChanged(nameof(FlagImage)); } }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public class CountryDetailPage : ContentPage
    {
        private Country _country;
        private Entry _nameEntry, _capitalEntry, _populationEntry;
        private Image _flagImage;
        private Button _changeImageButton;

        public CountryDetailPage(Country country)
        {
            _country = country;
            Title = country.Name;

            _flagImage = new Image { Source = _country.FlagImage, HeightRequest = 200, WidthRequest = 300 };

            _nameEntry = new Entry { Text = _country.Name, FontSize = 24, FontAttributes = FontAttributes.Bold };
            _nameEntry.TextChanged += (s, e) => _country.Name = _nameEntry.Text;

            _capitalEntry = new Entry { Text = _country.Capital, FontSize = 18 };
            _capitalEntry.TextChanged += (s, e) => _country.Capital = _capitalEntry.Text;

            _populationEntry = new Entry { Text = _country.Population.ToString(), FontSize = 18, Keyboard = Keyboard.Numeric };
            _populationEntry.TextChanged += (s, e) =>
            {
                if (int.TryParse(_populationEntry.Text, out int pop))
                    _country.Population = pop;
            };

            _changeImageButton = new Button { Text = "Vahetada lipp" };
            _changeImageButton.Clicked += OnChangeImageClicked;

            var saveButton = new Button { Text = "salvestada" };
            saveButton.Clicked += async (s, e) => await Navigation.PopAsync();

            var layout = new StackLayout
            {
                Padding = 20,
                Children = { _flagImage, _changeImageButton, _nameEntry, _capitalEntry, _populationEntry, saveButton }
            };

            Content = layout;
        }

        private async void OnChangeImageClicked(object sender, EventArgs e)
        {
            string newImagePath = await PickImageFromGallery();
            if (!string.IsNullOrWhiteSpace(newImagePath))
            {
                _country.FlagImage = newImagePath;
                _flagImage.Source = newImagePath;
            }
        }

        private async Task<string> PickImageFromGallery()
        {
            try
            {
                var result = await FilePicker.PickAsync(new PickOptions
                {
                    PickerTitle = "Valige lipp",
                    FileTypes = FilePickerFileType.Images
                });

                return result?.FullPath;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
