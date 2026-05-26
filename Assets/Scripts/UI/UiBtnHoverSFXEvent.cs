using System;
using UnityEngine;
using UnityEngine.EventSystems;

// Error: el nombre del archivo es "UiBtnHoverSFXEvent.cs" pero la clase se llama "UiButtonHoverSFXEvent".
public class UiButtonHoverSFXEvent : MonoBehaviour, IPointerEnterHandler
{
    public static event Action OnButtonHover;
    public static event Action OnButtonClick;
    // Suggestion: agregar "using UnityEngine.UI;" arriba y dejar el tipo como Button, sin fully qualified.
    private UnityEngine.UI.Button _btn;

    private void Awake()
    {
        _btn = GetComponent<UnityEngine.UI.Button>();
    }

    private void Start()
    {
        _btn.onClick.AddListener(ButtonClicked);
    }

    private void OnDestroy()
    {
        _btn.onClick.RemoveAllListeners();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnButtonHover?.Invoke();
    }

    public void ButtonClicked()
    {
        OnButtonClick?.Invoke();
    }
}