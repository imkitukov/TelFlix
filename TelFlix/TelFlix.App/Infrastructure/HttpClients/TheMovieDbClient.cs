using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace TelFlix.App.HttpClients
{
    public class TheMovieDbClient : ITheMovieDbClient
    {
        private const string apiKey = "207de229486742b95fba944e0d0509be";
        private readonly string ApiKeyPostFix = "?api_key={0}";

        private const string BaseUri = "https://api.themoviedb.org/3/";
        private const string MovieDetailsUri = "movie/{0}{1}{2}";

        private const string SearchMovieUri = "search/movie{0}&query={1}";
        private const string AppendVideosPostFix = "&append_to_response=videos";

        private const string ListAllGenresUri = "genre/movie/list{0}";
        private const string CastMovieUri = "movie/{0}/credits{1}";
        private const string ActorDetailsUri = "person/{0}{1}";

        private readonly HttpClient client;

        public TheMovieDbClient(HttpClient client)
        {
            this.client = client;
            this.client.BaseAddress = new Uri(BaseUri);
            this.client.DefaultRequestHeaders.Add("Accept", "application/json");
            this.ApiKeyPostFix = string.Format(ApiKeyPostFix, apiKey);
        }

        public async Task<string> GetMovieDetails(int movieId)
        {
            try
            {
                // https://api.themoviedb.org/3/movie/550?api_key=207de229486742b95fba944e0d0509be&append_to_response=videos

                //Here we are making the assumption that our HttpClient instance
                //has already had its base address set.
                string requestUri = string.Format(MovieDetailsUri, movieId, ApiKeyPostFix, AppendVideosPostFix);

                var response = await this.client.GetAsync(requestUri);

                response.EnsureSuccessStatusCode();

                return response.Content.ReadAsStringAsync().Result;
            }
            catch (HttpRequestException)
            {

                //_logger.LogError($"An error occured connecting to values API {ex.ToString()}");
                return "ERROR";
            }
        }

        public async Task<string> SearchMovie(string searchQuery)
        {
            try
            {
                // https://api.themoviedb.org/3/search/movie?api_key=207de229486742b95fba944e0d0509be&query=Jack+Reacher

                if (string.IsNullOrWhiteSpace(searchQuery))
                {
                    searchQuery = "";
                }

                searchQuery = searchQuery.Replace(" ", "+");

                string requestUri = string.Format(SearchMovieUri, ApiKeyPostFix, searchQuery);

                var response = await this.client.GetAsync(requestUri);

                response.EnsureSuccessStatusCode();

                return response.Content.ReadAsStringAsync().Result;
            }
            catch (HttpRequestException)
            {

                //_logger.LogError($"An error occured connecting to values API {ex.ToString()}");
                return "ERROR";
            }
        }

        public async Task<string> ListAllGenres()
        {
            try
            {
                // https://api.themoviedb.org/3/genre/movie/list?api_key=207de229486742b95fba944e0d0509be

                string requestUri = string.Format(ListAllGenresUri, ApiKeyPostFix);

                var response = await this.client.GetAsync(requestUri);

                response.EnsureSuccessStatusCode();

                return response.Content.ReadAsStringAsync().Result;
            }
            catch (HttpRequestException)
            {
                //_logger.LogError($"An error occured connecting to values API {ex.ToString()}");
                return "ERROR";
            }
        }

        public async Task<string> GetMovieActors(int movieId)
        {
            try
            {
                //https://api.themoviedb.org/3/movie/550/credits?api_key=207de229486742b95fba944e0d0509be

                string requestUri = string.Format(CastMovieUri, movieId, ApiKeyPostFix);

                var response = await this.client.GetAsync(requestUri);

                response.EnsureSuccessStatusCode();

                return response.Content.ReadAsStringAsync().Result;
            }
            catch (HttpRequestException)
            {

                //_logger.LogError($"An error occured connecting to values API {ex.ToString()}");
                return "ERROR";
            }
        }

        public async Task<string> GetActorDetails(int actorId)
        {
            try
            {
                //https://api.themoviedb.org/3/person/550?api_key=207de229486742b95fba944e0d0509be

                string requestUri = string.Format(ActorDetailsUri, actorId, ApiKeyPostFix);

                var response = await this.client.GetAsync(requestUri);

                response.EnsureSuccessStatusCode();

                return response.Content.ReadAsStringAsync().Result;
            }
            catch (HttpRequestException)
            {

                //_logger.LogError($"An error occured connecting to values API {ex.ToString()}");
                return "ERROR";
            }
        }
    }
}
