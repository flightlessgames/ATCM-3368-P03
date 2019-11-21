using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "NewUnit", menuName = "Production/Unit")]
public class UnitData : ScriptableObject
{
    [SerializeField] int _mineral = 0;
    public int Mineral { get { return _mineral; } }
    /*
    public int Gas = 0;
    public int Supply = 1;
    */

    [SerializeField] float _time = 10;
    public float Time { get { return _time; } }

    public ClickableUnit _unit;

    public Sprite _image;
}
