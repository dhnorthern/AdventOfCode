using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace DailyPuzzle.Day4
{
   public class PassportValidator
   {
      public bool IsValid( string passport, bool withRules )
      {
         var fields = passport.Split( ' ' );
         if ( fields.Length > 0 )
         {
            var foundFieldCount = 0;

            var fieldList = new List<string>( fields );

            foreach ( var requiredField in PassportFields )
            {
               var index = fieldList.FindIndex( f => string.Equals( f.Split( ':' )[0], requiredField.Item1, StringComparison.InvariantCultureIgnoreCase ) );
               var found = index >= 0;
               var passesRule = !withRules || ( found && requiredField.Item2( fields[index].Split( ':' )[1] ) );
               if ( found && passesRule || requiredField.Item1 == "cid" )
               {
                  foundFieldCount++;
                  if ( found ) Console.WriteLine( $"Found & passes Rule: {fields[index]}" );
               }
               else if ( !found )
               {
                  Console.WriteLine( $"Not found: {requiredField} in {passport}" );
               }
            }

            return foundFieldCount == PassportFields.Count;
         }

         return false;
      }

      public delegate bool FieldRule( string value );

      public class TupleList<T1, T2> : List<Tuple<T1, T2>>
      {
         public void Add( T1 item, T2 item2 )
         {
            Add( new Tuple<T1, T2>( item, item2 ) );
         }
      }

      private readonly TupleList<string,FieldRule> PassportFields = new TupleList<string,FieldRule>
      {
         { "byr", (v) => // Birth Year
                  {
                     // four digits; at least 1920 and at most 2002.
                     var year = int.Parse( v );
                     return 1920 <= year && year <= 2002;
                  } },
         { "iyr", (v) => // Issue Year
                  {
                     // four digits; at least 2010 and at most 2020
                     var year = int.Parse( v );
                     return 2010 <= year && year <= 2020;
                  } },
         { "eyr", (v) => // Expiration Year
                  {
                     // four digits; at least 2020 and at most 2030
                     var year = int.Parse( v );
                     return 2020 <= year && year <= 2030;
                  } },
         { "hgt", (v) => // Height
                  {
                     // a number followed by either cm or in:
                     // If cm, the number must be at least 150 and at most 193.
                     // If in, the number must be at least 59 and at most 76.
                     var regex = new Regex( @"(\d+)(cm|in)" );
                     var match = regex.Match( v );
                     if ( match.Success && match.Groups.Count == 3 )
                     {
                        var number = match.Groups[1].Value;
                        var units = match.Groups[2].Value;
                        var height = int.Parse( number );
                        return units == "cm"
                               ? 150 <= height && height <= 193
                               : units == "in" && 59 <= height && height <= 76;
                     }
                     else
                     {
                        return false;
                     }
                  } },
         { "hcl", (v) => // Hair Color
                  {
                     // a # followed by exactly six characters 0-9 or a-f
                     var regex = new Regex( @"#[a-fA-F0-9]{6}" );
                     var match = regex.Match( v );
                     return match.Success && v.Length == 7;
                  } },
         { "ecl", (v) => // Eye Color
                  {
                     // exactly one of: amb blu brn gry grn hzl oth
                     var colors = new List<string> { "amb","blu","brn","gry","grn","hzl","oth" };
                     return colors.FindIndex( color => color.ToLowerInvariant() == v.ToLowerInvariant() ) >= 0;
                  } },
         { "pid", (v) => // Passport ID
                  {
                     // a nine-digit number, including leading zeroes
                     var regex = new Regex( @"\d{9}" );
                     var match = regex.Match( v );
                     return match.Success && v.Length == 9;
                  } },
         { "cid", (v) => // Country ID
            true
         },
      };
   }
}
