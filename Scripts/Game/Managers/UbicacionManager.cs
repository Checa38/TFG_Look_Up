using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Localization;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
public class UbicacionManager : MonoBehaviour
{
    [SerializeField] private SceneLoadManager sceneLoadManager; 
    public Button botonSi;
    public Button botonNo;
    public GameObject textoPrefabCiudades; // Prefab del Panel
    public Transform parentElementCiudades; // Elemento padre bajo el cual se crearán los paneles
    public TextMeshProUGUI ubicacionText;
    public GameObject textoPlaceHolder; 
    private List<CiudadData> listaCiudades = new List<CiudadData>();
    void Start ()
    {
        ubicacionText.text = DatosEntreEscenas.ubicacion;

    }

    public IEnumerator CreatePanelsCiudades()
    {
        TextAsset csvData = Resources.Load<TextAsset>("Datos/worldcitiesLookUp");
        if (csvData == null)
        {
            Debug.LogError("No se pudo encontrar el archivo CSV en la carpeta Resources.");
            yield return null;
        }

        // Divide el contenido del archivo en líneas
        string[] rows = csvData.text.Split(new char[] { '\n' });

        // Instanciar el prefab y obtener la referencia al objeto instanciado
        /* GameObject instantiatedObject = Instantiate(textoPrefabCiudades, parentElementCiudades);

        // Obtener el componente TextMeshPro del objeto instanciado
        TextMeshProUGUI textMeshPro = instantiatedObject.GetComponentInChildren<TextMeshProUGUI>();
         // Cambiar el texto del componente TextMeshPro
       if (textMeshPro != null)
        {
            textMeshPro.text = "Busca y selecciona la ubicación que desees";
        }
        else
        {
            Debug.LogError("No se encontró un componente TextMeshProUGUI en el objeto instanciado.");
        }*/

        Debug.Log("Todos...");
        for (int i = 1; i < rows.Length-1; i++)
        {
            string row = rows[i];
            string[] columns = row.Split(new char[] { ';' });
            
            if (columns.Length == 4)
            {
                string city = columns[0].Trim();
                string latIt = columns[1].Trim();
                float latItFloat = float.Parse(latIt);
                string lngIt = columns[2].Trim();
                float lngItFloat = float.Parse(lngIt);
                string pais = columns[3].Trim();

                listaCiudades.Add(new CiudadData { name = city, country = pais, lat = latItFloat, lng = lngItFloat});
            }
            else
            {
                Debug.LogWarning($"La línea {i} no tiene el número correcto de columnas: {row}");
            }
        }
        yield return null;
    }

    public TMP_InputField ciudadBuscar;
    private List<CiudadData> ciudadesFiltradas = new List<CiudadData>();
    public GameObject panelMsgConfirmacion;
    public TextMeshProUGUI textoConfirmacion;
    public void BuscarCiudad()
    {
        // Eliminar espacios sobrantes al inicio y al final de la cadena de búsqueda
        string busqueda = ciudadBuscar.text.Trim();

        if (!string.IsNullOrEmpty(busqueda))
        {
            // Filtrar la lista de ciudades
            ciudadesFiltradas = listaCiudades
                .Where(c => c.name.StartsWith(busqueda, System.StringComparison.OrdinalIgnoreCase))
                .ToList();

            // Eliminar los hijos previos del elemento padre
            foreach (Transform child in parentElementCiudades)
            {
                Destroy(child.gameObject);
            }

            // Opcional: imprimir los nombres de las ciudades filtradas en la consola
            foreach (var ciudad in ciudadesFiltradas)
            {
                // Instanciar el prefab y obtener la referencia al objeto instanciado
                GameObject instantiatedObject = Instantiate(textoPrefabCiudades, parentElementCiudades);

                // Obtener el componente TextMeshPro del objeto instanciado
                TextMeshProUGUI textMeshPro = instantiatedObject.GetComponentInChildren<TextMeshProUGUI>();

                // Cambiar el texto del componente TextMeshPro
                if (textMeshPro != null)
                {
                    textMeshPro.text = $"{ciudad.name}, {ciudad.country}, Lat: {ciudad.lat}, Lng: {ciudad.lng}";
                }
                else
                {
                    Debug.LogError("No se encontró un componente TextMeshProUGUI en el objeto instanciado.");
                }

                // Obtener el botón del objeto instanciado
                Button button = instantiatedObject.GetComponentInChildren<Button>();
                if (button != null)
                {
                    button.onClick.AddListener(() => {
                        nameObjetivo = ciudad.name;
                        latitudObjetivo = ciudad.lat;
                        longitudObjetivo = ciudad.lng;
                        panelMsgConfirmacion.SetActive(true);
                        textoConfirmacion.text = $"{ciudad.name}, {ciudad.country}?";
                    });
                }
                else
                {
                    Debug.LogError("No se encontró un componente Button en el objeto instanciado.");
                }

                Debug.Log($"{ciudad.name}, {ciudad.country}, Lat: {ciudad.lat}, Lng: {ciudad.lng}");
                Debug.Log(ciudad.name);
            }
        }
    }

    private string nameObjetivo = "linares";
    private float longitudObjetivo = -3.6445510f;
    private float latitudObjetivo = 38.0973414f;
    public void CambiarDatosUbicacion()
    {
        Debug.Log("longitudObjetivo");
        Debug.Log(DatosEntreEscenas.longitud);
        DatosEntreEscenas.ubicacion = nameObjetivo;
        ubicacionText.text = nameObjetivo;
        DatosEntreEscenas.longitud = longitudObjetivo;
        DatosEntreEscenas.latitud = latitudObjetivo;
        Debug.Log("longitudObjetivo");
        Debug.Log(DatosEntreEscenas.longitud);
        

        PlayerPrefs.SetFloat("longitud", longitudObjetivo);
        PlayerPrefs.Save();
        PlayerPrefs.SetFloat("latitud", latitudObjetivo);
        PlayerPrefs.Save();
        PlayerPrefs.SetString("nombreUbi", nameObjetivo);
        PlayerPrefs.Save();
        PlayerPrefs.SetInt("changeUbi", 1);
        PlayerPrefs.Save();

        DatosEntreEscenas.cambiandoUbicacion = true;

        StartCoroutine(sceneLoadManager.LoadStellariumAPI());

    }
}
