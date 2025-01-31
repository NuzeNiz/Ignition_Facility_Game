﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Threading;

namespace IFP
{
    public class GameManagement_backup : MonoBehaviour
    {
        /// <summary>
        /// 20180403 SangBin : Singletone Pattern
        /// </summary>
       // public static GameManagement instance = null;

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
        private Transform[] tempSpawnpoints;

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
        /// 20180501 SangBin : Enemy Moth Prefabs
        /// </summary>
        [SerializeField]
        private GameObject enemyPrefab_type03;

        /// <summary>
        /// 20180403 SangBin : Enemy Bee Prefabs
        /// </summary>
        [SerializeField]
        private GameObject enemyPrefab_type01Boss;

        /// <summary>
        /// 20180501 SangBin : Enemy Moth Prefabs
        /// </summary>
        [SerializeField]
        private GameObject enemyPrefab_type02Boss;

        /// <summary>
        /// 20180501 SangBin : Enemy Moth Prefabs
        /// </summary>
        [SerializeField]
        private GameObject enemyPrefab_type03Boss;


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
        private int maxEnemy = 10;

        /// <summary>
        /// 20180403 SangBin : This Level Cleared or not
        /// </summary>
        [HideInInspector]
        public bool ThisStageAlive = true;

        /// <summary>
        /// 20180418 SangBin : Item Object Pool List
        /// </summary>
        private List<GameObject> itemObjectPool = new List<GameObject>();

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

        /// <summary>
        /// 20180403 SangBin : 
        /// </summary>
        void Awake()
        {
            //if (instance == null)
            //{
            //    instance = this;
            //    DontDestroyOnLoad(gameObject);
            //}
            //else
            //{
            //    DestroyImmediate(this);
            //}
        //    instance = this;
            maxItem = maxEachItem * itemSort;
        }

        /// <summary>
        /// 20180403 SangBin : 
        /// </summary>
        private void Start()
        {

            if (DefenseStationCtrl.instance != null && DefenseStationCtrl.instance.gameObject.activeSelf)
            {
                CreateEnemyObjectPool();

                GameObject.Find("EnemySpawnPoints").gameObject.transform.SetPositionAndRotation(DefenseStationCtrl.instance.DefenseStationTR.position, DefenseStationCtrl.instance.DefenseStationTR.rotation);

                tempSpawnpoints = GameObject.Find("EnemySpawnPoints").GetComponentsInChildren<Transform>();

                if (tempSpawnpoints.Length > 0)
                {
                    StartCoroutine(this.ActivateEnemy());
                }

            }

            CreateButterFlyObjectPool();
            CreateItemObjectPool();
            StartCoroutine(ActivateButterFly());
        }

        /// <summary>
        /// 20180403 SangBin : 
        /// </summary>
        private void Update()
        {
            //if(PlayerCtrl.instance.PlayerHP <= 0.0d || DefenseStationCtrl.instance.DefenseStation_HP <= 0.0d)
            //{
            //    GameOver();
            //}
            if (Input.GetKey(KeyCode.Escape))
            {
                Application.Quit();
            }
        }

