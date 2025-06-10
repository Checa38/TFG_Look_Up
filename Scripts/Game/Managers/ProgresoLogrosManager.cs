using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
public class ProgresoLogrosManager : MonoBehaviour
{
    private GameObject logrosObjeto;
    private GameObject panelPrefabBiblioteca; // Prefab del Panel
    private Transform parentElementBiblioteca; 
    public Web webScript;

    
    void Start()
    {
         GameObject mainObject = GameObject.Find("Main");

        if (mainObject != null)
        {
            webScript = mainObject.GetComponent<Web>();

            if (webScript != null)
            {
                //Debug.Log("Componente Web encontrado e inicializado correctamente.");
            }
            else
            {
                Debug.LogError("No se encontró el componente Web en el objeto Main.");
            }
        }
        else
        {
            Debug.LogError("No se encontró el objeto Main en la escena.");
        }

        // Initialize logrosObjeto by finding it in the scene
        DatosEntreEscenas.logros = GameObject.Find("Logros");

        // Initialize panelPrefabBiblioteca by loading it from the Resources folder
        panelPrefabBiblioteca = Resources.Load<GameObject>("Prefabs/PanelPrefab");

        // Initialize parentElementBiblioteca by finding the "Content" child inside logrosObjeto
        if (DatosEntreEscenas.logros != null)
        {
            parentElementBiblioteca = DatosEntreEscenas.logros.transform.Find("Scroll View/Viewport/Content");

            if (parentElementBiblioteca == null)
            {
                Debug.LogError("Content Transform not found!");
            }
        }
        else
        {
            Debug.LogError("Logros object not found!");
        }
    }
    // Diccionario que almacena los valores necesarios para considerar un logro como conseguido.
    static public Dictionary<string, int> valoresParaConseguirLogro = new Dictionary<string, int>()
    {
        { "biblio1", 5 },  
        { "biblio3", 25 },
        { "constdesc1", 10 },
        { "constdesc2", 30 },
        { "constdesc3", 60 },
        { "constdesc4", 88 },
        { "general4", 9 },
        { "general1", 1 },
        { "planet1", 1 },
        { "planet4", 8 },
        { "ubi2", 10 }
    };

    /*
    public void IncrementarProgreso(string nombreLogro)
    {
        // Buscar el logro en la lista de logros del jugador.
        infoLogros logro = DatosEntreEscenas.playerLogros.listaLogros.Find(l => l.name == nombreLogro);

        // Si el logro existe.
        if (logro != null && !logro.conseguido)
        {
            Debug.Log("Sumando 1 al progreso de " + nombreLogro + " de " + logro.progreso + " a ");
            // Sumar 1 al progreso.
            logro.progreso += 1;
            Debug.Log("A "+ logro.progreso);

            // Comprobar si se ha llegado al valor por el cual se considera conseguido.
            if (valoresParaConseguirLogro.ContainsKey(nombreLogro))
            {
                int valorParaConseguir = valoresParaConseguirLogro[nombreLogro];
                
                // Si el progreso alcanza el valor necesario.
                if (logro.progreso == valorParaConseguir)
                {
                    logro.conseguido = true;
                    Debug.Log("Logro conseguido: " + nombreLogro);
                    UpdateNotificacionLogro(nombreLogro);
                    NotificarLogro(nombreLogro);
                    CreatePanelsLogroNuevo(nombreLogro);
                }
            }
            else
            {
                Debug.LogWarning("El logro " + nombreLogro + " no tiene un valor definido para ser conseguido.");
            }
        }
        else
        {
            Debug.LogWarning("Logro ya conseguido o no encontrado: " + nombreLogro);
        }
    }*/

    public void IncrementarProgreso(string nombreLogro)
    {
        Debug.Log("VAMOS CON LOGRO " + nombreLogro);

        // Inicializar una variable para almacenar el resultado del callback
        bool logroConseguido = false;

        // Llamar a UpdateLogroProgreso y esperar hasta que finalice
        StartCoroutine(webScript.UpdateLogroProgreso(nombreLogro, (success) =>
        {
            logroConseguido = success; // Asignar el resultado del callback
        }));

        // Evaluar el resultado después de que la corutina haya finalizado
        if (DatosEntreEscenas.conseguido)
        {
            Debug.Log("ES TRUE QUE LOGROCONSEGUIDO");
            UpdateNotificacionLogro(nombreLogro);
            NotificarLogro(nombreLogro);
            CreatePanelsLogroNuevo(nombreLogro);
            DatosEntreEscenas.conseguido = false;
        }
        else
        {
            Debug.Log("El logro no se ha conseguido o la actualización falló.");
        }

        Debug.Log("Terminando incrementar progreso");
    }



