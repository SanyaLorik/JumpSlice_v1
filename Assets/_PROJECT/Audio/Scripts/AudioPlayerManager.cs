using SanyaBeerExtension;
using UnityEngine;
using UnityEngine.UI;

public class AudioPlayerManager : MonoBehaviour
{
    [Header("Button")]
    [SerializeField] private AudioSource _sourceButtonClick;

    private Button[] _buttons = null;

    [ContextMenu("AddClickButton")]
    private void AddClickButton()
    {
        _buttons?.ForEach(i => i.onClick.RemoveListener(OnButtonClick));

        _buttons = FindObjectsByType<Button>(FindObjectsInactive.Include);
        _buttons.ForEach(i => i.onClick.AddListener(OnButtonClick));
    }

    private void OnButtonClick()
    {
        _sourceButtonClick.Play();
    }
}