        /// <summary>
        /// 20180403 SangBin : Create enemy OP
        /// </summary>
        private void CreateEnemyObjectPool()
        {
            for (int i = 0; i < maxEnemy; i++)
            {
                GameObject enemy_type01 = (GameObject)Instantiate(enemyPrefab_type01);
                enemy_type01.name = "Enemy_type01_" + i.ToString();
                enemy_type01.SetActive(false);

                GameObject enemy_type02 = (GameObject)Instantiate(enemyPrefab_type02);
                enemy_type02.name = "Enemy_type02_" + i.ToString();
                enemy_type02.SetActive(false);

                GameObject enemy_type03 = (GameObject)Instantiate(enemyPrefab_type03);
                enemy_type03.name = "Enemy_type03_" + i.ToString();
                enemy_type03.SetActive(false);

                //GameObject Enemy_Moth = (GameObject)Instantiate(enemyPrefab_type02);
                //Enemy_Moth.name = "Enemy_Moth_" + i.ToString();
                //Enemy_Moth.transform.GetChild(0).gameObject.SetActive(false);
                //Enemy_Moth.SetActive(false);

                enemyObjectPool.Add(enemy_type01);
                enemyObjectPool.Add(enemy_type02);
                enemyObjectPool.Add(enemy_type03);
            }

            LoadingManagement.instance.FillLoadingGauge(25.0f);
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
        private IEnumerator ActivateEnemy()
        {
            while (ThisStageAlive)
            {
                yield return new WaitForSeconds(enemyGenerationPeriod);

                if (!ThisStageAlive)
                    yield break;

                foreach (GameObject enemy in enemyObjectPool)
                {
                    if (!enemy.activeSelf)
                    {
                        int index = Random.Range(1, tempSpawnpoints.Length);
                        enemy.transform.position = tempSpawnpoints[index].position;
                        StartCoroutine(OpenGate(tempSpawnpoints[index].GetChild(0).gameObject));
                        if (DefenseStationCtrl.instance != null && DefenseStationCtrl.instance.gameObject.activeSelf)
                            enemy.transform.parent = GoogleARCore.IF.TowerBuildController.instance.DefenseStation_Anchor_Tr;
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
        /// 20180606 SangBin : 
        /// </summary>
        private IEnumerator OpenGate(GameObject gObject)
        {
            if (!gObject.activeSelf)
            {
                if (DefenseStationCtrl.instance != null && DefenseStationCtrl.instance.gameObject.activeSelf)
                    gObject.transform.LookAt(DefenseStationCtrl.instance.DefenseStationTR);
                else
                    gObject.transform.LookAt(PlayerCtrl.instance.PlayerTr);

                gObject.transform.localRotation *= Quaternion.Euler(0.0f, 90.0f, 0.0f);

                gObject.SetActive(true);
                yield return new WaitForSeconds(3.0f);
                gObject.SetActive(false);
            }
            yield break;
        }

        /// <summary>
        /// 20180403 SangBin : GameOver
        /// </summary>
        public void GameOver()
        {
            SceneManager.LoadScene("GameScene C");
        }


        /// <summary>
        /// 20180604 SangBin : GameOver
        /// </summary>
        public void GameClear()
        {
            SceneManager.LoadScene("GameScene D");
        }

        /// <summary>
        /// 20180418 SangBin : Create Item OP
        /// </summary>
        private void CreateItemObjectPool()
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

                itemObjectPool.Add(Item_type01);
                itemObjectPool.Add(Item_type02);
                itemObjectPool.Add(Item_type03);
            }

            LoadingManagement.instance.FillLoadingGauge(9.0f);
        }

        /// <summary>
        /// 20180418 SangBin : Activate Item with the object pool
        /// </summary>
        public void ActivateItem(Transform butterflyTransform)
        {
            for (int i = 0; i < maxItem; i++)
            {
                int randIdex = Random.Range(0, maxItem);
                if (!itemObjectPool[randIdex].activeSelf)
                {
                    if (DefenseStationCtrl.instance != null && DefenseStationCtrl.instance.gameObject.activeSelf)
                        itemObjectPool[randIdex].transform.parent = GoogleARCore.IF.TowerBuildController.instance.DefenseStation_Anchor_Tr;

                    itemObjectPool[randIdex].transform.position = butterflyTransform.position;
                    itemObjectPool[randIdex].SetActive(true);
                    break;
                }
            }
        }

        /// <summary>
        /// 20180427 SangBin : Create ButterFly OP
        /// </summary>
        private void CreateButterFlyObjectPool()
        {
            for (int i = 0; i < 2; i++)
            {
                GameObject butterFly = (GameObject)Instantiate(butterFlyPrefab_type01);
                if (DefenseStationCtrl.instance != null && DefenseStationCtrl.instance.gameObject.activeSelf)
                    butterFly.transform.parent = GoogleARCore.IF.TowerBuildController.instance.DefenseStation_Anchor_Tr;
                butterFly.name = "Enemy_Butterfly_" + i.ToString();
                butterFly.SetActive(false);
                butterFlyObjectPool.Add(butterFly);
            }
            LoadingManagement.instance.FillLoadingGauge(1.0f);
        }

        /// <summary>
        /// 20180427 SangBin : Activate Butterfly with the object pool
        /// </summary>
        private IEnumerator ActivateButterFly()
        {
            while (ThisStageAlive)
            {
                foreach (GameObject butterFly in butterFlyObjectPool)
                {
                    if (!butterFly.activeSelf)
                    {
                        if (DefenseStationCtrl.instance != null && DefenseStationCtrl.instance.gameObject.activeSelf)
                            butterFly.transform.position = IFP.DefenseStationCtrl.instance.DefenseStationTR.position + (Vector3.right * 3) + (Vector3.up * 2);
                        else
                            butterFly.transform.position = PlayerCtrl.instance.PlayerTr.position + (Vector3.right * 3) + (Vector3.up * 2);
                        butterFly.SetActive(true);
                        break;
                    }
                }
                yield return new WaitForSeconds(5.0f);
            }
        }

        /// <summary>
        /// 20180617 SangBin : 
        /// </summary>
        public void ActivateChapter2Boss()
        {
            Vector3 tempV = PlayerCtrl.instance.PlayerTr.forward;
            tempV.y = 0.0f;
            tempV *= 10.0f;

            if (TempStageManagement.instance.CurrentStageLevel == 2)
            {
                GameObject Boss = Instantiate(enemyPrefab_type01Boss, (PlayerCtrl.instance.PlayerTr.position + tempV), Quaternion.identity);
            }
            else if (TempStageManagement.instance.CurrentStageLevel == 3)
            {
                GameObject Boss = Instantiate(enemyPrefab_type02Boss, (PlayerCtrl.instance.PlayerTr.position + tempV), Quaternion.identity);
            }
            else if (TempStageManagement.instance.CurrentStageLevel == 4)
            {
                GameObject Boss = Instantiate(enemyPrefab_type03Boss, (PlayerCtrl.instance.PlayerTr.position + tempV), Quaternion.identity);
            }
        }
    }
}