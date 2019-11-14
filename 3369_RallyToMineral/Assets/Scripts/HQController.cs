using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HQController : ClickableUnit
{
    public event Action<int> OnMineralGain = delegate { };

    private int _minerals = 50;
    private int _gas = 0;

    [SerializeField] MineralGroup _mineralGroup = null;


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
            _minerals += miner.HoldingResources;
            OnMineralGain?.Invoke(miner.HoldingResources);

            miner.DepositResources(_mineralGroup);
        }
    }
}
