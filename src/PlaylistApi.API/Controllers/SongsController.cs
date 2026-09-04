using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PlaylistApi.Application.DTOs.Songs;
using PlaylistApi.Application.Interfaces.Services;

namespace PlaylistApi.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SongsController : ControllerBase
    {
        private readonly ISongService _songService;

        public SongsController(ISongService songService)
        {
            _songService = songService;
        }


        [HttpGet("search")]
        public async Task<ActionResult<List<SongSearchResponse>>> Search([FromQuery] string query)
        {
            var response = await _songService.SearchAsync(query);

            return Ok(response);
        }


        [HttpPost("playlists/{playlistId}")]
        public async Task<ActionResult<SongSearchResponse>> AddToPlaylist(int playlistId, AddSongRequest request)
        {
            var response = await _songService.AddToPlaylistAsync(playlistId, request);

            return Ok(response);
        }


        [HttpDelete("playlists/{playlistId}/{songId}")]
        public async Task<IActionResult> DeleteFromPlaylist(int playlistId, int songId)
        {
            await _songService.DeleteFromPlaylistAsync(playlistId, songId);

            return NoContent();
        }
    }
}
