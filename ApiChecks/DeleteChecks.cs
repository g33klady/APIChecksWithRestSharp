using APIChecksWithRestSharp.Models;
using NUnit.Framework;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections;
using System.Net;

namespace ApiChecks
{
    [TestFixture]
    public class DeleteChecks
    {
        private TodoItem testItem;
        private static string _baseUrl;
        private static RestClient _client;

        [OneTimeSetUp]
        public void TestClassInitialize()
        {
            _baseUrl = "https://localhost:44367/api/Todo";
            _client = new RestClient(_baseUrl);
        }

        [SetUp]
        public void TestDataSetup()
        {
            TodoItem item = new TodoItem
            {
                Name = $"DeleteChecks item {new DateTime().Ticks}",
                DateDue = new DateTime(2020, 12, 31),
                IsComplete = false
            };
            var request = new RestRequest(Method.POST);

            request.RequestFormat = DataFormat.Json;
            request.AddJsonBody(item);
            request.AddHeader("CanAccess", "true");

            //Act
            IRestResponse<TodoItem> response = _client.Execute<TodoItem>(request);
            testItem = response.Data;
        }

        [Test]
        public void VerifyDeleteWithValidIdReturns204()
        {
            //Arrange
            var request = new RestRequest($"{testItem.Id}", Method.DELETE);
            request.AddHeader("CanAccess", "true");
            request.AddUrlSegment("id", testItem.Id);

            //Act
            IRestResponse response = _client.Execute(request);

            //Assert
            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode, $"Delete todo item with id {testItem.Id} should have returned a NoContent response; instead it returned {response.StatusCode}");
        }
    }
}
