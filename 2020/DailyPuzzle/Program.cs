using System.Collections.Generic;

namespace DailyPuzzle
{
   public class Program
   {
      public static void Main( string[] _ )
      {
         var puzzles = new List<IPuzzle>
         {
            //new Day1.Puzzle(),
            //new Day2.Puzzle(),
            //new Day3.Puzzle(),
            //new Day4.Puzzle(),
            //new Day5.Puzzle(),
            //new Day6.Puzzle(),
            //new Day7.Puzzle(),
            //new Day8.Puzzle(),
            //new Day9.Puzzle(),
            //new Day10.Puzzle(),
            new Day17.Puzzle(),
         };

         foreach ( var puzzle in puzzles )
         {
            System.Console.WriteLine( "----------------------------" );
            System.Console.WriteLine( $"{puzzle.Name}" );
            System.Console.WriteLine( "Part 1:" );
            puzzle.RunPart1();
            System.Console.WriteLine( "Part 2:" );
            puzzle.RunPart2();
         }
      }
   }
}
