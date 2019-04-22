using System.Collections.Generic;

public static class DictionaryExtensions
{
    public static V GetOrElse<K,V>(this Dictionary<K,V> self, K index, V orElse)
    {
        V value;
        if (self.TryGetValue(index, out value))
        {
            return value;
        }

        return orElse;
    }

    public static void RemoveIfExists<K,V>(this Dictionary<K,V> self, K index, V objToFind)
    {
        V value;
        if (self.TryGetValue(index, out value) && EqualityComparer<V>.Default.Equals(value, objToFind))
        {
            self.Remove(index);
        }
    }

    public static void RemoveIfExistsAndImplements<K, V, O>(this Dictionary<K,V> self, K index, O objToRemove)
        where O : class
        where V : class
    {
        V castObj = objToRemove as V;
        if (castObj != null)
        {
            self.RemoveIfExists(index, castObj);
        }
    }

    public static void AddIfImplements<K, V, O>(this Dictionary<K, V> self, K index, O objToAdd)
        where O : class
        where V : class
    {
        V castObj = objToAdd as V;
        if (castObj != null)
        {
            self.Add(index, castObj);
        }
    }
}
