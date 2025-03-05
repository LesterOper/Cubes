using System;
using DG.Tweening;
using Signals;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using Zenject;

namespace Core
{
    public class HintTMPView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI hint;
        private Tween _showHintTween;
        [SerializeField]private LocalizeStringEvent _localizedString;

        [Inject] private SignalBus _signalBus;

        private void Awake()
        {
            _signalBus.Subscribe<LocalizeHintSignal>(UpdateTerm);
            _localizedString.StringReference.StringChanged += ChangeLocale;
        }

        private void ShowHint()
        {
            if(_showHintTween != null && _showHintTween.active) _showHintTween.Kill();
            hint.DOFade(1, 0.2f).OnComplete(() =>
            {
                hint.DOFade(0, 0.2f).SetDelay(0.5f);
            });
        }

        private void ChangeLocale(string term)
        {
            hint.text = term;
        }

        private void UpdateTerm(LocalizeHintSignal signal)
        {
            _localizedString.StringReference.TableEntryReference = signal.term;
            _localizedString.RefreshString();
            ShowHint();
        }
    }
}