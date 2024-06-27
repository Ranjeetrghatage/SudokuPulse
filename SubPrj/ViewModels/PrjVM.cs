using Prism.Mvvm;
using SubPrj.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SubPrj.ViewModels
{
    public class PrjVM : BindableBase
    {
        public PrjVM()
        {
            RowsAndColumns = 9;
            GridData = new ObservableCollection<PrjM>();

            UpdateCurrentTime();
        }

        private string _currentTime;
        private string _timeleftToday;

        public string CurrentTime
        {
            get { return _currentTime; }
            set
            {
                _currentTime = value;
                RaisePropertyChanged(nameof(CurrentTime));
            }
        }


        public string TimeLeftToday
        {
            get { return _timeleftToday; }
            set
            {
                _timeleftToday = value;
                RaisePropertyChanged($"{nameof(TimeLeftToday)}");
            }
        }



        private double _hourAngle;
        private double _minuteAngle;
        private double _secondAngle;

        public double HourAngle
        {
            get { return _hourAngle; }
            set
            {
                _hourAngle = value;
                RaisePropertyChanged(nameof(HourAngle));
            }
        }

        public double MinuteAngle
        {
            get { return _minuteAngle; }
            set
            {
                _minuteAngle = value;
                RaisePropertyChanged(nameof(MinuteAngle));
            }
        }

        public double SecondAngle
        {
            get { return _secondAngle; }
            set
            {
                _secondAngle = value;
                RaisePropertyChanged(nameof(SecondAngle));
            }
        }


        private CancellationTokenSource _cancellationTokenSource;
        private int _rowsandcolumns;

        public int RowsAndColumns
        {
            get { return _rowsandcolumns; }
            set
            {
                _rowsandcolumns = value;
                RaisePropertyChanged(nameof(RowsAndColumns));

                _cancellationTokenSource?.Cancel();
                _cancellationTokenSource = new CancellationTokenSource();
                UpdateGrid(RowsAndColumns, _cancellationTokenSource.Token);
            }
        }

        private ObservableCollection<PrjM> gridData;
        public ObservableCollection<PrjM> GridData
        {
            get { return gridData; }
            set
            {
                gridData = value;
                RaisePropertyChanged(nameof(GridData));
            }
        }


        private async void UpdateCurrentTime()
        {
            DateTime now = DateTime.Now;
            DateTime endOfDay = now.Date.AddDays(1);
            TimeSpan timeLeft = endOfDay - now;

            string formattedCurrentTime = now.ToString("HH:mm:ss.fff tt");
            string formattedTimeLeft = string.Format("{0:00}:{1:00}:{2:00}.{3:000}",
                timeLeft.Hours, timeLeft.Minutes, timeLeft.Seconds, timeLeft.Milliseconds);

            CurrentTime = formattedCurrentTime;
            TimeLeftToday = formattedTimeLeft;

            HourAngle = 360 * ((now.Hour % 12) / 12.0);
            MinuteAngle = 360 * (now.Minute / 60.0);
            SecondAngle = 360 * (now.Second / 60.0);

            await Task.Delay(1);
            UpdateCurrentTime();
        }


        private async void UpdateGrid(int RowsAndColumns, CancellationToken cancellationToken)
        {
            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    await Task.Delay(1000, cancellationToken);
                    PrintBoard(GenerateValidSudokuGrid());
                }
            }
            catch { };
        }

        public void PrintBoard(int[,] board)
        {
            GridData.Clear();
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    GridData.Add(new PrjM
                    {
                        RowNumber = i,
                        ColumnNumber = j,
                        TextBoxText = board[i, j].ToString(),
                    });
                    ;
                }
            }
        }




        private int[,] GenerateValidSudokuGrid()
        {
            int[,] grid = new int[9, 9];
            FillSudoku(grid, 0, 0);
            return grid;
        }

        private bool FillSudoku(int[,] grid, int row, int col)
        {
            Random random = new Random();

            if (col == 9)
            {
                col = 0;
                row++;
                if (row == 9)
                {
                    return true;
                }
            }

            var numbers = Enumerable.Range(1, 9).OrderBy(_ => random.Next()).ToList(); // random list from 1 to 9  every time

            foreach (int num in numbers)
            {
                if (IsValid(grid, row, col, num))
                {
                    grid[row, col] = num;
                    if (FillSudoku(grid, row, col + 1))
                    {
                        return true;
                    }
                    grid[row, col] = 0;
                }
            }

            return false;
        }

        private bool IsValid(int[,] grid, int row, int col, int num)
        {
            for (int i = 0; i < 9; i++)
            {
                if (grid[row, i] == num || grid[i, col] == num)
                {
                    return false;
                }
            }

            // Check the 3x3 box
            int startRow = row - row % 3;
            int startCol = col - col % 3;
            for (int r = startRow; r < startRow + 3; r++)
            {
                for (int c = startCol; c < startCol + 3; c++)
                {
                    if (grid[r, c] == num)
                    {
                        return false;
                    }
                }
            }

            return true;
        }



    }
}


