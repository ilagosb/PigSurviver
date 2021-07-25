using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class SpellButtonAnimationController : MonoBehaviour
{
    private float _startY;
    private Tween _scaling;

    [SerializeField]
    private Image _icon;
    
    private void Awake()
    {
        transform.DOScale(new Vector3(1.05f, 1.05f), .8f)
            .SetEase(Ease.InQuad)
            .SetLoops(-1, LoopType.Yoyo);
    }

    public Sequence Hide()
    {
        var seq = DOTween.Sequence()
            .Append(transform.DORotate(new Vector3(360, 0), 3, RotateMode.FastBeyond360).SetRelative())
            .Join(_icon.DOFade(0, 1.5f))
            .Join(_icon.DOColor(new Color(0, 0, 0), 3))
            .SetEase(Ease.OutQuad)
            .SetAutoKill(true);
        return seq;
    }

    public void Show(float timeShowing)
    {
        DOTween.Sequence()
            .Append(_icon.DOColor(new Color(1, 1, 1), timeShowing))
            .Join(_icon.DOFade(1f, timeShowing))
            .SetEase(Ease.Linear);
    }
}
