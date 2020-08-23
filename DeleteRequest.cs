using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts
{
    class DeleteRequest : MonoBehaviour
    {
        public IEnumerator WebDeleteRequest()
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
            yield return StartCoroutine(WebDeleteRequestAsync(onPostEventSuccess, onPostEventError));
        }
        private IEnumerator WebDeleteRequestAsync(Action<string> onDeleteRequestError, Action<string> onDeleteRequestSuccess)
        {
            // Create our URL string
            string url = $"https://mywebrequest.com/api/object/delete/";

            // Create new UnityWebRequest, pass on our url and specify that our method is of type 'delete'
            UnityWebRequest webRequest = new UnityWebRequest(url, "DELETE");

            // Set request headers i.e. conent type, authorization etc
            webRequest.SetRequestHeader("Content-Type", "application/json");
            webRequest.SetRequestHeader("Authorization", "Bearer ABC-123");
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
                    onDeleteRequestSuccess?.Invoke("Delete Request Completed");
                }
            }
        }
    }
}

