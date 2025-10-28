using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ExpandUI : MonoBehaviour
{
    [SerializeField] private RectTransform _panel; // 펼쳐질 UI
    [SerializeField] private float _expandHeight = 200f; // 펼쳐졌을 때 높이
    [SerializeField] private float _speed = 5f; // 펼쳐지는 속도
    [SerializeField] private float _stayTime = 2f; // 펼쳐진 상태 유지 시간

    private bool _isAnimating = false;

    private void Start()
    {
        StartCoroutine(ExpandAndCollapse());
    }

    public void OnButtonClick()
    {
        _isAnimating = !_isAnimating;
    }

    private IEnumerator ExpandAndCollapse()
    {
        _isAnimating = true;

        Vector2 closedSize = new Vector2(_panel.sizeDelta.x, 0f);
        Vector2 openedSize = new Vector2(_panel.sizeDelta.x, _expandHeight);

        // 펼치기
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * _speed;
            _panel.sizeDelta = Vector2.Lerp(closedSize, openedSize, t);
            yield return null;
        }

        // 유지
        //yield return new WaitUntil(condition);

        // 접기
        t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * _speed;
            _panel.sizeDelta = Vector2.Lerp(openedSize, closedSize, t);
            yield return null;
        }

        _isAnimating = false;
        //yield return new WaitWhile(condition)
    }
}
