using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProductionUnit
{
    public GameObject UnitPrefab { get; private set; } = null;
    public int MineralsCost { get; private set; } = 0;
    public int GasCost { get; private set; } = 0;
    public int SupplyCost { get; private set; } = 1;
    public float TimeToProduce { get; private set; } = 5f;
}

public class ProductionBuilding : MonoBehaviour
{
    [SerializeField] List<ProductionUnit> _myProduction = new List<ProductionUnit>();
    [SerializeField] List<Button> _productionButtons = new List<Button>();

    //
}
