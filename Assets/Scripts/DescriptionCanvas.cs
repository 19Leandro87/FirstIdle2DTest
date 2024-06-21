 using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DescriptionCanvas : MonoBehaviour, IPointerDownHandler
{
    public static DescriptionCanvas Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private Image descrBackground;
    private bool shown;

    private void Awake() { Instance = this; }

    public void TriggerEnabled() {
        if (!shown) {
            descrBackground.gameObject.SetActive(true);
            descriptionText.gameObject.SetActive(true);
            shown = true;
        } else {
            descrBackground.gameObject.SetActive(false);
            descriptionText.gameObject.SetActive(false);
            shown = false;
        }
    }

    public void OnPointerDown(PointerEventData eventData) { TriggerEnabled(); }

    public void SetDescription(string description) { descriptionText.text = description; }
}
