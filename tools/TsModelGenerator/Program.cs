namespace Pims.Tools.TsModelGenerator
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var generator = new TsGenerator();
            generator.Generate();
        }
    }
}