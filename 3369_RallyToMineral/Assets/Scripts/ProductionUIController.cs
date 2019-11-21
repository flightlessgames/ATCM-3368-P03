using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Utility;   //using a Utility Script to save typing

[RequireComponent(typeof(HQController))]
public class ProductionUIController : MonoBehaviour
{
    [Header("Required")]
    [SerializeField] HQController _hq = null;
    [SerializeField] CanvasGroup _production = null;
    [SerializeField] CanvasGroup _default = null;
    [SerializeField] Image[] _queueImages = new Image[5];

    [SerializeField] Slider _productionSlider = null;

    [Header("Settings")]
    [SerializeField] Sprite _transparent = null;

    private Coroutine _timerDrawRoutine = null;

    private void OnEnable()
    {
        _hq.OnUpdateQueue += UpdateUI;
    }

    private void OnDisable()
    {
        _hq.OnUpdateQueue -= UpdateUI;
    }

    private void UpdateUI(List<UnitData> productionQueue)
    {
        Debug.Log("Updating Production UI");
        foreach (Image image in _queueImages)  //reset all images to adjust for smaller queue length
        {
            image.sprite = _transparent;
        }

        if (productionQueue.Count < 1)
        {
            ToggleCanvasGroup(1, _default);
            ToggleCanvasGroup(0, _production);
            return;
        }

        ToggleCanvasGroup(0, _default);
        ToggleCanvasGroup(1, _production);

        for (int i = 0; i < productionQueue.Count; i++)  //for loop to track position, image 1 replaces queue image 1
        {
            _queueImages[i].sprite = productionQueue[i]._image;
        }
    }
}
