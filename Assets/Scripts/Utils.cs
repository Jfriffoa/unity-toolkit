using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public static class RandomUtil {
    public static float Range(Vector2 limits)    => Random.Range(limits.x, limits.y);
    public static int   Range(Vector2Int limits) => Random.Range(limits.x, limits.y);

    public static T RandomPick<T>(T[] array) => array[Random.Range(0, array.Length)];
    public static (T, int) RandomPick<T>(T[] array, int last) {
        if (array.Length < 2) {
            Debug.LogWarning("Array is size 1, can't pick a different element");
            return (array[0], 0);
        }

        int idx = last;
        while (idx == last) {
            idx = Random.Range(0, array.Length);
        }
        return (array[idx], idx);
    }

    public static Vector2 RandomPoint(Rect zone) {
        var x = Random.Range(zone.xMin, zone.xMax);
        var y = Random.Range(zone.yMin, zone.yMax);
        return new Vector2(x, y);
    }
}

public static class TransformExtensions {
    public static void RandomizeChildOrder(this Transform transform) {
        for (int i = 0; i < transform.childCount; i++) {
            transform.GetChild(i).SetSiblingIndex(Random.Range(0, transform.childCount));
        }
    }

    public static void DestroyChildren(this Transform transform) {
        for (int i = transform.childCount - 1; i >= 0; i--) {
            Object.Destroy(transform.GetChild(i).gameObject);
        }
    }
}

public static class ArrayExtensions {
    //Fisher–Yates Shuffle
    public static void Shuffle<T>(this T[] arr) {
        int n = arr.Length;
        while (n > 1) {
            n--;
            var k = Random.Range(0, n + 1);
            var val = arr[k];
            arr[k] = arr[n];
            arr[n] = val;
        }
    }

    //Fisher–Yates Shuffle
    public static void Shuffle<T>(this List<T> list) {
        int n = list.Count;
        while (n > 1) {
            n--;
            var k = Random.Range(0, n + 1);
            var val = list[k];
            list[k] = list[n];
            list[n] = val;
        }
    }
}

public static class Curves {
    public enum CurveType { Linear, EaseIn, EaseOut, ExpEasIn, SmoothStep }
    
    public static float EaseIn(float t)     => 1f - Mathf.Cos(t * Mathf.PI * .5f);
    public static float EaseOut(float t)    => Mathf.Sin(t * Mathf.PI * .5f);
    public static float ExpEaseIn(float t)  => t * t;
    public static float SmoothStep(float t) => t * t * (3f - 2f * t);

    public static float ApplyCurve(float t, CurveType curveType) {
        switch(curveType) {
            case CurveType.EaseIn: return EaseIn(t);
            case CurveType.EaseOut: return EaseOut(t);
            case CurveType.ExpEasIn: return ExpEaseIn(t);
            case CurveType.SmoothStep: return SmoothStep(t);

            default: return t;
        }
    }
}

