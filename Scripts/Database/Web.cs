using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;


 public class Web : MonoBehaviour
{
    public GameObject errorText; // Asigna el objeto del texto de error en el Inspector
    public string direccion;
    void Start()
    {       
        
        //StartCoroutine(GetDate());
        //StartCoroutine(GetUsers());
        //StartCoroutine(Login("admin", "admin"));
        //StartCoroutine(RegisterUser("testuser3", "testuser3"));

    }
    /*
    public void ShowUserItems()
    {
        StartCoroutine(GetItemsIDs(Main.Instance.UserInfo.UserID));
    }
    */
    
    public IEnumerator GetGameData(string username)
    {
        WWWForm form = new WWWForm();
        form.AddField("loginUser", username);
        string path = direccion + "/lookupbbdd/GetGameData.php";
        Debug.Log("1");
        using (UnityWebRequest www = UnityWebRequest.Post(path, form))
        {
            Debug.Log("2");

            yield return www.Send();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Terminando de cargar datos de juego...");
                Debug.Log(www.downloadHandler.text);

                string jsonArray = www.downloadHandler.text;

                // Write the data to the file
                Debug.Log(Application.persistentDataPath);
                string filePath = Application.persistentDataPath + "/playerProgression.json";
                File.WriteAllText(filePath, jsonArray);

                Debug.Log("Datos del jugador guardados en: " + filePath);   
            }
        }
    }

    public IEnumerator Login(string username, string userpassword)
    {
        /*WWWForm form = new WWWForm();
        form.AddField("loginUser", username);
        form.AddField("loginPass", userpassword);

        string path = direccion + "/lookupbbdd/LogIn.php";

        Debug.Log(path);
        using (UnityWebRequest www = UnityWebRequest.Post(path, form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
                //Main.Instance.UserInfo.SetCredentials(username, userpassword);
                //Main.Instance.UserInfo.SetID(www.downloadHandler.text);
                if (www.downloadHandler.text.Contains("Invalid password.") || www.downloadHandler.text.Contains("No user found with that email."))
                {
                    Debug.Log("Wrong Credentials.");
                    errorText.SetActive(true);
                }
                else
                {
                    Debug.Log("Cargar datos de juego...");
                    yield return StartCoroutine(GetGameData(username));
                    Debug.Log("Cargar escena...");
                    SceneManager.LoadScene("Juego");

                   // Main.Instance.UserProfile.SetActive(true);
                    //Main.Instance.Login.gameObject.SetActive(false);
                }

            }
        }
        */

        string path = direccion + "/lookupbbdd/DatosEstrellas.php";
        Debug.Log(path);
        Text errorTextonly  = errorText.GetComponent<Text>();

        WWWForm form = new WWWForm();
                Debug.Log("1");
        form.AddField("loginUser", username);
        
        using (UnityWebRequest www = UnityWebRequest.Post(path, form))
        {
            Debug.Log("2");
            yield return www.SendWebRequest();
            Debug.Log("3");
            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
            }
        }

    }

    public IEnumerator Register(string username, string password, string passwordrepeat)
    {
        string path = direccion + "/lookupbbdd/LogIn.php";
        Text errorTextonly  = errorText.GetComponent<Text>();
        if (password != passwordrepeat)
        {
            Debug.Log("Las contraseñas no coinciden");
            errorTextonly.text = "Las contraseñas no coinciden";
            errorText.SetActive(true);
        }
        else
        {
            WWWForm form = new WWWForm();
            
            form.AddField("loginUser", username);
            form.AddField("loginPass", password);

            using (UnityWebRequest www = UnityWebRequest.Post(path, form))
            {
                yield return www.SendWebRequest();

                if (www.result != UnityWebRequest.Result.Success)
                {
                    errorTextonly.text = "Credenciales incorrectos";
                    errorText.SetActive(true);
                    Debug.Log(www.error);
                }
                else
                {
                    Debug.Log(www.downloadHandler.text);
                }
            }
        }
    }

    public IEnumerator DatosEstrellas(string username)
    {
        string path = direccion + "/lookupbbdd/DatosEstrellas.php";
        Text errorTextonly  = errorText.GetComponent<Text>();

        WWWForm form = new WWWForm();
            
        form.AddField("loginUser", username);

        using (UnityWebRequest www = UnityWebRequest.Post(path, form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
            }
        }
    }

}