using UnityEngine;

public static class MathUtils
{
    public const float EPSILON = 0.000001f;

    public static Vector2 ToVec2(this Vector3 self)
    {
        return new Vector2(self.x, self.y);
    }

    public static Vector3 ToVec3(this Vector2 self)
    {
        return new Vector3(self.x, self.y, 0.0f);
    }
}
