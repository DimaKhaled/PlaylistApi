namespace PlaylistApi.Application.Interfaces.ExternalServices
{
    public interface IMusicService
    {
        Task<List<SongSearchResponse>> SearchAsync(string query);
    }
}
