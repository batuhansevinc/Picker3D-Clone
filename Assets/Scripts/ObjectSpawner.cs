using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
namespace Picker3d
{

    public class ObjectSpawner : MonoBehaviour
    {
        [SerializeField]
        private float MoveSpeed = 5f;
        [SerializeField]
        private float Frequency = 5f;
        [SerializeField]
        private float Magnitude = 5f;
        [SerializeField]
        private int ObjectCount = 10;
        [SerializeField]
        private float SpawnFrequency = 2f;
        [SerializeField]
        private GameObject Object;
        [SerializeField]
        private Transform ObjectsParent;
        private Vector3 position;
        private bool spawnHasStart;
        private void Start()
        {
            position = transform.position;
            spawnHasStart = false;
        }
        IEnumerator SpawnObjects()
        {
            spawnHasStart = true;
            WaitForSeconds wait = new WaitForSeconds(SpawnFrequency);

            for (int i = 0; i < ObjectCount; i++)
            {
                Instantiate(Object, transform.position, Quaternion.identity, ObjectsParent);
                yield return wait;
            }

            GameManager.instance.DroneHasStart = false;
            gameObject.SetActive(false);
        }

        void Update()
        {
            if (GameManager.instance.DroneHasStart)
            {
                if (!spawnHasStart)
                    StartCoroutine(SpawnObjects());

                position += transform.forward * Time.deltaTime * MoveSpeed;
                transform.position = position + transform.right * Mathf.Sin(Time.time * Frequency) * Magnitude;
            }
        }
    }
}
