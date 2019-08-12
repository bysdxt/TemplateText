using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

namespace TemplateText {
    internal class Program {
        private static void Main(string[] args) {
            var narg = args.Length;
            if (narg < 2) {
                Console.WriteLine($"{Process.GetCurrentProcess().MainModule.ModuleName} <input file> <output file> [<key> <value>]...\nIn Template Text file, key should like `<xxx['key']xxx>`\ne.g.\n<abc[name]abc> means key 'name' ");
                return;
            }
            var keyvalue = new Dictionary<string, string>();
            for (var i = 1; (i += 2) < narg;)
                keyvalue[args[i - 1]] = args[i];
            File.WriteAllText(
                args[1],
                (new Regex(@"\<(?<x>.{0,32}?)\[(?<key>.{0,99}?)\]\k<x>\>", RegexOptions.Compiled | RegexOptions.ExplicitCapture | RegexOptions.CultureInvariant)).Replace(
                    File.ReadAllText(args[0]),
                    m => keyvalue.TryGetValue(m.Groups["key"].Value, out var value) ? value : null)
            );
        }
    }
}
