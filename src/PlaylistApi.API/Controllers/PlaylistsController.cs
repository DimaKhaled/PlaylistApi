using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PlaylistApi.Application.DTOs.Playlists;
using PlaylistApi.Application.Interfaces.Services;

namespace PlaylistApi.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PlaylistsController : ControllerBase
    {
        private readonly IPlaylistService _playlistService;

        public PlaylistsController(IPlaylistService playlistService)
        {
            _playlistService = playlistService;
        }


        [HttpPost]
        public async Task<ActionResult<PlaylistResponse>> Create(CreatePlaylistRequest request)
        {
            var response = await _playlistService.CreateAsync(request);

            return Ok(response);
        }


        [HttpGet]
        public async Task<ActionResult<List<PlaylistResponse>>> GetUserPlaylists()
        {
            var response = await _playlistService.GetUserPlaylistsAsync();

            return Ok(response);
        }


        [HttpGet("{playlistId}")]
        public async Task<ActionResult<PlaylistResponse>> GetById(int playlistId)
        {
            var response = await _playlistService.GetByIdAsync(playlistId);

            return Ok(response);
        }


        [HttpPut("{playlistId}")]
        public async Task<ActionResult<PlaylistResponse>> Update(int playlistId, UpdatePlaylistRequest request)
        {
            var response = await _playlistService.UpdateAsync(playlistId, request);

            return Ok(response);
        }


        [HttpDelete("{playlistId}")]
        public async Task<IActionResult> Delete(int playlistId)
        {
            await _playlistService.DeleteAsync(playlistId);

            return NoContent();
        }
    }
}
