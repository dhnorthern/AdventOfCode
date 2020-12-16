using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace DailyPuzzle.Day7
{
   public static class LuggageProcessor
   {
      public static Dictionary<string, List<BagReference>> GetContainerRelationships( List<string> rules )
      {
         var bagRules = new Dictionary<string, List<BagReference>>();

         foreach ( var rule in rules )
         {
            var bagMatch = new Regex( @"(?<bag>[a-z\s]+) bags contain+" ).Match( rule );
            if ( bagMatch.Success )
            {
               //Console.WriteLine( "-----------------------" );
               //Console.WriteLine( $"{bagMatch.Groups["bag"]}: " );

               var bagName = bagMatch.Groups["bag"].Value;
               if ( !bagRules.ContainsKey( bagName ) )
               {
                  bagRules[bagName] = new List<BagReference>();
               }

               var noBagMatch = new Regex( @" no other bags." ).Match( rule, bagMatch.Groups[0].Length );
               if ( noBagMatch.Success )
               {
                  //Console.WriteLine( " . 0 bags" );
               }
               else
               {
                  var bagContentMatches = new Regex( @"(?: (?<nbags>\d+) (?<inbag>[a-z\s]+) bag[s]?[,\.])" ).Matches( rule, bagMatch.Groups[0].Length );
                  var bagContentMatch = bagContentMatches.GetEnumerator();
                  while ( bagContentMatch.MoveNext() )
                  {
                     var bag = (Match)bagContentMatch.Current;
                     var bagCount = bag.Groups["nbags"];
                     var containedBagName = bag.Groups["inbag"].Value;
                     bagRules[bagName].Add( new BagReference { Count = int.Parse( bagCount.Value ), Name = containedBagName } );

                     //Console.WriteLine( $" . {bagCount} '{containedBagName}' bags" );
                  }
               }
            }
         }

         return bagRules;
      }

      public static int GetNumberOfBagsWhichCanContain( string soughtBagName, Dictionary<string, List<BagReference>> bagRelationships )
      {
         ContainsSoughtBagName.Clear();

         foreach ( var bagRelationship in bagRelationships )
         {
            if ( bagRelationship.Key == soughtBagName )
            {
               continue;
            }
            if ( ContainsSoughtBagName.ContainsKey( bagRelationship.Key ) )
            {
               continue;
            }
            if ( BagCanContain( bagRelationship.Key, bagRelationship.Value, soughtBagName, bagRelationships ) )
            {
               ContainsSoughtBagName[bagRelationship.Key] = true;
            }
         }

         return ContainsSoughtBagName.Count;
      }

      public static int GetNumberOfBagsContainedWithin( string searchBagName, Dictionary<string, List<BagReference>> bagRelationships )
      {
         var bagCount = 0;

         foreach ( var bagRelationship in bagRelationships[searchBagName] )
         {
            bagCount += bagRelationship.Count * BagCount( bagRelationships[bagRelationship.Name], bagRelationships );
         }

         return bagCount;
      }

      private static bool BagCanContain( string _/*containerBagName*/, IReadOnlyCollection<BagReference> containerBagReferences, string soughtBagName, IReadOnlyDictionary<string, List<BagReference>> bagRelationships )
      {
         if ( containerBagReferences.Count == 0 )
         {
            return false;
         }

         foreach ( var containerContainerBagRef in containerBagReferences )
         {
            if ( containerContainerBagRef.Name == soughtBagName )
            {
               return true;
            }
            if ( ContainsSoughtBagName.ContainsKey( containerContainerBagRef.Name ) )
            {
               return true;
            }
            if ( BagCanContain( containerContainerBagRef.Name, bagRelationships[containerContainerBagRef.Name], soughtBagName, bagRelationships ) )
            {
               ContainsSoughtBagName[containerContainerBagRef.Name] = true;
               return true;
            }
         }

         return false;
      }

      private static int BagCount( List<BagReference> bagReferences, IReadOnlyDictionary<string, List<BagReference>> bagRelationships )
      {
         var bagCount = 1;

         if ( bagReferences.Count == 0 )
         {
            return bagCount;
         }

         foreach ( var containerContainerBagReference in bagReferences )
         {
            bagCount += containerContainerBagReference.Count * BagCount( bagRelationships[containerContainerBagReference.Name], bagRelationships );
         }

         return bagCount;
      }

      private static readonly Dictionary<string,bool> ContainsSoughtBagName = new Dictionary<string, bool>();
   }
}
