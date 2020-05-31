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
            //var request = Helpers.GetAllTodoItemsRequest();

            //Act
            IRestResponse response = _client.Execute(Helpers.GetAllTodoItemsRequest());

            //Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, $"GET all todo items did not return a success status code; it returned {response.StatusCode}");
        }

        [Test]
        public void VerifyGetTodoItemWithId1ReturnsId1()
        {
            //Arrange
            var expectedId = 1;
            //var request = Helpers.GetSingleTodoItemRequest(expectedId);

            //Act
            IRestResponse<TodoItem> response = _client.Execute<TodoItem>(Helpers.GetSingleTodoItemRequest(expectedId));

            //Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, $"GET todo item w/ id {expectedId} did not return a success status code; it returned {response.StatusCode}");

            Assert.AreEqual(expectedId, response.Data.Id, $"GET todo item w/ id {expectedId} did not return item with id {expectedId}, it returned {response.Data.Id}");

            StringAssert.AreEqualIgnoringCase("Walk the dog", response.Data.Name, $"Actual name should have been 'Walk the dog' but was {response.Data.Name}");
        }

        [Test]
        public void VerifyPostWithAllValidValuesReturns201()
        {
            //Arrange
            TodoItem expectedItem = Helpers.GetTestTodoItem();
            //var request = Helpers.PostTodoItemRequest(expectedItem);

            //Act
            IRestResponse response = _client.Execute(Helpers.PostTodoItemRequest(expectedItem));

            //Assert
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode, $"Post new todo item should have returned a Created status code; instead it returned {response.StatusCode}");
        }

        [Test, TestCaseSource(typeof(TestDataClass), "PutTestData")]
        public string VerifyPut(TodoItem item)
        {
            //Arrange
            //var request = Helpers.PutTodoItemRequest(1, item);

            //Act
            IRestResponse response = _client.Execute(Helpers.PutTodoItemRequest(1, item));

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
                yield return new TestCaseData(Helpers.GetTestTodoItem()).Returns("NoContent").SetName("happy path");
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
