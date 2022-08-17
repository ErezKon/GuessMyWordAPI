using GuessMyWordAPI.IServices;
using GuessMyWordAPI.Models;
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
        public WordsController(IWordService wordService)
        {
            _wordService = wordService;
        }

        [HttpGet("GetWord")]
        public WordVM GetWord(WordVM word)
        {
            if(word.ID.HasValue)
            {
                return new WordVM(_wordService.GetWordById(word.ID.Value));
            } 
            else
            {
                return new WordVM(_wordService.GetWordByGuid(word.Guid));
            }
        }

        [HttpGet("GetRandomWord")]
        public WordVM GetRandomWord(string language, int? length)
        {
            return new WordVM(_wordService.GetRandomWord(language, length));
        }


        [HttpPost("AddWord")]
        public WordVM AddWord(WordVM word)
        {
            return new WordVM(_wordService.AddWord(new WordModel
            {
                Language = word.Language,
                Word = word.Word
            }));
        }
    }
}
