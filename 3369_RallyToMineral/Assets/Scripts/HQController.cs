using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(BuildingRally))]
public class HQController : ClickableUnit
{
    public event Action<int> OnMineralGain = delegate { };

    [SerializeField] MineralGroup _mineralGroup = null;

    public static ResourceData MineralData = null;
    [SerializeField] ResourceData _mineralData = null;

    [SerializeField] List<Production> _productionButtons = new List<Production>();

    private BuildingRally _rallyPoint = null;

    private void Awake()
    {
        if(MineralData == null)
        {
            MineralData = _mineralData;
        }

        _rallyPoint = GetComponent<BuildingRally>();
    }

    private void OnEnable()
    {
        foreach(Production button in _productionButtons)
        {
            button.OnProduce += ProduceUnit;
        }
    }

    void ProduceUnit(UnitData unit)
    {
        if (MineralData.currentResource < unit.Mineral) //&&GasData.curr > Unit.gas && SupplyData.curr + Unit.supply < maxSupply
            Debug.Log("Cannot Produce " + unit.name);   //not enough minerals VO
        else
        {
            Debug.Log("Producing " + unit.name);
            MineralData.Spend(unit.Mineral);
        }
    }

    protected override void InteractWithObject(GameObject target)
    {
        Debug.Log("Not Implemented in HQController");
        switch (target.tag)
        {
            case "Mineral":
                ChildInvokeNewLocation(target.transform.position + Vector3.down);
                break;
            default:
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger Enter");
        MinerController miner = other.GetComponent<MinerController>();
        if (miner.HoldingResources > 0)
        {
            Debug.Log("Miner with Resources");
            //switch(miner.resourceEnum //case minerals
            MineralData.currentResource += miner.HoldingResources;
            MineralData.CallUpdate();

            OnMineralGain?.Invoke(miner.HoldingResources);

            miner.DepositResources(_mineralGroup);
        }
    }
}
