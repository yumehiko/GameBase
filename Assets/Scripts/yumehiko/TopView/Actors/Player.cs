using UnityEngine;
using yumehiko.TopView.Inputs;
using UniRx;

namespace yumehiko.TopView.Actors
{
    /// <summary>
    /// プレイヤー。トップビューゲームの操作される主体。
    /// </summary>
    public class Player : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D body;
        
        private TopViewInputs inputs;
        private void Awake()
        {
            // コントロールを登録。
            inputs = new TopViewInputs();
            inputs.Player.Move.performed += ctx => Move(ctx.ReadValue<Vector2>());
            inputs.Enable();
        }

        private void Move(Vector2 input)
        {
            
        }

        private void OnDestroy()
        {
            inputs.Dispose();
            inputs.Disable();
        }
    }
}