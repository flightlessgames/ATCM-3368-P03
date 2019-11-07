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
                _recentDownClick = hit.point;

                ClickableUnit tempUnit = hit.transform.GetComponent<ClickableUnit>();
                if(tempUnit != null)
                {
                    OnUnit?.Invoke(tempUnit);   //clicking on unit selects the unit
                    _targetUnit = tempUnit;
                    _targetUnit.RollCall();
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
                float deltaClick = (_recentDownClick - hit.point).magnitude;
                if(deltaClick > 5f)
                {
                    Debug.Log("Drag Distance: " + deltaClick);
                    Vector3 center = ((_recentDownClick - hit.point).normalized * deltaClick / 2) + _recentDownClick;
                    Collider[] colliders = Physics.OverlapBox(center, Vector3.one * deltaClick);
                    foreach(Collider col in colliders)
                    {
                        _targetUnit = col.gameObject.GetComponent<ClickableUnit>();
                        _targetUnit?.RollCall();
                        //create selection GROUP
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

                Debug.Log("Click on " + hit.transform.name);
                _targetUnit?.IdentifyHit(hit);
            }
        }
    }
}
