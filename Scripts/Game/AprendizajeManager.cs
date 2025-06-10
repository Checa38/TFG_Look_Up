using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AprendizajeManager : MonoBehaviour
{
    // Método que recibe una URL de YouTube como parámetro
    public void OpenLink(string url)
    {
        // Verifica si la URL no es nula o vacía
        if (!string.IsNullOrEmpty(url))
        {
            // Abre la URL en el navegador predeterminado del sistema
            Application.OpenURL(url);
        }
        else
        {
            Debug.LogWarning("La URL proporcionada está vacía o es nula.");
        }
    }
}
