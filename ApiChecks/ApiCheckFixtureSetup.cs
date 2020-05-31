using NUnit.Framework;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApiChecks
{
    [SetUpFixture]
    public class ApiCheckBase
    {
        public static string _baseUrl;
        public static RestClient _client;

        [OneTimeSetUp]
        public void TestFixtureInitialize()
        {
            _baseUrl = "https://localhost:44367/api/Todo";
            _client = new RestClient(_baseUrl);
        }
    }
}
