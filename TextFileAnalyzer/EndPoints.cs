using System.Text;
using TextFileAnalyzer.Services;

namespace TextFileAnalyzer
{
    public static class EndPoints
    {
        public static void AddEndPoints(this WebApplication app)
        {
            app.MapGet("/api", () => "hello world");

            app.MapPost("/api", async (HttpContext context, ITextFileAnalyzerService textAnalyzerService) =>
            {
                using (var memoryStream = new MemoryStream())
                {
                    await context.Request.Body.CopyToAsync(memoryStream);
                    memoryStream.Seek(0, SeekOrigin.Begin);

                    if (memoryStream.Length > 0)
                    {
                        var bytes = memoryStream.ToArray();
                        var text = Encoding.UTF8.GetString(bytes);

                        var wordCount = textAnalyzerService.CountWords(text);
                        var characterCount = textAnalyzerService.CountCharacters(text);
                        var duplicatedWords = textAnalyzerService.FindDuplicatedWords(text);

                        return Results.Ok(new { WordCount = wordCount, CharacterCount = characterCount, DuplicatedWords = duplicatedWords });
                    }
                    else
                    {
                        return Results.BadRequest("O arquivo está vazio.");
                    }
                }
            });
        }
    }
}
