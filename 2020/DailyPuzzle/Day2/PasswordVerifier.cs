using System;
using System.Collections.Generic;

namespace DailyPuzzle.Day2
{
   public class PasswordVerifier
   {
      public int ValidByRange( List<string> passwordRules )
      {
         var validCount = 0;

         //"7-9 l: vslmtglbc"
         foreach ( var passwordRule in passwordRules )
         {
            var pieces = passwordRule.Split(' ');
            if ( pieces.Length == 3 )
            {
               var range = pieces[0].Split('-');
               if ( range.Length == 2 )
               {
                  var low = int.Parse( range[0] );
                  var high = int.Parse( range[1] );
                  var letter = pieces[1][0];
                  var password = pieces[2];

                  if ( low < high )
                  {
                     var letterCount = 0;
                     using ( CharEnumerator passwordEnumerator = password.GetEnumerator() )
                     {
                        while ( passwordEnumerator.MoveNext() )
                        {
                           letterCount += passwordEnumerator.Current == letter ? 1 : 0;
                        }

                        if ( low <= letterCount && letterCount <= high )
                        {
                           ++validCount;
                        }
                        else
                        {
                           Console.WriteLine( $"Invalid: {password} '{letter}' ({low}..{high})" );
                        }
                     }
                  }
               }
            }
         }

         return validCount;
      }

      public int ValidByPositions( List<string> passwordRules )
      {
         var validCount = 0;

         //"7-9 l: vslmtglbc"
         foreach ( var passwordRule in passwordRules )
         {
            var pieces = passwordRule.Split(' ');
            if ( pieces.Length == 3 )
            {
               var range = pieces[0].Split('-');
               if ( range.Length == 2 )
               {
                  var firstPosition = int.Parse( range[0] );
                  var secondPosition = int.Parse( range[1] );
                  var letter = pieces[1][0];
                  var password = pieces[2];

                  if ( firstPosition <= password.Length && secondPosition <= password.Length )
                  {
                     var first = password[firstPosition - 1];
                     var second = password[secondPosition - 1];

                     if ( first == letter && second != letter || first != letter && second == letter )
                     {
                        ++validCount;
                     }
                     else
                     {
                        Console.WriteLine( $"Invalid: {password} '{letter}' {firstPosition}:{first}, {secondPosition}:{second}" );
                     }
                  }
               }
            }
         }

         return validCount;
      }
   }
}
