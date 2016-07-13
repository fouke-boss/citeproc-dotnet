using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CiteProc.Compilation;
using CiteProc.Data;

namespace CiteProc.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            // init
            var passes = 0;
            var unsupported = new Dictionary<string, List<ITestCase>>();

            // create tests
            var tests = new ITest[]
            {
                new Fixtures.Test("Debugging", "Debugging"),
                //new CodeTesting.IntegrationTest(),
                new Fixtures.Test("Basic Tests - v1.0", "v10.BasicTests"),
                new Fixtures.Test("CSL Test Suite - v1.0", "v10.CslTestSuite"),
            }
            .Where(x => x.Cases.Length > 0)
            .ToArray();

            // padding
            var total = tests
                .SelectMany(x => x.Cases)
                .Count();
            var padding = tests
                .SelectMany(x => x.Cases)
                .Select(x => x.Name.Length)
                .Max() + 2;

            // log
            var logpath = string.Format("testrun {0:yyyyMMdd HHmm}.txt", DateTime.Now);

            // execute
            int index = 0;
            using (var log = new System.IO.StreamWriter(logpath))
            {
                foreach (var test in tests)
                {
                    // init
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine(test.Name);

                    // cases
                    foreach (var @case in test.Cases)
                    {
                        // write name
                        Console.ResetColor();
                        Console.Write(" ");
                        Console.Write(@case.Name.PadRight(padding));

                        // progress
                        var left = Console.CursorLeft;
                        var top = Console.CursorTop;
                        Console.Write(string.Format("{0}/{1}", index, total));
                        Console.SetCursorPosition(left, top);

                        // execute
                        var start = DateTime.Now;
                        try
                        {
                            // execute
                            var result = @case.Execute(log);

                            // show result
                            if (result)
                            {
                                // write
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.Write("Pass");

                                // add
                                passes++;
                            }
                            else
                            {
                                // write
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.Write("Fail");
                            }
                        }
                        catch (FeatureNotSupportedException ex)
                        {
                            // process
                            if (!unsupported.ContainsKey(ex.Feature))
                            {
                                unsupported.Add(ex.Feature, new List<ITestCase>());
                            }
                            unsupported[ex.Feature].Add(@case);

                            // write
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.Write("N/S ");
                        }

                        // done
                        var duration = DateTime.Now.Subtract(start).TotalMilliseconds;

                        // duration
                        Console.ResetColor();
                        Console.Write("  ({0:0} ms)", duration);
                        Console.WriteLine();

                        // next
                        index++;
                    }
                }

                // summary
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("Summary".PadRight(padding + 1));
                Console.WriteLine();

                // log
                log.WriteLine();
                log.WriteLine();
                log.WriteLine("".PadRight(80, '-'));
                log.WriteLine("-- Summary".PadRight(78).PadRight(80, '-'));
                log.WriteLine("".PadRight(80, '-'));

                // totals
                var notsupported = unsupported.Sum(x => x.Value.Count());
                WriteTotal(log, ConsoleColor.Green, "Pass", padding, passes, total);
                WriteTotal(log, ConsoleColor.Yellow, "N/S ", padding, notsupported, total);
                WriteTotal(log, ConsoleColor.Red, "Fail", padding, (total - passes - notsupported), total);
                log.WriteLine("".PadRight(80, '-'));

                // 
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("Totals".PadRight(padding + 1));
                Console.Write(string.Format("{0:0.0}%", (double)passes / (double)(total - notsupported) * 100d));
                Console.WriteLine();

                // log
                log.WriteLine(string.Format("     {0:0.0}%", (double)passes / (double)(total - notsupported) * 100d));
                log.WriteLine("".PadRight(80, '-'));

                // unsupported
                foreach (var entry in unsupported.OrderByDescending(x => x.Value.Count))
                {
                    // header
                    log.WriteLine("{0}{1}", entry.Key.PadRight(40), entry.Value.Count);
                }
            }

            // done
            Console.ReadKey(true);
        }
        private static void WriteTotal(System.IO.StreamWriter log, ConsoleColor color, string header, int padding, int part, int total)
        {
            // init
            var length = total.ToString().Length;

            // console
            Console.Write("".PadRight(padding + 1));
            Console.ForegroundColor = color;
            Console.Write(header);
            Console.ResetColor();
            Console.Write("  ");
            Console.Write(string.Format("{0}/{1}", part.ToString().PadLeft(length), total));
            Console.WriteLine();
            Console.ResetColor();

            // log
            log.WriteLine("{0}{1}/{2}", header.PadRight(5), part.ToString().PadLeft(length), total);
        }

        public void Example()
        {
            // Load a style from disk (or use one of the overloads for reading
            // from a stream, a text reader or an xml reader).
            var style = File.Load("harvard-cite-them-right.csl");

            // Compile the style to get a processor instance.
            var processor = Processor.Compile(style);

            // Data of the referenced items (books, articles, etc.) is accessed
            // through the IDataProvider interface. CiteProc.NET comes with a
            // default implementation of this interface that supports the
            // CSL JSON format.
            processor.DataProviders = DataProvider.Load("items.json", DataFormat.Json);

            // Now, you are ready to render citations and bibliographies using
            // the selected style:
            var entries = processor.GenerateBibliography();

            // The result is an instance of a CiteProc.Formatting.Run class.
            // This instance can then be converted to the desired format. CiteProc
            // supports plain text and HTML out-of-the-box; the CiteProc.WpfDemo
            // project contains an example of how to show the result in a WPF
            // TextBlock. Other formats can be added easily.

            // Austen, J. (1995) Pride and Prejudice. New York, NY: Dover Publications.
            var plainText = entries.First().ToPlainText();

            // Austen, J. (1995) <i>Pride and Prejudice</i>. New York, NY: Dover Publications.
            var html = entries.First().ToHtml();
        }
    }
}