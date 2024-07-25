using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Top10UsedWords
{
    public class WordCalculator
    {
        private readonly int _minLength;

        public WordCalculator(int minLength)
        {
            _minLength = minLength;
        }

        public async Task<IEnumerable<KeyValuePair<string, int>>> CountWordsInFolderAsync(string folderPath)
        {
            var files = Directory.GetFiles(folderPath, "*.txt", SearchOption.AllDirectories);
            return await CountWordsInFilesAsync(files);
        }

        public async Task<IEnumerable<KeyValuePair<string, int>>> CountWordsInFilesAsync(string[] files)
        {
            var wordCounts = new ConcurrentDictionary<string, int>();

            var tasks = files.Select(file => Task.Run(async () =>
            {
                var fileWordCounts = await CountWordsInFileAsync(file);

                foreach (var kvp in fileWordCounts)
                {
                    wordCounts.AddOrUpdate(kvp.Key, kvp.Value, (key, oldValue) => oldValue + kvp.Value);
                }
            })).ToArray();

            await Task.WhenAll(tasks);

            return wordCounts
                .Where(kvp => kvp.Key.Length >= _minLength)
                .OrderByDescending(kvp => kvp.Value)
                .Take(10);
        }

        private async Task<Dictionary<string, int>> CountWordsInFileAsync(string filePath)
        {
            var wordCounts = new Dictionary<string, int>();

            using (var reader = new StreamReader(filePath))
            {
                string line;
                while ((line = await reader.ReadLineAsync()) != null)
                {
                    var words = line
                        .Split(new[] { ' ', '\r', '\n', '\t', ',', '.', '!', '?', ';', ':', '-', '_', '[', ']', '(', ')', '{', '}', '\"', '\'' },
                        StringSplitOptions.RemoveEmptyEntries);

                    foreach (var word in words)
                    {
                        var cleanedWord = word.Trim().ToLower();
                        if (cleanedWord.Length >= _minLength)
                        {
                            if (wordCounts.ContainsKey(cleanedWord))
                            {
                                wordCounts[cleanedWord]++;
                            }
                            else
                            {
                                wordCounts[cleanedWord] = 1;
                            }
                        }
                    }
                }
            }

            return wordCounts;
        }
    }
}
