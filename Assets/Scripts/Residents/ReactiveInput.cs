using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

/// <summary>
/// Unityの入力システムをUniRxでリアクティブに。
/// </summary>
public static class ReactiveInput
{
    private static UserInputs inputActions;

    private static Vector2ReactiveProperty onMove = new Vector2ReactiveProperty();
    private static BoolReactiveProperty onMaru = new BoolReactiveProperty();
    private static BoolReactiveProperty onPeke = new BoolReactiveProperty();
    private static BoolReactiveProperty onPause = new BoolReactiveProperty();

    /// <summary>
    /// 軸操作の入力。
    /// </summary>
    public static ReadOnlyReactiveProperty<Vector2> OnMove => onMove.ToReadOnlyReactiveProperty();

    /// <summary>
    /// マルボタンの入力。
    /// </summary>
    public static ReadOnlyReactiveProperty<bool> OnMaru => onMaru.ToReadOnlyReactiveProperty();

    /// <summary>
    /// ペケボタンの入力。
    /// </summary>
    public static ReadOnlyReactiveProperty<bool> OnPeke => onPeke.ToReadOnlyReactiveProperty();

    /// <summary>
    /// ポーズボタンの入力。
    /// </summary>
    public static ReadOnlyReactiveProperty<bool> OnPause => onPause.ToReadOnlyReactiveProperty();

    static ReactiveInput()
    {
        SubscribeInputs();
    }

    private static void SubscribeInputs()
    {
        //inputActionを生成
        inputActions = new UserInputs();
        inputActions.Enable();

        //各操作入力のON/OFFを購読。
        inputActions.Player.Maru.started += context => onMaru.Value = true;
        inputActions.Player.Maru.canceled += context => onMaru.Value = false;

        inputActions.Player.Peke.started += context => onPeke.Value = true;
        inputActions.Player.Peke.canceled += context => onPeke.Value = false;

        inputActions.Player.Move.performed += context => onMove.Value = context.ReadValue<Vector2>();
        inputActions.Player.Move.canceled += context => onMove.Value = context.ReadValue<Vector2>();

        inputActions.Player.Pause.started += context => onPause.Value = true;
        inputActions.Player.Pause.canceled += context => onPause.Value = false;
    }

    /// <summary>
    /// 操作入力を許可する。
    /// </summary>
    public static void EnableInputs()
    {
        inputActions.Enable();
    }

    /// <summary>
    /// 操作入力を禁止する。
    /// </summary>
    public static void DisableInputs()
    {
        inputActions.Disable();
    }
}
