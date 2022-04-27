﻿using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
{
    [SerializeField] private List<TKey> keys = new List<TKey>();
    [SerializeField] private List<TValue> values = new List<TValue>();
    
    public void OnBeforeSerialize()
    {
        keys.Clear();
        values.Clear();

        foreach (KeyValuePair<TKey, TValue> pair in this)
        {
            keys.Add(pair.Key);
            values.Add(pair.Value);
        }
    }

    public void OnAfterDeserialize()
    {
        this.Clear();

        if (keys.Count != values.Count)
        {
            Debug.Log("Error when attempting to deserialize SerializableDictionary. " +
                      "Amount of keys ("+keys.Count+") " +
                      "does not match amount of values ("+values.Count+")");
        }

        for (int i = 0; i < keys.Count; i++)
        {
            this.Add(keys[i], values[i]);
        }        
    }
}
