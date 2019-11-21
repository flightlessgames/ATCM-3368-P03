using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(BuildingRally))]
public class HQController : ClickableUnit   //HQController might be childed further, inherit from "building" but only 1 building in this project. do as P.S. work
{
    
    public event Action<int> OnMineralGain = delegate { };
    public event Action<List<UnitData>> OnUpdateQueue = delegate { }; //<generic<generic>> 
    public event Action OnUnitSpawned = delegate { };

    [Header("HQ - Required")]
    [SerializeField] MineralGroup _mineralGroup = null;
    [SerializeField] ResourceData _mineralData = null;
    [SerializeField] Transform[] _pointsGroup = new Transform[0];
    public Transform[] ResourceReturnPoints { get { return _pointsGroup; } }

    [SerializeField] List<Production> _productionButtons = new List<Production>();

    private BuildingRally _rallyPoint = null;
    private Transform _spawnPoint = null;
    private List<UnitData> _productionQueue = new List<UnitData>();
    /* Using LIST instead of QUEUE
     * Mostly functional as a Queue, one exception
     * Not able to remove the last/recent item (cancel production order)
     * Must be a LIST
     */
    private Coroutine _unitProduceRoutine = null;  //one routine to rule them all

    private void Awake()
    {
        _rallyPoint = GetComponent<BuildingRally>();
    }

    private void Start()
    {
        _spawnPoint = _pointsGroup[0];
        _mineralData.CallUpdate();
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
        if (_mineralData.currentResource < unit.Mineral)//&&GasData.curr > Unit.gas && SupplyData.curr + Unit.supply < maxSupply
        {
            Debug.Log("Cannot Produce " + unit.name);   //not enough minerals VO
            return;
        }

        if(_productionQueue.Count >= 5) //invalid return architecture   //we only queue up to 5
            return;
        
        Debug.Log("Producing " + unit.name);
        _mineralData.Spend(unit.Mineral);
        _productionQueue.Add(unit);

        OnUpdateQueue?.Invoke(_productionQueue);

        if(_unitProduceRoutine == null) //if we are not currently producing,
            _unitProduceRoutine = StartCoroutine(UnitProduceRoutine()); //produce
    }

    IEnumerator UnitProduceRoutine()
    {
        //first, asses the front of the queue
        UnitData unit = _productionQueue[0];

        //TODO ui timer, simulate UnitProductionRoutine
        yield return new WaitForSeconds(unit.Time);

        ClickableUnit unitObject = Instantiate(unit._unit.gameObject, _spawnPoint.position, _spawnPoint.rotation).GetComponent<ClickableUnit>(); //instantiate & asign
        OnUnitSpawned?.Invoke();

        Vector3 rayStart = _rallyPoint.RallyPoint + (Vector3.up * 10);  //to re-create the Auto-Move, we're hijacking the PlayerInput algorithm
        Debug.DrawRay(rayStart, Vector3.down * 10, Color.yellow, 1f);
        Debug.Log(rayStart);

        RaycastHit hit;

        if (Physics.Raycast(rayStart, Vector3.down, out hit, 10f))
        {
            if (hit.transform.gameObject != gameObject) //if rall is null, or is this, do not send a move command
                unitObject.Identify(hit);   //else, make the spawned unit move
        }

        _productionQueue.RemoveAt(0); //now it is safe to dequeue
        //ui event invoke unit produced

        if (_productionQueue.Count > 0)
            _unitProduceRoutine = StartCoroutine(UnitProduceRoutine());
        else
            _unitProduceRoutine = null;

        OnUpdateQueue?.Invoke(_productionQueue);
    }

    public void CancelQueue()
    {
        _productionQueue.RemoveAt(_productionQueue.Count - 1);//count -1 is last position
        if (_productionQueue.Count < 1)
            StopCoroutine(_unitProduceRoutine); //if queue get reset to 0, stop production ASAP

        //TODO ui event invoke unit cancelled (simulate through unit produced invoke?)
        OnUpdateQueue?.Invoke(_productionQueue);
    }

    protected override void InteractWithObject(GameObject target)
    {
        Debug.Log("Not Implemented in HQController");
        switch (target.tag)
        {
            case "Mineral":
                ChildInvokeNewLocation(target.transform.position);
                break;
            default:
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        MinerController miner = other.GetComponent<MinerController>();
        if (miner.HoldingResources > 0)
        {

            //switch(miner.resourceEnum //case minerals
            _mineralData.currentResource += miner.HoldingResources;
            _mineralData.CallUpdate();

            OnMineralGain?.Invoke(miner.HoldingResources);

            miner.DepositResources(_mineralGroup);
        }
    }
}
