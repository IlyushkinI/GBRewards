using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class CustomButton : Button
{
    public const string EasingFieldName = nameof(_easing);
    public const string TransitionFieldName = nameof(_transition);
    public const string DurationFieldName = nameof(_duration);
    public const string PowerFieldName = nameof(_power);

    [SerializeField] private Ease _easing = Ease.Linear;
    [SerializeField] private TransitionType _transition;
    [SerializeField] private float _duration;
    [SerializeField] private float _power;

    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
        ShowAnimation();
    }

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

public enum TransitionType
{
    None,
    Rotation,
    Scale
}
