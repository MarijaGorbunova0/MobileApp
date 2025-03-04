using Microsoft.Maui.Controls;

namespace TARpv23_Mobiile_App
{
    public partial class Blank : ContentPage
    {
        private bool isCrossTurn = true; 
        private Image[,] cellImages = new Image[3, 3]; 
        private Button StartBTN; 
        private Grid grid;

        public Blank()
        {
            StartBTN = new Button
            {
                Text = "Mängima",
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                BackgroundColor = Colors.LightBlue,
                TextColor = Colors.White,
                FontSize = 20,
                Padding = new Thickness(20, 10)
            };
            StartBTN.Clicked += StartGame;

            grid = new Grid
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
                },
                IsVisible = false
            };

            for (int row = 0; row < 3; row++)
            {
                for (int column = 0; column < 3; column++)
                {
                    AddFrameToGrid(grid, column, row);
                }
            }

            Content = new StackLayout
            {
                VerticalOptions = LayoutOptions.Center,
                Children = { StartBTN }
            };
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

            if (CheckForWin(row, column))
            {
                DisplayAlert("Võit!", $"Võit {(isCrossTurn ? "Cross" : "Toe")}!", "OK");
                ResetGame();
                return;
            }

            if (CheckForDraw())
            {
                DisplayAlert("Loosi!", "Kõik täis!", "OK.");
                ResetGame();
                return;
            }

            isCrossTurn = !isCrossTurn;
        }

        private bool CheckForWin(int row, int column)
        {
            var currentSign = isCrossTurn ? "rist.png" : "ring.png";

            if (cellImages[row, 0].Source?.ToString().EndsWith(currentSign) == true &&
                cellImages[row, 1].Source?.ToString().EndsWith(currentSign) == true &&
                cellImages[row, 2].Source?.ToString().EndsWith(currentSign) == true)
                return true;

            if (cellImages[0, column].Source?.ToString().EndsWith(currentSign) == true &&
                cellImages[1, column].Source?.ToString().EndsWith(currentSign) == true &&
                cellImages[2, column].Source?.ToString().EndsWith(currentSign) == true)
                return true;

            if (row == column &&
                cellImages[0, 0].Source?.ToString().EndsWith(currentSign) == true &&
                cellImages[1, 1].Source?.ToString().EndsWith(currentSign) == true &&
                cellImages[2, 2].Source?.ToString().EndsWith(currentSign) == true)
                return true;

            if (row + column == 2 &&
                cellImages[0, 2].Source?.ToString().EndsWith(currentSign) == true &&
                cellImages[1, 1].Source?.ToString().EndsWith(currentSign) == true &&
                cellImages[2, 0].Source?.ToString().EndsWith(currentSign) == true)
                return true;

            return false;
        }

        private bool CheckForDraw()
        {
            for (int row = 0; row < 3; row++)
            {
                for (int column = 0; column < 3; column++)
                {
                    if (!cellImages[row, column].IsVisible)
                        return false; 
                }
            }
            return true; 
        }

        private void ResetGame()
        {
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

        private void StartGame(object sender, EventArgs e)
        {
            StartBTN.IsVisible = false;

            ResetGame();
            grid.IsVisible = true;

            Content = grid;
        }
    }
}