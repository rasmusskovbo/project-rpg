using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Dialogue
{
    [SerializeField] private List<string> lines;

    public List<string> Lines
    {
        get => lines;
        set => lines = value;
    }
}
