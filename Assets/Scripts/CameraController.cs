using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Picker3d
{
    public class CameraController : MonoBehaviour
    {
        [Range(0, 1)]
        [SerializeField] private float smoothness;
        Vector3 _offset;
        Transform player;
        PlayerController PlayerController;
        Vector3 _TPosition;
        public static CameraController instance;

        private void Awake()
        {
            if (instance == null)
                instance = this;
        }
            private void Start()
        {
            player = PlayerController.instance.transform;
            _offset = transform.position - player.position;
        }

        private void LateUpdate()
        {
            _TPosition = player.position + _offset;

            transform.position = Vector3.Lerp(transform.position,
            new Vector3(transform.position.x, _TPosition.y, _TPosition.z), smoothness);

        }

    }


}

