using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Picker3d
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        private float TouchSpeed = 5f;
        [SerializeField]
        private Vector3 ClampsLimit;
        private Touch _touch; // Mobile
        private float _playerspeed;
        public static PlayerController instance;

        private void Awake()
        {
            if (instance == null)
                instance = this;
        }

        private void Start()
        {
            _playerspeed = GameManager.instance.PlayerSpeed;
        }

        private void FixedUpdate()
        {
            if (!GameManager.instance.StopPlayer)
            {
                transform.Translate(transform.forward * Time.deltaTime * _playerspeed);

#if UNITY_EDITOR
                if (Input.GetMouseButton(0))
                {
                    var mouseX = Input.GetAxis("Mouse X");
                    var mouseY = Input.GetAxis("Mouse Y");
                    var touch = Vector3.Slerp(transform.position, transform.position + new Vector3(mouseX, 0f, mouseY), TouchSpeed * Time.deltaTime * 1000);
                    transform.position = new Vector3(Mathf.Clamp(touch.x, -ClampsLimit.x, ClampsLimit.x), transform.position.y, transform.position.z);
                }
#else
       if (Input.touchCount > 0)
            {
                _touch = Input.GetTouch(0);
                if (_touch.phase == TouchPhase.Moved)
                {
                    transform.position = new Vector3(
                        Mathf.Clamp(transform.position.x, -Clamps.x, Clamps.x) + _touch.deltaPosition.x * TouchSpeed,
                        transform.position.y,
                        transform.position.z + _touch.deltaPosition.y * TouchSpeed);
                }
            }
#endif
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "EndPoint")
            {
                other.gameObject.SetActive(false);
                GameManager.instance.StopPlayer = true;
                StartCoroutine(GameManager.instance.CheckGameOver());
                transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
            }


            if (other.tag == "Modifier")
            {
                transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
                other.gameObject.SetActive(false);
            }
        }

        public IEnumerator MoveToStartPosition(Vector3 startPosition)
        {
            yield return new WaitForSeconds(3f);
            transform.eulerAngles = Vector3.zero;
            transform.position = new Vector3(0, 0, transform.position.z);
            transform.Translate(startPosition);
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.tag == "Object")
            {
                if (GameManager.instance.StopPlayer)
                {
                    other.GetComponent<Rigidbody>().AddForce(Vector3.forward, ForceMode.Impulse);
                }
                else
                {
                    other.GetComponent<Rigidbody>().AddForce(transform.position - other.GetComponent<Rigidbody>().position * 100f * Time.fixedDeltaTime);
                }
            }
        }
    }
}



