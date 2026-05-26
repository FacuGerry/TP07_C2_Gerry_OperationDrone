using UnityEngine;

[CreateAssetMenu(fileName = "KeyBindings", menuName = "GameSettings/KeyBindings")]

public class KeyBindingsSO : ScriptableObject
{
    [Header("Movement")]
    public KeyCode forward;
    public KeyCode left;
    public KeyCode backward;
    public KeyCode right;
    public KeyCode up;
    public KeyCode down;
    [Header("Camera Control")]
    public KeyCode changePOV;
    [Header("Pause")]
    // Suggestion: en vez de pause/pause2/pause3 usar KeyCode[] pauseKeys para soportar N teclas sin agregar campos.
    public KeyCode pause;
    public KeyCode pause2;
    public KeyCode pause3;
    [Header("Shoot")]
    public KeyCode shoot;
    public KeyCode secondShoot;
    public KeyCode showCheat;
}
