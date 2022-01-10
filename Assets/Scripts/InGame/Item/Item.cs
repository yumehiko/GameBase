using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace yumehiko.ItemSystem
{
    /// <summary>
	/// アイテム。ゲーム上の物品などを表す。
	/// </summary>
    public class Item
    {
        public int ID { get; }
        public string Name { get; }
        //TODO: Tagを追加。


        public Item(int id, string name)
        {
            ID = id;
            Name = name;
        }
    }
}
