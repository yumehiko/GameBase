using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace yumehiko.Item.GridInventorySystem
{
    /// <summary>
    /// グリッドインベントリ。1ないし複数のスロットを持つ。
    /// </summary>
    public class GridInventory
    {
        public readonly static float SlotSize = 64.0f;
        public Slot[,] Slots { get; }
        public Vector2Int Size { get; }

        private readonly List<GridItem> items = new List<GridItem>(); 


        public GridInventory(Vector2Int size)
        {
            Size = size;
            Slots = new Slot[size.x, size.y];

            for (int y = 0; y < Size.x; y++)
            {
                for (int x = 0; x < Size.y; x++)
                {
                    Slots[x, y] = new Slot();
                }
            }

        }

        /// <summary>
        /// アイテムをポケットに追加する。
        /// </summary>
        /// <param name="slotPosition">配置する座標（左上原点）。</param>
        /// <param name="item">配置するもの。</param>
        public void AddItem(GridItem item, Vector2Int slotPosition)
        {
            if (!CanPlace(slotPosition, item))
            {
                throw new Exception("配置できない位置が指定された。");
            }

            //占有物のスロット上の右端と下端の座標
            int endX = slotPosition.x + item.Size.x;
            int endY = slotPosition.y + item.Size.y;

            //範囲内全てのスロットに占有物を配置する。
            for (int y = slotPosition.y; y < endY; y++)
            {
                for (int x = slotPosition.x; x < endX; x++)
                {
                    Slots[x, y].Fill(item);
                }
            }

            items.Add(item);
        }

        /// <summary>
        /// 指定したアイテムをポケットから除外する。
        /// </summary>
        /// <param name="item"></param>
        public void RemoveItem(GridItem item, Vector2Int slotPosition)
        {
            if(!Contains(item))
            {
                throw new Exception("アイテムがインベントリ内に存在しない。");
            }

            //占有物のスロット上の右端と下端の座標
            int endX = slotPosition.x + item.Size.x;
            int endY = slotPosition.y + item.Size.y;

            //範囲内全てのスロットから占有物を削除。
            for (int y = slotPosition.y; y < endY; y++)
            {
                for (int x = slotPosition.x; x < endX; x++)
                {
                    Slots[x, y].Empty();
                }
            }

            items.Remove(item);
        }

        /// <summary>
        /// アイテムをスロットに配置できるかを返す。
        /// </summary>
        /// <param name="position">配置する座標（左上原点）。</param>
        /// <param name="item">配置するもの。</param>
        public bool CanPlace(Vector2Int position, GridItem item)
        {
            //占有物のスロット上の右端と下端の座標
            int endX = position.x + item.Size.x;
            int endY = position.y + item.Size.y;

            //占有物の右端または下端がポケットサイズをはみでるなら、false。
            if (endX > Size.x || endY > Size.y)
            {
                return false;
            }

            //範囲内のすべてのSlotが空でないなら、false。
            for (int y = position.y; y < endY; y++)
            {
                for (int x = position.x; x < endX; x++)
                {
                    if (!Slots[x, y].CanPlace(item))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// 指定したアイテムがインベントリ内に存在するか確認する。
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(GridItem item)
        {
            return items.Contains(item);
        }

        public void DebugSlotEmptyInfo()
        {
            string info = "";

            for (int y = 0; y < Size.x; y++)
            {
                string newLine = "";
                for (int x = 0; x < Size.y; x++)
                {
                    newLine += Slots[x, y].IsEmpty ? 0 : 1;
                }
                info += newLine + Environment.NewLine;
            }

            Debug.Log(info);
        }

    }
}