    public void NotificarLogro(string nombreLogro)
    {

            // Instanciar el prefab de notificación del logro, especificando el padre si se proporciona

            if (DatosEntreEscenas.notificacionLogros != null)
            {
                DatosEntreEscenas.notificacionLogros.SetActive(true);
            }
            else
            {
                Debug.LogWarning("notificacionLogros es null.");
            }

            // Se puede agregar lógica adicional para personalizar la notificación
            // como mostrar el nombre del logro conseguido en el prefab, etc.
            Debug.Log("Notificación de logro instanciada: " + nombreLogro);
    }

    public void UpdateNotificacionLogro(string logroName)
    {
        // Acceder al GameObject GONotificacionLogro a través de la variable estática
        GameObject notificationObj = DatosEntreEscenas.notificacionLogros;

        if (notificationObj == null)
        {
            Debug.LogError("No se encontró el objeto GONotificacionLogro en DatosEntreEscenas.");
            return;
        }

        // Buscar el panel dentro del GameObject
        Transform panelTransform = notificationObj.transform.Find("NotificacionLogro/Panel");

        if (panelTransform == null)
        {
            Debug.LogError("No se encontró el Panel en GONotificacionLogro.");
            return;
        }

        // Modificar el texto de TextNombreLogro (asumiendo que es un TextMeshPro)
        TextMeshProUGUI textNombreLogro = panelTransform.Find("TextNombreLogro").GetComponent<TextMeshProUGUI>();
        if (textNombreLogro != null)
        {
            textNombreLogro.text = LocalizationSettings.StringDatabase.GetLocalizedString("logros", "nom" + logroName);
        }
        else
        {
            Debug.LogError("No se encontró el componente TextMeshProUGUI en TextNombreLogro.");
        }

        RawImage rawImage = panelTransform.Find("Image").GetComponent<RawImage>();
        if (rawImage != null)
        {
            // Cargar la imagen de los recursos
            var texture = Resources.Load<Texture2D>("Logros/" + logroName);
            if (texture != null)
            {
                rawImage.texture = texture;
            }
            else
            {
                Debug.LogWarning($"No se pudo cargar la imagen para {logroName}.");
            }
        }
        else
        {
            Debug.LogError("No se encontró el componente RawImage en el panel.");
        }
    }

    public void CreatePanelsLogroNuevo(string logroName)
    {

        GameObject panel = Instantiate(panelPrefabBiblioteca, parentElementBiblioteca);
           // panel.GetComponentInChildren<Text>().text = logroObj.name;
           // panel.GetChild(1).GetComponent<Text>()= logroObj.name;
           Text[] textsLogros = panel.GetComponentsInChildren<Text>();
           if (textsLogros.Length > 1)
            {
                // Acceder al primer texto
                textsLogros[0].text = LocalizationSettings.StringDatabase.GetLocalizedString("logros", "nom" + logroName);
                // Acceder al segundo texto
                textsLogros[1].text = LocalizationSettings.StringDatabase.GetLocalizedString("logros", logroName);
            }
            else
            {
                Debug.LogError("No se encontraron suficientes componentes Text en los hijos.");
            }

            var texture = Resources.Load<Texture2D>("Logros/" + logroName);
            RawImage rawImage = panel.GetComponentInChildren<RawImage>();
            if (rawImage != null)
            {
                if (texture != null)
                {
                    rawImage.texture = texture;
                }
                else
                {
                    Debug.LogWarning($"No se pudo cargar la imagen para {logroName} ");
                }
            }
            else
            {
                Debug.LogWarning("No se encontró el componente RawImage en el panel prefab.");
            }

            Button button = panel.GetComponent<Button>();
            if (button == null)
            {
                button = panel.AddComponent<Button>();
            }

        }

}
