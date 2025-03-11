using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI; // For handling Unity UI elements

public class ChatBot : MonoBehaviour
{

    void Update()
    {
        // Trigger API call when the player presses the "E" key
        if (Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(CallHuggingFaceAPI("Task: Notify player A that player B is reaching the goal"));
            Debug.Log("E is pressed");
        }
    }

    private IEnumerator CallHuggingFaceAPI(string inputText)
    {
        Task<string> task = HuggingFaceAPI.GetHuggingFaceResponse(inputText);

        // Wait for the async task to complete
        while (!task.IsCompleted) yield return null;

        if (task.IsFaulted)
        {
            Debug.LogError("API Error: " + task.Exception?.Message);
        }
        else
        {
            string response = task.Result;

            // Print to Console first
            Debug.Log("Hugging Face Response: " + response);
        }
    }
}
