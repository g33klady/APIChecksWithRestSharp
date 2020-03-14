using APIChecksWithRestSharp.Models;
using NUnit.Framework;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;

namespace ApiChecks
{
    [TestFixture]
    public class ApiChecksClass
    {
        [Test]
        public void VerifyGetAllTodoItemsReturns200()
        {
            //Arrange
            var client = new RestClient("https://localhost:44367/api/Todo");
            var request = new RestRequest(Method.GET);

            //Act
            IRestResponse response = client.Execute(request);

            //Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, $"GET all todo items did not return a success status code; it returned {response.StatusCode}");
        }

        [Test]
        public void VerifyGetItem1ReturnsNameWalkTheDog()
        {
            //TODO: Refactor - this Arrange is not DRY
            //Arrange
            var client = new RestClient("https://localhost:44367/api/Todo/1");
            var request = new RestRequest(Method.GET);

            //Act
            IRestResponse<TodoItem> todoItem = client.Execute<TodoItem>(request);

            //Assert
            Assert.AreEqual(HttpStatusCode.OK, todoItem.StatusCode, $"GET todo item 1 did not return a success status code; it returned {todoItem.StatusCode}");
            StringAssert.AreEqualIgnoringCase("Walk the Dog", todoItem.Data.Name, $"Actual name should have been 'walk the dog' but it was {todoItem.Data.Name}"); //This is fragile! 
        }

    }
}
