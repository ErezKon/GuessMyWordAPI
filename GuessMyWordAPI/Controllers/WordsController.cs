using GuessMyWordAPI.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GuessMyWordAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WordsController : ControllerBase
    {
        [HttpGet("GetWord")]
        public WordVM GetWord(WordVM word)
        {
            return word;
        }

        [HttpGet("GetRandomWord")]
        public WordVM GetRandomWord(string language)
        {
            return new WordVM
            {
                ID = Guid.NewGuid(),
                Language = language,
                Word = "Test Word"
            };
        }


        [HttpGet("AddWord")]
        public WordVM AddWord(WordVM word)
        {
            return word;
        }
    }
}
