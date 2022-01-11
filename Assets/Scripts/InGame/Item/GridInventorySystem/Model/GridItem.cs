using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace yumehiko.ItemSystem.GridInventory
{
    /// <summary>
	/// スロットの占有者。スロットに実際に配置されるアイテム。
	/// </summary>
    public class GridItem
    {
        /// <summary>
        /// アイテムのイデア。
        /// </summary>
        public GridItemIdea Idea;

        /// <summary>
        /// 現在の数量。
        /// </summary>
        public IReadOnlyReactiveProperty<int> Stack => stack;

        /// <summary>
        /// 現在のスロット上の座標。
        /// </summary>
        public Vector2Int SlotPosition { get; private set; }

        /// <summary>
        /// アイテムのサイズ。
        /// </summary>
        public Vector2Int Size => Idea.Size;

        private IntReactiveProperty stack;


        public GridItem(GridItemIdea idea, int amount, Vector2Int slotPosition)
        {
            Idea = idea;
            stack = new IntReactiveProperty(amount);
            SlotPosition = slotPosition;
        }

        /// <summary>
        /// 可能なら、このアイテムにターゲットのスタックを合わせる。
        /// </summary>
        /// <param name="target"></param>
        /// <returns>スタックをマージした結果、最大スタック量をあふれたあまりの値。</returns>
        public int TryMergeStack(GridItem target)
        {
            //イデアが異なるなら、それは異なるものとみなす。
            if (Idea != target.Idea)
            {
                throw new System.Exception("異なるアイテムをマージしようとした。");
            }

            int mergeSize = stack.Value += target.stack.Value;
            stack.Value = Mathf.Min(mergeSize, Idea.MaxStack);

            return mergeSize - Idea.MaxStack;
        }

        /// <summary>
        /// スタック量を指定する。
        /// </summary>
        /// <param name="amount"></param>
        public void SetStackAmount(int amount)
        {
            stack.Value = amount;
        }

        public void Move(Vector2Int slotPosition)
        {
            SlotPosition = slotPosition;
        }
    }
}
