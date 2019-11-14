using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HQController : ClickableUnit
{
    public event Action<int> OnMineralGain = delegate { };

    public static ResourceData Minerals = null;

    [Header("Required")]
    [SerializeField] MineralGroup _mineralGroup = null;

    [Header("Settings")]
    [SerializeField] ResourceData _mineralsData = null;

    private void Awake()
    {
        if(Minerals == null)
        {
            Minerals = _mineralsData;
        }
    }


    protected override void InteractWithObject(GameObject target)
    {
        Debug.Log("Not Implemented in HQController");
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger Enter");
        MinerController miner = other.GetComponent<MinerController>();
        if (miner.HoldingResources > 0)
        {
            Debug.Log("Miner with Resources");
            //switch(miner.resourceEnum //case minerals
            Minerals.currentResource += miner.HoldingResources;
            OnMineralGain?.Invoke(miner.HoldingResources);
            Minerals.CallUpdate();

            miner.DepositResources(_mineralGroup);
        }
    }
}
