using System;
using System.Windows.Forms;

namespace mayinTarlasi
{
    public partial class Form1 : Form
    {
        private const int gridSize = 4;
        private const int mineCount = 4;

        private Button[,] grid;
        private bool[,] hasMine;
        private bool[,] revealed;
        private bool gameStarted = false;
        private bool gameOver;
        private DateTime startTime;

        public Form1()
        {
            InitializeComponent();
            InitializeGrid();
            PlaceMines();
        }

        private void InitializeGrid()
        {
            grid = new Button[gridSize, gridSize];
            hasMine = new bool[gridSize, gridSize];
            revealed = new bool[gridSize, gridSize];
            gameOver = false;

            for (int i = 0; i < gridSize; i++)
            {
                for (int j = 0; j < gridSize; j++)
                {
                    Button btn = new Button();
                    btn.Name = $"btn_{i}_{j}";
                    btn.Size = new System.Drawing.Size(50, 50);
                    btn.Margin = new Padding(0);
                    btn.Tag = new Tuple<int, int>(i, j);
                    btn.Click += Btn_Click;
                    tableLayoutPanel1.Controls.Add(btn, i, j);
                    grid[i, j] = btn;
                }
            }
        }

        private void PlaceMines()
        {
            Random rnd = new Random();
            for (int i = 0; i < mineCount; i++)
            {
                int x = rnd.Next(0, gridSize);
                int y = rnd.Next(0, gridSize);
                if (!hasMine[x, y])
                {
                    hasMine[x, y] = true;
                }
                else
                {
                    i--;
                }
            }
        }

        private void Btn_Click(object sender, EventArgs e)
        {
            if (!gameStarted)
                return;

            if (gameOver) return;

            Button btn = (Button)sender;
            Tuple<int, int> position = (Tuple<int, int>)btn.Tag;
            int x = position.Item1;
            int y = position.Item2;

            if (hasMine[x, y])
            {
                MessageBox.Show("Mayýna Bastýnýz.");
                gameOver = true;
                RevealMines();
                ShowGameTime();
            }
            else
            {
                int nearbyMines = CountNearbyMines(x, y);
                btn.Text = nearbyMines.ToString();
                revealed[x, y] = true;
                CheckWin();
            }
        }

        private int CountNearbyMines(int x, int y)
        {
            int count = 0;
            for (int i = Math.Max(0, x - 1); i <= Math.Min(gridSize - 1, x + 1); i++)
            {
                for (int j = Math.Max(0, y - 1); j <= Math.Min(gridSize - 1, y + 1); j++)
                {
                    if (hasMine[i, j])
                    {
                        count++;
                    }
                }
            }
            return count;
        }

        private void RevealMines()
        {
            for (int i = 0; i < gridSize; i++)
            {
                for (int j = 0; j < gridSize; j++)
                {
                    if (hasMine[i, j])
                    {
                        grid[i, j].Text = "M";
                    }
                }
            }
        }


        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            RestartGame();
            label1.Text = "";
        }

        private void RestartGame()
        {
            Array.Clear(hasMine, 0, hasMine.Length);
            Array.Clear(revealed, 0, revealed.Length);

            foreach (Button btn in tableLayoutPanel1.Controls)
            {
                btn.Text = "";
            }

            PlaceMines();

            gameOver = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!gameStarted)
            {
                RestartGame();
                gameStarted = true;
            }

            label1.Text = "";
        }

        private void StartGame()
        {
            startTime = DateTime.Now;

            RestartGame();
            gameStarted = true;
        }

        private void ShowGameTime()
        {
            DateTime endTime = DateTime.Now;

            TimeSpan elapsedTime = endTime - startTime;

            int score = CalculateScore(elapsedTime);

            MessageBox.Show($"Oyun {elapsedTime.TotalSeconds} saniyede tamamlandý. Skor: {score}");

            label1.Text = $"Skor: {score}";
        }

        private int CalculateScore(TimeSpan elapsedTime)
        {
            int score = (int)(10000 / elapsedTime.TotalSeconds);
            return score;
        }

        private void CheckWin()
        {

            bool allNonMineCellsClicked = true;
            for (int i = 0; i < gridSize; i++)
            {
                for (int j = 0; j < gridSize; j++)
                {
                    if (!hasMine[i, j] && !revealed[i, j])
                    {
                        allNonMineCellsClicked = false;
                        break;
                    }
                }
                if (!allNonMineCellsClicked)
                    break;
            }

            if (allNonMineCellsClicked)
            {
                gameOver = true;
                int score = CalculateScore(DateTime.Now - startTime);
                MessageBox.Show($"Tebrikler! Oyunu kazandýnýz.");
                label1.Text = $"Skor: {score}";
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }
    }
}