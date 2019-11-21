using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class CancelProduction : MonoBehaviour
{
    public event Action OnCancel = delegate { };

    private Button _button = null;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(CancelCommand);
    }

    private void CancelCommand()
    {
        Debug.Log("cancel command");
        OnCancel?.Invoke();
    }
}
