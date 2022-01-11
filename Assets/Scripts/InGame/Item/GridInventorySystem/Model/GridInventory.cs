using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace yumehiko.ItemSystem.GridInventory
{
    /// <summary>
    /// グリッドインベントリ。1ないし複数のスロットを持つ。
    /// </summary>
    public class GridInventory
    {
        /// <summary>
        /// グリッドインベントリの1スロットの一辺のスクリーン上でのサイズ。
        /// </summary>
        public readonly static int SlotSize = 64;
        public Slot[,] Slots { get; }
        public Vector2Int Size { get; }

        public readonly List<GridItem> Items = new List<GridItem>(); 


        public GridInventory(Vector2Int size)
        {
            Size = size;
            Slots = new Slot[size.x, size.y];

            for (int y = 0; y < Size.y; y++)
            {
                for (int x = 0; x < Size.x; x++)
                {
                    Slots[x, y] = new Slot();
                }
            }
        }

        /// <summary>
        /// ScriptableObjectを元にアイテムを追加する。
        /// </summary>
        /// <param name="itemObject"></param>
        /// <param name="amount"></param>
        /// <param name="slotPosition"></param>
        public void AddItemBasedObject(GridItemIdea itemObject, int amount, Vector2Int slotPosition)
        {
            GridItem item = new GridItem(itemObject, amount, slotPosition);
            AddItem(item, slotPosition);
        }

        /// <summary>
        /// アイテムをスロットに追加する。
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

            Items.Add(item);
        }

        /// <summary>
        /// 指定したアイテムをスロットから除外する。
        /// </summary>
        /// <param name="item"></param>
        public void RemoveItem(GridItem item)
        {
            if(!Contains(item))
            {
                throw new Exception("アイテムがインベントリ内に存在しない。");
            }

            //占有物のスロット上の右端と下端の座標
            int endX = item.SlotPosition.x + item.Size.x;
            int endY = item.SlotPosition.y + item.Size.y;

            //範囲内全てのスロットから占有物を削除。
            for (int y = item.SlotPosition.y; y < endY; y++)
            {
                for (int x = item.SlotPosition.x; x < endX; x++)
                {
                    Slots[x, y].Empty();
                }
            }

            Items.Remove(item);
        }

        /// <summary>
        /// アイテムをスロットに配置できるかを返す。
        /// </summary>
        /// <param name="slotPosition">配置するもののスロット座標（左上原点）。</param>
        /// <param name="item">配置するもの。</param>
        public bool CanPlace(Vector2Int slotPosition, GridItem item)
        {
            //占有物の左端または上端がポケットサイズをはみでるなら、false。
            if (slotPosition.x < 0 || slotPosition.y < 0)
            {
                return false;
            }

            //占有物のスロット上の右端と下端の座標
            int endX = slotPosition.x + item.Size.x;
            int endY = slotPosition.y + item.Size.y;

            //占有物の右端または下端がポケットサイズをはみでるなら、false。
            if (endX > Size.x || endY > Size.y)
            {
                return false;
            }

            //範囲内のすべてのSlotが空でないなら、false。
            for (int y = slotPosition.y; y < endY; y++)
            {
                for (int x = slotPosition.x; x < endX; x++)
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
        /// アイテムをスロットに配置できるかを返す。
        /// 外枠にはみでる場合は、はみでないサイズを返す。
        /// </summary>
        /// <param name="slotPosition"></param>
        /// <param name="item"></param>
        /// <param name="maskedSize"></param>
        /// <returns></returns>
        public bool CanPlaceWithMaskSize(Vector2Int slotPosition, GridItem item, out Vector2Int maskedSize)
        {
            //占有物の左端または上端がポケットサイズをはみでるなら、false。
            if (slotPosition.x < 0 || slotPosition.y < 0)
            {
                maskedSize = Vector2Int.zero;
                return false;
            }

            //占有物の位置が右端または下端より外に指定されている場合は、false。
            if(slotPosition.x >= Size.x || slotPosition.y >= Size.y)
            {
                maskedSize = Vector2Int.zero;
                return false;
            }

            //占有物のスロット上の右端と下端の座標
            int endX = slotPosition.x + item.Size.x;
            int endY = slotPosition.y + item.Size.y;

            //占有物の右端または下端がポケットサイズを一部はみでるなら、false。
            if (endX > Size.x || endY > Size.y)
            {
                int maskedX = endX - Size.x <= 0 ? item.Size.x : endX - Size.x;
                int maskedY = endY - Size.y <= 0 ? item.Size.y : endY - Size.y;
                maskedSize = new Vector2Int(maskedX, maskedY);
                return false;
            }

            //範囲内のすべてのSlotが空でないなら、false。
            for (int y = slotPosition.y; y < endY; y++)
            {
                for (int x = slotPosition.x; x < endX; x++)
                {
                    if (!Slots[x, y].CanPlace(item))
                    {
                        maskedSize = item.Size;
                        return false;
                    }
                }
            }

            maskedSize = item.Size;
            return true;
        }

        /// <summary>
        /// 指定したアイテムがインベントリ内に存在するか確認する。
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(GridItem item)
        {
            return Items.Contains(item);
        }
    }
}
