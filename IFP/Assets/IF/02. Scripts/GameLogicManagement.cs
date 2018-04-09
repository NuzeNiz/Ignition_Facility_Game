using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace IF
{
    public class GameLogicManagement : MonoBehaviour
    {
        /// <summary>
        /// 20180403 SangBin : Singletone Pattern
        /// </summary>
        public static GameLogicManagement GM_Instance = null;

        /// <summary>
        /// 20180403 SangBin : Arbitrary Sound Volume
        /// </summary>
        private float soundVolume = 1.0f;

        /// <summary>
        /// 20180403 SangBin : Sound Manager Mute
        /// </summary>
        public bool isSoundEffectMute = false;

        /// <summary>
        /// 20180403 SangBin : Temporary Respawn Points
        /// </summary>
        private Transform[] TempSpawnpoints;

        /// <summary>
        /// 20180403 SangBin : Enemy Prefabs
        /// </summary>
        public GameObject enemyPrefabs_type01;

        /// <summary>
        /// 20180403 SangBin : Enemy Object Pool List
        /// </summary>
        private List<GameObject> enemyObjectPool = new List<GameObject>();

        /// <summary>
        /// 20180403 SangBin : Enemy Generation period
        /// </summary>
        private float enemyGenerationPeriod = 2.0f;

        /// <summary>
        /// 20180403 SangBin : Contraints of the number of Enemy
        /// </summary>
        private int maxEnemy = 3;

        /// <summary>
        /// 20180403 SangBin : This Level Cleared or not
        /// </summary>
        private bool thisLevelState = true;

        //-----------------------------------------------------------------------------------------------------------------------------

        void Awake()
        {
            GM_Instance = this;
            GameObject.Find("TempFloor").transform.parent = GoogleARCore.IF.TowerBuildController.TBController.DefenseStation_Anchor_Tr;
        }

        private void Start()
        {
            CreateEnemyObjectPool();

            TempSpawnpoints = GameObject.Find("EnemySpawnPoints").GetComponentsInChildren<Transform>();

            if (TempSpawnpoints.Length > 0)
            {
                StartCoroutine(this.GenerateEnemy());
            }
        }

        private void Update()
        {
            if(PlayerCtrl.PlayerInstance.PlayerHP <= 0.0d)
            {
                GameOver();
            }
        }

        /// <summary>
        /// 20180403 SangBin : Create enemy OP
        /// </summary>
        void CreateEnemyObjectPool()
        {
            for (int i = 0; i < maxEnemy; i++)
            {
                GameObject EnemyObj_test = (GameObject)Instantiate(enemyPrefabs_type01);
                EnemyObj_test.name = "ENEMY_TYPE01_" + i.ToString();
                EnemyObj_test.SetActive(false);
                enemyObjectPool.Add(EnemyObj_test);
            }
        }

        /// <summary>
        /// 20180403 SangBin : Make a effect sound
        /// </summary>
        public void SoundEffect(Vector3 pos, AudioClip soundEffectFile)
        {
            if (isSoundEffectMute)
                return;

            GameObject soundObject = new GameObject();
            soundObject.transform.position = pos;

            AudioSource audioSource = soundObject.AddComponent<AudioSource>();
            audioSource.clip = soundEffectFile;
            audioSource.minDistance = 5.0f;
            audioSource.maxDistance = 10.0f;
            audioSource.volume = soundVolume;

            audioSource.Play();

            Destroy(soundObject, soundEffectFile.length);
        }

        /// <summary>
        /// 20180403 SangBin : Generate enemy with the object pool
        /// </summary>
        IEnumerator GenerateEnemy()
        {
            while (thisLevelState)
            {
                yield return new WaitForSeconds(enemyGenerationPeriod);

                if (!thisLevelState)
                    yield break;

                foreach (GameObject enemy in enemyObjectPool)
                {
                    if (!enemy.activeSelf)
                    {
                        int index = Random.Range(1, TempSpawnpoints.Length);
                        enemy.transform.position = TempSpawnpoints[index].position;
                        enemy.transform.parent = GoogleARCore.IF.TowerBuildController.TBController.DefenseStation_Anchor_Tr;
                        enemy.SetActive(true);
                        break;
                    }
                }

                //int enemyCount = (int)GameObject.FindGameObjectsWithTag("TESTENERMY").Length;

                //if (enemyCount < maxEnemy)
                //{
                //    int index = Random.Range(1, points.Length);
                //    Instantiate(enemyPrefabs, points[index].position, points[index].rotation);

                //    yield return new WaitForSeconds(enemygenerationPeriod);
                //}
                //else
                //{
                //    yield return null;
                //}
            }
        }

        void GameOver()
        {
            SceneManager.LoadScene("GameScene C");
        }
    }
}