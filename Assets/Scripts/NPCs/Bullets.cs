using System.Collections.Generic;
using UnityEngine;

// Suggestion: el nombre "Bullets" es engañoso; esta clase es un pool de balas. Mejor llamarla "BulletPool".
public class Bullets : MonoBehaviour
{
    // Suggestion: singleton expuesto como campo público static.
    // Convención: "public static Bullets Instance { get; private set; }".
    // Warning: si algún script accede a Bullets.instance antes de que se ejecute este Awake (orden de ejecución),
    // va a recibir null y fallar (ej: NpcController.Shooting llama Bullets.instance.GetPooledObject()).
    public static Bullets instance;
    // Warning: campo público; debería ser private con accessor.
    public List<GameObject> pooledObjects = new List<GameObject>();
    [SerializeField] private GameObject _objectToPool;
    [SerializeField] private int _amountToPool;

    private List<Rigidbody> _pooledRigidbodies = new List<Rigidbody>();

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    private void Start()
    {
        GameObject tmp;
        for (int i = 0; i < _amountToPool; i++)
        {
            tmp = Instantiate(_objectToPool);
            // Suggestion: usar tmp.transform.SetParent(transform) en vez de asignar .parent directo
            tmp.transform.parent = gameObject.transform;
            tmp.SetActive(false);

            pooledObjects.Add(tmp);
            _pooledRigidbodies.Add(tmp.GetComponent<Rigidbody>());
        }
    }

    // Warning: Media - el pool es de tamaño fijo y NO crece. Cuando todas las balas están activas, devuelve null silenciosamente.
    // El llamante (NpcController.Shooting) tiene que chequear null o el disparo se "pierde" sin feedback. En un Object Pool
    // robusto se expande el pool y se reciclan las balas viejas.
    public GameObject GetPooledObject()
    {
        for (int i = 0; i < _amountToPool; i++)
        {
            if (!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }
        return null;
    }

    // Suggestion: búsqueda lineal O(n) para mapear GameObject → Rigidbody. Si el GO ya tiene el Rigidbody como componente,
    // un simple bullet.GetComponent<Rigidbody>() lo resuelve. Alternativamente, usar Dictionary<GameObject, Rigidbody>.
    public Rigidbody GetRigidbody(GameObject bullet)
    {
        for (int i = 0; i < _amountToPool; i++)
        {
            if (bullet == pooledObjects[i])
            {
                return _pooledRigidbodies[i];
            }
        }
        return null;
    }
}
