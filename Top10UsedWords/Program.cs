using Top10UsedWords;

string folderPath = "C:\\Users\\Ivan\\source\\repos\\Top10UsedWords\\Top10UsedWordsTest\\bin\\Debug\\net7.0\\LargeTestFiles";

int minLength = 7;

if (!Directory.Exists(folderPath))
{
    Console.WriteLine("Указанная папка не существует.");
    return;
}

var wordCalculator = new WordCalculator(minLength);

var topWords = await wordCalculator.CountWordsInFolderAsync(folderPath);

Console.WriteLine("Топ 10 слов:");

foreach (var kvp in topWords)
{
    Console.WriteLine($"{kvp.Key}: {kvp.Value}");
}
Console.ReadKey();
