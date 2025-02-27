namespace TARpv23_Mobiile_App;

public partial class StartPage : ContentPage
{
    public List<ContentPage> lehed = new List<ContentPage>() { new TextPage(0), new FigurePage(1), new ValgusfoorPage(), new slider(), new Lumememm()};
    public List<string> tekstid = new List<string> { "TekstPage", "FigurePage", "Valgusfoor", "rgb", "Lumememm" };
    ScrollView sv;
    VerticalStackLayout vsl;
    Button showPagesButton;

    public StartPage()
    {
        Title = "Avaleht";
        vsl = new VerticalStackLayout { BackgroundColor = Color.FromArgb("#C2E3E3") };

 
        showPagesButton = new Button
        {
            Text = "Näita lehti",
            BackgroundColor = Color.FromArgb("#F0C0CF"),
            TextColor = Color.FromArgb("#000000f"),
            BorderWidth = 10,
            FontFamily = "OpenSans-Regular",
            FontSize = 20
        };
        vsl.Add(showPagesButton);
        showPagesButton.Clicked += ShowPagesButton_Clicked;

        sv = new ScrollView { Content = vsl };
        Content = sv;
    }

    private void ShowPagesButton_Clicked(object sender, EventArgs e)
    {
        vsl.Children.Remove(showPagesButton);

        for (int i = 0; i < tekstid.Count; i++)
        {
            Button nupp = new Button
            {
                Text = tekstid[i],
                BackgroundColor = Color.FromArgb("#F0C0CF"),
                TextColor = Color.FromArgb("#000000f"),
                BorderWidth = 10,
                ZIndex = i,
                FontFamily = "OpenSans-Regular",
                FontSize = 20
            };
            vsl.Add(nupp);
            nupp.Clicked += Lehte_avamine;
        }
    }

    private async void Lehte_avamine(object? sender, EventArgs e)
    {
        Button btn = (Button)sender;
        await Navigation.PushAsync(lehed[btn.ZIndex]);
    }

    private async void Tagasi_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new MainPage());
    }
}
