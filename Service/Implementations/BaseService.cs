using System;
using System.Net.Http;

namespace Service.Implementations
{
    public class BaseService
    {
        protected HttpClient httpClient = null;
        public BaseService()
        {
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("https://localhost:44343/");
        }
    }
}
