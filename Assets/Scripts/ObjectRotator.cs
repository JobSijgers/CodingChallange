using System.Collections;
using UnityEngine;

public class ObjectRotator : MonoBehaviour
{
    [SerializeField] private float rotationDuration = 2;
    [SerializeField] private Vector3 startRotation;
    [SerializeField] private Vector3 endRotation;

    private Coroutine rotationCoroutine;

    private bool state;

    public void Toggle()
    {
        if (rotationCoroutine != null)
            return;
        state = !state;
        rotationCoroutine = StartCoroutine(ToggleRotation());
    }

    private IEnumerator ToggleRotation()
    {
        Vector3 start = state ? startRotation : endRotation;
        Vector3 end = state ? endRotation : startRotation;
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime / rotationDuration;
            transform.rotation = Quaternion.Euler(Vector3.Lerp(start, end, t));
            yield return null;
        }
        rotationCoroutine = null;
    }
}