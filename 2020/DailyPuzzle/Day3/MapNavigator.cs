using System;
using System.Collections.Generic;
using System.Text;

namespace DailyPuzzle.Day3
{
   public class MapNavigator
   {
      public int CountTrees( ref List<string> map, Tuple<int, int> slope )
      {
         var treeCount = 0;
         var currentColumn = 0;
         var lastRow = map.Count - slope.Item2;
         for ( var row = 0; row < lastRow; row += slope.Item2 )
         {
            currentColumn += slope.Item1;
            var nextRow = row + slope.Item2;

            if ( map[nextRow].Length <= currentColumn )
            {
               for ( var r = nextRow; r < map.Count; ++r )
               {
                  map[r] += map[r];
               }
            }
            if ( map[nextRow].Substring( currentColumn, 1 ) == "#" )
            {
               ++treeCount;
               map[nextRow] = ReplaceAt( map[nextRow], currentColumn, 'X' );
            }
            else
            {
               map[nextRow] = ReplaceAt( map[nextRow], currentColumn, '0' );
            }
         }
         return treeCount;
      }
      public int CountTrees( List<string> map, Tuple<int, int> slope )
      {
         var treeCount = 0;
         var currentColumn = 0;
         var lastRow = map.Count - slope.Item2;
         for ( var row = 0; row < lastRow; row += slope.Item2 )
         {
            currentColumn += slope.Item1;
            var nextRow = row + slope.Item2;

            if ( map[nextRow].Length <= currentColumn )
            {
               for ( var r = nextRow; r < map.Count; ++r )
               {
                  map[r] += map[r];
               }
            }
            if ( map[nextRow].Substring( currentColumn, 1 ) == "#" )
            {
               ++treeCount;
            }
         }
         return treeCount;
      }

      private static string ReplaceAt( string s, int position, char replacement )
      {
         return new StringBuilder( s ) { [position] = replacement }.ToString();
      }
   }
}
