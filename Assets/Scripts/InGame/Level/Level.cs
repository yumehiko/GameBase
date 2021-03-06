using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using yumehiko.Resident;
using DG.Tweening;

namespace yumehiko
{
    /// <summary>
    /// レベル。あるシーンの塊を一区切りのゲーム的課題とみなしたもの。ステージ。
    /// </summary>
    public class Level : MonoBehaviour
    {
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