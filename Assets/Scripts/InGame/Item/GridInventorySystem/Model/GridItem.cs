using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace yumehiko.Item.GridInventorySystem
{
    /// <summary>
	/// スロットの占有者。スロットに実際に配置されるアイテム。
	/// </summary>
    public class GridItem : ISlotOccupant
    {
        /// <summary>
        /// アイテム。
        /// </summary>
        public Item Item { get; }

        /// <summary>
        /// 数量。
        /// </summary>
        public int Amount { get; }

        /// <summary>
        /// 占有するスロットサイズ。
        /// </summary>
        public Vector2Int Size { get; }

        /// <summary>
        /// 表示用のスプライト。
        /// </summary>
        public Sprite Sprite { get; }

        public GridItem(Item item, int amount, Vector2Int size, Sprite sprite)
        {
            Item = item;
            Amount = amount;
            Size = size;
            Sprite = sprite;
        }
    }
}
