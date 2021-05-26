// 算術小達人比賽遊戲規則：
// 輸入有幾名玩家與玩家暱稱後，按照順序等待進入遊戲，程式會隨機指派一道數學題目，玩家須在最短時間內算出正確答案，錯一題即失去資格。
// 當所有玩家結束遊戲後會結算成績，按照***答對題數/(總共花費時間+1)***來進行排序與公佈排名。

using System;
using System.Timers;

namespace calculatorGame
{
  public struct Player
  {
    public string Name;
    public int Score;
    public int TotalTime;
    public string MissingQuestionStr;
    public void Print()
    {
      Console.WriteLine($"{Name}: {Score}, {TotalTime}, {MissingQuestionStr}.");
    }
  }
  class Program
  {
    private static int second = 0;
    private static double level = 0.0;
    static void Main(string[] args)
    {
      // print game rule
      Console.WriteLine("遊戲規則：\n  1. 遊戲開始時會要求輸入玩家人數以及每位玩家的名稱，再輸入遊玩等級，之後開始輪流按下 Enter/Return 開始遊戲。");
      Console.WriteLine("    備註：遊玩等級為題目和答案的位數，即等級1代表題目和答案都會是個位數的題目，等級2代表二位數題目和答案，以此類推...。");
      Console.WriteLine("  2. 遊戲中會隨機產生數道數學題目，作答中的玩家必須輸入對應的答案，如果遇到除法則只需輸入整數位即可，如答錯則會輪到下一位玩家。");
      Console.WriteLine("  3. 最後程式會自動根據每位玩家的答對題目、作答時間來排名，並在最後將排名顯示出來。\n");
      // define player
      Console.Write("輸入玩家人數: ");
      int maxNPlayer = Convert.ToInt16(Console.ReadLine());
      while (maxNPlayer <= 0)
      {
        Console.Write("玩家人數必須大於0，請再輸入玩家人數: ");
        maxNPlayer = Convert.ToInt16(Console.ReadLine());
      }
      Player[] players = new Player[maxNPlayer];

      // enter players' name
      for (int i = 0; i < maxNPlayer; ++i)
      {
        Console.Write($"請輸入第{i + 1}名玩家的名字: ");
        players[i].Name = Console.ReadLine();
      }

      Console.Write("輸入遊玩等級: ");
      level = Convert.ToDouble(Console.ReadLine());
      while (level <= 0)
      {
        Console.Write("遊玩等級必須大於0，請再輸入遊玩等級: ");
        level = Convert.ToDouble(Console.ReadLine());
      }

      // enter game in order
      for (int i = 0; i < maxNPlayer; ++i)
      {
        Console.Write($"玩家 {players[i].Name}，按下 enter/return 鍵開始遊戲: ");
        Console.ReadLine();
        var result = playerGame();
        players[i].Score = result.score;
        players[i].TotalTime = result.totalTime;
        players[i].MissingQuestionStr = result.missingQuestionStr;
      }

      // sort by score and print and calculate winner
      Array.Sort(players, delegate (Player p1, Player p2)
      {
        // p1.Print();
        // p2.Print();
        return (int)((((double)Math.Pow(p2.Score, 2) / (p2.TotalTime + 1)) - ((double)Math.Pow(p1.Score, 2) / (p1.TotalTime + 1))) * 10);
      });
      Console.WriteLine("\n#    Name   Score    Time  Incorrect At");
      for (int i = 0; i < maxNPlayer; ++i)
      {
        Console.WriteLine($"{i + 1}{players[i].Name,8}{players[i].Score,8}{players[i].TotalTime,8}{players[i].MissingQuestionStr,14}");
      }
    }

    private static (int score, int totalTime, string missingQuestionStr) playerGame()
    {
      int score = 0;
      int total_time = 0;
      string missingAt = "";
      Boolean lose = false;

      System.Timers.Timer aTimer = new System.Timers.Timer(1000);
      aTimer.Elapsed += OnTimedEvent;
      aTimer.AutoReset = true;
      aTimer.Enabled = true;

      while (!lose)
      {
        // acquire question and answer
        var puzzle = getPuzzle();
        Console.Write("\n" + puzzle.question + " = ");
        string userAns = Console.ReadLine();

        if (userAns == puzzle.answer)
        {
          score++;
          total_time += second;
          Console.WriteLine($"Correct!!! Score: {score}, Total time: {total_time}s.");
        }
        else
        {
          missingAt = puzzle.question;
          lose = true;
        }

        second = 0;
      }

      aTimer.Stop();
      aTimer.Dispose();

      Console.WriteLine($"Incorrect. Score: {score}, Total time: {total_time}s.");
      return (score, total_time, missingAt);
    }

    private static (string question, string answer) getPuzzle()
    {
      string que = "", ans = "";
      var rand = new Random();
      int n1, n2, op;

      do
      {
        // Generate 2 random integers between 1 and 99.
        n1 = rand.Next(1, (int)Math.Pow(10, (int)level));
        n2 = rand.Next(1, (int)Math.Pow(10, (int)level));
        // Generate 1 random integer between 0 and 3.
        op = rand.Next(4);
      } while (CalculateByCode(n1, n2, op) >= (int)Math.Pow(10, (int)level) || CalculateByCode(n1, n2, op) <= (int)Math.Pow(10, (int)level * -1));

      switch (op)
      {
        case 0:
          que = $"{n1} + {n2}";
          ans = (n1 + n2).ToString();
          break;
        case 1:
          que = $"{n1} - {n2}";
          ans = (n1 - n2).ToString();
          break;
        case 2:
          que = $"{n1} * {n2}";
          ans = (n1 * n2).ToString();
          break;
        case 3:
          que = $"{n1} / {n2}";
          ans = (n1 / n2).ToString();
          break;
      }

      return (que, ans);
    }

    private static void OnTimedEvent(Object source, ElapsedEventArgs e)
    {
      second++;
    }

    private static int CalculateByCode(int n1, int n2, int op)
    {
      int ans = 0;

      switch (op)
      {
        case 0:
          ans = n1 + n2;
          break;
        case 1:
          ans = n1 - n2;
          break;
        case 2:
          ans = n1 * n2;
          break;
        case 3:
          ans = n1 / n2;
          break;
      }

      return ans;
    }
  }
}



