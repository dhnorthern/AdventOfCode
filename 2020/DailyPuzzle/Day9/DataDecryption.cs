using System.Collections.Generic;

namespace DailyPuzzle.Day9
{
   public static class DataDecryption
   {
      public static long FindInvalidNumber( List<long> numbers, int preambleSize )
      {
         var priorNumberSet = numbers.GetRange( 0, preambleSize );

         var oldestIndex = 0;

         for ( var i = preambleSize; i < numbers.Count; ++i )
         {
            if ( !MatchSumTwoNumbers( numbers[i], priorNumberSet ) )
            {
               return numbers[i];
            }

            priorNumberSet[oldestIndex] = numbers[i];
            oldestIndex = ( oldestIndex + 1 ) % preambleSize;
         }

         System.Diagnostics.Debug.Assert( false, "Invalid input" );
         return 0L;
      }

      public static List<long> FindContiguousNumberSetMatchesSum( long numberToMatch, List<long> numbers )
      {
         for ( var i = 0; i < numbers.Count - 1; ++i )
         {
            var sum = numbers[i];

            for ( var j = i + 1; j < numbers.Count; ++j )
            {
               sum += numbers[j];
               if ( sum == numberToMatch )
               {
                  return numbers.GetRange( i, j - i + 1 );
               }
               if ( sum > numberToMatch )
               {
                  break;
               }
            }
         }

         System.Diagnostics.Debug.Assert( false, "Invalid input" );
         return new List<long>();
      }

      private static bool MatchSumTwoNumbers( long numberToMatch, IReadOnlyList<long> numberSet )
      {
         for ( var i = 0; i < numberSet.Count - 1; ++i )
         {
            for ( var j = i + 1; j < numberSet.Count; ++j )
            {
               if ( numberToMatch == numberSet[i] + numberSet[j] )
               {
                  return true;
               }
            }
         }

         return false;
      }
   }
}
