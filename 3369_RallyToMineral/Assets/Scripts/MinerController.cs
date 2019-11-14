using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Unit_Motor))]
public class MinerController : ClickableUnit
{
    public event Action<ResourcePatch> OnMining = delegate { };

    private Unit_Motor _motor = null;

    [SerializeField] float _timeToMine = 2f;
    private bool _isMining = false;
    private Coroutine _miningRoutine = null;
    private ResourcePatch _minerals = null;
    private HQController _headquarters = null;

    private int _holdingResources = 0;
    public int HoldingResources { get { return _holdingResources; } }
    

    private void Awake()
    {
        _motor = GetComponent<Unit_Motor>();
    }

    private void OnEnable()
    {
        _motor.OnMove += StopMining;
    }

    protected override void InteractWithObject(GameObject target)
    {
        switch (target.tag)
        {
            case "Mineral":

                FindMinerals(target.transform.GetComponent<ResourcePatch>());
                break;

            case "Headquarters":

                _headquarters = target.transform.GetComponent<HQController>();
                ReturnResources(_headquarters);
                break;

            default:
                break;
        }
    }
    #region Mining Minerals
    public void FindMinerals(ResourcePatch mineral)
    {
        ChildInvokeNewLocation(mineral.MiningPoint);
        _minerals = mineral;
        mineral.AddToQueue(this);

        _headquarters = mineral.NearestHQ;

        OnMining?.Invoke(mineral);
    }

    public void MineMinerals(int resourceValue)
    {
        _miningRoutine = StartCoroutine(MiningMineralsDelay(resourceValue));
    }

    IEnumerator MiningMineralsDelay(int resourceValue)
    {
        _isMining = true;

        yield return new WaitForSeconds(_timeToMine);

        if (_isMining)
        {
            _holdingResources = resourceValue;
            InteractWithObject(_headquarters.gameObject);//once we move towards HQ, implicit StopMining from Motor.OnMove
            
        }
    }

    private void StopMining()
    {
        if (_isMining)
            _isMining = false;
        
        if (_minerals != null)
        {
            _minerals.RemoveFromQueue(this);
            _minerals.StopMining();
            _minerals = null;
        }
        //if _gas
    }
    #endregion

    #region HQ
    private void ReturnResources(HQController hq)
    {
        ChildInvokeNewLocation(_headquarters.transform.position);
    }

    public void DepositResources(MineralGroup group)
    {
        _holdingResources = 0;

        //switch resourcesEnum  //case minerals
        group.FindNeighborPatch(this);
    }
    #endregion
}
