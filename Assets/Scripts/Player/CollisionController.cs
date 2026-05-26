using System;
using UnityEngine;

public class CollisionController : MonoBehaviour
{
    public static event Action<int> OnPlayerCrashed;

    private void OnCollisionEnter(Collision collision)
    {
        OnPlayerCrashed?.Invoke((int)collision.relativeVelocity.magnitude);
        // Warning: Debug.Log en cada colisión inunda la consola.
        Debug.Log("player collided");
    }
}
