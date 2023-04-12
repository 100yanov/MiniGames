namespace MiniGames.Pages.MemoryMatrix
{
    public partial class MemoryMatrix
    {
        private const int gridSize = 6;
        private bool canClick = false;
        private List<Cell> cells = new();
        private int triesLeft;

        public int TriesLeft
        {
            get { return triesLeft; }
            set
            {
                triesLeft = value;
                if (value == 0)
                {
                    ShowCells();
                }
            }
        }
        
        protected override async Task OnInitializedAsync()
        {
            await StartGame();
        }

        private async Task StartGame()
        {
            canClick = false;
            TriesLeft = gridSize;
            var random = new Random();

            cells.Clear();
            for (int i = 0; i < gridSize * gridSize; i++)
            {
                cells.Add(new Cell());
            }

            for (int i = 0; i < gridSize; i++)
            {
                int index;
                do
                {
                    index = random.Next(0, gridSize * gridSize);
                }
                while (cells[index].IsCorrect);
                cells[index].IsCorrect = true;
            }

            ShowCells();
            await Task.Delay(3000);
            HideCells();

            canClick = true;
        }

        private void HideCells()
        {
            for (int i = 0; i < gridSize * gridSize; i++)
            {
                if (cells[i].IsCorrect)
                {
                    cells[i].Color = Colors.Default;
                }
            }
            StateHasChanged();
        }

        private void ShowCells()
        {
            for (int i = 0; i < gridSize * gridSize; i++)
            {
                if (cells[i].IsCorrect)
                {
                    cells[i].Color = Colors.Correct;
                }
            }
            StateHasChanged();
        }

        private void CellClicked(int index)
        {
            if (cells[index].IsClicked || !canClick)
            {
                return;
            }

            if (cells[index].IsCorrect)
            {
                cells[index].Color = Colors.Correct;
            }
            else
            {
                cells[index].Color = Colors.Incorrect;
            }
            cells[index].IsClicked = true;
            if (--TriesLeft == 0)
            {
                canClick = false;
            }

            StateHasChanged();
        }

        private class Cell
        {
            internal bool IsClicked;

            public bool IsCorrect { get; set; } = false;

            public string Color { get; set; } = Colors.Default;
        }

        public static class Colors
        {
            public const string Default = "#5d443c";
            public const string Correct = "#4dbcb6";
            public const string Incorrect = "red";
        }
    }
}