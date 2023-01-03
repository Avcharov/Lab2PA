using System.Diagnostics;
using System.Drawing;
using System.Collections.Generic;
namespace System.Text.Encoding.Unicode;
enum CellState { Close, Open };
internal class Program
{
    public static void Main(string[] args)
    {
        int _Width, _Height;
        Cell[,] Cells;
        _Width = 10;
        _Height = 10;
        Cells = new Cell[_Width, _Height];
        #region Generation
        for (int y = 0; y < _Height; y++)
            for (int x = 0; x < _Width; x++)
                Cells[x, y] = new Cell(new Point(x, y));
        Random rand = new Random();
        Int32 startX = rand.Next(0);
        Int32 startY = rand.Next(0);
        Stack<Cell> path = new Stack<Cell>();
        Cells[startX, startY].Visited = true;
        path.Push(Cells[startX, startY]);
        #region Creation
        while (path.Count > 0)
        {
            Cell _cell = path.Peek();
            List<Cell> nextStep = new List<Cell>();
            if (_cell.Position.X > 0 && !Cells[Convert.ToInt32(_cell.Position.X - 1), Convert.ToInt32(_cell.Position.Y)].Visited)
                nextStep.Add(Cells[Convert.ToInt32(_cell.Position.X) - 1, Convert.ToInt32(_cell.Position.Y)]);
            if (_cell.Position.X < _Width - 1 && !Cells[Convert.ToInt32(_cell.Position.X) + 1, Convert.ToInt32(_cell.Position.Y)].Visited)
                nextStep.Add(Cells[Convert.ToInt32(_cell.Position.X) + 1, Convert.ToInt32(_cell.Position.Y)]);
            if (_cell.Position.Y > 0 && !Cells[Convert.ToInt32(_cell.Position.X), Convert.ToInt32(_cell.Position.Y) - 1].Visited)
                nextStep.Add(Cells[Convert.ToInt32(_cell.Position.X), Convert.ToInt32(_cell.Position.Y) - 1]);
            if (_cell.Position.Y < _Height - 1 && !Cells[Convert.ToInt32(_cell.Position.X), Convert.ToInt32(_cell.Position.Y) + 1].Visited)
                nextStep.Add(Cells[Convert.ToInt32(_cell.Position.X), Convert.ToInt32(_cell.Position.Y) + 1]);
            if (nextStep.Count() > 0)
            {
                Cell next = nextStep[rand.Next(nextStep.Count())];
                if (next.Position.X != _cell.Position.X)
                {
                    if (_cell.Position.X - next.Position.X > 0)
                    {
                        _cell.Left = CellState.Open;
                        next.Right = CellState.Open;
                    }
                    else
                    {
                        _cell.Right = CellState.Open;
                        next.Left = CellState.Open;
                    }
                }
                if (next.Position.Y != _cell.Position.Y)
                {
                    if (_cell.Position.Y - next.Position.Y > 0)
                    {
                        _cell.Top = CellState.Open;
                        next.Bottom = CellState.Open;
                    }
                    else
                    {
                        _cell.Bottom = CellState.Open;
                        next.Top = CellState.Open;
                    }
                }
                next.Visited = true;
                path.Push(next);
            }
            else
            {
                path.Pop();
            }
        }
        #endregion
        for (int i = 0; i < _Height; i++)
            for (int j = 0; j < _Width; j++)
            {
                if ((Cells[i, j].Left == CellState.Open &&
                    Cells[i, j].Right == CellState.Open &&
                    Cells[i, j].Top == CellState.Open

                    ) ||
                    (Cells[i, j].Bottom == CellState.Open &&
                    Cells[i, j].Right == CellState.Open &&
                    Cells[i, j].Top == CellState.Open
                    ) ||
                    (Cells[i, j].Left == CellState.Open &&
                    Cells[i, j].Bottom == CellState.Open &&
                    Cells[i, j].Top == CellState.Open
                    ) ||
                    (Cells[i, j].Left == CellState.Open &&
                    Cells[i, j].Right == CellState.Open &&
                    Cells[i, j].Bottom == CellState.Open

                    )
                    )
                {
                    int x = rand.Next(1, 4);
                    switch (x)
                    {
                        case 1:
                            Cells[i, j].Left = CellState.Close;
                            break;
                        case 2:
                            Cells[i, j].Right = CellState.Close;
                            break;
                        case 3:
                            Cells[i, j].Bottom = CellState.Close;
                            break;
                        case 4:
                            Cells[i, j].Top = CellState.Close;
                            break;
                    }
                }
            }

        string[,] matr = new string[_Height * 2 + 1, _Width * 2 + 1];
        for (int i = 0; i < _Height * 2 + 1; i++)
            for (int j = 0; j < _Width * 2 + 1; j++)
            {
                if ((i + j) % 2 != 0)
                {
                    matr[i, j] = " ";
                }
                else
                {
                    matr[i, j] = "1";
                }
                if (i % 2 == 0)
                    matr[i, j] = " ";
            }
        for (int i = 0; i < _Height; i++)
            for (int j = 0; j < _Width; j++)
            {
                if (Cells[i, j].Left == CellState.Open)
                {
                    matr[i * 2 + 1, j * 2 - 1 + 1] = "-";
                }
                if (Cells[i, j].Top == CellState.Open)
                {
                    matr[i * 2 - 1 + 1, j * 2 + 1] = "|";
                }
                if (Cells[i, j].Right == CellState.Open)
                {
                    matr[i * 2 + 1, j * 2 + 1 + 1] = "-";
                }
                if (Cells[i, j].Bottom == CellState.Open)
                {
                    matr[i * 2 + 1 + 1, j * 2 + 1] = "|";
                }
            }
        matr[0, 0] = @"\";
        matr[_Width * 2, _Height * 2] = @"\";
        for (int i = -1; i < _Height * 2 + 1 + 1; i++)
        {
            for (int j = -1; j < _Width * 2 + 1 + 1; j++)
            {
                if ((i == -1 || i == _Width * 2 + 1) && j < _Width * 2 + 1)
                {
                    Console.Write(" _");
                }
                else if ((j == -1 || j == _Height * 2 + 1))
                {
                    Console.Write("|");
                }
                else
                {
                    Console.Write(matr[i, j] + " ");
                }
            }
            Console.WriteLine();
        }


        #endregion

        Cell start = Cells[_Width - 1, _Height - 1];
        Cell[,] aPath = new Cell[_Width, _Height];
        Cell[,] fwdPath = new Cell[_Width, _Height];
        float[,] g_score = new float[_Width, _Height];
        for (int p = 0; p < _Height; p++)
            for (int j = 0; j < _Width; j++)
            {
                g_score[p, j] = float.PositiveInfinity;
            }
        g_score[start.Position.X, start.Position.Y] = 0;
        float[,] f_score = new float[_Width, _Height];
        for (int k = 0; k < _Height; k++)
            for (int j = 0; j < _Width; j++)
            {
                f_score[k, j] = float.PositiveInfinity;
            }
        f_score[start.Position.X, start.Position.Y] = h(start, Cells[0, 0]);
        PriorityQueue<Cell, float> open = new PriorityQueue<Cell, float>();
        open.Enqueue(start, h(start, Cells[0, 0]));
        while (open.Count > 0)
        {
            Cell currCell = open.Dequeue();
            string[] directions = new string[] { "Left", "Right", "Top", "Bottom" };
            if (currCell.Position.X == 1 && currCell.Position.Y == 1)
            {

                Console.Write("(" + 1 + "," + 1 + ") ");
                Console.Write("(" + 1 + "," + 1 + ") ");
                Console.Write("(" + 0 + "," + 0 + ") ");
                //Console.Write(String.Format("{0,10:D}", "(" + 1 + "," + 1 + ") "));

                break;
            }
            foreach (string direction in directions)
            {
                Cell childCell = Cells[0, 0];
                bool visited = false;
                if (direction == "Left" && currCell.Left == CellState.Open && currCell.Position.Y != 0)
                {
                    childCell = Cells[currCell.Position.X, currCell.Position.Y - 1];
                    visited = true;
                }
                if (direction == "Top" && currCell.Top == CellState.Open && currCell.Position.X != 0)
                {
                    childCell = Cells[currCell.Position.X - 1, currCell.Position.Y];
                    visited = true;
                }
                if (direction == "Right" && currCell.Right == CellState.Open && currCell.Position.Y != _Width - 1)
                {
                    childCell = Cells[currCell.Position.X, currCell.Position.Y + 1];
                    visited = true;
                }
                if (direction == "Bottom" && currCell.Bottom == CellState.Open && currCell.Position.X != _Height - 1)
                {
                    childCell = Cells[currCell.Position.X + 1, currCell.Position.Y];
                    visited = true;
                }
                if (visited)
                {
                    float temp_g_score = g_score[currCell.Position.X, currCell.Position.Y] + 1;
                    float temp_f_score = temp_g_score + h(childCell, Cells[0, 0]);
                    if (temp_f_score < f_score[childCell.Position.X, childCell.Position.Y])
                    {
                        g_score[childCell.Position.X, childCell.Position.Y] = temp_g_score;
                        f_score[childCell.Position.X, childCell.Position.Y] = temp_f_score;
                        open.Enqueue(childCell, temp_f_score);
                        aPath[childCell.Position.X, childCell.Position.Y] = currCell;
                        Console.Write("(" + currCell.Position.X + "," + currCell.Position.Y + ") ");
                    }
                }
            }
        }
        Console.WriteLine(g_score);
        Console.WriteLine(f_score);
        int q = 0;

        Console.WriteLine();
        Console.WriteLine();

        Cell cell = Cells[1, 1];
        while (cell != start)
        {

            if (cell != null && aPath[cell.Position.X, cell.Position.Y] != null)
            {
                fwdPath[aPath[cell.Position.X, cell.Position.Y].Position.X, aPath[cell.Position.X, cell.Position.Y].Position.Y] = cell;
                cell = aPath[cell.Position.X, cell.Position.Y];
            }
            else
            {
                Console.WriteLine("Restart program, Labyrith doesnt have solutions");
            }


        }


        for (int z = 1; z < _Height; z++)
        {
            for (int j = 0; j < _Width; j++)
            {
                if (fwdPath[z, j] != null)
                    Console.Write(String.Format("{0,10:D}", "(" + fwdPath[z, j].Position.X + "," + fwdPath[z, j].Position.Y + ") "));
                else Console.Write(String.Format("{0,10:D}", "(" + "/" + "," + "/" + ") "));
            }
            Console.WriteLine();
        }

        Console.WriteLine();

        float h(Cell cell1, Cell cell2)
        {
            int x1 = cell1.Position.X;
            int y1 = cell1.Position.Y;
            int x2 = cell2.Position.X;
            int y2 = cell2.Position.Y;
            return Math.Abs(x1 - x2) + Math.Abs(y1 - y2);
        }
    }
}
class Cell
{
    public Cell(Point currentPosition)
    {
        Visited = false;
        Position = currentPosition;
    }
    public CellState Left { get; set; }
    public CellState Right { get; set; }
    public CellState Bottom { get; set; }
    public CellState Top { get; set; }
    public Boolean Visited { get; set; }
    public Point Position { get; set; }
}
class Graph
{
    private int V;
    private List<int>[] adj;
    bool flag = false;
    public Graph(int v)
    {
        V = v;
        adj = new List<int>[v];
        for (int i = 0; i < v; ++i)
            adj[i] = new List<int>();
    }
    public void AddEdge(int v, int w)
    {
        adj[v].Add(w);
    }
    public void DFSUtil(int v, bool[] visited, int limit)
    {
        if (limit >= 0)
            if (!flag)
            {
                visited[v] = true;
                Console.Write(v + " ");
                List<int> vList = adj[v];
                foreach (var n in vList)
                {
                    if (!visited[n])
                        if (n != 99)
                        {
                            DFSUtil(n, visited, limit - 1);
                        }
                        else
                        {
                            flag = true;
                            Console.Write(n + " ");
                            break;
                        }
                }
            }
    }
    public void DFS(int v, int limit)
    {
        bool[] visited = new bool[V];
        DFSUtil(v, visited, limit);
    }
}
