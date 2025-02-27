namespace TARpv23_Mobiile_App;

public partial class ValgusfoorPage : ContentPage
{
    private bool isOn = false;
    private bool isAutoMode = false;
    private bool isNightMode = false;
    private Label header;
    private List<StackLayout> lights;
    private readonly List<Color> aktiivsed = new List<Color> { Colors.Red, Colors.Yellow, Colors.Green };
    private readonly List<string> vastused = new List<string> { "Peatu", "Oota", "Mine" };

    public ValgusfoorPage()
    {
        Title = "Valgusfoor";
        header = new Label
        {
            Text = "Valgusfoor",
            FontSize = 24,
            HorizontalOptions = LayoutOptions.Center
        };

        lights = new List<StackLayout>();
        StackLayout lightsStack = new StackLayout
        {
            Spacing = 10,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Start
        };

        for (int i = 0; i < 3; i++)
        {
            var lightCircle = new BoxView
            {
                Color = Colors.Gray,
                HeightRequest = 100,
                WidthRequest = 100,
                CornerRadius = 50
            };

            var lightContainer = new StackLayout
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Children = { lightCircle }
            };

            int index = i;
            var tapGesture = new TapGestureRecognizer();
            tapGesture.Tapped += (s, e) =>
            {
                if (!isOn || isAutoMode || isNightMode)
                    return;

                header.Text = vastused[index];
            };
            lightContainer.GestureRecognizers.Add(tapGesture);

            lightsStack.Children.Add(lightContainer);
            lights.Add(lightContainer);
        }

        Button onButton = new Button { Text = "Sisse" };
        onButton.Clicked += (s, e) => TurnOn();

        Button offButton = new Button { Text = "Välja" };
        offButton.Clicked += (s, e) => TurnOff();

        Button nightModeButton = new Button { Text = "Öörežiim" };
        nightModeButton.Clicked += (s, e) => StartNightMode();

        StackLayout control = new StackLayout
        {
            Orientation = StackOrientation.Horizontal,
            HorizontalOptions = LayoutOptions.Center,
            Spacing = 20,
            Children = { onButton, offButton, nightModeButton }
        };

        Content = new StackLayout
        {
            Spacing = 20,
            Padding = new Thickness(20),
            VerticalOptions = LayoutOptions.Center,
            Children = { header, lightsStack, control }
        };
    }

    private async void TurnOn()
    {
        isOn = true;
        isAutoMode = true;
        isNightMode = false;

        header.Text = "Valgusfoor on sisse lülitatud";

        while (isAutoMode)
        {
            for (int i = 0; i < lights.Count; i++)
            {
                var box = (BoxView)lights[i].Children[0];

                box.Color = aktiivsed[i];
                header.Text = vastused[i];
                await Task.Delay(1000);

                box.Color = Colors.Gray;
                await Task.Delay(500);
            }
        }
    }

    private void TurnOff()
    {
        isOn = false;
        isAutoMode = false;
        isNightMode = false;

        header.Text = "Valgusfoor on välja lülitatud";
        foreach (var light in lights)
        {
            var box = (BoxView)light.Children[0];
            box.Color = Colors.Gray;
        }
    }

    private async void StartNightMode()
    {
        if (!isOn)
        {
            header.Text = "Lülita esmalt valgusfoor sisse";
            return;
        }

        isNightMode = true;
        isAutoMode = false;

        header.Text = "Öörežiim aktiivne!";

        while (isNightMode)
        {
            foreach (var light in lights)
            {
                var box = (BoxView)light.Children[0];
                box.Color = Colors.Gray;
            }
            await Task.Delay(500);

            foreach (var light in lights)
            {
                var box = (BoxView)light.Children[0];
                box.Color = Colors.Transparent;
            }
            await Task.Delay(500);
        }
    }
}
