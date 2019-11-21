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
    [SerializeField] float _timerIncrement = 0.2f;

    private Coroutine _timerDrawRoutine = null;

    private void OnEnable()
    {
        _hq.OnUpdateQueue += UpdateUI;
        _hq.OnStartProduceUnit += StartAnimation;
    }

    private void OnDisable()
    {
        _hq.OnUpdateQueue -= UpdateUI;
        _hq.OnStartProduceUnit -= StartAnimation;
    }

    private void UpdateUI(List<UnitData> queue)
    {
        foreach (Image image in _queueImages)  //reset all images to adjust for smaller queue length
        {
            image.sprite = _transparent;
        }

        if (queue.Count < 1)
        {
            ToggleCanvasGroup(1, _default);
            ToggleCanvasGroup(0, _production);

            if (_timerDrawRoutine != null)
                StopCoroutine(_timerDrawRoutine);

            return;
        }

        ToggleCanvasGroup(0, _default);
        ToggleCanvasGroup(1, _production);

        for (int i = 0; i < queue.Count; i++)  //for loop to track position, image 1 replaces queue image 1
        {
            _queueImages[i].sprite = queue[i]._image;
        }
    }

    private void StartAnimation(UnitData unit)
    {
        _productionSlider.value = 0;

        if (_timerDrawRoutine != null)
            StopCoroutine(_timerDrawRoutine);

        _timerDrawRoutine = StartCoroutine(SliderAnimationRoutine(unit.Time));
    }
    
    IEnumerator SliderAnimationRoutine(float max)
    {
        _productionSlider.maxValue = max;
        while (_productionSlider.value < _productionSlider.maxValue)
        {
            yield return new WaitForSeconds(_timerIncrement);
            _productionSlider.value += _timerIncrement;
        }
    }
}
