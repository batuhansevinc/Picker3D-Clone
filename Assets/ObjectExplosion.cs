using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Picker3d
{
    public class ObjectExplosion : MonoBehaviour
    {
        [SerializeField] private GameObject miniobject;

        private void OnTriggerEnter(Collider other)
        {

            if (other.tag == "Player")
            {
                CreateMiniObjects();
            }
        }

        public void CreateMiniObjects()
        {
            GameObject miniobjects = Instantiate(miniobject, transform.position + Vector3.down, Quaternion.identity);
            miniobjects.transform.parent = transform.parent;
            transform.GetComponent<MeshRenderer>().enabled = false;
            foreach (Transform child in miniobjects.transform)
            {
                if (child.transform.GetComponent<Rigidbody>() != null)
                {
                    child.transform.GetComponent<Rigidbody>().AddForce(Vector3.back * 10f);

                }
            }
        }
    }
}