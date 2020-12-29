using System;
using System.Collections.Generic;

namespace DailyPuzzle.Day17
{
   public class Puzzle : IPuzzle
   {
      public string Name => "Conway Cubes";

      public void RunPart1()
      {
         var layers = new Dictionary<int/*Z*/, List<string>>
         {
            { 0, Input }
         };

         const int CyclesToRun = 6;

         var simulator = new EnergySimulator();
         for ( int cycle = 1; cycle <= CyclesToRun; ++cycle )
         {
            simulator.ExtendLayers3D( layers );
            simulator.RunCycle3D( layers );
         }

         Console.WriteLine( $"Active cubes = {simulator.CountCycles( layers )}" );
      }

      public void RunPart2()
      {
      }

      private List<string> Input = new List<string>
      {
         "##..####",
         ".###....",
         "#.###.##",
         "#....#..",
         "...#..#.",
         "#.#...##",
         "..#.#.#.",
         ".##...#.",
      };

      private List<string> TestInput = new List<string>
      {
         ".#.",
         "..#",
         "###",
      };
   }
}
