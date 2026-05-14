namespace Godot.Bindings.Tests;

internal static class MathfExtensions
{
    private const float Sqrt3 = (float)1.7320508075688772935274463415059M;
    private const float Sqrt12 = 1 / Mathf.Sqrt2;
    private const float Sqrt13 = 1 / Sqrt3;

    extension(Mathf)
    {
        public static float Sqrt3 => Sqrt3;
        public static float Sqrt12 => Sqrt12;
        public static float Sqrt13 => Sqrt13;
    }
}
