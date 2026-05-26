using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private SoundDataSO _data;

    [SerializeField] private Slider _sliderMaster;
    [SerializeField] private Slider _sliderMusic;
    [SerializeField] private Slider _sliderSFX;

    private void Start()
    {
        _sliderMaster.value = _data.masterVol;
        _sliderMusic.value = _data.musicVol;
        _sliderSFX.value = _data.sfxVol;

        OnMasterChanged(_data.masterVol);
        OnMusicChanged(_data.musicVol);
        OnSFXChanged(_data.sfxVol);

        _sliderMaster.onValueChanged.AddListener(OnMasterChanged);
        _sliderMusic.onValueChanged.AddListener(OnMusicChanged);
        _sliderSFX.onValueChanged.AddListener(OnSFXChanged);
    }

    private void OnDestroy()
    {
        _sliderMaster.onValueChanged.RemoveAllListeners();
        _sliderMusic.onValueChanged.RemoveAllListeners();
        _sliderSFX.onValueChanged.RemoveAllListeners();
    }

    // Suggestion: extraer a const string para tenerlos centralizados.
    // Bug: Baja - cuando el slider está en 0, Mathf.Log10(0) = -Infinity. El AudioMixer puede tolerarlo
    // pero matemáticamente no es correcto. Solución: Clampear.
    public void OnMasterChanged(float vol)
    {
        _data.masterVol = vol;
        _data.mixer.SetFloat("MasterVol", Mathf.Log10(vol) * 20);
    }

    public void OnMusicChanged(float vol)
    {
        _data.musicVol = vol;
        _data.mixer.SetFloat("MusicVol", Mathf.Log10(vol) * 20);
    }

    public void OnSFXChanged(float vol)
    {
        _data.sfxVol = vol;
        _data.mixer.SetFloat("SFXVol", Mathf.Log10(vol) * 20);
    }

}
