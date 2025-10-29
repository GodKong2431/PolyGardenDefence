using UnityEngine;
using UnityEngine.EventSystems;

public class MouseOnImage : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("InfoText")]
    [SerializeField] private string _infoText;

    [Header("PanelController")]
    [SerializeField] private TowerInfoPanelController _panelController;

    //마우스가 올라갔을 때 실행
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log($"{gameObject.name} 에 마우스 올라감");
        _panelController.ShowPanel(_infoText);
    }

    //마우스가 내려갔을 때 실행
    public void OnPointerExit(PointerEventData eventData)
    {
        _panelController.HidePanel();
    }
}
