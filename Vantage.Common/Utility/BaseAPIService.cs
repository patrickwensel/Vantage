using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Vantage.Common.Utility
{
    public abstract class BaseAPIService
    {
        private readonly RestClient _client;

        public BaseAPIService()
        {
            _client = new RestClient();
        }

        protected void SetBaseUrlAndTimeout(string baseUrl, int timeoutInMilliseconds = 90000)
        {
            _client.BaseUrl = new Uri(baseUrl);
            _client.Timeout = timeoutInMilliseconds;
        }

        protected RestClient GetClient()
        {
            return _client;
        }

        protected async Task<T> PostRequest<T>(string urlSegment, object requestBody)
        {
            RestRequest request = new RestRequest(urlSegment);
            request.Method = Method.POST;
            request.AddHeader("Accept", "application/json");

            if (requestBody != null)
                request.AddJsonBody(requestBody);

            try
            {
                var apiResponse = await _client.ExecuteAsync(request);
                return JsonConvert.DeserializeObject<T>(apiResponse.Content);
            }
            catch (TaskCanceledException ex)
            {
                throw ex;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        protected async Task<T> PutRequest<T>(string urlSegment, object requestBody)
        {
            RestRequest request = new RestRequest(urlSegment);
            request.Method = Method.PUT;
            request.AddHeader("Accept", "application/json");

            if (requestBody != null)
                request.AddJsonBody(requestBody);

            try
            {
                var apiResponse = await _client.ExecuteAsync(request);
                return JsonConvert.DeserializeObject<T>(apiResponse.Content);
            }
            catch (TaskCanceledException ex)
            {
                throw ex;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        protected async Task<T> DeleteRequest<T>(string urlSegment, object requestBody = null)
        {
            RestRequest request = new RestRequest(urlSegment);
            request.Method = Method.DELETE;
            request.AddHeader("Accept", "application/json");

            if (requestBody != null)
                request.AddJsonBody(requestBody);

            try
            {
                var apiResponse = await _client.ExecuteAsync(request);
                return JsonConvert.DeserializeObject<T>(apiResponse.Content);
            }
            catch (TaskCanceledException ex)
            {
                throw ex;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        protected async Task<T> GetRequest<T>(string urlSegment)
        {
            RestRequest request = new RestRequest(urlSegment);
            request.Method = Method.GET;
            request.AddHeader("Accept", "application/json");

            try
            {
                var apiResponse = await _client.ExecuteAsync(request);
                var returnObject = JsonConvert.DeserializeObject<T>(apiResponse.Content);
                return returnObject;
            }
            catch (System.Net.Http.HttpRequestException ex)
            {
                throw ex;
            }
            catch (TaskCanceledException ex)
            {
                throw ex;
            }
        }

        protected async Task<IRestResponse> GetRequest(string urlSegment)
        {
            RestRequest request = new RestRequest(urlSegment);
            request.Method = Method.GET;
            request.AddHeader("Accept", "application/json");

            try
            {
                return await _client.ExecuteAsync(request);
            }
            catch (System.Net.Http.HttpRequestException ex)
            {
                throw ex;
            }
            catch (TaskCanceledException ex)
            {
                throw ex;
            }
        }

        protected async Task<IRestResponse> PostRequest(string urlSegment, object requestBody, IDictionary<string, object> parameters = null, string contenttype = null)
        {
            RestRequest request = new RestRequest(urlSegment);
            request.Method = Method.POST;
            request.AddHeader("Accept", "application/json");

            if (requestBody != null)
                request.AddJsonBody(requestBody);

            if (parameters != null && parameters.Count > 0)
            {
                foreach (var item in parameters)
                {
                    request.AddParameter(item.Key, item.Value);
                }
            }

            if (!string.IsNullOrEmpty(contenttype))
                request.AddHeader("Content-Type", contenttype);
            else
                request.AddHeader("Content-Type", "application/json");

            try
            {
                return await _client.ExecuteAsync(request);
            }
            catch (TaskCanceledException ex)
            {
                throw ex;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        protected async Task<IRestResponse> PutRequest(string urlSegment, object requestBody)
        {
            RestRequest request = new RestRequest(urlSegment);
            request.Method = Method.PUT;
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "application/json");

            if (requestBody != null)
                request.AddJsonBody(requestBody);

            try
            {
                var apiResponse = await _client.ExecuteAsync(request);
                return apiResponse;
            }
            catch (TaskCanceledException ex)
            {
                throw ex;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
