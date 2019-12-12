using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class TweeningPunchScaleEffect : MonoBehaviour
{
    [SerializeField] private Vector3 _punch;
    [SerializeField] private float _duration = 0.5f;
    [SerializeField] private int _vibrato = 2;
    [SerializeField] private float _elasticity = 1;
    [SerializeField] private float _delay = 2;
    [SerializeField] private Ease _ease = Ease.Linear;
    private Tweener _tweener;
    [SerializeField] private bool _includeDelay = true;
    [SerializeField] private float _changeDelayTo = -1;
    [SerializeField] private int _loop = -1;

    private void Start()
    {
        _tweener?.Kill();
        _tweener = transform.DOPunchScale(_punch, _duration, _vibrato, _elasticity).SetDelay(_delay).SetLoops(_loop).SetEase(_ease).OnKill(
            () =>
            {
                _tweener.Restart(_includeDelay, _changeDelayTo);
            });
      
    }

    private void OnDestroy()
    {
        _tweener?.Kill();
    }
}