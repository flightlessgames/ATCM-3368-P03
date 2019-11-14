using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "NewResource", menuName = "Resource")]
public class ResourceData : ScriptableObject
{
    public event Action<int> UpdateResource = delegate { };

    public int initialResource;

    [NonSerialized]
    public int currentResource;

    public int maxumimResource = 10000;

    private void OnEnable()
    {
        currentResource = initialResource;
        CallUpdate();
    }

    public void CallUpdate()
    {
        UpdateResource?.Invoke(currentResource);
    }
}
