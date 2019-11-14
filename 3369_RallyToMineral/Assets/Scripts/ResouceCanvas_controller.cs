using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResouceCanvas_controller : MonoBehaviour
{
    [SerializeField] ResourceData _minerals = null;
    [SerializeField] Text _mineralsText = null;

    private void OnEnable()
    {
        _minerals.OnUpdateResource += UpdateMineralsUI;
    }

    private void OnDisable()
    {
        _minerals.OnUpdateResource -= UpdateMineralsUI;
    }

    void UpdateMineralsUI(int value)
    {
        _mineralsText.text = "Minerals: " + value;
    }
}
