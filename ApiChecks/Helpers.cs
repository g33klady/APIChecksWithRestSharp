using APIChecksWithRestSharp.Models;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApiChecks
{
    public static class Helpers
    {
        public static RestRequest GetAllTodoItemsRequest()
        {
            var request = new RestRequest(Method.GET);
            return request;
        }

        public static RestRequest GetSingleTodoItemRequest(long id)
        {
            var request = new RestRequest($"{id}", Method.GET);
            request.AddUrlSegment("id", id);
            return request;
        }

        public static RestRequest PostTodoItemRequest(TodoItem item)
        {
            var request = new RestRequest(Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddJsonBody(item);
            request.AddHeader("CanAccess", "true");
            return request;
        }

        public static RestRequest PutTodoItemRequest(long id, TodoItem item)
        {
            var request = new RestRequest($"{id}", Method.PUT);
            request.RequestFormat = DataFormat.Json;
            request.AddJsonBody(item);
            request.AddHeader("CanAccess", "true");
            request.AddUrlSegment("id", id);
            return request;
        }

        public static RestRequest DeleteTodoItemRequest(long id)
        {
            var request = new RestRequest($"{id}", Method.DELETE);
            request.AddHeader("CanAccess", "true");
            request.AddUrlSegment("id", id);
            return request;
        }
    }
}
