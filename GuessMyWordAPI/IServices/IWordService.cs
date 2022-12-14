using GuessMyWordAPI.Models;

namespace GuessMyWordAPI.IServices
{
    public interface IWordService
    {
        WordModel AddWord(string language, string word, string? description);
        WordModel GetWordById(long wordId, bool withMetada = false);
        WordModel GetWordByGuid(Guid wordGuid, bool withMetada = false);
        WordModel GetRandomWord(string language, int? length);
        WordModel SolveWord(long wordId, int tries);
    }
}
