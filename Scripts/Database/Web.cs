using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
 public class Web : MonoBehaviour
{
    public GameObject feedbackLogin;
    public GameObject errorTextPanel; 
    public GameObject iniciarSesion; 
    public GameObject registrarse; 
    public GameObject errorText;
    void Start()
    {       
        
        if (errorTextPanel != null)
        {
            errorTextPanel.SetActive(false);
        }
        if (feedbackLogin != null)
        {
            Debug.Log("no es null");
            feedbackLogin.SetActive(false);
        }

      /*  if (PlayerPrefs.GetInt("changeUbi", 0) == 1)
        {
            PlayerPrefs.SetInt("changeUbi", 0);
            PlayerPrefs.Save();
            StartCoroutine(Login(PlayerPrefs.GetString("nombre", "test"), PlayerPrefs.GetString("contrasena", "test")));
        }*/
    }
    
    public IEnumerator GetGameData(string username)
    {
        WWWForm form = new WWWForm();
        form.AddField("loginUser", username);
        string path = DatosEntreEscenas.urlToLocalHost + "/lookupbbdd/GetGameData.php";
        using (UnityWebRequest www = UnityWebRequest.Post(path, form))
        {

            yield return www.Send();

            if (www.isNetworkError || www.isHttpError)
            {
                errorTextPanel.SetActive(true);
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

        // Cambiar el color del botón a gris
        Button button = iniciarSesion.GetComponent<Button>();
        ColorBlock colors = button.colors;
        Color originalNormalColor = colors.normalColor;  // Guardar el color original
        colors.normalColor = Color.gray;  // Cambiar a gris
        button.colors = colors;  // Aplicar el cambio

        // Desactivar el botón mientras se realiza la operación
        button.interactable = false;

        TextMeshProUGUI errorTextonly = feedbackLogin.GetComponent<TextMeshProUGUI>();
        WWWForm form = new WWWForm();
        form.AddField("loginUser", username);
        form.AddField("loginPass", userpassword);
        Debug.Log("AQUI 3");
        string path = DatosEntreEscenas.urlToLocalHost + "/lookupbbdd/LogIn.php";

        Debug.Log(path);
        using (UnityWebRequest www = UnityWebRequest.Post(path, form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                errorTextonly.text = LocalizationSettings.StringDatabase.GetLocalizedString("errores", "e1001");
                feedbackLogin.SetActive(true);
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
                if (!www.downloadHandler.text.Contains("Login correcto."))
                {
                    Debug.Log("Credenciales erróneos.");
                    errorTextonly.text = LocalizationSettings.StringDatabase.GetLocalizedString("preGame", "credencialE");
                    feedbackLogin.SetActive(true);
                }
                else
                {
                    Debug.Log("Cargar datos de juego...");
                    yield return StartCoroutine(GetGameData(username));
                    Debug.Log("Cambiando variables estáticas...");
                    DatosEntreEscenas.correo = username;
                    DatosEntreEscenas.logged = true;
                    Debug.Log("Cargar escena...");

                    PlayerPrefs.SetString("nombre", username);

                    PlayerPrefs.Save();
                    PlayerPrefs.SetString("contrasena", userpassword);
                    PlayerPrefs.Save();

                    SceneManager.LoadScene("Juego");
                }
            }
        }
        Debug.Log("AQUI 4");

        // Restaurar el color original del botón al finalizar el proceso
        colors.normalColor = originalNormalColor;
        button.colors = colors;  // Aplicar el cambio

        // Reactivar el botón
        button.interactable = true;
    }

    public IEnumerator Register(string username, string password, string passwordrepeat)
    {
        // Cambiar el color del botón a gris
        Button button = registrarse.GetComponent<Button>();
        ColorBlock colors = button.colors;
        Color originalNormalColor = colors.normalColor;  // Guardar el color original
        colors.normalColor = Color.gray;  // Cambiar a gris
        button.colors = colors;  // Aplicar el cambio

        // Desactivar el botón mientras se realiza la operación
        button.interactable = false;

        // Usar try-finally para asegurarse de que el botón siempre se restaure
        try
        {
            string path = DatosEntreEscenas.urlToLocalHost + "/lookupbbdd/Register.php";
            Text errorTextonly = feedbackLogin.GetComponent<Text>();

            // Verificación del formato del correo electrónico
            if (!IsValidEmail(username))
            {
                errorTextonly.text = LocalizationSettings.StringDatabase.GetLocalizedString("preGame", "noValidMail");
                feedbackLogin.SetActive(true);
                yield break;  // Finaliza la corrutina si el correo no es válido
            }

            // Verificación de la coincidencia de contraseñas
            if (password != passwordrepeat)
            {
                Debug.Log("Las contraseñas no coinciden");
                errorTextonly.text = LocalizationSettings.StringDatabase.GetLocalizedString("preGame", "noMatchPass");
                feedbackLogin.SetActive(true);
                yield break;  // Finaliza la corrutina si las contraseñas no coinciden
            }

            // Verificación de la longitud de la contraseña
            if (password.Length < 6)
            {
                errorTextonly.text = "La contraseña debe tener más de 6 caracteres.";
                feedbackLogin.SetActive(true);
                yield break;  // Finaliza la corrutina si la contraseña es demasiado corta
            }

            // Si todas las validaciones son correctas, proceder con el registro
            WWWForm form = new WWWForm();
            form.AddField("loginUser", username);
            form.AddField("loginPass", password);

            using (UnityWebRequest www = UnityWebRequest.Post(path, form))
            {
                yield return www.SendWebRequest();

                if (www.result != UnityWebRequest.Result.Success)
                {
                    errorTextonly.text = LocalizationSettings.StringDatabase.GetLocalizedString("errores", "e1001");
                    feedbackLogin.SetActive(true);
                    Debug.Log(www.error);
                }
                else
                {
                    yield return StartCoroutine(Login(username, password));
                    Debug.Log(www.downloadHandler.text);
                }
            }
        }
        finally
        {
            // Restaurar el color original del botón al finalizar el proceso o en caso de error
            colors.normalColor = originalNormalColor;
            button.colors = colors;  // Aplicar el cambio

            // Reactivar el botón
            button.interactable = true;
        }
    }


    // Función auxiliar para validar el formato del correo electrónico
    private bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }


    public IEnumerator DatosEstrellas(string username)
    {
        string path = DatosEntreEscenas.urlToLocalHost + "/lookupbbdd/DatosEstrellas.php";
        Text errorTextonly  = feedbackLogin.GetComponent<Text>();

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

    public IEnumerator UpdateGameData(infoCuerpoProgreso astro, System.Action<bool> callback)
    {
        Debug.Log("Actualizar json local");
        if (Miscelanea.UpdatePlayerProgress(astro))
        {
            Debug.Log("Actualizado");
            WWWForm form = new WWWForm();
            Debug.Log(Miscelanea.CombinePlayerData());
            form.AddField("correo", DatosEntreEscenas.correo);
            form.AddField("gameData", Miscelanea.CombinePlayerData());

            string path = DatosEntreEscenas.urlToLocalHost + "/lookupbbdd/UpdateGameData.php";

            using (UnityWebRequest www = UnityWebRequest.Post(path, form))
            {
                yield return www.SendWebRequest();

                if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log(www.error);
                    callback(false);
                }
                else
                {
                    Debug.Log("Datos del juego actualizados correctamente.");
                    Debug.Log(www.downloadHandler.text);
                    callback(true);
                }
            }
        }
        else
        {
            Debug.Log("No se llamará a la bbdd.");
            callback(false);
        }
    }

    public IEnumerator UpdateLogroProgreso(string nombreLogro, System.Action<bool> callback)
    {

        Debug.Log("Actualizar progreso de logro en el json local");
        
        // Llamar a la función que actualiza el progreso local del logro en Miscelanea.
        bool logroActualizado = Miscelanea.UpdateLogroProgreso(nombreLogro);

        if (logroActualizado)
        {
            Debug.Log("Logro actualizado. Enviando datos al servidor...");

            // Preparar el formulario con los datos del logro y los datos de juego combinados.
            WWWForm form = new WWWForm();
            form.AddField("correo", DatosEntreEscenas.correo);
            form.AddField("gameData", Miscelanea.CombinePlayerData());

            Debug.Log("Seguimos por aqui");

            // URL del servidor para actualizar los datos del logro.
            string path = DatosEntreEscenas.urlToLocalHost + "/lookupbbdd/UpdateGameData.php";

            // Enviar la solicitud POST al servidor.
            using (UnityWebRequest www = UnityWebRequest.Post(path, form))
            {
                Debug.Log("Using www");
                yield return www.SendWebRequest();
                Debug.Log("Despues  ");

                // Verificar si la solicitud tuvo éxito.
                if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log(www.error);
                    callback(false); // Informar de que hubo un error en la actualización.
                }
                else
                {
                    Debug.Log("Progreso del logro actualizado correctamente en la base de datos.");
                    Debug.Log(www.downloadHandler.text);
                    callback(true); // Informar de que la actualización fue exitosa.
                }
            }
        }
        else
        {
            Debug.Log("El progreso del logro no cambió o ya estaba completado. No se llamará a la bbdd.");
            callback(false); // Informar de que no hubo necesidad de actualización.
        }
    }



}