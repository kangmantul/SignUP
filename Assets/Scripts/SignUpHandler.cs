using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;
using System.Text;
using Unity.VisualScripting;


public class SignUpHandler : MonoBehaviour
{
    public InputField emailInput;
    public InputField passwordInput;
    public InputField rePasswordInput;
    public Button submitButton;

    void Start()
    {
        submitButton.onClick.AddListener(Submit);
    }

    void Submit()
    {
        string email = emailInput.text;
        string password = passwordInput.text;
        string rePassword = rePasswordInput.text;

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            Debug.LogWarning("Email or password is kosong.");
            return;
        }

        if (password != rePassword)
        {
            Debug.LogWarning("Passwords gak match.");
            return;
        }

        SignUp_data data = new SignUp_data(email, password);
        string json = JsonUtility.ToJson(data);
        Debug.Log("JSON: " + json);

        StartCoroutine(SendDatabase(json));
    }

    [Serializable]
    public class SignUp_data
    {
        public string email;
        public string password;
        public string createDate;

        public SignUp_data(string email, string password)
        {
            this.email = email;
            this.password = password;
            this.createDate = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss");
        }
    }

    IEnumerator SendDatabase (string jsonData)
    {
        string url = ""; 

        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Sign up successful: " + request.downloadHandler.text);
        }
        else
        {
            Debug.LogError("Error sending data: " + request.error);
        }
    }
}
