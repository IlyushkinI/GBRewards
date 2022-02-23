using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform))]
public class CustomSlider : Slider
{
    public const string EasingFieldName = nameof(_easing);
    public const string TransitionFieldName = nameof(_transition);
    public const string DurationFieldName = nameof(_duration);
    public const string PowerFieldName = nameof(_power);

    [SerializeField] private Ease _easing = Ease.Linear;
    [SerializeField] private TransitionType _transition;
    [SerializeField] private float _duration;
    [SerializeField] private float _power;


    private void ShowAnimation()
    {
        switch (_transition)
        {
            case TransitionType.None:
                break;
            case TransitionType.Rotation:
                (transform as RectTransform).DOShakeRotation(_duration, Vector3.forward * _power)
                    .SetEase(_easing);
                break;
            case TransitionType.Scale:
                (transform as RectTransform).DOShakeScale(_duration, _power, 4)
                    .SetEase(_easing);
                break;
        }
    }
}
