using Microsoft.Maui.Controls;
using System;
using System.Linq;

namespace TARpv23_Mobiile_App
{
    public partial class Blank : ContentPage
    {
        private bool isCrossTurn = true; 
        private Image[,] cellImages = new Image[3, 3]; 
        private Grid grid;
        private Button StartBTN; 
        private bool AgainstBot = false;
        private Random random = new Random(); 
        private string selectedTheme = "Light";

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
            StartBTN.Clicked += SelectMode;


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
                    AddFrame(grid, column, row);
                }
            }

            Content = new StackLayout
            {
                VerticalOptions = LayoutOptions.Center,
                Children = { StartBTN }
            };
        }

        private void SelectMode(object sender, EventArgs e)
        {
            StartBTN.IsVisible = false;

            var friendModeButton = new Button
            {
                Text = "Mängi sõbraga",
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                BackgroundColor = Colors.LightGreen,
                TextColor = Colors.White,
                FontSize = 20,
                Padding = new Thickness(20, 10)
            };
            friendModeButton.Clicked += (s, e) => ShowFirstMoveSelection(false);

            var botModeButton = new Button
            {
                Text = "Mängi botiga",
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                BackgroundColor = Colors.LightCoral,
                TextColor = Colors.White,
                FontSize = 20,
                Padding = new Thickness(20, 10)
            };
            botModeButton.Clicked += (s, e) => StartGame(true);

            Content = new StackLayout
            {
                VerticalOptions = LayoutOptions.Center,
                Children = { friendModeButton, botModeButton }
            };
        }

        private void ShowFirstMoveSelection(bool isBotMode)
        {
  
            var layout = new StackLayout { VerticalOptions = LayoutOptions.Center };

            var MovePicker = new Picker
            {
                Title = "Kes alustab?",
                ItemsSource = new List<string> { "Kressik", "Noliks" },
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };

            var themePicker = new Picker
            {
                Title = "Vali teema",
                ItemsSource = new List<string> { "Valge", "Tume" },
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };

            themePicker.SelectedIndexChanged += (sender, args) =>
            {
                selectedTheme = themePicker.SelectedIndex == 0 ? "Light" : "Dark"; 
                ApplyTheme(selectedTheme); 
            };

            MovePicker.SelectedIndexChanged += (sender, args) =>
            {
                if (MovePicker.SelectedIndex == 0) 
                {
                    isCrossTurn = true;
                }
                else if (MovePicker.SelectedIndex == 1) 
                {
                    isCrossTurn = false;
                }
            };

            var startButton = new Button
            {
                Text = "Alusta mängu",
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                BackgroundColor = Colors.LightBlue,
                TextColor = Colors.White,
                FontSize = 20,
                Padding = new Thickness(20, 10)
            };
            startButton.Clicked += (s, e) =>
            {
                if (MovePicker.SelectedIndex == -1)
                {
                    DisplayAlert("Error", "Palun vali, kes alustab!", "OK");
                    return;
                }
                StartGame(false); 
            };

            layout.Children.Add(MovePicker);
            layout.Children.Add(themePicker);
            layout.Children.Add(startButton);

            Content = layout;
        }

        private void ApplyTheme(string theme)
        {
            if (theme == "Light")
            {
                this.BackgroundColor = Colors.White;
                StartBTN.BackgroundColor = Colors.LightBlue;
                StartBTN.TextColor = Colors.White;
            }
            else if (theme == "Dark")
            {
                this.BackgroundColor = Colors.Black;
                StartBTN.BackgroundColor = Colors.Gray;
                StartBTN.TextColor = Colors.White;
            }
        }

        private void StartGame(bool againstBot)
        {
            AgainstBot = againstBot; 
            ResetGame(); 
            grid.IsVisible = true; 

            Content = grid;

            if (AgainstBot && !isCrossTurn)
            {
                BotMove();
            }
        }

        private void AddFrame(Grid grid, int column, int row)
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
            tapGestureRecognizer.Tapped += (s, e) => CellClicked(row, column);
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

        private void CellClicked(int row, int column)
        {
   
            if (cellImages[row, column].IsVisible)
                return; 

            cellImages[row, column].Source = isCrossTurn ? "rist.png" : "ring.png";
            cellImages[row, column].IsVisible = true; 

            if (CheckForWin(row, column))
            {
                DisplayAlert("Võit!", $"Võit {(isCrossTurn ? "Cross" : "Toe")}!", "OK");
                ShowDialog();
                return;
            }

            if (CheckForDraw())
            {
                DisplayAlert("Loosi!", "Kõik täis!", "OK.");
                ShowDialog();
                return;
            }

            isCrossTurn = !isCrossTurn;
            if (AgainstBot && !isCrossTurn)
            {
                BotMove();
            }
        }

        private async void BotMove()
        {
            var emptyCells = (from row in Enumerable.Range(0, 3)
                              from column in Enumerable.Range(0, 3)
                              where !cellImages[row, column].IsVisible
                              select (row, column)).ToList();

            if (emptyCells.Any())
            {
                int delayTime = random.Next(1000, 3001); 

                await Task.Delay(delayTime);

                var (row, column) = emptyCells[random.Next(emptyCells.Count)];
                cellImages[row, column].Source = "ring.png"; 
                cellImages[row, column].IsVisible = true;

                if (CheckForWin(row, column))
                {
                    DisplayAlert("Võit!", "Bot võitis!", "OK");
                    ShowDialog();
                    return;
                }

                if (CheckForDraw())
                {
                    DisplayAlert("Loosi!", "Kõik täis!", "OK.");
                    ShowDialog();
                    return;
                }

                isCrossTurn = !isCrossTurn;
            }
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

        private async void ShowDialog()
        {
            bool playAgain = await DisplayAlert("Mäng läbi", "Kas soovite uuesti mängida?", "Jah", "Ei");
            if (playAgain)
            {
                ResetGame();
                grid.IsVisible = true; 
                Content = grid; 
            }
            else
            {
                SelectMode(null, null); 
            }
        }
    }
}
