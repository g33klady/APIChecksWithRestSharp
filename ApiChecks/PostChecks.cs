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
    public class PostChecks : ApiCheckBase
    {
        private TodoItem testItem;

        [TearDown]
        public void TestDataCleanUp()
        {
            IRestResponse response = _client.Execute(Helpers.DeleteTodoItemRequest(testItem.Id));
            if (response.StatusCode != HttpStatusCode.NoContent)
            {
                Console.WriteLine($"Unable to delete {testItem.Id} - {response.StatusCode}");
            }
            //this could easily be a database cleanup script call
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

        //TODO: parameterize checks for invalid name, missing name, invalid datedue; include security checks - xss, sql injection
        //TODO: POST performance check; use Stopwatch
    }
}
