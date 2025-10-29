using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseOnImage : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("InfoText")]
    [SerializeField] private TextMeshProUGUI _ChangeText;
    [SerializeField] private List<string> _TowerInfoText = new List<string>();

    [Header("PanelController")]
    [SerializeField] private TowerInfoPanelController _panelController;

    //마우스가 올라갔을 때 실행
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log($"{gameObject.name} 에 마우스 올라감");
        _ChangeText.text = $"<size=33>{_TowerInfoText[0]}\t{_TowerInfoText[1]}</size>\n<size=27>{_TowerInfoText[2]}</size>";
        _panelController.ShowPanel(_ChangeText.text);
    }

    //마우스가 내려갔을 때 실행
    public void OnPointerExit(PointerEventData eventData)
    {
        _panelController.HidePanel();
    }
}
