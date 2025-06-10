using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using System;
public class BibliotecaManager : MonoBehaviour
{
    private TextMeshProUGUI titleText;
    private TextMeshProUGUI descriptionText;
    private TextMeshProUGUI fechaYDatosText;
    public GameObject content; 
    public RawImage rawImageComponent;
    public GameObject bibliotecaObject; 
    public GameObject bibliotecaDetallesObject; 
    public AudioSource efectoSonido;

    void Start()
    {
        titleText = content.transform.Find("Title").GetComponent<TextMeshProUGUI>();
        descriptionText = content.transform.Find("Descripcion").GetComponent<TextMeshProUGUI>();
        fechaYDatosText = content.transform.Find("FechaDatos").GetComponent<TextMeshProUGUI>();
    }
    public GameObject panelPrefabBiblioteca; // Prefab del Panel
    public Transform parentElementBiblioteca; // Elemento padre bajo el cual se crearán los paneles

    /*public IEnumerator CreatePanelsBiblioteca()
    {
        foreach (infoCuerpoProgreso objetoCielo in DatosEntreEscenas.playerProgress.discovered_constellations)
        {
            //if (!objetoCielo.discovered)
            //    continue;

            GameObject panel = Instantiate(panelPrefabBiblioteca, parentElementBiblioteca);
            panel.GetComponentInChildren<Text>().text = objetoCielo.name;
            //panel.GetChild(1).GetComponent<Text>()= objetoCielo.name;
            var texture = Resources.Load<Texture2D>("Cielo_ilustraciones/" + objetoCielo.name);
            RawImage rawImage = panel.GetComponentInChildren<RawImage>();
            if (rawImage != null)
            {
                if (texture != null)
                {
                    rawImage.texture = texture;
                }
                else
                {
                    Debug.LogWarning($"No se pudo cargar la imagen para {objetoCielo.name} ");
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

            button.onClick.AddListener(() => {
                StartCoroutine(SetContent(objetoCielo.name, objetoCielo.location, objetoCielo.date, objetoCielo.azimuth, objetoCielo.altitude));
                Texture2D newTexture = Resources.Load<Texture2D>("Ilustraciones_png/" + objetoCielo.name);
                if (newTexture != null)
                {
                    rawImageComponent.texture = newTexture;
                }
                else
                {
                    Debug.LogWarning("Texture not found in Resources/Ilustraciones_png/" + objetoCielo.name);
                }

                bibliotecaObject.SetActive(false);
                bibliotecaDetallesObject.SetActive(true);
            });
        }
        yield return null;
    }*/

    public IEnumerator CreatePanelsBiblioteca()
    {
        foreach (infoCuerpoProgreso objetoCielo in DatosEntreEscenas.playerProgress.discovered_constellations)
        {
            //Debug.Log("Creando un panel"+ objetoCielo.name);
            yield return StartCoroutine(CrearPanelIndividual(objetoCielo));
        }
        yield return null;
    }

    public IEnumerator CrearPanelIndividual(infoCuerpoProgreso objetoCielo)
    {
        GameObject panel = Instantiate(panelPrefabBiblioteca, parentElementBiblioteca);
        panel.GetComponentInChildren<Text>().text = objetoCielo.name;
        
        var texture = Resources.Load<Texture2D>("Cielo_ilustraciones/" + objetoCielo.name);
        RawImage rawImage = panel.GetComponentInChildren<RawImage>();
        if (rawImage != null)
        {
            if (texture != null)
            {
                rawImage.texture = texture;
            }
            else
            {
                Debug.LogWarning($"No se pudo cargar la imagen para {objetoCielo.name} ");
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

        button.onClick.AddListener(() => {
            efectoSonido.Play();
            StartCoroutine(SetContent(objetoCielo.name, objetoCielo.location, objetoCielo.date, objetoCielo.azimuth, objetoCielo.altitude));
            Texture2D newTexture = Resources.Load<Texture2D>("Ilustraciones_png/" + objetoCielo.name);
            if (newTexture != null)
            {
                rawImageComponent.texture = newTexture;
            }
            else
            {
                rawImageComponent.texture = Resources.Load<Texture2D>("Ilustraciones_png/noconstelacion");
                Debug.LogWarning("Texture not found in Resources/Ilustraciones_png/" + objetoCielo.name);
            }

            bibliotecaObject.SetActive(false);
            bibliotecaDetallesObject.SetActive(true);
        });
        yield return null;
    }

    public IEnumerator SetContent(string title, string location, string fecha, float azimuth, float altitude)
    {
        if (titleText != null && descriptionText != null)
        {
            titleText.text = title;
            descriptionText.text = LocalizationSettings.StringDatabase.GetLocalizedString("biblioteca", title);
            fechaYDatosText.text = location + " " + fecha + " Azimuth " + azimuth + " Altitude " + altitude;
        }
        else
        {
            Debug.LogError("Title or Description Text component not found!");
        }
        yield return null;
    }
}
