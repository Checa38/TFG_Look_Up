using UnityEngine;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    public bool efectos;
    [SerializeField] private AudioSource myAudioSource;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Image handleImage;

    private void Start()
    {
        float volume = 1.0f;
        if (efectos)
        {
            volume = PlayerPrefs.GetFloat("volumenEfectos", 1.0f);
        }
        else
        {
            volume = PlayerPrefs.GetFloat("volumenMusica", 1.0f);
        }
        musicSlider.value = volume;
        myAudioSource.volume = volume;
        musicSlider.onValueChanged.AddListener(delegate { SetMusicVolume(); });
        UpdateHandleImage(volume);
    }

    public void SetMusicVolume()
    {
        float volume = musicSlider.value;
        myAudioSource.volume = volume;
        UpdateHandleImage(volume);
    }

    public void GuardarPreferencias()
    {
        if (efectos)
        {
            PlayerPrefs.SetFloat("volumenEfectos", musicSlider.value);
            PlayerPrefs.Save();
        }
        else
        {
            PlayerPrefs.SetFloat("volumenMusica", musicSlider.value);
            PlayerPrefs.Save();
        }
    }

    private void UpdateHandleImage(float volume)
    {
        if (volume == 0)
        {
            if (efectos)
            {
                handleImage.sprite = Resources.Load<Sprite>("Botones_ajustes/sonidoMuted");
            }
            else
            {
                handleImage.sprite = Resources.Load<Sprite>("Botones_ajustes/musicMuted");
            }
        }
        else
        {
            if (efectos)
            {
                handleImage.sprite = Resources.Load<Sprite>("Botones_ajustes/sonidoOn");
            }
            else
            {
                handleImage.sprite = Resources.Load<Sprite>("Botones_ajustes/musicOn");
            }
        }
    }
}
