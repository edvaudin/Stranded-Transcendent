using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.Scripting;

#if UNITY_EDITOR
[UnityEditor.InitializeOnLoad]
#endif
[Preserve]
[DisplayStringFormat("{up}/{left}/{down}/{right}")]
public class CustomComposite : InputBindingComposite<Vector2>
{

    // NOTE: This is a modified copy of Vector2Composite

    [InputControl(layout = "Button")]
    public int up = 0;
    [InputControl(layout = "Button")]
    public int down = 0;
    [InputControl(layout = "Button")]
    public int left = 0;
    [InputControl(layout = "Button")]
    public int right = 0;

    private bool upPressedLastFrame;
    private bool downPressedLastFrame;
    private bool leftPressedLastFrame;
    private bool rightPressedLastFrame;
    private float upPressTimestamp;
    private float downPressTimestamp;
    private float leftPressTimestamp;
    private float rightPressTimestamp;

    public override Vector2 ReadValue(ref InputBindingCompositeContext context)
    {
        var upPressed = context.ReadValueAsButton(up);
        var downPressed = context.ReadValueAsButton(down);
        var leftPressed = context.ReadValueAsButton(left);
        var rightPressed = context.ReadValueAsButton(right);

        if (upPressed && !upPressedLastFrame) upPressTimestamp = Time.time;
        if (downPressed && !downPressedLastFrame) downPressTimestamp = Time.time;
        if (leftPressed && !leftPressedLastFrame) leftPressTimestamp = Time.time;
        if (rightPressed && !rightPressedLastFrame) rightPressTimestamp = Time.time;

        float x = (leftPressed, rightPressed) switch
        {
            (false, false) => 0f,
            (true, false) => -1f,
            (false, true) => 1f,
            (true, true) when rightPressTimestamp > leftPressTimestamp => 1f,
            (true, true) when rightPressTimestamp < leftPressTimestamp => -1f,
            (true, true) => 0f
        };

        float y = (downPressed, upPressed) switch
        {
            (false, false) => 0f,
            (true, false) => -1f,
            (false, true) => 1f,
            (true, true) when upPressTimestamp > downPressTimestamp => 1f,
            (true, true) when upPressTimestamp < downPressTimestamp => -1f,
            (true, true) => 0f
        };

        const float diagonal = 0.707107f;
        if (x != 0f && y != 0f)
        {
            x *= diagonal;
            y *= diagonal;
        }

        upPressedLastFrame = upPressed;
        downPressedLastFrame = downPressed;
        leftPressedLastFrame = leftPressed;
        rightPressedLastFrame = rightPressed;

        return new Vector2(x, y);
    }

    public override float EvaluateMagnitude(ref InputBindingCompositeContext context)
    {
        var value = ReadValue(ref context);
        return value.magnitude;
    }

#if UNITY_EDITOR
    static CustomComposite()
    {
        Initialize();
    }
#endif

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    static void Initialize()
    {
        InputSystem.RegisterBindingComposite<CustomComposite>();
    }
}