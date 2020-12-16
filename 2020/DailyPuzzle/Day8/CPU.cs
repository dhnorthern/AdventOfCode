using System.Collections.Generic;

namespace DailyPuzzle.Day8
{
   public class CPU
   {
      public CPU( IEnumerable<string> instructions )
      {
         foreach ( var instruction in instructions )
         {
            var parameters = instruction.Split( ' ' );
            if ( parameters.Length == 2 )
            {
               Instructions.Add( new Instruction { Operation = parameters[0], Argument = int.Parse( parameters[1] ), Executed = false } );
            }
         }
      }

      public bool GetAccumulatorValue( out int accumulator )
      {
         accumulator = 0;

         var currentInstructionIndex = 0;
         while ( currentInstructionIndex >= 0 )
         {
            var currentInstruction = Instructions[currentInstructionIndex];

            if ( currentInstruction.Executed )
            {
               // Where infinite loop occurs, time to stop
               return false;
            }

            currentInstruction.Executed = true;

            switch ( currentInstruction.Operation )
            {
               case "nop":
                  ++currentInstructionIndex;
                  break;
               case "acc":
                  accumulator += currentInstruction.Argument;
                  ++currentInstructionIndex;
                  break;
               case "jmp":
                  currentInstructionIndex += currentInstruction.Argument;
                  if ( 0 > currentInstructionIndex && currentInstructionIndex >= Instructions.Count )
                  {
                     // Invalid instruction, may be experimental so just leave
                     return false;
                  }
                  break;
               default:
                  System.Diagnostics.Debug.Assert( false, $"Unknown instruction '{currentInstruction.Operation}'" );
                  return false;
            }

            if ( currentInstructionIndex == Instructions.Count )
            {
               // Running the last instruction terminates the instruction execution successfully
               return true;
            }
         }

         System.Diagnostics.Debug.Assert( false, "Invalid input" );
         return false;
      }

      public int GetAccumulatorValueWithCorrectedInstruction()
      {
         foreach ( var currentInstruction in Instructions )
         {
            // Modify instruction by swapping "nop" and "jmp"
            if ( currentInstruction.Operation == "nop" && currentInstruction.Argument != 0 ) // cannot change "nop +0" --> "jmp +0"
            {
               currentInstruction.Operation = "jmp";
               if ( /*Try*/GetAccumulatorValue( out var accumulator ) )
               {
                  return accumulator;
               }
               currentInstruction.Operation = "nop"; // restore
               ResetInstructionExecutionFlags();
            }
            else if ( currentInstruction.Operation == "jmp" )
            {
               currentInstruction.Operation = "nop";
               if ( /*Try*/GetAccumulatorValue( out var accumulator ) )
               {
                  return accumulator;
               }
               currentInstruction.Operation = "jmp"; // restore
               ResetInstructionExecutionFlags();
            }
         }

         System.Diagnostics.Debug.Assert( false, "Invalid input or wrong result" );
         return 0;
      }

      private void ResetInstructionExecutionFlags()
      {
         foreach ( var currentInstruction in Instructions )
         {
            currentInstruction.Executed = false;
         }
      }

      public class Instruction
      {
         public string Operation
         {
            get;
            set;
         }
         public int Argument
         {
            get;
            set;
         }
         public bool Executed
         {
            get;
            set;
         }
      }
      private readonly List<Instruction> Instructions = new List<Instruction>();
   }
}
