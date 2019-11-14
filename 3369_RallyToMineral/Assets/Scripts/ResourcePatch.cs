using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcePatch : MonoBehaviour
{
    [Header("Required")]
    [SerializeField] MineralGroup _group = null;
    private List<MinerController> _miningQueue = new List<MinerController>();
    public int MiningQueue { get { return _miningQueue.Count; } }
    public bool _activeMining = false;

    private Vector3 _closePoint = Vector3.zero;
    public Vector3 MiningPoint { get { return _closePoint; } }
    private HQController _nearestHQ = null;
    public HQController NearestHQ { get { return _nearestHQ; } }

    [Header("Settings")]
    [SerializeField] int _resourceValue = 5;

    private void Awake()
    {
        CheckForNearHQ();
    }

    private void Start()
    {
        StartCoroutine(PeriodicMiningCheck());
    }

    private void CheckForNearHQ()
    {
        HQController[] allHQ = FindObjectsOfType<HQController>();
        float minDist = Mathf.Infinity;
        foreach (HQController hq in allHQ)
        {
            float dist = (hq.transform.position - transform.position).magnitude;
            if (dist < minDist)
            {
                _nearestHQ = hq;
            }
        }

        transform.LookAt(_nearestHQ.transform);
        _closePoint = transform.position + transform.forward;
    }

    public void AddToQueue(MinerController scv)
    {
        if(_miningQueue.Count <= _group.AverageQueue)   //if we are relatively empty compared to the mineral line
        {
            Debug.Log("Added Miner to Queue");
            _miningQueue.Add(scv);
        }
        else
        {
            Debug.Log("Finding a Neighbor");
            _group.FindNeighborPatch(scv);
        }
    }

    public void RemoveFromQueue(MinerController scv)
    {
        foreach(MinerController miner in _miningQueue)
        {
            if(miner == scv)
            {
                Debug.Log("Remove SCV from Queue");
                _miningQueue.Remove(scv);
            }
        }
    }

    IEnumerator PeriodicMiningCheck()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.2f);
            UpdateMiningQueue();
        }
    }

    public void StopMining()
    {
        _activeMining = false;
        UpdateMiningQueue();
    }

    private void UpdateMiningQueue()
    {

        if (_miningQueue.Count <= 0)    //do not update queue if no miners in queue
            return;

        if (_activeMining)  //do not update queue if we are currently mining
            return;

        foreach (MinerController miner in _miningQueue) //find a miner
        {
            if ((miner.transform.position - _closePoint).magnitude > 1f)    //skip miners that are too far
                continue;

            miner.MineMinerals(_resourceValue); //tell that miner to mine
            _activeMining = true;

            break;  //ends foreach, does not end Routine
        }
    }
}
