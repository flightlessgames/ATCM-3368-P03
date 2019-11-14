using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceCanvas_Controller : MonoBehaviour
{
    public static ResourceData Minerals;

    [Header("Required")]
    [SerializeField] ResourceData _mineralsData = null;
    [SerializeField] Text _mineralsText = null;

    private void Awake()
    {
        if(Minerals == null)
        {
            Minerals = _mineralsData;
        }
    }

    private void OnEnable()
    {
        Minerals.UpdateResource += UpdateMineralsUI;
    }

    private void OnDisable()
    {
        Minerals.UpdateResource -= UpdateMineralsUI;
    }

    void UpdateMineralsUI(int newMinerals)
    {
        _mineralsText.text = "Minerals: " + newMinerals;

    }
}
