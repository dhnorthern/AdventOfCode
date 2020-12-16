namespace DailyPuzzle
{
   public interface IPuzzle
   {
      string Name { get; }

      void RunPart1();
      void RunPart2();
   }
}
