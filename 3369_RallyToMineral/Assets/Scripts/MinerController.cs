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

    private void OnDisable()
    {
        _motor.OnMove -= StopMining;
    }

    protected override void InteractWithObject(GameObject target)
    {
        switch (target.tag)
        {
            case "Mineral":

                FindMinerals(target.transform.GetComponent<ResourcePatch>());
                break;

            case "Headquarters":
                if(_headquarters.gameObject != target)
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
        Transform returnPoint = null;
        float minDist = Mathf.Infinity;

        foreach (Transform HQDeposit in _headquarters.ResourceReturnPoints) //like ResoursePatch, searches for nearest POINT on HQ, using saved HQ from resource assignment.
        {
            float dist = (HQDeposit.position - transform.position).magnitude;
            if (dist < minDist)
            {
                returnPoint = HQDeposit;
                minDist = dist;
            }
        }
        ChildInvokeNewLocation(returnPoint.position);
    }

    public void DepositResources()    //odd name, but this is for when adjacent to HQ, causes "unload" resources and go find new resource to mine
    {
        _holdingResources = 0;

        FindMinerals(_minerals);    //goes towards saved mineralpatch, begins search routine if minerals are occupied
    }
    #endregion
}
