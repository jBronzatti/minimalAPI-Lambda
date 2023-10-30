using System.Text.RegularExpressions;

namespace TextFileAnalyzer.Services
{
    public interface ITextFileAnalyzerService
    {
        int CountCharacters(string text);
        int CountWords(string text);
        public Dictionary<string, int> FindDuplicatedWords(string fileContent);
    }

    public class TextFileAnalyzerService : ITextFileAnalyzerService
    {
        public int CountCharacters(string text)
        {
            return text.Length;
        }

        public int CountWords(string text)
        {
            string pattern = @"\b\w+\b";
            MatchCollection matches = Regex.Matches(text, pattern);
            return matches.Count;
        }

        public Dictionary<string, int> FindDuplicatedWords(string fileContent)
        {
            var duplicates = new Dictionary<string, int>();
            var uniqueWords = new HashSet<string>();

            var words = Regex.Matches(fileContent, @"\b\w+\b").Select(m => m.Value.ToLower()).ToList();

            foreach (var word in words)
            {
                if (!uniqueWords.Add(word))
                {
                    if (duplicates.ContainsKey(word))
                    {
                        duplicates[word] += 1;
                        continue;
                    }
                    duplicates.Add(word, 1);
                }
            }

            return duplicates;
        }

    }
}
