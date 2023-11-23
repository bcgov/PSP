namespace Pims.Tools.TsModelGenerator
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var generator = new TsGenerator();
            generator.Generate();
        }
    }
}