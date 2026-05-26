using System.Collections;
using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    // Suggestion: typo "corroutine" → "coroutine". Aparece repetido en varios scripts del proyecto.
    private IEnumerator _corroutineMoving;

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    private IEnumerator StartMoving(Vector3 startPos, float distance, float duration, GameObject player)
    {
        Vector3 start = startPos;
        transform.position = start;

        Vector3 end = player.transform.position + player.transform.forward * distance;
        end.y = 0f;

        float time = 0f;

        while (time < duration)
        {
            float t = time / duration;

            Vector3 pos = Vector3.Lerp(start, end, t);

            float fall = (-Physics.gravity.y) * (time * time) / 2;
            pos.y = start.y - fall;

            transform.position = pos;

            if (pos.y <= end.y)
            {
                transform.position = end;
                yield break;
            }

            time += Time.deltaTime;
            yield return null;
        }

        transform.position = end;
        gameObject.SetActive(false);
    }

    public void Shoot(Vector3 startPos, float distance, float duration, GameObject player)
    {
        if (_corroutineMoving != null)
            StopCoroutine(_corroutineMoving);

        _corroutineMoving = StartMoving(startPos, distance, duration, player);
        StartCoroutine(_corroutineMoving);
    }
}