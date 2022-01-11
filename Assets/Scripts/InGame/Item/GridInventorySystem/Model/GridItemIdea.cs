using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace yumehiko.ItemSystem.GridInventory
{
    /// <summary>
    /// グリッドアイテムのイデア。
    /// ScriptableObject。実在するりんごにたいする想像のりんごのようなもの。
    /// </summary>
    [CreateAssetMenu(menuName = "ScriptableObjects/GridItemIdea")]
    public class GridItemIdea : ScriptableObject
    {
        [SerializeField] private string itemName;
        [SerializeField] private Sprite sprite;
        [SerializeField] private Vector2Int size;
        [SerializeField] private int maxStack;

        public string Name => itemName;
        public Sprite Sprite => sprite;
        public Vector2Int Size => size;
        public int MaxStack => maxStack;
    }
}
