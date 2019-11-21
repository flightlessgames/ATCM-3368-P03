using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

[RequireComponent(typeof(Button))]
public class Production : MonoBehaviour
{
    public event Action<UnitData> OnProduce = delegate { };

    [SerializeField] Button _button = null;
    [SerializeField] UnitData _productionUnit = null;

    private void Awake()
    {
        if (_button == null)
            _button = GetComponent<Button>();

        _button.onClick.AddListener(CallUnitProduction);
    }

    void CallUnitProduction()
    {
        OnProduce(_productionUnit);
    }
}
