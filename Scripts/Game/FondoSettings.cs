using UnityEngine;
using UnityEngine.UI;

public class FondoSettings : MonoBehaviour
{
    [SerializeField] public Material fondoMaterial; // Asigna el material del plano en el inspector.
    [SerializeField] public Slider translucencySlider; // Slider para modificar la translucidez.
    [SerializeField] public Image handleImage;

    private void Start()
    {
        float translucency = PlayerPrefs.GetFloat("translucencyLevel", 0.25f);
        translucencySlider.value = translucency;
        SetTranslucencyLevel(translucency);
        translucencySlider.onValueChanged.AddListener(delegate { SetTranslucencyLevel(translucencySlider.value); });
        UpdateHandleImage(translucency);
    }

    public void SetTranslucencyLevel(float value)
    {
        Color color = fondoMaterial.color;
        color.a = value; // Modificar la transparencia del material.
        fondoMaterial.color = color;
        UpdateHandleImage(value);
    }

    public void GuardarPreferencias()
    {
        PlayerPrefs.SetFloat("translucencyLevel", translucencySlider.value);
        PlayerPrefs.Save();
    }

    private void UpdateHandleImage(float translucency)
    {
        if (translucency == 0)
        {
            handleImage.sprite = Resources.Load<Sprite>("Botones_ajustes/translucencyOff");
        }
        else
        {
            handleImage.sprite = Resources.Load<Sprite>("Botones_ajustes/translucencyOn");
        }
    }
}
