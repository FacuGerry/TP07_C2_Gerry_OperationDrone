using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "SoundData", menuName = "GameSettings/SoundData")]

public class SoundDataSO : ScriptableObject
{
    // Warning: los volúmenes se mutan en runtime por SoundManager. Al ser un SO, los cambios persisten entre Play sessions
    // en el editor (puede ser deseado para "guardar settings", pero hay que tenerlo presente).
    public AudioMixer mixer;
    public float masterVol;
    public float musicVol;
    public float sfxVol;
}