using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineralGroup : MonoBehaviour
{
    [SerializeField] List<ResourcePatch> _myMinerals = new List<ResourcePatch>();

    private int _averageQueue = 0;
    public int AverageQueue { get { return _averageQueue; } }

    private void Start()
    {
        StartCoroutine(CalculateAverageQueue());
    }

    IEnumerator CalculateAverageQueue()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.2f);

            int totalQueue = 0;
            foreach(ResourcePatch patch in _myMinerals)
            {
                totalQueue += patch.MiningQueue;
            }

            _averageQueue = totalQueue / _myMinerals.Count;
        }
    }

    public void FindNeighborPatch(MinerController scv)
    {
        foreach(ResourcePatch patch in _myMinerals)
        {
            if(patch.MiningQueue <= _averageQueue)
            {
                scv.FindMinerals(patch);
                return;
            }
        }
    }
}
