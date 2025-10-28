using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ExpandUI : MonoBehaviour
{
    [Header("MovePanel")]
    [SerializeField] private RectTransform _panel;

    [Header("ButtonImage")]
    [SerializeField] private Image _image;
    [SerializeField] private Sprite[] _buttonSprites;

    [Header("AnimSpeed")]
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _moveDistance = 200f;

    private bool _isOpen = false;
    private Coroutine _animCoroutine;

    private Vector2 _closedPos;
    private Vector2 _openedPos;

    private void Awake()
    {
        _openedPos = _panel.anchoredPosition;
        _closedPos = _openedPos + new Vector2(0, _moveDistance);
        _panel.anchoredPosition = _closedPos;
    }

    public void OnButtonClick()
    {
        _isOpen = !_isOpen;


        _image.sprite = _buttonSprites[_isOpen ? 1 : 0];

        if (_animCoroutine != null)
            StopCoroutine(_animCoroutine);

        _animCoroutine = StartCoroutine(AnimatePanel(_isOpen));
    }

    private IEnumerator AnimatePanel(bool open)
    {
        float t = 0f;
        Vector2 start = _panel.anchoredPosition;
        Vector2 target = open ? _openedPos : _closedPos;

        while (t < 1f)
        {
            t += Time.deltaTime * _speed;
            _panel.anchoredPosition = Vector2.Lerp(start, target, t);
            yield return null;
        }

        _panel.anchoredPosition = target;
        _animCoroutine = null;
    }
}
