using UnityEngine;

namespace yumehiko.Items.GridInventorySystem.Model
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
