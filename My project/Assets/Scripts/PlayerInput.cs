using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    //이렇게 하는 것을 상수로 해야한다.
    public static readonly string AxisVertical = "Vertical";
    public static readonly string AxisHorizontal = "Horizontal";
    public static readonly string Fire1 = "Fire1";
    public static readonly string Reload1 = "Reload";

    public float Move { get; private set; }
    public float Rotate { get; private set; }
    public bool Fire { get;private set; }
    public bool Reload { get; private set; }

    private void Update()
    {
        Move = Input.GetAxis(AxisVertical);
        Rotate = Input.GetAxis(AxisHorizontal);
        Fire = Input.GetButton(Fire1);
        Reload = Input.GetButton(Reload1);

    }

}
