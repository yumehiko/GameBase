using System;
using UnityEngine;
using UniRx;

namespace yumehiko.ItemSystem.GridInventory
{
    /// <summary>
    /// スロット。アイテムなどが配置できるひとつのマス。
    /// </summary>
    public class Slot
    {
        /// <summary>
        /// 占有しているもの。
        /// </summary>
        public GridItem Item { get; private set; }

        /// <summary>
        /// 空いているか。
        /// </summary>
        public bool IsEmpty => Item == null;

        /// <summary>
        /// スロットを指定したアイテムで埋める。
        /// </summary>
        /// <param name="parent"></param>
        public void Fill(GridItem item)
        {
            if(item == null)
            {
                throw new ArgumentNullException();
            }

            Item = item;
        }

        /// <summary>
        /// スロットを空ける。
        /// </summary>
        public void Empty()
        {
            Item = null;
        }

        /// <summary>
        /// 指定した占有物をここに配置できるかを返す。
        /// </summary>
        /// <returns></returns>
        public bool CanPlace(GridItem item)
        {
            //同一の占有物ならtrue（1マスずれるだけとかなので）。
            if (item == Item)
            {
                return true;
            }

            //空ならtrue。
            return Item == null;
        }
    }
}
