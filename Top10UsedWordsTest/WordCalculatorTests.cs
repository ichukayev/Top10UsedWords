using Top10UsedWords;


namespace Top10UsedWordsTest
{
    public class WordCalculatorTests
    {
        private string _testFolder;
        public WordCalculatorTests()
        {
            var maxWordsInFile = 100;
            var maxFilesInFolder = 10;
            _testFolder = CreateLargeTestFolderWithFiles(maxWordsInFile, maxFilesInFolder);
        }

        [Theory]
        [InlineData(10, "longestword", 200)]
        public async Task CountWordsInFilesAsync_ReturnsCorrectWordCountsOfTopFirstWord(int minLength, string topFirstWordExpected, int topFirstWordCountExpected)
        {
            // Arrange
            var wordCalculator = new WordCalculator(minLength);

            // Act
            var result = await wordCalculator.CountWordsInFolderAsync(_testFolder);

            // Assert
            var topFirstWordActual = result.First();
            Assert.Equal(topFirstWordExpected, topFirstWordActual.Key);
            Assert.Equal(topFirstWordCountExpected, topFirstWordActual.Value);
        }

        private string CreateLargeTestFolderWithFiles(int maxWordsInFiles, int maxFilesInFolder)
        {
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "LargeTestFiles");
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            var subFolderPath = Path.Combine(folderPath, "SubFolder");
            if (!Directory.Exists(subFolderPath))
            {
                Directory.CreateDirectory(subFolderPath);
            }

            var words = new[] { "example", "test", "longestword", "random", "generate", "file", "text", "data", "sample", "words" };

            for (int i = 1; i <= maxFilesInFolder; i++)
            {
                var filePath = Path.Combine(folderPath, $"large_testfile_{i}.txt");
                using (var writer = new StreamWriter(filePath))
                {
                    for (int j = 0; j < maxWordsInFiles; j++)
                    {
                        var word = words[j % words.Length];
                        writer.Write(word + " ");
                    }
                }

                var subFilePath = Path.Combine(subFolderPath, $"sub_large_testfile_{i}.txt");
                using (var writer = new StreamWriter(subFilePath))
                {
                    for (int j = 0; j < maxWordsInFiles; j++)
                    {
                        var word = words[j % words.Length];
                        writer.Write(word + " ");
                    }
                }
            }

            return folderPath;
        }
    } 
}