namespace TARpv23_Mobiile_App;

public partial class slider : ContentPage
{
    private BoxView colorBox;
    private Slider redSlider, greenSlider, blueSlider;
    private Label colorValueLabel;
    private Button randomColorButton;

    public slider()
    {
        Title = "RGB";

        var header = new Label
        {
            Text = "rgb",
            FontSize = 24,
            HorizontalOptions = LayoutOptions.Center
        };

        colorBox = new BoxView
        {
            Color = Colors.Black,
            HeightRequest = 200,
            WidthRequest = 200,
            HorizontalOptions = LayoutOptions.Center
        };

        colorValueLabel = new Label
        {
            Text = "RGB(0, 0, 0)",
            FontSize = 18,
            HorizontalOptions = LayoutOptions.Center
        };

        redSlider = CreateSlider();
        greenSlider = CreateSlider();
        blueSlider = CreateSlider();

        redSlider.ValueChanged += ColorChange;
        greenSlider.ValueChanged += ColorChange;
        blueSlider.ValueChanged += ColorChange;

        randomColorButton = new Button
        {
            Text = "juhuslik värv",
            FontSize = 16,
            HorizontalOptions = LayoutOptions.Center,
            Margin = new Thickness(0, 10)
        };
        randomColorButton.Clicked += RandomColor;

        var slidersLayout = new StackLayout
        {
            Children =
            {
                SliderLayout("R: ", redSlider),
                SliderLayout("G: ", greenSlider),
                SliderLayout("B: ", blueSlider)
            }
        };

        Content = new StackLayout
        {
            Padding = new Thickness(20),
            Spacing = 20,
            VerticalOptions = LayoutOptions.Center,
            Children = { header, colorBox, colorValueLabel, randomColorButton, slidersLayout }
        };
    }

    private Slider CreateSlider()
    {
        return new Slider
        {
            Minimum = 0,
            Maximum = 255,
            Value = 0,
            WidthRequest = 300
        };
    }

    private StackLayout SliderLayout(string labelText, Slider slider)
    {
        return new StackLayout
        {
            Orientation = StackOrientation.Horizontal,
            Children =
            {
                new Label { Text = labelText, VerticalOptions = LayoutOptions.Center },
                slider
            }
        };
    }

    private void ColorChange(object sender, ValueChangedEventArgs e)
    {
        int r = (int)redSlider.Value;
        int g = (int)greenSlider.Value;
        int b = (int)blueSlider.Value;
        colorBox.Color = Color.FromRgb(r, g, b);

        colorValueLabel.Text = $"RGB({r}, {g}, {b})";
    }

    private void RandomColor(object sender, EventArgs e)
    {
        Random random = new Random();
        int r = random.Next(0, 256);
        int g = random.Next(0, 256);
        int b = random.Next(0, 256);

        redSlider.Value = r;
        greenSlider.Value = g;
        blueSlider.Value = b;

        colorBox.Color = Color.FromRgb(r, g, b);
        colorValueLabel.Text = $"RGB({r}, {g}, {b})";
    }
}