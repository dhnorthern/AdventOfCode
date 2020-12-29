using System;
using System.Collections.Generic;
using System.Linq;

namespace DailyPuzzle.Day17
{
   public class EnergySimulator
   {
      private const char inactiveCube = '.';
      private const char activeCube = '#';

      // Insert inactive cubes around existing set
      public void ExtendLayers3D( Dictionary<int/*Z*/, List<string>> layers )
      {
         if ( layers.Count == 0 )
         {
            return;
         }

         Console.WriteLine( $"-----ExtendLayers-----" );

         var newWidth = layers.First().Value.First().Length + 2;
         var newHeight = layers.First().Value.Count + 2;

         // New neighbors in exterior (X,Y)

         foreach ( var key in layers.Keys.ToList() )
         {
            var currentLayer = layers[key];

            var replacementLayer = new List<string>();

            // Around the outside of each layer in X
            foreach ( var row in currentLayer )
            {
               replacementLayer.Add( string.Format( $"{inactiveCube}{row}{inactiveCube}" ) );
            }

            // Around the outside of each layer in Y
            replacementLayer.Insert( 0, new string( inactiveCube, newWidth ) );
            replacementLayer.Add( new string( inactiveCube, newWidth ) );

            DisplayLayer( key, replacementLayer );
            layers[key] = replacementLayer;
         }

         // New layers in Z

         var newLayer = new List<string>();
         for ( int row = 0; row < newHeight; ++row )
         {
            newLayer.Add( new string( inactiveCube, newWidth ) );
         }

         var lowestLayer = layers.Keys.Min();
         layers.Add( lowestLayer - 1, newLayer );

         var highestLayer = layers.Keys.Max();
         layers.Add( highestLayer + 1, newLayer );
      }

      public void RunCycle3D( Dictionary<int/*Z*/, List<string>> layers )
      {
         var stateChanges = new List<Tuple<int, int, int>>();

         System.Console.WriteLine( $"-----RunCycle-----" );

         foreach ( var layer in layers )
         {
            var z = layer.Key;

            var x = 0;
            foreach ( var row in layer.Value )
            {
               var y = 0;
               var column = row.GetEnumerator();
               while ( column.MoveNext() )
               {
                  var cube = column.Current;

                  var cubeIndex = new Tuple<int, int, int>( x, y, z );
                  var cubeIsActive = IsActiveState( layers, cubeIndex );

                  var activeNeighbors = 0;
                  foreach ( var neighborIndex in NeighborIndices( cubeIndex ) )
                  {
                     activeNeighbors += IsActiveState( layers, neighborIndex ) ? 1 : 0;
                  }

                  if ( cubeIsActive && ( 2 > activeNeighbors || activeNeighbors > 3 ) )
                  {
                     stateChanges.Add( cubeIndex );
                  }
                  else if ( !cubeIsActive && activeNeighbors == 3 )
                  {
                     stateChanges.Add( cubeIndex );
                  }

                  ++y;
               }

               ++x;
            }
         }

         foreach ( var changes in stateChanges )
         {
            ChangeState( layers, changes );
         }

         DisplayLayers( layers );
      }

      public int CountCycles( Dictionary<int/*Z*/, List<string>> layers )
      {
         var cycleCount = 0;

         foreach ( var layer in layers )
         {
            foreach ( var rowCubes in layer.Value )
            {
               foreach ( var cube in rowCubes )
               {
                  cycleCount += cube == activeCube ? 1 : 0;
               }
            }
         }

         return cycleCount;
      }

      private IEnumerable<Tuple<int, int, int>> NeighborIndices( Tuple<int, int, int> cubeIndex )
      {
         var x = cubeIndex.Item1;
         var y = cubeIndex.Item2;
         var z = cubeIndex.Item3;

         for ( int zRelativeNeighbor = -1; zRelativeNeighbor <= 1; ++zRelativeNeighbor )
         {
            yield return new Tuple<int, int, int>( x - 1, y - 1, z + zRelativeNeighbor );
            yield return new Tuple<int, int, int>( x + 0, y - 1, z + zRelativeNeighbor );
            yield return new Tuple<int, int, int>( x + 1, y - 1, z + zRelativeNeighbor );
            yield return new Tuple<int, int, int>( x + 1, y + 0, z + zRelativeNeighbor );
            yield return new Tuple<int, int, int>( x + 1, y + 1, z + zRelativeNeighbor );
            yield return new Tuple<int, int, int>( x + 0, y + 1, z + zRelativeNeighbor );
            yield return new Tuple<int, int, int>( x - 1, y + 1, z + zRelativeNeighbor );
            yield return new Tuple<int, int, int>( x - 1, y + 0, z + zRelativeNeighbor );
         }

         yield return new Tuple<int, int, int>( x + 0, y + 0, z - 1 );
         yield return new Tuple<int, int, int>( x + 0, y + 0, z + 1 );
      }

      private char GetState( Dictionary<int/*Z*/, List<string>> layers, Tuple<int, int, int> cubeIndex )
      {
         var x = cubeIndex.Item1;
         var y = cubeIndex.Item2;
         var z = cubeIndex.Item3;

         if ( x < 0 || y < 0 || !layers.ContainsKey( z ) )
         {
            return inactiveCube;
         }

         var layer = layers[z];
         if ( y >= layer.Count )
         {
            return inactiveCube;
         }

         var row = layer.ElementAt( y );
         if ( x >= row.Length )
         {
            return inactiveCube;
         }

         return row.ElementAt( x );
      }

      private bool IsActiveState( Dictionary<int/*Z*/, List<string>> layers, Tuple<int, int, int> cubeIndex )
      {
         return GetState( layers, cubeIndex ) == activeCube;
      }

      private void ChangeState( Dictionary<int/*Z*/, List<string>> layers, Tuple<int, int, int> cubeIndex )
      {
         var x = cubeIndex.Item1;
         var y = cubeIndex.Item2;
         var z = cubeIndex.Item3;

         var layerCopy = new List<string>( layers[z] );
         var layerElement = layerCopy.ElementAt( y );
         layerCopy.RemoveAt( y );

         switch ( GetState( layers, cubeIndex ) )
         {
            case inactiveCube:
               layerCopy.Insert( y, layerElement.ReplaceAt( x, activeCube ) );
               break;
            case activeCube:
               layerCopy.Insert( y, layerElement.ReplaceAt( x, inactiveCube ) );
               break;
         }

         layers[z] = layerCopy;
      }

      private void DisplayLayers( Dictionary<int/*Z*/, List<string>> layers )
      {
         var iter = layers.GetEnumerator();
         while ( iter.MoveNext() )
         {
            var currentLayer = iter.Current;
            DisplayLayer( currentLayer.Key, currentLayer.Value );
         }
      }

      private void DisplayLayer( int z, List<string> layer )
      {
         Console.WriteLine( $"Z={z}" );
         for ( int row = 0; row < layer.Count; ++row )
         {
            Console.WriteLine( $"    {layer[row]}" );
         }
      }
   }
}
