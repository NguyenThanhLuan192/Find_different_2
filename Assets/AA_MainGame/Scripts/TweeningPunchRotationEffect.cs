using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UniRx;
using UnityEngine;

public class TweeningPunchRotationEffect : MonoBehaviour
{
    [SerializeField] private float _delay = 2f;
    [SerializeField] private Vector3 _punch = Vector3.one;
    [SerializeField] private float _duration = 0.5f;
    [SerializeField] private int _vibrato = 2;
    [SerializeField] private float _elasticity = 1;
    [SerializeField] private int _loops = -1;
    [SerializeField] private Ease _easy = Ease.Linear;
    private Tweener _tweener;
    [SerializeField] private bool _includeDelay = true;
    [SerializeField] private float _changeDelayTo = -1;
    [SerializeField] private LoopType _loopType;
    [SerializeField] float _timeRepeatTween = 10;

    private void Start()
    {
        _tweener?.Kill();

        if (_timeRepeatTween > 0)
            Observable.Interval(TimeSpan.FromSeconds(_timeRepeatTween)).TakeUntilDestroy(gameObject).Subscribe(_ =>
            {
                _tweener = transform.DOPunchRotation(_punch, _duration, _vibrato, _elasticity)
                    .SetLoops(_loops, _loopType)
                    .SetEase(_easy)
                    .SetDelay(_delay).OnKill(() => { });
            });

        _tweener = transform.DOPunchRotation(_punch, _duration, _vibrato, _elasticity).SetLoops(_loops, _loopType)
            .SetEase(_easy)
            .SetDelay(_delay).OnKill(() => { });
    }


    private void OnDestroy()
    {
        _tweener?.Kill();
    }
}