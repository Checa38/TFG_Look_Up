using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SceneLoadManager : MonoBehaviour
{
    [SerializeField] private Slider loadbar;
    [SerializeField] private GameObject loadPanel;

    [SerializeField] private GameObject ubicacion;

    [SerializeField] private GameObject confirmacionUbicacion;
    [SerializeField] private GameObject logroNuevo;
    [SerializeField] private GameObject centro;

    [SerializeField] private MainManager mainManager;  // Referencia al StellariumAPI
    [SerializeField] private Canvas canvasToDisable;  // Canvas a deshabilitar

    void Start()
    {
        StartCoroutine(LoadStellariumAPI());
    }

    public IEnumerator LoadStellariumAPI()
    {
        canvasToDisable.gameObject.SetActive(true);
        // Primera fase: Avanzar la barra hasta el 50% en 5 segundos
        float duration = 1.0f;
        float targetProgress = 0.5f;
        float elapsed = 0.0f;

        DatosEntreEscenas.ubicacion = PlayerPrefs.GetString("nombreUbi", "Córdoba");
        DatosEntreEscenas.longitud = PlayerPrefs.GetFloat("longitud", -4.7727500f);
        DatosEntreEscenas.latitud = PlayerPrefs.GetFloat("latitud", 37.8915500f);

         if (centro != null)
        {
            // Crear una lista temporal para los objetos que deben ser eliminados
            var childrenToRemove = new System.Collections.Generic.List<Transform>();

            // Recorrer los hijos de "Centro" y agregar a la lista los que no sean "Techo"
            foreach (Transform child in centro.transform)
            {
                if (child.name != "Techo")
                {
                    childrenToRemove.Add(child);
                }
            }

            // Destruir los objetos en la lista para evitar problemas de modificación de la colección durante la iteración
            foreach (Transform child in childrenToRemove)
            {
                Destroy(child.gameObject);
            }
        }
        else
        {
            Debug.LogError("El GameObject 'Centro' no está asignado.");
        }

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            loadbar.value = Mathf.Lerp(0, targetProgress, elapsed / duration);
            yield return null;
        }

        // Segunda fase: Iniciar la carga del StellariumAPI
        yield return StartCoroutine(mainManager.Inicio());
        

        targetProgress = 2.0f;
        elapsed = 0.0f;
         while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            loadbar.value = Mathf.Lerp(0.5f, targetProgress, elapsed / duration);
            yield return null;
        }

        confirmacionUbicacion.SetActive(false);
        ubicacion.SetActive(false);
        logroNuevo.SetActive(false);

        // Deshabilitar el canvas
        canvasToDisable.gameObject.SetActive(false);
    }
}
