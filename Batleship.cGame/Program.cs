using System;

namespace BattleshipGame
{
    public class BattleshipGame
    {
        private const int BoardSize = 10;
        private const int NumShips = 10;

        private char[,] player1Board;
        private char[,] player2Board;


        private int player1ShipsRemaining;
        private int player2ShipsRemaining;

        private bool isPlayer1Turn;


        public BattleshipGame()
        {
            player1Board = new char[BoardSize, BoardSize];
            player2Board = new char[BoardSize, BoardSize];

            InitializeBoard(player1Board);
            InitializeBoard(player2Board);

            player1ShipsRemaining = NumShips;
            player2ShipsRemaining = NumShips;

            isPlayer1Turn = true;
        }

        private void InitializeBoard(char[,] board)
        {
            for (int i = 0; i < BoardSize; i++)
            {
                for (int j = 0; j < BoardSize; j++)
                {
                    board[i, j] = '-';
                }
            }
        }

        public void StartGame()
        {
            Console.WriteLine("Добро пожаловать в морской бой!");


            PlaceShips(player1Board, "Игрок 1");
            Console.Clear();
            PlaceShips(player2Board, "Игрок 2");
            Console.Clear();

            Console.WriteLine("Корабли были поставлены. Игра начинается.");


            while (player1ShipsRemaining > 0 && player2ShipsRemaining > 0)
            {
                if (isPlayer1Turn)
                {
                    Console.WriteLine("Очередь первого игрока:");
                    Shoot(player2Board, "Игрок 1");
                }
                else
                {
                    Console.WriteLine("Очередь второго игрока:");
                    Shoot(player1Board, "Игрок 2");
                }

                isPlayer1Turn = !isPlayer1Turn;
            }

            if (player1ShipsRemaining == 0)
            {
                Console.WriteLine("Игрок 2 победил!");
            }
            else
            {
                Console.WriteLine("Игрок 1 победил!");
            }
        }

        private void PlaceShips(char[,] board, string playerName)
        {
            Console.WriteLine($"{playerName}, твоя очередь ставить корабли.");

            for (int i = 1; i <= NumShips; i++)
            {
                Console.WriteLine($"Ставим корабль номер {i} из {NumShips}");

                int shipSize = GetShipSize(i);
                bool isValidPlacement = false;

                while (!isValidPlacement)
                {
                    Console.Write("Введите координату по строке (0-9): ");
                    int startRow = int.Parse(Console.ReadLine());

                    Console.Write("Введите координату по столбцу (0-9): ");
                    int startCol = int.Parse(Console.ReadLine());

                    Console.Write("Введите направление (H - горизонтально, V - вертикально): ");
                    char direction = char.ToUpper(Console.ReadLine()[0]);

                    isValidPlacement = ValidateShipPlacement(board, startRow, startCol, direction, shipSize);

                    if (!isValidPlacement)
                    {
                        Console.WriteLine("Неправильная позиция корабля. Попробуй ещё раз.");
                    }
                    else
                    {
                        PlaceShip(board, startRow, startCol, direction, shipSize);
                        PrintBoard(board);
                    }
                }
            }
        }

        private bool ValidateShipPlacement(char[,] board, int startRow, int startCol, char direction, int shipSize)
        {
            if (startRow < 0 || startRow >= BoardSize || startCol < 0 || startCol >= BoardSize)
            {
                return false;
            }

            if (direction != 'H' && direction != 'V')
            {
                return false;
            }

            int endRow = startRow;
            int endCol = startCol;

            if (direction == 'H')
            {
                endCol += shipSize - 1;
            }
            else
            {
                endRow += shipSize - 1;
            }

            if (endRow >= BoardSize || endCol >= BoardSize)
            {
                return false;
            }

            for (int i = startRow - 1; i <= endRow + 1; i++)
            {
                for (int j = startCol - 1; j <= endCol + 1; j++)
                {
                    if ((i >= startRow && i <= endRow) && (j >= startCol && j <= endCol))
                    {
                        continue;
                    }

                    if (i < 0 || i >= BoardSize || j < 0 || j >= BoardSize || board[i, j] != '-')
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private void PlaceShip(char[,] board, int startRow, int startCol, char direction, int shipSize)
        {
            int endRow = startRow;
            int endCol = startCol;

            if (direction == 'H')
            {
                endCol += shipSize - 1;
            }
            else
            {
                endRow += shipSize - 1;
            }

            for (int i = startRow; i <= endRow; i++)
            {
                for (int j = startCol; j <= endCol; j++)
                {
                    board[i, j] = 'S';
                }
            }
        }

        private void Shoot(char[,] board, string playerName)
        {
            Console.Write($"{playerName}, введите координаты строки для удара (0-9): ");
            int row = int.Parse(Console.ReadLine());

            Console.Write($"{playerName}, введите координату столбца для удара (0-9): ");
            int col = int.Parse(Console.ReadLine());

            if (board[row, col] == 'S')
            {
                Console.Clear();
                Console.WriteLine("Удар!");
                board[row, col] = 'X';

                if (playerName == "Игрок 1")
                {
                    player2ShipsRemaining--;
                }
                else
                {
                    player1ShipsRemaining--;
                }

                Shoot(board, playerName);
            }
            else if (board[row, col] == '-')
            {
                Console.Clear();
                Console.WriteLine("Промах!");
                board[row, col] = 'O';
            }
            else
            {
                Console.Clear();
                Console.WriteLine("В эту точку уже стреляли. Попробуй еще раз.");
                Shoot(board, playerName);
            }
        }

        private void PrintBoard(char[,] board)
        {
            Console.WriteLine("   1 2 3 4 5 6 7 8 9 10");

            for (int i = 0; i < BoardSize; i++)
            {
                if (i<9)
                {
                    Console.Write($" {i+1} ");
                }
                else
                {
                    Console.Write($"{i+1} ");
                }

                for (int j = 0; j < BoardSize; j++)
                {
                    Console.Write($"{board[i, j]} ");
                }

                Console.WriteLine();
            }

            Console.WriteLine();
        }

        private int GetShipSize(int shipNumber)
        {
            if (shipNumber <= 4)
            {
                return 1;
            }
            else if (shipNumber <= 7)
            {
                return 2;
            }
            else if (shipNumber <= 9)
            {
                return 3;
            }
            else
            {
                return 4;
            }
        }
    }

    public class Program
    {
        public static void Main()
        {
            var game = new BattleshipGame();
            game.StartGame();
        }
    }
}
