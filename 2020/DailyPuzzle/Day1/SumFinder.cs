using System;
using System.Collections.Generic;

namespace DailyPuzzle.Day1
{
   public class SumFinder
   {
      public List<Tuple<int, int>> FindPairsThatSumToTotal( List<int> input, int totalSought )
      {
         var pairs = new List<Tuple<int, int>>();

         for ( var i = 0; i < input.Count; ++i )
         {
            for ( var j = i + 1; j < input.Count; ++j )
            {
               var sum = input[i] + input[j];
               if ( sum == totalSought )
               {
                  pairs.Add( new Tuple<int, int>( input[i], input[j] ) );
               }
            }
         }

         return pairs;
      }

      public List<Tuple<int, int, int>> FindTriplesThatSumToTotal( List<int> input, int totalSought )
      {
         var triples = new List<Tuple<int, int, int>>();

         for ( var i = 0; i < input.Count; ++i )
         {
            for ( var j = i + 1; j < input.Count; ++j )
            {
               for ( var k = j + 1; k < input.Count; ++k )
               {
                  var sum = input[i] + input[j] + input[k];
                  if ( sum == totalSought )
                  {
                     triples.Add( new Tuple<int, int, int>( input[i], input[j], input[k] ) );
                  }
               }
            }
         }

         return triples;
      }
   }
}
