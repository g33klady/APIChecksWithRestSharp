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
    public class PostChecks
    {
        private static string _baseUrl;
        private static RestClient _client;
        private TodoItem testItem;

        [OneTimeSetUp]
        public void TestClassInitialize()
        {
            _baseUrl = "https://localhost:44367/api/Todo";
            _client = new RestClient(_baseUrl);
        }

        [TearDown]
        public void TestDataCleanUp()
        {
            IRestResponse response = _client.Execute(Helpers.DeleteTodoItemRequest(testItem.Id));
            if (response.StatusCode != HttpStatusCode.NoContent)
            {
                Console.WriteLine($"Unable to delete {testItem.Id} - {response.StatusCode}");
            }
        }

        [Test]
        public void VerifyPostWithAllValidValuesReturns201()
        {
            //Arrange
            TodoItem expectedItem = Helpers.GetTestTodoItem();
            //var request = Helpers.PostTodoItemRequest(expectedItem);

            //Act
            IRestResponse<TodoItem> response = _client.Execute<TodoItem>(Helpers.PostTodoItemRequest(expectedItem));
            testItem = response.Data;

            //Assert
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode, $"Post new todo item should have returned a Created status code; instead it returned {response.StatusCode}");
        }
    }
}
