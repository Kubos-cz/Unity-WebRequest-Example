using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
﻿using Newtonsoft.Json;

namespace Assets.Scripts
{
    class PatchRequest : MonoBehaviour
    {
        public IEnumerator WebPatchRequest()
        {
            Action<string> onPostEventSuccess = (result) =>
            {
                // Do something when web request returns success
                Debug.Log("Web request succesfull");
            };
            Action<string> onPostEventError = (result) =>
            {
                // Do something when web request returns error
                Debug.Log($"Error: {result}");
            };
            // Call a coroutine containing our request
            yield return StartCoroutine(WebPatchRequestAsync(onPostEventSuccess, onPostEventError));
        }
        private IEnumerator WebPatchRequestAsync(Action<string> onDeleteRequestError, Action<string> onDeleteRequestSuccess)
        {
            // Create request URL string
            string url = $"https://mywebrequest.com/api/object/patch/";

            //Create request body
            Dictionary<string, string> requestBody = new Dictionary<string, string>()
            {
                {"Property1", "Hello" },
                {"Property2", "There"}
            };
            // Serialize body as a Json string
            string requestBodyString = JsonConvert.SerializeObject(requestBody);
            // Convert Json body string into a byte array
            byte[] requestBodyData = System.Text.Encoding.UTF8.GetBytes(requestBodyString);

            // Create new UnityWebRequest, pass on our url and body as a byte array
            UnityWebRequest webRequest = UnityWebRequest.Put(url, requestBodyData);
            // Specify that our method is of type 'patch'
            webRequest.method = "PATCH";

            // Set request headers i.e. conent type, authorization etc
            webRequest.SetRequestHeader("Content-Type", "application/json");
            webRequest.SetRequestHeader("Authorization", "Bearer ABC-123");
            webRequest.SetRequestHeader("Content-length", (requestBodyData.Length.ToString()));

            // Set the default download buffer
            webRequest.downloadHandler = new DownloadHandlerBuffer();

            // Send the request itself
            yield return webRequest.SendWebRequest();

            // Check for errors
            if (webRequest.isNetworkError || webRequest.isHttpError)
            {   
                // Invoke error action
                onDeleteRequestError?.Invoke(webRequest.error);
            }
            else
            {
                // Check when response is received
                if (webRequest.isDone)
                {
                    // Invoke success action
                    onDeleteRequestSuccess?.Invoke("Patch Request Completed");
                }
            }
        }
    }
}

