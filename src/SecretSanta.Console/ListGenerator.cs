using SecretSanta.Core;

namespace SecretSanta.Console
{
    public class ListGenerator
    {
        public static void CreateSanatasList(string[] args)
        {
            var participants = ReadFile(args[0]);
            var outputFile = args[1];
            var bannedPairs = args.Length > 2 
                ? ReadBannedPairs(args[2])
                : new Dictionary<string, string>();

            var pairs = SecretSantaGenerator.Generate(participants, bannedPairs);

            WriteOutput(outputFile, pairs);
        }

        private static IEnumerable<string> ReadFile(string filePath)
        {
            return File.ReadLines(filePath).Select(record => record.Trim());
        }

        private static IDictionary<string, string> ReadBannedPairs(string filePath)
        {
            var dict = new Dictionary<string, string>();

            foreach (var line in ReadFile(filePath))
            {
                var splitRecord = line.Split(",");
                dict.Add(splitRecord[0].Trim(), splitRecord[1].Trim());
            }

            return dict;
        }

        private static void WriteOutput(string filePath, IDictionary<string, string> recordPairs)
        {
            var records = recordPairs.Select(pair => string.Concat(pair.Key.ToString(), " -> ", pair.Value.ToString()));

            File.WriteAllLines(filePath, records);
        }
    }
}
