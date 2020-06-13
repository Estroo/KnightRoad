using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PauseMenuText : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Text theText;
    private Color newColor = new Color(140f / 255f, 140f / 255f, 140f / 255f);
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        theText.color = newColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        theText.color = Color.white;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        theText.color = Color.white;
    }
}
