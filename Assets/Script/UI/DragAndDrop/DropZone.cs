using UnityEngine;
using UnityEngine.EventSystems;

public class DropZone : MonoBehaviour
{
    // 원하는 UI가 드롭되면 실행되는 메서드
    public void OnTowerImageDrop(Draggable droppedUI)
    {
        Debug.Log($"{gameObject.name}에 {droppedUI.name}이 드롭됨!");



        //메서드 실행
        //UpgradeManager.Placement();
    }
}
