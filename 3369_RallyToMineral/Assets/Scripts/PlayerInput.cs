using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerInput : MonoBehaviour
{
    public event Action<RaycastHit> OnClick = delegate { };
    public event Action<ClickableUnit> OnUnit = delegate { };

    public static ClickableUnit _targetUnit;  //todo GROUP

    private Vector3 _recentDownClick = Vector3.zero;

    // Update is called once per frame
    void Update()
    {
        CheckMouseInput();
    }

    private void CheckMouseInput()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))   //leftclick
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(ray.origin, ray.direction * 100, Color.yellow, 2f);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100f))
            {
                OnClick?.Invoke(hit);
                _recentDownClick = hit.point;   //remember LMB Down Location

                ClickableUnit tempUnit = hit.transform.GetComponent<ClickableUnit>();   //if collider has associated ClickableUnit script
                if (tempUnit != null)
                {
                    SelectUnit(tempUnit);
                }
            }
        }

        if (Input.GetKeyUp(KeyCode.Mouse0)) //release left click
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(ray.origin, ray.direction * 100, Color.yellow, 2f);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100f))
            {
                Vector3 deltaVector = hit.point - _recentDownClick; //compare distance between LMB Down and Up
                Vector3 halfExtent = deltaVector / 2;
                Vector3 center = halfExtent + _recentDownClick;

                #region Debug Cube
                Debug.DrawRay(center, Vector3.up, Color.blue, 1f);

                Debug.DrawLine(center + new Vector3(halfExtent.x, 0, 0), center + new Vector3(-halfExtent.x, 0, 0), Color.blue, 1f);  //debug cross-secntion
                Debug.DrawLine(center + new Vector3(0, 0, halfExtent.z), center + new Vector3(0, 0, -halfExtent.z), Color.blue, 1f);

                Debug.DrawLine(_recentDownClick, _recentDownClick + new Vector3(halfExtent.x * 2, 0, 0), Color.blue, 1f);   //debug box
                Debug.DrawLine(_recentDownClick, _recentDownClick + new Vector3(0, 0, halfExtent.z * 2), Color.blue, 1f);

                Debug.DrawLine(hit.point, hit.point - new Vector3(halfExtent.x * 2, 0, 0), Color.blue, 1f);   //debug box
                Debug.DrawLine(hit.point, hit.point - new Vector3(0, 0, halfExtent.z * 2), Color.blue, 1f);


                Debug.DrawLine(center + new Vector3(halfExtent.x, 1, 0), center + new Vector3(-halfExtent.x, 1, 0), Color.blue, 1f);  //debug cross-secntion
                Debug.DrawLine(center + new Vector3(0, 1, halfExtent.z), center + new Vector3(0, 1, -halfExtent.z), Color.blue, 1f);

                Debug.DrawLine(_recentDownClick + Vector3.up, _recentDownClick + new Vector3(halfExtent.x * 2, 1, 0), Color.blue, 1f);   //debug box
                Debug.DrawLine(_recentDownClick + Vector3.up, _recentDownClick + new Vector3(0, 1, halfExtent.z * 2), Color.blue, 1f);

                Debug.DrawLine(hit.point + Vector3.up, hit.point - new Vector3(halfExtent.x * 2, -1, 0), Color.blue, 1f);   //debug box
                Debug.DrawLine(hit.point + Vector3.up, hit.point - new Vector3(0, -1, halfExtent.z * 2), Color.blue, 1f);
                #endregion

                Collider[] colliders = Physics.OverlapBox(center, halfExtent + Vector3.up);   //check for colliders within box
                foreach (Collider col in colliders)
                {
                    Debug.DrawRay(col.transform.position, Vector3.up, Color.red, 1f);

                    ClickableUnit tempUnit = col.transform.GetComponent<ClickableUnit>();   //if collider has associated ClickableUnit script
                    if (tempUnit != null)
                    {
                        SelectUnit(tempUnit);

                    }
                }
            }

        }

        if (Input.GetKeyDown(KeyCode.Mouse1))   //rightclick
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(ray.origin, ray.direction * 100, Color.yellow, 2f);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100f))
            {
                OnClick?.Invoke(hit);

                _targetUnit?.Identify(hit);  //if we have a unit selected, when we RMB do something
            }
        }
    }

    private void SelectUnit(ClickableUnit tempUnit)
    {
        _targetUnit?.Deselect();

        _targetUnit = tempUnit;
        _targetUnit?.RollCall();

        OnUnit?.Invoke(_targetUnit);   //selects the unit
    }
}
