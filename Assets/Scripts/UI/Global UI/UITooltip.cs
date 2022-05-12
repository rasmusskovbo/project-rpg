using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

//[ExecuteInEditMode()]
public class UITooltip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI subtitle;
    [SerializeField] private TextMeshProUGUI body;
    [SerializeField] private LayoutElement layoutElement;
    [SerializeField] private int characterWrapLimit;

    private void Start()
    {
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Application.isEditor) ResizePreferredSize();

        Vector2 position = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, position);

        float pivotX = (screenPoint.x / Screen.width);
        float pivotY = (screenPoint.y / Screen.height);
        
        this.GetComponent<RectTransform>().pivot = new Vector2(pivotX, pivotY);
        transform.position = position;
        
        
    }

    private void ResizePreferredSize()
    {
        int titleLength = title.text.Length;
        int subtitleLength = subtitle.text.Length;
        int bodyLength = body.text.Length;

        layoutElement.enabled =
            (titleLength > characterWrapLimit || subtitleLength > characterWrapLimit || bodyLength > characterWrapLimit);
    }

    public void SetText(string body, string subtitle = "", string title = "")
    {
        if (string.IsNullOrEmpty(title))
        {
            this.title.gameObject.SetActive(false);
        }
        else
        {
            this.title.gameObject.SetActive(true);
            this.title.text = title;
        }
        if (string.IsNullOrEmpty(subtitle))
        {
            this.subtitle.gameObject.SetActive(false);
        }
        else
        {
            this.subtitle.gameObject.SetActive(true);
            this.subtitle.text = subtitle;
        }

        this.body.text = body;
        
        ResizePreferredSize();
    }
}
