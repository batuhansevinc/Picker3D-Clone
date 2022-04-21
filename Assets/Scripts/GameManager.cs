using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;
namespace Picker3d
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField]
        private Transform Player;
        [SerializeField]
        private TextMeshProUGUI CurrentLevelText;
        [SerializeField]
        private TextMeshProUGUI NextLevelText;
        [SerializeField]
        private List<Image> PartImages;
        [SerializeField]
        private GameObject GameFailedPanel;
        [SerializeField]
        private GameObject GameSuccessPanel;
        [SerializeField]
        private GameObject GameStartPanel;
        public static GameManager instance;
        public float PlayerSpeed;
        public bool DroneHasStart;
        public bool StopPlayer;
        public int LevelMultiplier;
        public int CurrentLevel;
        public int CurrentPart;
        private Level Level;
        private bool GameFailed;


        private void Awake()
        {
            if (instance == null)
                instance = this;
            else
                Destroy(gameObject);

            PlayerSpeed = 7f; 
            StopPlayer = true;
            DroneHasStart = false;

            //PlayerPrefs.DeleteAll();        /// Remove Slash Symbols To Reset Saved Level Data    
            if (!PlayerPrefs.HasKey("Level"))
                PlayerPrefs.SetInt("Level", 1);

            CurrentPart = 1;
            CurrentLevel = PlayerPrefs.GetInt("Level");
            LevelMultiplier = (CurrentLevel / 10) + 1;
            GameStartPanel.SetActive(true);

            if (CurrentLevel > 10)
            {
                if (!PlayerPrefs.HasKey("LevelModel"))
                {
                    PlayerPrefs.SetInt("LevelModel", UnityEngine.Random.Range(1, 10));
                }

                var levelObject = Resources.Load<GameObject>(String.Format("Levels/Level{0}", PlayerPrefs.GetInt("LevelModel")));
                Instantiate(levelObject);
            }
            else
            {
                var levelObject = Resources.Load<GameObject>(String.Format("Levels/Level{0}", CurrentLevel));
                Instantiate(levelObject);
            }
        }

        private void Start()
        {
            Level = FindObjectOfType<Level>();
            UpdateUI();
        }

        private void UpdateUI()
        {
            CurrentLevelText.text = String.Format("{0}", CurrentLevel);
            NextLevelText.text = String.Format("{0}", CurrentLevel + 1);

            for (int i = 0; i < CurrentPart - 1; i++)
                PartImages[i].color = Color.yellow;
        }

        public IEnumerator CheckGameOver()
        {
            yield return new WaitForSeconds(2.0f);

            if (StopPlayer && !GameFailed)
            {
                var currentObjectHolder = Level.ObjectHolders.Where(x => x.Key.PartNumber == CurrentPart).FirstOrDefault();
                var isSuccess = currentObjectHolder.Value.IsSuccess();

                if (isSuccess)
                {
                    StartCoroutine(BridgeUp());
                    CurrentPart++;
                    StartCoroutine(DoorUp());
                    Player.GetChild(0).localScale *= 1.2f;
                    UpdateUI();
                    StopPlayer = false;
                    Destroy(currentObjectHolder.Key.transform.Find("Objects").gameObject);
                    PlayerPrefs.DeleteKey("LevelModel");
                    var nextObjectHolder = Level.ObjectHolders.Where(x => x.Key.PartNumber == CurrentPart).FirstOrDefault();

                    if (nextObjectHolder.Key != null && nextObjectHolder.Key.gameObject.GetComponentInChildren<ObjectSpawner>() != null)
                    {
                        DroneHasStart = true;
                    }
                    if (CurrentPart == 4)
                    {
                        GameSuccessPanel.SetActive(true);
                        StopPlayer = true;
                        PlayerPrefs.SetInt("Level", PlayerPrefs.GetInt("Level") + 1);
                    }
                }
                else
                {
                    GameFailed = true;
                    StopPlayer = true;
                    GameFailedPanel.SetActive(true);
                }
            }
        }

        IEnumerator BridgeUp()
        {
            var sector = Level.Parts[CurrentPart - 1].transform.Find("Sector");
            var bridge = Level.Parts[CurrentPart - 1].transform.Find("Bridge");

            while (Mathf.Abs(bridge.position.y - sector.position.y) > 0.01f)
            {
                bridge.transform.position = Vector3.MoveTowards(bridge.position, new Vector3(bridge.position.x, sector.position.y, bridge.position.z), Time.deltaTime * 10f);
                yield return null;
            }
        }

        IEnumerator DoorUp()
        {   
            int textCount;
            textCount = UnityEngine.Random.Range(0,3);
            var part = Level.Parts[CurrentPart - 1].transform.Find("Confetti").GetComponent<ParticleSystem>();
            var anim = Level.Parts[CurrentPart - 1].transform.Find("Reactions").GetChild(textCount);
            anim.gameObject.SetActive(true);
            anim.GetComponent<Animator>().enabled = true;
            var door = Level.Parts[CurrentPart - 1].transform.Find("Door");
            var doorHolderLeft = door.transform.GetChild(0);
            var doorHolderRight = door.transform.GetChild(1);
            part.Play();
            yield return new WaitForSeconds(0.9f);
            anim.gameObject.SetActive(false);

            while (doorHolderRight.transform.position.x < 5.5f)
            {
                doorHolderRight.transform.Rotate(Vector3.back * Time.deltaTime * 60f, Space.World);
                doorHolderLeft.transform.Rotate(Vector3.forward * Time.deltaTime * 60f, Space.World);
                doorHolderRight.transform.Translate(Vector3.right * Time.deltaTime * 0.40f, Space.World);
                yield return null;
            }
        }

        public void OnRestartGameClicked()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void OnNextLevelClicked()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void OnStartClicked()
        {
            GameStartPanel.SetActive(false);
            StopPlayer = false;
        }
    }
}
