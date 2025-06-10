using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class CieloManager : MonoBehaviour
{
    public GameObject constelacionPrefab; // Prefab de las constelaciones
    public Transform parentElementConstelacion; // Elemento padre bajo el cual se crearán las constelaciones
    public Web webScript;
    public AudioSource AudioSuccess;
    public AudioSource AudioYaDescubierto;
    public GameObject PlanetaPrefab; // Elemento padre bajo el cual se crearán los planetas
    public GameObject dialogoDescubierto;
    private CanvasGroup canvasGroup;
    void Start()
    {
        canvasGroup = dialogoDescubierto.GetComponent<CanvasGroup>();
    }
    IEnumerator ShowDialogo()
    {
        // Activar el objeto y establecer su opacidad a 1
        dialogoDescubierto.SetActive(true);
        canvasGroup.alpha = 1;

        // Esperar 5 segundos
        yield return new WaitForSeconds(2);

        // Iniciar desvanecimiento
        float fadeDuration = 1.5f; // Duración del desvanecimiento en segundos
        float fadeSpeed = 1f / fadeDuration;
        while (canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= Time.deltaTime * fadeSpeed;
            yield return null;
        }

        // Desactivar el objeto después de que se desvanezca completamente
        dialogoDescubierto.SetActive(false);
    }
    public IEnumerator CreateObjetosCielo(List<CelestialObjectInfo> celestialObjects, BibliotecaManager bibliotecaManager)
    {

        foreach (CelestialObjectInfo objetoCielo in celestialObjects)
        {
            if (objetoCielo.objectType != "planeta")
            {
                GameObject spriteObject = Instantiate(constelacionPrefab, parentElementConstelacion);

                spriteObject.name = objetoCielo.name;

                SpriteRenderer spriteRenderer = spriteObject.GetComponent<SpriteRenderer>();

                if (spriteRenderer == null)
                {
                    Debug.LogWarning("El prefab no contiene un componente SpriteRenderer.");
                    continue;
                }

                var sprite = Resources.Load<Sprite>("Constelaciones_cielo/" + objetoCielo.name);
                if (sprite != null)
                {
                    spriteRenderer.sprite = sprite;
                }
                else
                {
                    Debug.LogWarning($"No se pudo cargar el sprite para {objetoCielo.name} ");
                }
                spriteRenderer.transform.localPosition = objetoCielo.unityPosition;

                if (objetoCielo.unityPosition.y < 2)
                {
                    /*Debug.Log("Menor que 5");
                    Debug.Log(objetoCielo.name);
                    Debug.Log(spriteRenderer.transform.localScale);*/
                    spriteRenderer.transform.localScale = new Vector3(0, 0, 0); 
                    
                }

                
                spriteRenderer.transform.LookAt(Camera.main.transform);

                TextMeshPro titleTransform = spriteObject.transform.Find("Title").GetComponent<TextMeshPro>();;
                if (titleTransform != null)
                {
                    titleTransform.text = objetoCielo.name;
                }
                else
                {
                    Debug.LogWarning("No se encontró el objeto hijo 'Title'.");
                }

                

                // Añadir el listener al botón
                Transform buttonTransform = spriteObject.transform.Find("Canvas/Button");
                if (buttonTransform != null)
                {
                    Button button = buttonTransform.GetComponent<Button>();
                    if (button != null)
                    {

                        //if (!objetoCielo.discovered)
                        //{
                            button.onClick.AddListener(() => {
                                infoCuerpoProgreso astro = new infoCuerpoProgreso
                                {
                                    name = objetoCielo.name,
                                    altitude = objetoCielo.altitude,
                                    azimuth = objetoCielo.azimuth,
                                    distance = objetoCielo.distance,
                                    location = DatosEntreEscenas.ubicacion,
                                    date = DatosEntreEscenas.horaDescargaDatos.ToString()
                                };

                                ProgresoLogrosManager progresoLogrosManager = button.GetComponent<ProgresoLogrosManager>();
                                if (progresoLogrosManager != null)
                                {       
                                    progresoLogrosManager.IncrementarProgreso("constDesc1");
                                    progresoLogrosManager.IncrementarProgreso("constDesc2");
                                    progresoLogrosManager.IncrementarProgreso("constDesc3");
                                    progresoLogrosManager.IncrementarProgreso("constDesc4");
                                }
                                else
                                {
                                    Debug.LogWarning("El componente 'ProgresoLogrosManager' no se encontró en el botón.");
                                }

                                StartCoroutine(webScript.UpdateGameData(astro, (success) => {
                                    if (success)
                                    {
                                        StartCoroutine(bibliotecaManager.CrearPanelIndividual(astro));
                                        AudioSuccess.Play();
                                    }
                                    else
                                    {
                                        if(!dialogoDescubierto.activeSelf)
                                        {
                                            StartCoroutine(ShowDialogo());
                                        }
                                        
                                        AudioYaDescubierto.Play();
                                    }
                                }));
                            });
                        //}
                        
                    }
                    else
                    {
                        Debug.LogWarning("El objeto 'Button' no contiene un componente Button.");
                    }
                }
                else
                {
                    Debug.LogWarning("No se encontró el objeto hijo 'Button'.");
                }
            }
            else // Planeta
            {
                GameObject object3D = Instantiate(PlanetaPrefab, parentElementConstelacion);
                object3D.name = objetoCielo.name;
                MeshRenderer meshRenderer = object3D.GetComponent<MeshRenderer>();

                if (meshRenderer == null)
                {
                    Debug.LogWarning("El prefab no contiene un componente MeshRenderer.");
                    continue;
                }

                // Cargar y asignar el material desde la carpeta Resources
                Material material = Resources.Load<Material>($"Materiales/{objetoCielo.name}");
                if (material != null)
                {
                    meshRenderer.material = material;
                }
                else
                {
                    Debug.LogWarning($"No se pudo cargar el material para {objetoCielo.name}");
                }

                // Posicionar el objeto 3D en la escena
                object3D.transform.localPosition = objetoCielo.unityPosition;

                // Escalar el objeto si está muy cerca de la cámara
                if (objetoCielo.unityPosition.y < 2)
                {
                    object3D.transform.localScale = new Vector3(0, 0, 0); 
                }

                // Hacer que el objeto 3D mire a la cámara
                object3D.transform.LookAt(Camera.main.transform);

                // Modificar el texto del título
                Transform titleTransform = object3D.transform.Find("ConstelacionPrefab/Title");
                if (titleTransform != null)
                {
                    TextMeshPro titleText = titleTransform.GetComponent<TextMeshPro>();
                    if (titleText != null)
                    {
                        titleText.text = objetoCielo.name;
                    }
                    else
                    {
                        Debug.LogError("No se encontró el componente TextMeshPro en el objeto Title.");
                    }
                }
                else
                {
                    Debug.LogError("No se encontró el objeto 'Title' en el objeto 3D del planeta.");
                }

                // Añadir el listener al botón
                Transform buttonTransform = object3D.transform.Find("ConstelacionPrefab/Canvas/Button");
                if (buttonTransform != null)
                {
                    Button button = buttonTransform.GetComponent<Button>();
                    if (button != null)
                    {
                        button.onClick.AddListener(() => {
                            infoCuerpoProgreso astro = new infoCuerpoProgreso
                            {
                                name = objetoCielo.name,
                                altitude = objetoCielo.altitude,
                                azimuth = objetoCielo.azimuth,
                                distance = objetoCielo.distance,
                                location = DatosEntreEscenas.ubicacion,
                                date = DatosEntreEscenas.horaDescargaDatos.ToString()
                            };

                            ProgresoLogrosManager progresoLogrosManager = button.GetComponent<ProgresoLogrosManager>();
                            if (progresoLogrosManager != null)
                            {       
                                progresoLogrosManager.IncrementarProgreso("planet1");
                                progresoLogrosManager.IncrementarProgreso("planet4");
                            }
                            else
                            {
                                Debug.LogWarning("El componente 'ProgresoLogrosManager' no se encontró en el botón.");
                            }

                            StartCoroutine(webScript.UpdateGameData(astro, (success) => {
                                if (success)
                                {
                                    StartCoroutine(bibliotecaManager.CrearPanelIndividual(astro));
                                    AudioSuccess.Play();
                                }
                                else
                                {
                                    if(!dialogoDescubierto.activeSelf)
                                    {
                                        StartCoroutine(ShowDialogo());
                                    }
                                    
                                    AudioYaDescubierto.Play();
                                }
                            }));
                        });
                    }
                    else
                    {
                        Debug.LogWarning("El objeto 'Button' no contiene un componente Button.");
                    }
                }
                else
                {
                    Debug.LogWarning("No se encontró el objeto hijo 'Button'.");
                }
            }



            
        }
        yield return null;

    }

    
}
