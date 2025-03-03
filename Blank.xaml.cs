namespace TARpv23_Mobiile_App
{
    public partial class Blank : ContentPage
    {
        private bool isCrossTurn = true;
        private Image[,] cellImages = new Image[3, 3]; 
        private Button StartBTN;
        public Blank()
        {
            var grid = new Grid
            {
                RowDefinitions =
                {
                    new RowDefinition { Height = 150 },
                    new RowDefinition { Height = 150 },
                    new RowDefinition { Height = 150 }
                },
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = 150 },
                    new ColumnDefinition { Width = 150 },
                    new ColumnDefinition { Width = 150 }
                }
            };
            StartBTN = new Button
            {
                Text = "Начать игру",
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                BackgroundColor = Colors.LightBlue,
                TextColor = Colors.White,
                FontSize = 20,
                Padding = new Thickness(20, 10)
            };

            for (int row = 0; row < 3; row++)
            {
                for (int column = 0; column < 3; column++)
                {
                    AddFrameToGrid(grid, column, row);
                }
            }
            Content = grid;
        }

        private void AddFrameToGrid(Grid grid, int column, int row)
        {
            var frame = new Frame
            {
                BorderColor = Colors.Black,
                BackgroundColor = Colors.LightGray,
                Padding = new Thickness(10),
                CornerRadius = 5,
                HasShadow = false
            };

            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += (s, e) => OnCellClicked(row, column);
            frame.GestureRecognizers.Add(tapGestureRecognizer);

            var image = new Image
            {
                Aspect = Aspect.AspectFit,
                IsVisible = false 
            };
            cellImages[row, column] = image;

            frame.Content = image;

            grid.Children.Add(frame);
            Grid.SetColumn(frame, column);
            Grid.SetRow(frame, row);
        }

        private void OnCellClicked(int row, int column)
        {

            if (cellImages[row, column].IsVisible)
                return; 

            cellImages[row, column].Source = isCrossTurn ? "rist.png" : "ring.png";
            cellImages[row, column].IsVisible = true;

            isCrossTurn = !isCrossTurn;
        }
        private void StartGame(object sender, EventArgs e)
        {
            StartBTN.IsVisible = false;

            for (int row = 0; row < 3; row++)
            {
                for (int column = 0; column < 3; column++)
                {
                    cellImages[row, column].IsVisible = false;
                    cellImages[row, column].Source = null; 
                }
            }

            isCrossTurn = true;
        }
    }
}