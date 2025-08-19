using BuildingBlock.Extensions;
using BuildingBlock.Logging;
using SwaggerDoc.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlock.Web
{
    public class BaseHttpClient
    {
        protected readonly IAppLogger<BaseHttpClient> _logger;
        protected readonly HttpClient _httpClient;
        protected readonly ICurrentUserProvider _currentUserProvider;
        public BaseHttpClient(HttpClient httpClient, IAppLogger<BaseHttpClient> logger, ICurrentUserProvider currentUserProvider)
        {
            _httpClient = httpClient;
            _logger = logger;
            _currentUserProvider = currentUserProvider;
        }
        public async Task<TResult?> GetAsync<TResult>(string uri, CancellationToken cancellationToken = default)
        {
            var responseString = await GetStringAsync(uri, cancellationToken);
            return responseString.FromJson<TResult>();
        }
        public async Task<string> GetStringAsync(string uri, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation($"Get {uri}");
            var httpResponse = await _httpClient.GetAsync(uri, cancellationToken);
            httpResponse.EnsureSuccessStatusCode();
            var responseString = await httpResponse.Content.ReadAsStringAsync(cancellationToken);
            _logger.LogInformation($"Get {uri} -> Response {responseString}");
            return responseString;
        }
        public async Task<TResult?> GetWithHeaderAsync<TResult>(string uri, Dictionary<string, string> headers, CancellationToken cancellationToken = default)
        {
            if (headers != null)
            {
                foreach (var header in headers)
                {
                    if (header.Key == "Bearer")
                    {
                        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers
                            .AuthenticationHeaderValue(header.Key, header.Value);
                        continue;
                    }
                    _httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
                }
            }
            return await GetAsync<TResult>(uri, cancellationToken);
        }

        private async Task<string> PostStringAsync(string uri, string dataString, CancellationToken cancellationToken = default)
        {
            var httpContent = new StringContent(dataString, Encoding.UTF8, "application/json");

            _logger.LogInformation($"Post {uri} {dataString}");
            var httpResponse = await _httpClient.PostAsync(uri, httpContent, cancellationToken);
            httpResponse.EnsureSuccessStatusCode();
            var responseString = await httpResponse.Content.ReadAsStringAsync();
            _logger.LogInformation($"Post {uri} {dataString} -> Response {responseString}");
            return responseString;
        }

        public async Task<TResult?> PostStringAsync<TResult>(string uri, string dataString, CancellationToken cancellationToken = default)
        {
            var responseString = await PostStringAsync(uri, dataString, cancellationToken);

            return responseString.FromJson<TResult>(); ;
        }

        // Post response string
        public async Task<string> PostResponseStringAsync<TRequest>(string uri, TRequest data, CancellationToken cancellationToken = default)
        {
            var dataString = data.ToJson();

            var responseString = await PostStringAsync(uri, dataString, cancellationToken);

            return responseString;
        }

        // Nomal Post
        public async Task<TResult?> PostAsync<TRequest, TResult>(string uri, TRequest data, CancellationToken cancellationToken = default)
        {
            var dataString = data.ToJson();
            var responseString = await PostStringAsync(uri, dataString, cancellationToken);

            return responseString.FromJson<TResult>();
        }

        // Post with token
        public async Task<TResult?> PostWithTokenAsync<TRequest, TResult>(string uri, TRequest data, CancellationToken cancellationToken = default)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Bearer", _currentUserProvider.Token);
            return await PostWithHeadersAsync<TRequest, TResult>(uri, data, headers, cancellationToken);
        }

        // Post with customize headers
        public async Task<TResult?> PostWithHeadersAsync<TRequest, TResult>(string uri, TRequest data, Dictionary<string, string> headers, CancellationToken cancellationToken = default)
        {
            var dataString = data.ToJson();
            return await PostStringWithHeadersAsync<TResult>(uri, dataString, headers, cancellationToken);
        }


        public async Task<TResult?> PostStringWithHeadersAsync<TResult>(string uri, string dataString, Dictionary<string, string> headers, CancellationToken cancellationToken = default)
        {
            if (headers != null)
            {
                foreach (var header in headers)
                {
                    if (header.Key == "Bearer")
                    {
                        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers
                            .AuthenticationHeaderValue(header.Key, header.Value);
                        continue;
                    }
                    _httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
                }
            }
            return await PostStringAsync<TResult>(uri, dataString, cancellationToken);
        }

        public async Task<TResult?> PostFormDataAsync<TResult>(string uri, MultipartFormDataContent form, CancellationToken cancellationToken = default)
        {
            var httpResponse = await _httpClient.PostAsync(uri, form, cancellationToken);
            httpResponse.EnsureSuccessStatusCode();
            var responseString = await httpResponse.Content.ReadAsStringAsync();
            _logger.LogInformation($"PostFormData {uri}  -> Response {responseString}");
            return responseString.FromJson<TResult>();
        }

        public async Task<TResult?> PutStringAsync<TResult>(string uri, string dataString, CancellationToken cancellationToken = default)
        {

            var httpContent = new StringContent(dataString, Encoding.UTF8, "application/json");

            _logger.LogInformation($"Put {uri} {dataString}");
            var httpResponse = await _httpClient.PutAsync(uri, httpContent, cancellationToken);
            httpResponse.EnsureSuccessStatusCode();
            var responseString = await httpResponse.Content.ReadAsStringAsync();
            _logger.LogInformation($"Put {uri} {dataString} -> Response {responseString}");
            return responseString.FromJson<TResult>();
        }

        public async Task<TResult?> PutAsync<TRequest, TResult>(string uri, TRequest data, CancellationToken cancellationToken = default)
        {
            var dataString = data.ToJson();
            return await PutStringAsync<TResult>(uri, dataString, cancellationToken);
        }

        public async Task<Stream> GetStreamAsync(string url)
        {
            return await _httpClient.GetStreamAsync(url);
        }

        public async Task<TResult?> DeleteAsync<TResult>(string uri, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation($"Delete {uri}");
            var httpResponse = await _httpClient.DeleteAsync(uri, cancellationToken);
            httpResponse.EnsureSuccessStatusCode();
            var responseString = await httpResponse.Content.ReadAsStringAsync(cancellationToken);
            _logger.LogInformation($"Delete {uri} -> Response {responseString}");
            return responseString.FromJson<TResult>();
        }


        public async Task<TResult?> DeleteWithHeadersAsync<TResult>(string uri, Dictionary<string, string> headers, CancellationToken cancellationToken = default)
        {
            if (headers != null)
            {
                foreach (var header in headers)
                {
                    if (header.Key == "Bearer")
                    {
                        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers
                            .AuthenticationHeaderValue(header.Key, header.Value);
                        continue;
                    }
                    _httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
                }
            }
            return await DeleteAsync<TResult>(uri, cancellationToken);
        }
    }
}
