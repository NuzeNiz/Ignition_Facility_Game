using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Threading;

namespace IF
{
    public class GameLogicManagement : MonoBehaviour
    {
        /// <summary>
        /// 20180403 SangBin : Singletone Pattern
        /// </summary>
        public static GameLogicManagement GLM_Instance = null;

        /// <summary>
        /// 20180403 SangBin : Arbitrary Sound Volume
        /// </summary>
        private float soundVolume = 1.0f;

        /// <summary>
        /// 20180403 SangBin : Sound Manager Mute
        /// </summary>
        [HideInInspector]
        public bool isSoundEffectMute = false;

        /// <summary>
        /// 20180403 SangBin : Temporary Respawn Points
        /// </summary>
        private Transform[] TempSpawnpoints;

        /// <summary>
        /// 20180403 SangBin : Enemy Bee Prefabs
        /// </summary>
        [SerializeField]
        private GameObject enemyPrefab_type01;

        /// <summary>
        /// 20180501 SangBin : Enemy Moth Prefabs
        /// </summary>
        [SerializeField]
        private GameObject enemyPrefab_type02;

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
        [HideInInspector]
        public bool thisLevelState = true;

        /// <summary>
        /// 20180418 SangBin : Item Object Pool List
        /// </summary>
        private List<GameObject> ItemObjectPool = new List<GameObject>();

        /// <summary>
        /// 20180418 SangBin : Item Prefabs
        /// </summary>
        [SerializeField]
        private GameObject itemPrefab_type01;

        /// <summary>
        /// 20180418 SangBin : Item Prefabs
        /// </summary>
        [SerializeField]
        private GameObject itemPrefab_type02;

        /// <summary>
        /// 20180418 SangBin : Item Prefabs
        /// </summary>
        [SerializeField]
        private GameObject itemPrefab_type03;

        /// <summary>
        /// 20180418 SangBin : Butterfly Object Pool List
        /// </summary>
        private List<GameObject> butterFlyObjectPool = new List<GameObject>();

        /// <summary>
        /// 20180418 SangBin : Butterfly Prefabs
        /// </summary>
        [SerializeField]
        private GameObject butterFlyPrefab_type01;

        /// <summary>
        /// 20180427 SangBin : Contraints of the number of Each Item
        /// </summary>
        private int maxEachItem = 3;

        /// <summary>
        /// 20180427 SangBin : The number of Items
        /// </summary>
        private int itemSort = 3;

        /// <summary>
        /// 20180427 SangBin : Contraints of the number of Each Item
        /// </summary>
        private int maxItem;

        //-----------------------------------------------------------------------------------------------------------------------------

        void Awake()
        {
            GLM_Instance = this;
            maxItem = maxEachItem * itemSort;
        }

        private void Start()
        {
            CreateEnemyObjectPool();

            TempSpawnpoints = GameObject.Find("EnemySpawnPoints").GetComponentsInChildren<Transform>();

            if (TempSpawnpoints.Length > 0)
            {
                StartCoroutine(this.ActivateEnemy());
            }

            CreateButterFlyObjectPool();
            CreateItemObjectPool();
            StartCoroutine(ActivateButterFly());
        }

