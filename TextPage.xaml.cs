namespace TARpv23_Mobiile_App;

public partial class TextPage : ContentPage
{
    Label lbl;
    Editor editor;
    HorizontalStackLayout hsl;
    List<string> buttons = new List<string> { "Tagasi", "Avaleht", "Edasi" };
    Random rnd = new Random();

    public TextPage(int k)
    {
        BackgroundColor = Color.FromArgb("#C2E3E3");
        lbl = new Label
        {
            Text = "Pealkiri",
            TextColor = Color.FromArgb("#000000f"),
            FontFamily = "OpenSans-Regular",
            FontAttributes = FontAttributes.Bold,
            TextDecorations = TextDecorations.Underline,
            FontSize = 28,
            HorizontalTextAlignment = TextAlignment.Center,
            VerticalTextAlignment = TextAlignment.Center,    
        };

        TapGestureRecognizer tapGestureRecognizer = new TapGestureRecognizer();
        tapGestureRecognizer.Tapped += TapText_Tapped;
        lbl.GestureRecognizers.Add(tapGestureRecognizer);

        editor = new Editor
        {
            Placeholder = "Vihje: Sisesta siia tekst",
            PlaceholderColor = Color.FromArgb("#A8E0E4"),
            BackgroundColor = Color.FromArgb("#A8E0E4"),
            TextColor = Color.FromArgb("#000000f"),
            FontSize = 28,
            FontAttributes = FontAttributes.Italic,
            HorizontalTextAlignment = TextAlignment.Center,  
            VerticalTextAlignment = TextAlignment.Center,
        };

        editor.TextChanged += Teksti_sisestamine;

        hsl = new HorizontalStackLayout
        {
            Spacing = 20,
            HorizontalOptions = LayoutOptions.CenterAndExpand 
        };

        for (int i = 0; i < 3; i++)
        {
            Button b = new Button
            {
                Text = buttons[i],
                ZIndex = i,
                WidthRequest = DeviceDisplay.Current.MainDisplayInfo.Width / 8.3,
            };
            hsl.Add(b);
            b.Clicked += Liikumine;
        }

     
        VerticalStackLayout vst = new VerticalStackLayout
        {
            Children = { lbl, editor, hsl },
            VerticalOptions = LayoutOptions.CenterAndExpand,  
            HorizontalOptions = LayoutOptions.CenterAndExpand 
        };

        Content = vst;
    }

    private void TapText_Tapped(object? sender, TappedEventArgs e)
    {
        lbl.FontSize = 34; 

        Device.StartTimer(TimeSpan.FromMilliseconds(200), () =>
        {
            lbl.FontSize = 28; 
            return false; 
        });
    }

    private async void Liikumine(object? sender, EventArgs e)
    {
        Button btn = (Button)sender;
        if (btn.ZIndex == 0)
        {
            await Navigation.PushAsync(new TextPage(btn.ZIndex));
        }
        else if (btn.ZIndex == 1)
        {
            await Navigation.PushAsync(new StartPage());
        }
        else if (btn.ZIndex == 2)
        {
            await Navigation.PushAsync(new Timer_Page());
        }
        else if (btn.ZIndex == 3)
        {
            await Navigation.PushAsync(new ValgusfoorPage());
        }
        else
        {
            await Navigation.PushAsync(new FigurePage(btn.ZIndex));
        }
    }

    private void Teksti_sisestamine(object? sender, TextChangedEventArgs e)
    {
        lbl.Text = editor.Text;
    }

    private async void Tagasi_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new MainPage());
    }
}
