using UnityEngine;
using UnityEngine.UI;

public class KeepingVolume : MonoBehaviour
{
    
    public MainButton SO;

    public Slider MainVolume;

    public Slider MusicVolume;

    public Slider SfxandUIVolume;

    void Start()
    {
        MainVolume.value = SO.MainVolume;

        MusicVolume.value = SO.MusicVolume;

        SfxandUIVolume.value = SO.SfxandUIVolume;

        MainVolume.onValueChanged.AddListener((val) => SO.MainVolume = val);
        MusicVolume.onValueChanged.AddListener((val) => SO.MusicVolume = val);
        SfxandUIVolume.onValueChanged.AddListener((val) => SO.SfxandUIVolume = val);
    
    }
}
