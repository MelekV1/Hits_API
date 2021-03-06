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
    public class ArtistController : ControllerBase
    {
        private readonly IArtistService _artistService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public ArtistController(IArtistService artistService, IMapper mapper, ILogger<ArtistController> logger)
        {
            this._mapper = mapper;
            this._artistService = artistService;
            this._logger = logger;
        }

        [HttpGet("")]
        public async Task<ActionResult<IEnumerable<ArtistResource>>>GetAllArtists()
        {
            var artists = await _artistService.GetAllArtists();
            var artistResource = _mapper.Map<IEnumerable<Artist>, IEnumerable<ArtistResource>>(artists);

            return Ok(artistResource);

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ArtistResource>> GetArtistById(int id)
        {
            var artist = await _artistService.GetArtistById(id);
            var artistResource = _mapper.Map<Artist, ArtistResource>(artist);

            return Ok(artistResource);
        }

        [HttpPost("")]
        public async Task<ActionResult<ArtistResource>> CreateArtist([FromBody] SaveArtistResource saveArtistResource)
        {
            var validator = new SaveArtistResourceValidator();
            var validationResult = await validator.ValidateAsync(saveArtistResource);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var artistToCreate = _mapper.Map<SaveArtistResource, Artist>(saveArtistResource);

            var newArtist = await _artistService.CreateArtist(artistToCreate);  
            var artist = await _artistService.GetArtistById(newArtist.Id);
            var artistResource = _mapper.Map<Artist, ArtistResource>(artist);
            return Ok(artistResource);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ArtistResource>> UpdateArtist(int id,[FromBody]SaveArtistResource saveArtistResource)
        {
            var validator = new SaveArtistResourceValidator();
            var validationResult = await validator.ValidateAsync(saveArtistResource);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);
            var artistToBeUpdated = await _artistService.GetArtistById(id);
            if (artistToBeUpdated == null)
                return NotFound();
            var artist = _mapper.Map<SaveArtistResource, Artist>(saveArtistResource);
            await _artistService.UpdateArtist(artistToBeUpdated, artist);
            var updatedArtist = await _artistService.GetArtistById(id);
            var updatedArtistResource = _mapper.Map<Artist, ArtistResource>(updatedArtist);
            return Ok(updatedArtistResource);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteArtist(int id)
        {
            var artist = await _artistService.GetArtistById(id);

            await _artistService.DeleteArtist(artist);

            return NoContent();
        }

    }
}
