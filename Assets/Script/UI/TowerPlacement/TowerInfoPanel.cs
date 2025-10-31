using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class TowerInfoPanelController : MonoBehaviour
{
    [Header("Panel")]
    [SerializeField] private RectTransform _panel;

    [Header("InfoText")]
    [SerializeField] private TextMeshProUGUI _infoText;

    [Header("Move")]
    [SerializeField] private float _moveDistance = 100f;
    [SerializeField] private float _duration = 0.3f;

    private Vector2 _originalPos;
    private Coroutine _currentCoroutine;

    private void Awake()
    {
        //기존 포시젼을 앵커포지션으로 만들고 비활성화
        _originalPos = _panel.anchoredPosition;
        _panel.gameObject.SetActive(false);
    }

    //텍스트를 받고 코루틴 시작 메서드
    public void ShowPanel(string text)
    {
        //정해진 텍스트 넣고
        _infoText.text = text.ToString();

        //저장한 코루틴이 있다면 파괴
        if (_currentCoroutine != null)
            StopCoroutine(_currentCoroutine);

        //패널을 활성화하고
        //코루틴 실행하고 저장
        _panel.gameObject.SetActive(true);
        _currentCoroutine = StartCoroutine(SlidePanel(_originalPos + new Vector2(0, _moveDistance), _originalPos, true));
    }


    //패널 숨기는 메서드
    public void HidePanel()
    {
        //저장한 코루틴이 있다면 파괴
        if (_currentCoroutine != null)
            StopCoroutine(_currentCoroutine);

        //코루틴 실행하고 저장
        _currentCoroutine = StartCoroutine(SlidePanel(_originalPos, _originalPos + new Vector2(0, _moveDistance), false));
    }

    //시작, 끝, 열때 호출한건지 닫을 때 호출한건지 파악
    private IEnumerator SlidePanel(Vector2 start, Vector2 end, bool showing)
    {
        //경과 시간을 0으로 만듬
        //패널의 앵커포지션을 시작으로
        float elapsed = 0f;
        _panel.anchoredPosition = start;

        //경과 시간이 정해진 시간보다 낮으면 반복
        while (elapsed < _duration)
        {
            //경과 시간에 프레임당 시간을 더함
            elapsed += Time.deltaTime;

            //경과 시간을 지속시간으로 나눈 값을 0과1사이의 값으로 제한
            //패널의 앵커포지션을 프레임마다 lerp로 부드럽게 이동
            float t = Mathf.Clamp01(elapsed / _duration);
            _panel.anchoredPosition = Vector2.Lerp(start, end, t);
            yield return null;
        }

        //패널의 앵커 포지션을 end로 변경
        _panel.anchoredPosition = end;

        //닫을때 호출했다면 비활성화
        if (!showing)
            _panel.gameObject.SetActive(false);
    }
}
