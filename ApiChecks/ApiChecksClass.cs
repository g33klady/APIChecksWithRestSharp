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
    public class ApiChecksClass
    {
        private static string _baseUrl;
        private static RestClient _client;

        [OneTimeSetUp]
        public void TestClassInitialize()
        {
            _baseUrl = "https://localhost:44367/api/Todo";
            _client = new RestClient(_baseUrl);
        }

        [Test]
        public void VerifyGetAllTodoItemsReturns200()
        {
            //Arrange
            var request = new RestRequest(Method.GET);

            //Act
            IRestResponse response = _client.Execute(request);

            //Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, $"GET all todo items did not return a success status code; it returned {response.StatusCode}");
        }

        [Test]
        public void VerifyGetTodoItemWithId1ReturnsId1()
        {
            //Arrange
            var expectedId = 1;
            var request = new RestRequest($"{expectedId}", Method.GET);
            request.AddUrlSegment("id", expectedId);

            //Act
            IRestResponse<TodoItem> response = _client.Execute<TodoItem>(request);

            //Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, $"GET todo item w/ id {expectedId} did not return a success status code; it returned {response.StatusCode}");

            Assert.AreEqual(expectedId, response.Data.Id, $"GET todo item w/ id {expectedId} did not return item with id {expectedId}, it returned {response.Data.Id}");

            StringAssert.AreEqualIgnoringCase("Walk the dog", response.Data.Name, $"Actual name should have been 'Walk the dog' but was {response.Data.Name}");
        }

        [Test]
        public void VerifyPostWithAllValidValuesReturns201()
        {
            //Arrange
            TodoItem expectedItem = new TodoItem
            {
                Name = "mow the lawn",
                DateDue = new DateTime(2020, 12, 31),
                IsComplete = false
            };
            var request = new RestRequest(Method.POST);

            request.RequestFormat = DataFormat.Json;
            request.AddJsonBody(expectedItem);
            request.AddHeader("CanAccess", "true");

            //Act
            IRestResponse response = _client.Execute(request);

            //Assert
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode, $"Post new todo item should have returned a Created status code; instead it returned {response.StatusCode}");
        }

        [Test, TestCaseSource(typeof(TestDataClass), "PutTestData")]
        public string VerifyPut(TodoItem item)
        {
            //Arrange
            var request = new RestRequest("1", Method.PUT);

            request.RequestFormat = DataFormat.Json;
            request.AddJsonBody(item);
            request.AddHeader("CanAccess", "true");
            request.AddUrlSegment("id", 1);

            //Act
            IRestResponse response = _client.Execute(request);

            //Assert
            return response.StatusCode.ToString();
        }
    }

    public class TestDataClass
    {
        public static IEnumerable PutTestData
        {
            get
            {
                yield return new TestCaseData(new TodoItem
                {
                    Name = "mow the neighbor's lawn",
                    DateDue = new DateTime(2020, 12, 31),
                    IsComplete = false
                }).Returns("NoContent").SetName("happy path");
                yield return new TestCaseData(new TodoItem
                {
                    Name = "",
                    DateDue = new DateTime(2020, 12, 31),
                    IsComplete = false
                }).Returns("BadRequest").SetName("blank name");
                yield return new TestCaseData(new TodoItem
                {
                    DateDue = new DateTime(2020, 12, 31),
                    IsComplete = false
                }).Returns("BadRequest").SetName("missing name field");
            }
        }
    }
}
