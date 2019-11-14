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
    private ResourcePatch _mineralPatch = null;
    public ResourcePatch RecentResource { get { return _mineralPatch; } }
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
        Debug.Log("Interacting with " + target.transform.name);
        //switch cases: minerals, structure, default
        //strucures switch cases: command center, default

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
        Debug.Log("Found a Mineral Patch");

        ChildInvokeNewLocation(mineral.MiningPoint);
        _mineralPatch = mineral;
        mineral.AddToQueue(this);

        _headquarters = mineral.NearestHQ;

        OnMining?.Invoke(mineral);
    }

    public void MineMinerals(int resourceValue)
    {
        Debug.Log("Mining Minerals");
        _miningRoutine = StartCoroutine(MiningMineralsDelay(resourceValue));
    }

    IEnumerator MiningMineralsDelay(int resourceValue)
    {
        _isMining = true;

        yield return new WaitForSeconds(_timeToMine);
        Debug.Log("End of Mining Timer");

        if (_isMining)
        {
            Debug.Log("Completed Mining");
            _holdingResources = resourceValue;
            InteractWithObject(_headquarters.gameObject);//once we move towards HQ, implicit StopMining from Motor.OnMove
            
        }
    }

    private void StopMining()
    {
        Debug.Log("Stop Mining");
        if (_isMining)
            _isMining = false;
        
        if (_mineralPatch != null)
        {
            _mineralPatch.RemoveFromQueue(this);
            _mineralPatch.StopMining();
            _mineralPatch = null;
        }
        //if _gas
    }
    #endregion

    #region HQ
    private void ReturnResources(HQController hq)
    {
        if (_holdingResources <= 0) //TODO option to hide button in UI if HoldingResources <= 0
            return;

        Debug.Log("Returning to Base");
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
