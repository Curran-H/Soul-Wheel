using System.Collections;
using UnityEngine;

public static class TransformExtensions
{
    public static IEnumerator Move(this Transform t, Vector3 target, float duration)
    {
        Vector3 diffVector = (target - t.position);
        float diffLenght = diffVector.magnitude;
        diffVector.Normalize();
        float counter = 0f;
        while (counter < duration)
        {
            float movAmount = (Time.deltaTime * diffLenght / duration);
            t.position += diffVector * movAmount;
            counter += Time.deltaTime;
            if (Vector2.Distance(t.position, target) < 0.01f)
            {
                t.position = target;
                yield break;
            }
            yield return null;
        }

        t.position = target;
    }

    public static IEnumerator Scale(this Transform t, Vector3 target, float duration)
    {
        Vector3 diffVector = (target - t.localScale);
        float diffLength = diffVector.magnitude;
        diffVector.Normalize();
        float counter = 0;
        while (counter < duration)
        {
            float movAmount = (Time.deltaTime * diffLength / duration);
            t.localScale += diffVector * movAmount;
            counter += Time.deltaTime;
            if (t.localScale.x <= 0 || t.localScale.y <= 0) break;
            yield return null;
        }

        t.localScale = target;
    }
}