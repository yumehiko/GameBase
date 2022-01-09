using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace yumehiko.Item.GridInventorySystem
{
    /// <summary>
	/// スロットの占有者。スロットに実際に配置されるアイテム。
	/// </summary>
    public class GridItem
    {
        /// <summary>
        /// アイテム。
        /// </summary>
        public Item Item { get; }

        /// <summary>
        /// 数量。
        /// </summary>
        public ReadOnlyReactiveProperty<int> Stack => stack.ToReadOnlyReactiveProperty();

        public int MaxStack { get; }

        /// <summary>
        /// 占有するスロットサイズ。
        /// </summary>
        public Vector2Int Size { get; }

        /// <summary>
        /// 表示用のスプライト。
        /// </summary>
        public Sprite Sprite { get; }

        private IntReactiveProperty stack;

        public GridItem(Item item, int amount, int maxStack, Vector2Int size, Sprite sprite)
        {
            Item = item;
            stack = new IntReactiveProperty(amount);
            MaxStack = maxStack;
            Size = size;
            Sprite = sprite;
        }

        /// <summary>
        /// 可能なら、このアイテムにターゲットのスタックを合わせる。
        /// </summary>
        /// <param name="target"></param>
        /// <returns>スタックをマージした結果、最大スタック量をあふれたあまりの値。</returns>
        public int TryMergeStack(GridItem target)
        {
            if (Item.ID != target.Item.ID)
            {
                throw new System.Exception("異なるアイテムをマージしようとした。");
            }

            int mergeSize = stack.Value += target.stack.Value;
            stack.Value = Mathf.Min(mergeSize, MaxStack);

            return mergeSize - MaxStack;
        }

        /// <summary>
        /// スタック量を指定する。
        /// </summary>
        /// <param name="amount"></param>
        public void SetStackAmount(int amount)
        {
            stack.Value = amount;
        }
    }
}
