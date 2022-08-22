using GuessMyWordAPI.IServices;
using GuessMyWordAPI.Models;
using GuessMyWordAPI.Services;
using GuessMyWordAPI.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GuessMyWordAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WordsController : ControllerBase
    {
        private readonly IWordService _wordService;
        private readonly IMyLogger _myLogger;
        public WordsController(IWordService wordService, IMyLogger myLogger)
        {
            _wordService = wordService;
            _myLogger = myLogger;
        }


        [HttpGet("GetWord")]
        public WordVM GetWord(long? id, string? guid, bool withMetadata = false)
        {
            if(id.HasValue)
            {
                return new WordVM(_wordService.GetWordById(id.Value, withMetadata));
            } 
            else
            {
                return new WordVM(_wordService.GetWordByGuid(Guid.Parse(guid), withMetadata));
            }
        }

        [HttpGet("GetRandomWord")]
        public WordVM GetRandomWord(string language, int? length)
        {
            return new WordVM(_wordService.GetRandomWord(language, length));
        }


        [HttpPost("AddWord")]
        public WordVM AddWord([FromBody] WordVM word)
        {
            return new WordVM(_wordService.AddWord(word.Language, word.Word, word.Description));
        }

        [HttpPost("SolveWord")]
        public WordVM SolveWord(SolveWordVM word)
        {
            return new WordVM(_wordService.SolveWord(word.ID, word.SolveIndex));
        }
    }
}
