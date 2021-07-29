using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HitsAPI.api.Resource;
using HitsAPI.api.Validators;
using HitsAPI.Core.Models;
using HitsAPI.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace HitsAPI.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MusicsController : ControllerBase
    {
        private readonly IMusicService _musicService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public MusicsController(IMusicService musicService, IMapper mapper, ILogger<MusicsController> logger)
        {
            this._mapper = mapper;
            this._musicService = musicService;
            this._logger = logger;
        }

        [HttpGet("")]
        public async Task<ActionResult<IEnumerable<Music>>> GetAllMusics()
        {
            var musics = await _musicService.GetAllWithArtist();
            var musicResources = _mapper.Map<IEnumerable<Music>, IEnumerable<MusicResource>>(musics);
            return Ok(musicResources);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MusicResource>> GetMusicById(int id)
        {
            var music = await _musicService.GetMusicById(id);
            var musicResource = _mapper.Map<Music, MusicResource>(music);

            return Ok(musicResource);
        }

        [HttpPost("")]
        public async Task<ActionResult<MusicResource>> CreateMusic([FromBody] SaveMusicResource saveMusicResource)
        {
            var validator = new SaveMusicResourceValidator();
            var ValidationResult = await validator.ValidateAsync(saveMusicResource);

            if (!ValidationResult.IsValid)
                return BadRequest(ValidationResult.Errors);
            _logger.LogInformation("we are here");
            var musicToCreate = _mapper.Map<SaveMusicResource, Music>(saveMusicResource);
            var newMusic = await _musicService.CreateMusic(musicToCreate);
            var music = await _musicService.GetMusicById(newMusic.Id);
            var musicResource = _mapper.Map<Music, MusicResource>(music);
            return Ok(musicResource);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<MusicResource>> UpdateMusic(int id, [FromBody] SaveMusicResource saveMusicResource)
        {
            var validator = new SaveMusicResourceValidator();
            var validationResult = await validator.ValidateAsync(saveMusicResource);

            var requestIsInvalid = id == 0 || !validationResult.IsValid;

            if (requestIsInvalid)
                return BadRequest(validationResult.Errors);

            var musicToBeUpdate = await _musicService.GetMusicById(id);

            if (musicToBeUpdate == null)
                return NotFound();

            var music = _mapper.Map<SaveMusicResource, Music>(saveMusicResource);

            await _musicService.UpdateMusic(musicToBeUpdate, music);

            var updatedMusic = await _musicService.GetMusicById(id);
            var updatedMusicResource = _mapper.Map<Music, MusicResource>(updatedMusic);

            return Ok(updatedMusicResource);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMusic(int id)
        {
            if (id == 0)
                return BadRequest();

            var music = await _musicService.GetMusicById(id);

            if (music == null)
                return NotFound();

            await _musicService.DeleteMusic(music);

            return NoContent();
        }

    }
}
