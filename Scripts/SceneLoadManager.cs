using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SceneLoadManager : MonoBehaviour
{
    [SerializeField] private Slider loadbar;
    [SerializeField] private GameObject loadPanel;
    [SerializeField] private StellariumAPI stellariumAPI;  // Referencia al StellariumAPI
    [SerializeField] private Canvas canvasToDisable;  // Canvas a deshabilitar

    void Start()
    {
        StartCoroutine(LoadStellariumAPI());
    }

    IEnumerator LoadStellariumAPI()
    {
        // Primera fase: Avanzar la barra hasta el 50% en 5 segundos
        float duration = 1.0f;
        float targetProgress = 0.5f;
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            loadbar.value = Mathf.Lerp(0, targetProgress, elapsed / duration);
            yield return null;
        }

        // Segunda fase: Iniciar la carga del StellariumAPI
        yield return StartCoroutine(stellariumAPI.Inicio());

        targetProgress = 2.0f;
        elapsed = 0.0f;
         while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            loadbar.value = Mathf.Lerp(0.5f, targetProgress, elapsed / duration);
            yield return null;
        }

        // Deshabilitar el canvas
        canvasToDisable.gameObject.SetActive(false);
    }
}
