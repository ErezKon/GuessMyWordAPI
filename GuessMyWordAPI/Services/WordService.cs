using GuessMyWordAPI.DataLayer;
using GuessMyWordAPI.IServices;
using GuessMyWordAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace GuessMyWordAPI.Services
{
    public class WordService : IWordService
    {
        private readonly WordContext _wordDB;
        private readonly Random _random;
        private readonly IConfiguration _config;
        private readonly int maxTries = 6;
        public WordService(IConfiguration config)
        {
            //_wordDB = new WordContext(config);
            _random = new Random();
            _config = config;
        }
        public WordModel AddWord(string language, string word, string? description = null)
        {
            if (string.IsNullOrWhiteSpace(language) || string.IsNullOrWhiteSpace(word))
            {
                throw new ArgumentNullException("Missing data, Language/Word");
            }
            using var context = new WordContext(_config);
            var existingWord = context.Words
            .FirstOrDefault(w => w.Language.Equals(language) && w.Word.Equals(word));
            if (existingWord != null)
            {
                return existingWord;
            }
            var wordModel = new WordModel
            {
                Guid = Guid.NewGuid(),
                Language = language.ToLower(),
                Word = word.ToUpper()
            };
            if(!string.IsNullOrWhiteSpace(description))
            {
                wordModel.Description = description;
            }
            context.Add(wordModel);
            context.SaveChanges();
            wordModel.Metadata = new List<WordMetadata>();
            for (int i = 0; i <= maxTries; i++)
            {
                wordModel.Metadata.Add(new WordMetadata
                {
                    WordId = wordModel.ID,
                    SolveCount = 0,
                    SolveIndex = i,
                });
            }
            context.SaveChanges();
            return wordModel;
        }

        public WordModel GetRandomWord(string language, int? length)
        {
            using var context = new WordContext(_config);
            var lang = language.ToLower();
            var query = context.Words
                .Where(w => w.Language.Equals(lang));
            if (length.HasValue && length.Value > 0)
            {
                query = query
                    .Where(w => w.Word.Length == length.Value);
            }

            var lst = query.ToList();
            return lst[_random.Next(lst.Count)];
        }

        public WordModel GetWordByGuid(Guid wordGuid, bool withMetadata = false)
        {
#pragma warning disable CS8603 // Possible null reference return.
            using var context = new WordContext(_config);
            return withMetadata ?
                context
                .Words
                .Include(w => w.Metadata)
                .FirstOrDefault(w => w.Guid.Equals(wordGuid)) :
                context
                .Words
                .FirstOrDefault(w => w.Guid.Equals(wordGuid));
#pragma warning restore CS8603 // Possible null reference return.
        }

        public WordModel GetWordById(long wordId, bool withMetada = false)
        {
            //var hebrewWordsPath2 = @"C:\Users\Erez_Konforti\source\repos\GuessMyWord\src\assets\he_full.txt";
            //var words = File.ReadAllLines(hebrewWordsPath2)
            //    .Select(l =>
            //    {
            //        var spl = l.Trim().Split(' ');
            //        return spl[0];
            //    })
            //    .Where(w => w.Length >=3 && w.Length <=9)
            //    .ToList();

            using var context = new WordContext(_config);
            //var existing = context.Words
            //    .Where(w => w.Language.Equals("hebrew"))
            //    .ToList()
            //    .Distinct()
            //    .GroupBy(w => w.Word)
            //    .ToDictionary(k => k.Key, v => v.Key);

            //for (int i = 0; i < words.Count; i++)
            //{
            //    var word = words[i];
            //    if(existing.ContainsKey(word))
            //    {
            //        Console.WriteLine($"Word ${word} exist, skipping.");
            //        continue;
            //    }
            //    Console.WriteLine($"Word ${word} does not exist, adding.");
            //    AddWord("hebrew", word);
            //}
            //context.SaveChanges();
            //using var context = new WordContext(_config);
#pragma warning disable CS8603 // Possible null reference return.
            return withMetada ?
                context
                .Words
                .Include(w => w.Metadata)
                .FirstOrDefault(w => w.ID == wordId) :
                context
                .Words
                .FirstOrDefault(w => w.ID == wordId);
#pragma warning restore CS8603 // Possible null reference return.
        }

        public WordModel SolveWord(long wordId, int tries)
        {
            using var context = new WordContext(_config);
            var word = context
                .Words
                .Include(w => w.Metadata)
                .FirstOrDefault(w => w.ID == wordId);
            if(word != null)
            {
                var meta = word.Metadata
                    .FirstOrDefault(m => m.SolveIndex == tries);
                if(meta != null)
                {
                    meta.SolveCount++;
                    context.SaveChanges();
                }
            }
            return word;
        }
    }
}
