using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace yumehiko
{
    /// <summary>
    /// レベルの出口。プレイヤーが侵入したとき、このレベルをクリアしたとみなす。
    /// </summary>
    public class LevelExit2D : MonoBehaviour
    {
        [SerializeField] private int chooseID = -1;
        [SerializeField] private Level level;

        private bool wasEntered = false;

        private void Reset()
        {
            var level = GameObject.Find("Level").GetComponent<Level>();
            if(level == null)
            {
                Debug.LogWarning("レベルが自動アタッチできません。");
                return;
            }
            this.level = level;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (wasEntered)
            {
                return;
            }

            //侵入者がプレイヤーか確認し、違うなら無視。
            var player = collision.GetComponent<IPlayer>();
            if (player == null)
            {
                return;
            }

            //プレイヤーがすでに死んでいるときは、無視。
            if(player.IsDied.Value)
            {
                return;
            }

            //このレベルをクリアする。
            player.SetCanControl(false);
            level.BeatLevel(chooseID);
        }
    }
}