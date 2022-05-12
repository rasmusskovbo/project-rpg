using System;
using UnityEngine;

[Serializable]
public class TooltipInfo
{
    [SerializeField] private string title;
    [SerializeField] private string subtitle;
    [SerializeField][TextArea] private string body;

    public TooltipInfo(string title, string subtitle, string body)
    {
        this.title = title;
        this.subtitle = subtitle;
        this.body = body;
    }

    public string Title
    {
        get => title;
        set => title = value;
    }

    public string Subtitle
    {
        get => subtitle;
        set => subtitle = value;
    }

    public string Body
    {
        get => body;
        set => body = value;
    }
}
