using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using DG.Tweening;

namespace yumehiko.Audio.Music
{
    public class BeatResponder : MonoBehaviour
    {
        [SerializeField] private Image fillCircle;
        [SerializeField] private Image beatCircle;
        [SerializeField] private Color circleColor;

        private Sequence fillSequence;

        private void Awake()
        {
            MusicManager.Metronom.BeatCount
                .Subscribe(_ => ReactBeat())
                .AddTo(this);

            MusicManager.OnStartMusic
                .Subscribe(_ => StartMusic())
                .AddTo(this);

            MusicManager.OnStopMusic
                .Subscribe(_ => StopMusic())
                .AddTo(this);
        }

        private void ReactBeat()
        {
            fillSequence?.Kill();
            fillSequence = DOTween.Sequence();

            beatCircle.rectTransform.localScale = new Vector3(0.5f, 0.5f);
            beatCircle.color = circleColor;
            fillCircle.rectTransform.localScale = Vector3.one;

            fillSequence.Append(beatCircle.rectTransform.DOScale(1.0f, 0.5f));
            fillSequence.Join(beatCircle.DOFade(0.0f, 0.5f));
            fillSequence.Join(fillCircle.rectTransform.DOScale(0.75f, 0.5f));

            fillSequence.SetUpdate(true);
            fillSequence.SetLink(gameObject);
        }

        private void StartMusic()
        {
            fillCircle.enabled = true;
            beatCircle.enabled = true;
        }

        private void StopMusic()
        {
            fillSequence?.Kill();
            fillCircle.enabled = false;
            beatCircle.enabled = false;
        }
    }
}
