using UnityEngine;
using yumehiko.Platformer.Actors.Parts;

namespace yumehiko.Platformer.Level
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
                UnityEngine.Debug.LogWarning("レベルが自動アタッチできません。");
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
            var player = collision.GetComponent<ILevelBeater>();
            if (player == null)
            {
                return;
            }

            //クリアできない状態なら無視。
            if(!player.CanBeat)
            {
                return;
            }

            //このレベルをクリアする。
            player.OnBeatLevel(); // プレイヤー側の処理。
            level.BeatLevel(chooseID);
        }
    }
}