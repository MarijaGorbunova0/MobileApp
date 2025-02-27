namespace TARpv23_Mobiile_App;

public partial class Lumememm : ContentPage
{
    private Frame head, body, bottom;
    private Slider opacitySlider;
    private Label colorValueLabel;
    private Button addImageButton;
    private StackLayout snowmanLayout;
    private Image botinkiImage;
    public Lumememm()
    {
        Title = "Lumememm";

        var header = new Label
        {
            Text = "lumememm",
            FontSize = 24,
            HorizontalOptions = LayoutOptions.Center
        };

        head = CreateCircle(150, Colors.White);
        body = CreateCircle(100, Colors.White);
        bottom = CreateCircle(50, Colors.White);

        head.GestureRecognizers.Add(new TapGestureRecognizer
        {
            Command = new Command(() => CircleTap(head))
        });
        body.GestureRecognizers.Add(new TapGestureRecognizer
        {
            Command = new Command(() => CircleTap(body))
        });
        bottom.GestureRecognizers.Add(new TapGestureRecognizer
        {
            Command = new Command(() => CircleTap(bottom))
        });

        snowmanLayout = new StackLayout
        {
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center,
            Spacing = 0,
            Children = { bottom, body, head }
        };

        opacitySlider = new Slider
        {
            Minimum = 0,
            Maximum = 1,
            Value = 1,
            WidthRequest = 300,
            HorizontalOptions = LayoutOptions.Center
        };
        opacitySlider.ValueChanged += OpacityChange;

        colorValueLabel = new Label
        {
            Text = "sulama 100%",
            FontSize = 18,
            HorizontalOptions = LayoutOptions.Center
        };

        addImageButton = new Button
        {
            Text = "lisada saapad",
            FontSize = 16,
            HorizontalOptions = LayoutOptions.Center,
            Margin = new Thickness(0, 10)
        };
        addImageButton.Clicked += AddImage;

        Content = new StackLayout
        {
            Padding = new Thickness(20),
            Spacing = 20,
            VerticalOptions = LayoutOptions.Center,
            Children = { header, snowmanLayout, opacitySlider, colorValueLabel, addImageButton }
        };
    }

    private Frame CreateCircle(double size, Color color)
    {
        return new Frame
        {
            BackgroundColor = color,
            HeightRequest = size,
            WidthRequest = size,
            CornerRadius = (float)size / 2,
            HorizontalOptions = LayoutOptions.Center
        };
    }

    private void OpacityChange(object sender, ValueChangedEventArgs e)
    {
        double opacity = opacitySlider.Value;

        head.Opacity = opacity;
        body.Opacity = opacity;
        bottom.Opacity = opacity;

        colorValueLabel.Text = $"sulama {(opacity * 100):F0}%";
    }

    private void CircleTap(Frame circle)
    {
        Random random = new Random();
        double hue = random.NextDouble() * 360;
        Color randomColor = Color.FromHsla(hue / 360, 1.0, 0.5);
        circle.BackgroundColor = randomColor;
    }

    private async void AddImage(object sender, EventArgs e)
    {
        bool addHat = await DisplayAlert("lisada saapad", "lisada saapad", "ja", "ei");

        if (addHat)
        {
            if (botinkiImage == null)
            {
               botinkiImage = new Image
                {
                    Source = "botinki.png", 
                    WidthRequest = 120,
                    HeightRequest = 120,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Start,
                    Margin = new Thickness(0, -50, 0, 0) 
                };

                snowmanLayout.Children.Add(botinkiImage);
            }
            else
            {

                await DisplayAlert("info", "juba olemas", "OK");
            }
        }
    }
}
