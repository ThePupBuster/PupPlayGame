using UnityEngine;

public static class ArrayExtensions
{
    public static T SelectRandom<T>(this T[] self)
    {
        return self[Random.Range(0, self.Length)];
    }
}
