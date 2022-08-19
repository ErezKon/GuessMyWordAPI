﻿using GuessMyWordAPI.IServices;
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
        public WordVM GetWord(long? id, string? guid)
        {
            if(id.HasValue)
            {
                return new WordVM(_wordService.GetWordById(id.Value));
            } 
            else
            {
                return new WordVM(_wordService.GetWordByGuid(Guid.Parse(guid)));
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
            return new WordVM(_wordService.AddWord(new WordModel
            {
                Language = word.Language,
                Word = word.Word
            }));
        }
    }
}
