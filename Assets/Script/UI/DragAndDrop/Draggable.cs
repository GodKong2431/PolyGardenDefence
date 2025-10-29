using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private TowerType _selectTowerType;
    private Canvas _canvas;
    private RectTransform _rectTransform;
    private CanvasGroup _canvasGroup;

    private Vector2 _originalPosition;



    public TowerType SelectTowerType
    {
        get { return _selectTowerType; }
    }









    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        // 자신의 위치, 그룹, 캔버스를 전역 변수로 저장
        // 캔버스 그룹은 투명도와 드래그 중 다른 UI와의 상호작용을 막기 위해서
        _rectTransform = GetComponent<RectTransform>();
        _canvasGroup = GetComponent<CanvasGroup>();
        _canvas = GetComponentInParent<Canvas>();
    }

    // 드래그가 시작될 때
    public void OnBeginDrag(PointerEventData eventData)
    {
        // 기존 포지션을 자신의 앵커 포지션으로 저장
        _originalPosition = _rectTransform.anchoredPosition;

        // 캔버스 그룹이 있다면
        // 상호작용을 막고 투명도를 0.8로 변경
        if (_canvasGroup != null)
        {
            _canvasGroup.blocksRaycasts = false;
            _canvasGroup.alpha = 0.5f;
        }
    }

    // 드래그 중일 때
    public void OnDrag(PointerEventData eventData)
    {
        // 자신의 앵커 포지션을 이전 프레임 대비 움직임을 더해줌
        // eventData.delta는 이전 프레임 대비 움직인양
        // _canvas.scaleFactor는 다른 해상도에서도 같은 양을 움직이게 나눠 준다.
        _rectTransform.anchoredPosition += eventData.delta / _canvas.scaleFactor;
    }

    // 드래그가 끝났을 떄
    public void OnEndDrag(PointerEventData eventData)
    {
        // 드랍이 끝났으면 자신의 위치로 복귀
        _rectTransform.anchoredPosition = _originalPosition;
        // 그룹이 있다면 상호작용 및 투명도 원상복구
        if (_canvasGroup != null)
        {
            _canvasGroup.blocksRaycasts = true;
            _canvasGroup.alpha = 1f;
        }

        // 마우스 포지션에서 레이를 발사하고
        // 맞은 오브젝트의 DropZone을 가져와 null이 아니라면
        // Tower이미지가 드랍되었을 때 실행될 메서드를 실행
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            DropZone dropZone = hit.collider.GetComponent<DropZone>();
            if (dropZone != null)
            {
                dropZone.OnTowerImageDrop(this); // 원하는 UI가 드롭됨
            }
        }
    }
}
