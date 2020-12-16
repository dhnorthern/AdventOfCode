using System;
using System.Collections;

namespace DailyPuzzle.Day6
{
   public class AnswerCounter
   {
      public int CountUniqueAnswers( string answers, int _ )
      {
         Array answerList = Array.CreateInstance( typeof(char), 'z' - 'a' + 1 );

         using ( CharEnumerator answerEnumerator = answers.GetEnumerator() )
         {
            while ( answerEnumerator.MoveNext() )
            {
               answerList.SetValue( answerEnumerator.Current, answerEnumerator.Current - 'a' );
            }
         }

         var count = 0;
         IEnumerator e = answerList.GetEnumerator();
         while ( e.MoveNext() )
         {
            count += e.Current != null && (char)e.Current == default( char ) ? 0 : 1;
         }

         return count;
      }

      public int CountConsensus( string answers, int groupSize )
      {
         Array answerCounts = Array.CreateInstance( typeof(int), 'z' - 'a' + 1 );

         using ( CharEnumerator answerEnumerator = answers.GetEnumerator() )
         {
            while ( answerEnumerator.MoveNext() )
            {
               var index = answerEnumerator.Current - 'a';
               answerCounts.SetValue( (int)answerCounts.GetValue( index ) + 1, index );
            }
         }

         var count = 0;
         IEnumerator e = answerCounts.GetEnumerator();
         while ( e.MoveNext() )
         {
            count += e.Current != null && (int)e.Current == groupSize ? 1 : 0;
         }

         return count;
      }
   }
}
