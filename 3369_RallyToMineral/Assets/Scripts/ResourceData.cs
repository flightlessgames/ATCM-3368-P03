using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "NewResource", menuName = "Resource/Collectible")]
public class ResourceData : ScriptableObject
{
    public event Action<int> OnUpdateResource = delegate { };

    [SerializeField] int initialResource = 0;
    
    [NonSerialized] public int currentResource;

    private void OnEnable()
    {
        currentResource = initialResource;
        CallUpdate();
    }

    public void Spend(int cost)
    {
        currentResource -= cost;
        CallUpdate();
    }

    public void CallUpdate()
    {
        OnUpdateResource?.Invoke(currentResource);
    }
}
