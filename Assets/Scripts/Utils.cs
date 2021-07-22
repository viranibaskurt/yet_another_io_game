using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Utils 
{
    public static Vector3 GetRandomPosition(float radius = 100.0f)
    {
        Vector3 result = Vector3.zero;
        for (int i = 0; i < 100; i++)
        {
            Vector2 randomPt2d = Random.insideUnitCircle * radius;
            Vector3 randomPt3d = new Vector3(randomPt2d.x, 0, randomPt2d.y);
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPt3d, out hit, 1.0f, NavMesh.AllAreas))
            {
                result = randomPt3d;
            }
        }
        return result;
    }
}
public static class IListExtensions
{
    /// <summary>
    /// Shuffles the element order of the specified list.
    /// </summary>
    public static void Shuffle<T>(this IList<T> ts)
    {
        var count = ts.Count;
        var last = count - 1;
        for (var i = 0; i < last; ++i)
        {
            var r = UnityEngine.Random.Range(i, count);
            var tmp = ts[i];
            ts[i] = ts[r];
            ts[r] = tmp;
        }
    }

}