        private void Update()
        {
            if(PlayerCtrl.PlayerInstance.PlayerHP <= 0.0d || DefenseStationCtrl.DS_Instance.DefenseStation_HP <= 0.0d)
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
                GameObject Enemy_Bee = (GameObject)Instantiate(enemyPrefab_type01);
                Enemy_Bee.name = "Enemy_Bee_" + i.ToString();
                Enemy_Bee.SetActive(false);


                GameObject Enemy_Moth = (GameObject)Instantiate(enemyPrefab_type02);
                Enemy_Moth.name = "Enemy_Moth_" + i.ToString();
                Enemy_Moth.SetActive(false);

                enemyObjectPool.Add(Enemy_Bee);
                enemyObjectPool.Add(Enemy_Moth);
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
        /// 20180403 SangBin : Activate enemy with the object pool
        /// </summary>
        IEnumerator ActivateEnemy()
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

        /// <summary>
        /// 20180403 SangBin : GameOver
        /// </summary>
        void GameOver()
        {
            SceneManager.LoadScene("GameScene C");
        }

        /// <summary>
        /// 20180418 SangBin : Create Item OP
        /// </summary>
        void CreateItemObjectPool()
        {
            for (int i = 0; i < maxEachItem; i++)
            {
                GameObject Item_type01 = (GameObject)Instantiate(itemPrefab_type01);
                //Item_type01.transform.parent = GoogleARCore.IF.TowerBuildController.TBController.DefenseStation_Anchor_Tr;
                Item_type01.name = "Item_Cinnamon_" + i.ToString();
                Item_type01.SetActive(false);

                GameObject Item_type02 = (GameObject)Instantiate(itemPrefab_type02);
                //Item_type02.transform.parent = GoogleARCore.IF.TowerBuildController.TBController.DefenseStation_Anchor_Tr;
                Item_type02.name = "Item_Type02_" + i.ToString();
                Item_type02.SetActive(false);

                GameObject Item_type03 = (GameObject)Instantiate(itemPrefab_type03);
                //Item_type03.transform.parent = GoogleARCore.IF.TowerBuildController.TBController.DefenseStation_Anchor_Tr;
                Item_type03.name = "Item_Type03_" + i.ToString();
                Item_type03.SetActive(false);

                ItemObjectPool.Add(Item_type01);
                ItemObjectPool.Add(Item_type02);
                ItemObjectPool.Add(Item_type03);
            }
        }

        /// <summary>
        /// 20180418 SangBin : Activate Item with the object pool
        /// </summary>
        public void ActivateItem(Transform butterflyTransform)
        {
            for (int i = 0; i < maxItem; i++)
            {
                int randIdex = Random.Range(0, maxItem);
                if (!ItemObjectPool[randIdex].activeSelf)
                {
                    ItemObjectPool[randIdex].transform.parent = GoogleARCore.IF.TowerBuildController.TBController.DefenseStation_Anchor_Tr;
                    ItemObjectPool[randIdex].transform.position = butterflyTransform.position;
                    ItemObjectPool[randIdex].SetActive(true);
                    ItemObjectPool[randIdex].GetComponent<ItemType01Ctrl>().StartTrackingPlayer();
                    //ItemObjectPool[randIdex].SendMessage("StartTrackingPlayer", SendMessageOptions.DontRequireReceiver);
                    break;
                }
            }
        }

        /// <summary>
        /// 20180427 SangBin : Create ButterFly OP
        /// </summary>
        void CreateButterFlyObjectPool()
        {
            for (int i = 0; i < 2; i++)
            {
                GameObject butterFly = (GameObject)Instantiate(butterFlyPrefab_type01);
                butterFly.transform.parent = GoogleARCore.IF.TowerBuildController.TBController.DefenseStation_Anchor_Tr;
                butterFly.name = "Enemy_Butterfly_" + i.ToString();
                butterFly.SetActive(false);
                butterFlyObjectPool.Add(butterFly);
            }
        }

        /// <summary>
        /// 20180427 SangBin : Activate Butterfly with the object pool
        /// </summary>
        IEnumerator ActivateButterFly()
        {
            while (thisLevelState)
            {
                foreach (GameObject butterFly in butterFlyObjectPool)
                {
                    if (!butterFly.activeSelf)
                    {
                        butterFly.transform.position = IF.DefenseStationCtrl.DS_Instance.DefenseStationTR.position + (Vector3.right * 3) + (Vector3.up * 2);
                        butterFly.SetActive(true);
                        break;
                    }
                }
                yield return new WaitForSeconds(5.0f);
            }
        }
    }
}