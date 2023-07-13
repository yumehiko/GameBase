using DG.Tweening;
using UniRx;
using UnityEngine;
using yumehiko.Platformer.Actors;
using yumehiko.Resident;

namespace yumehiko.Platformer.Level
{
    /// <summary>
    /// レベル。あるシーンの塊を一区切りのゲーム的課題とみなしたもの。ステージ。
    /// </summary>
    public class Level : MonoBehaviour
    {
        [SerializeField] private Player player;
        private void Awake()
        {
            player.IsDied
                .Where(isTrue => isTrue)
                .Subscribe(_ => RelordLevel())
                .AddTo(this);
        }

        /// <summary>
        /// このレベルをクリアする。
        /// </summary>
        /// <param name="nextSceneID">次のレベルのシーンID。-1なら現在のレベル+1。</param>
        public void BeatLevel(int nextSceneID = -1)
        {
            int finalDestinationLevel = nextSceneID == -1 ? nextSceneID : LoadManager.CurrentSceneID + 1;
            PlayerPrefs.SetInt("LastLevel", finalDestinationLevel);
            LoadManager.RequireLoadScene(finalDestinationLevel);
        }

        /// <summary>
        /// このレベルをリロードする。
        /// </summary>
        public void RelordLevel()
        {
            _ = DOVirtual.DelayedCall(1.0f, () => LoadManager.RequireResetScene(), false).SetLink(gameObject);
        }
    }
}