using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Threading;

namespace IFP
{
    public class GameManagement : MonoBehaviour
    {
        /// <summary>
        /// 20180403 SangBin : Singletone Pattern
        /// </summary>
        public static GameManagement instance = null;

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

        private List<GameObject> et01ObjectPool = new List<GameObject>();
        private List<GameObject> et02ObjectPool = new List<GameObject>();
        private List<GameObject> et03ObjectPool = new List<GameObject>();

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

        private bool thisWaveAlive = true;

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


        private int currentWaveLevel = 1;

        //private int[] wave01_et01_spawn_info = new int[] { 2, 5 };
        //private int[] wave01_et02_spawn_info = new int[] { 0, 3, 4, 7 };
        //private int[] wave01_et03_spawn_info = new int[] { 1, 6 };

        //private int[] wave02_et01_spawn_info = new int[] { 0, 1, 4, 7 };
        //private int[] wave02_et02_spawn_info = new int[] { 2, 5 };
        //private int[] wave02_et03_spawn_info = new int[] { 3, 6 };

        //private int[] wave03_et01_spawn_info = new int[] { 2, 4 };
        //private int[] wave03_et02_spawn_info = new int[] { 3, 6 };
        //private int[] wave03_et03_spawn_info = new int[] { 0, 1, 5, 7 };

        public int currentWaveDeathEnemCount = 0;

        public delegate void Wave_EventHandler();
        public static event Wave_EventHandler Display_enemyDeathCount;

        private bool isWaveClear = false;
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
            instance = this;
            maxItem = maxEachItem * itemSort;
        }

        /// <summary>
        /// 20180403 SangBin : 
        /// </summary>
        private void Start()
        {
            if (TempStageManagement.instance.CurrentStageLevel == 10)
            {
                CreateEnemyObjectPool();

                GameObject.Find("EnemySpawnPoints").gameObject.transform.SetPositionAndRotation(DefenseStationCtrl.instance.DefenseStationTR.position, DefenseStationCtrl.instance.DefenseStationTR.rotation);

                tempSpawnpoints = GameObject.Find("EnemySpawnPoints").GetComponentsInChildren<Transform>();

                //if (tempSpawnpoints.Length > 0)
                //{
                //    StartCoroutine(this.ActivateEnemy());
                //}

                CreateButterFlyObjectPool();
                CreateItemObjectPool();

                Display_enemyDeathCount();

                enemyGenerationPeriod = 10.0f;
            }
            else
            {
                if (DefenseStationCtrl.instance != null && DefenseStationCtrl.instance.gameObject.activeSelf)
                {
                    CreateEnemyObjectPool();

                    GameObject.Find("EnemySpawnPoints").gameObject.transform.SetPositionAndRotation(DefenseStationCtrl.instance.DefenseStationTR.position, DefenseStationCtrl.instance.DefenseStationTR.rotation);

                    tempSpawnpoints = GameObject.Find("EnemySpawnPoints").GetComponentsInChildren<Transform>();

                    //if (tempSpawnpoints.Length > 0)
                    //{
                    //    StartCoroutine(this.ActivateEnemy());
                    //}

                }

                CreateButterFlyObjectPool();
                CreateItemObjectPool();
                //StartCoroutine(ActivateButterFly());
            }
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

        //private int[] GetSpawnInfo(int enemytype)
        //{
        //    switch (currentWaveLevel)
        //    {
        //        case 1:
        //            switch (enemytype)
        //            {
        //                case 1:
        //                    return wave01_et01_spawn_info;
        //                case 2:
        //                    return wave01_et02_spawn_info;
        //                case 3:
        //                    return wave01_et03_spawn_info;
        //            }
        //            break;
        //        case 2:
        //            switch (enemytype)
        //            {
        //                case 1:
        //                    return wave02_et01_spawn_info;
        //                case 2:
        //                    return wave02_et02_spawn_info;
        //                case 3:
        //                    return wave02_et03_spawn_info;
        //            }
        //            break;
        //        case 3:
        //            switch (enemytype)
        //            {
        //                case 1:
        //                    return wave03_et01_spawn_info;
        //                case 2:
        //                    return wave03_et02_spawn_info;
        //                case 3:
        //                    return wave03_et03_spawn_info;
        //            }
        //            break;
        //        default:
        //            return null;
        //    }

        //    return null;
        //}

        public void AddDeathCount()
        {
            if (!isWaveClear)
            {
                currentWaveDeathEnemCount++;

                if (currentWaveDeathEnemCount == GetTotalEnemyCountConst())
                {
                    isWaveClear = true;
                    StopCoroutine(ActivateEnemy());
                    currentWaveDeathEnemCount = 0;

                    StartCoroutine(LevelUpWave());
                }

                Display_enemyDeathCount();
            }
        }

        private IEnumerator LevelUpWave()
        {
            currentWaveLevel++;

            if (currentWaveLevel == 4)
            {
                GameUIManagement.instance.timerState = false;
                
                //이 시점에서 게임 플레이 시간을 랭킹 서버에 저장!!

                transform.GetChild(0).GetChild(4).gameObject.SetActive(true);
                yield return new WaitForSeconds(5.0f);
                transform.GetChild(0).GetChild(4).gameObject.SetActive(false);

                GameClear();
            }
            else
            {
                transform.GetChild(0).GetChild(4).gameObject.SetActive(true);
                yield return new WaitForSeconds(5.0f);
                transform.GetChild(0).GetChild(4).gameObject.SetActive(false);

                StartCoroutine(ActivateEnemy());
            }
        }

        public int GetTotalEnemyCountConst()
        {
            switch (currentWaveLevel)
            {
                case 1:
                    return 40;
                case 2:
                    return 52;
                case 3:
                    return 60;

                default:
                    return 1;
            }
        }

        private int GetActivationNumConst(string keyWord, int enemytype)
        {
            switch (keyWord)
            {
                case "APC":
                    switch (currentWaveLevel)
                    {
                        case 1:
                            switch (enemytype)
                            {
                                case 1:
                                    return 2;
                                case 2:
                                    return 4;
                                case 3:
                                    return 2;
                            }
                            break;
                        case 2:
                            switch (enemytype)
                            {
                                case 1:
                                    return 4;
                                case 2:
                                    return 2;
                                case 3:
                                    return 2;
                            }
                            break;
                        case 3:
                            switch (enemytype)
                            {
                                case 1:
                                    return 2;
                                case 2:
                                    return 2;
                                case 3:
                                    return 4;
                            }
                            break;
                        default:
                            return 1;
                    }
                    break;

                case "EachEnemyCountConst":
                    switch (currentWaveLevel)
                    {
                        case 1:
                            switch (enemytype)
                            {
                                case 1:
                                    return 10;
                                case 2:
                                    return 20;
                                case 3:
                                    return 10;
                            }
                            break;
                        case 2:
                            switch (enemytype)
                            {
                                case 1:
                                    return 24;
                                case 2:
                                    return 14;
                                case 3:
                                    return 14;
                            }
                            break;
                        case 3:
                            switch (enemytype)
                            {
                                case 1:
                                    return 16;
                                case 2:
                                    return 16;
                                case 3:
                                    return 28;
                            }
                            break;
                        default:
                            return 1;
                    }
                    break;
            }

            return 1;
        }

            /// <summary>
            /// 20180403 SangBin : Create enemy OP
            /// </summary>
            private void CreateEnemyObjectPool()
        {
            if (IFP.TempStageManagement.instance.CurrentStageLevel == 10)
            {
                for (int i = 0; i < 15; i++)
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

                    et01ObjectPool.Add(enemy_type01);
                    et02ObjectPool.Add(enemy_type02);
                    et03ObjectPool.Add(enemy_type03);

                    LoadingManagement.instance.FillLoadingGauge(1.5f);
                }
            }
            else
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

                    LoadingManagement.instance.FillLoadingGauge(2.5f);
                }
            }


            //LoadingManagement.instance.FillLoadingGauge(25.0f);
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
            //if (IFP.TempStageManagement.instance.CurrentStageLevel == 10)
            //{
            //    int activation_Count = 0;
            //    //int Activate_Count_et02 = 0;
            //    //int Activate_Count_et03 = 0;
            //    int[] tempArr;

            //    transform.GetChild(0).GetChild(currentWaveLevel).gameObject.SetActive(true);
            //    yield return new WaitForSeconds(3.0f);
            //    transform.GetChild(0).GetChild(currentWaveLevel).gameObject.SetActive(false);

            //    transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
            //    yield return new WaitForSeconds(1.0f);
            //    transform.GetChild(0).GetChild(0).gameObject.SetActive(false);


            //    while (thisWaveAlive)
            //    {
            //        yield return new WaitForSeconds(enemyGenerationPeriod);

            //        if (!thisWaveAlive)
            //            yield break;

            //        //요기서 위치배열 받아와서
            //        tempArr = GetSpawnInfo(1);
            //        foreach (GameObject enemy in et01ObjectPool)
            //        {
            //            if (!enemy.activeSelf)
            //            {
            //                enemy.transform.position = tempSpawnpoints[tempArr[activation_Count]].position;
            //                StartCoroutine(OpenGate(tempSpawnpoints[tempArr[activation_Count]].GetChild(0).gameObject));
            //                enemy.transform.parent = GoogleARCore.IF.TowerBuildController.instance.DefenseStation_Anchor_Tr;
            //                enemy.SetActive(true);
            //                activation_Count++;
            //            }

            //            if (activation_Count == tempArr.Length)
            //            {
            //                activation_Count = 0;
            //                break;
            //            }
            //        }

            //        tempArr = GetSpawnInfo(2);
            //        foreach (GameObject enemy in et02ObjectPool)
            //        {
            //            if (!enemy.activeSelf)
            //            {
            //                enemy.transform.position = tempSpawnpoints[tempArr[activation_Count]].position;
            //                StartCoroutine(OpenGate(tempSpawnpoints[tempArr[activation_Count]].GetChild(0).gameObject));
            //                enemy.transform.parent = GoogleARCore.IF.TowerBuildController.instance.DefenseStation_Anchor_Tr;
            //                enemy.SetActive(true);
            //                activation_Count++;


            //            }

            //            if (activation_Count == tempArr.Length)
            //            {
            //                activation_Count = 0;
            //                break;
            //            }
            //        }

            //        tempArr = GetSpawnInfo(3);
            //        foreach (GameObject enemy in et03ObjectPool)
            //        {
            //            if (!enemy.activeSelf)
            //            {
            //                enemy.transform.position = tempSpawnpoints[tempArr[activation_Count]].position;
            //                StartCoroutine(OpenGate(tempSpawnpoints[tempArr[activation_Count]].GetChild(0).gameObject));
            //                enemy.transform.parent = GoogleARCore.IF.TowerBuildController.instance.DefenseStation_Anchor_Tr;
            //                enemy.SetActive(true);
            //                activation_Count++;


            //            }

            //            if (activation_Count == tempArr.Length)
            //            {
            //                activation_Count = 0;
            //                break;
            //            }
            //        }

            //    }
            //}
            if (IFP.TempStageManagement.instance.CurrentStageLevel == 10)
            {
                isWaveClear = false;
                currentWaveDeathEnemCount = 0;
                Display_enemyDeathCount();

                int activation_Count = 0;

                int et01_Current_Count = 0;
                int et02_Current_Count = 0;
                int et03_Current_Count = 0;

                int et01_Max_Count = GetActivationNumConst("EachEnemyCountConst", 1);
                int et02_Max_Count = GetActivationNumConst("EachEnemyCountConst", 2);
                int et03_Max_Count = GetActivationNumConst("EachEnemyCountConst", 3);

                yield return new WaitForSeconds(1.0f);
                transform.GetChild(0).GetChild(currentWaveLevel).gameObject.SetActive(true);
                yield return new WaitForSeconds(4.0f);
                transform.GetChild(0).GetChild(currentWaveLevel).gameObject.SetActive(false);

                transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
                yield return new WaitForSeconds(1.0f);
                transform.GetChild(0).GetChild(0).gameObject.SetActive(false);


                while (thisWaveAlive)
                {
                    if (!thisWaveAlive)
                        yield break;

                    if (et01_Current_Count < et01_Max_Count)
                    {
                        foreach (GameObject enemy in et01ObjectPool)
                        {
                            if (!enemy.activeSelf)
                            {
                                int index = Random.Range(0, tempSpawnpoints.Length);
                                enemy.transform.position = tempSpawnpoints[index].position;
                                StartCoroutine(OpenGate(tempSpawnpoints[index].GetChild(0).gameObject));
                                enemy.transform.parent = GoogleARCore.IF.TowerBuildController.instance.DefenseStation_Anchor_Tr;
                                enemy.SetActive(true);
                                activation_Count++;
                                et01_Current_Count++;
                            }

                            if (activation_Count == GetActivationNumConst("APC", 1))
                            {
                                activation_Count = 0;
                                break;
                            }
                        }
                    }

                    if (et02_Current_Count < et02_Max_Count)
                    {
                        foreach (GameObject enemy in et02ObjectPool)
                        {
                            if (!enemy.activeSelf)
                            {
                                int index = Random.Range(0, tempSpawnpoints.Length);
                                enemy.transform.position = tempSpawnpoints[index].position;
                                StartCoroutine(OpenGate(tempSpawnpoints[index].GetChild(0).gameObject));
                                enemy.transform.parent = GoogleARCore.IF.TowerBuildController.instance.DefenseStation_Anchor_Tr;
                                enemy.SetActive(true);
                                activation_Count++;
                                et02_Current_Count++;
                            }

                            if (activation_Count == GetActivationNumConst("APC", 2))
                            {
                                activation_Count = 0;
                                break;
                            }
                        }
                    }

                    if (et03_Current_Count < et03_Max_Count)
                    {
                        foreach (GameObject enemy in et03ObjectPool)
                        {
                            if (!enemy.activeSelf)
                            {
                                int index = Random.Range(0, tempSpawnpoints.Length);
                                enemy.transform.position = tempSpawnpoints[index].position;
                                StartCoroutine(OpenGate(tempSpawnpoints[index].GetChild(0).gameObject));
                                enemy.transform.parent = GoogleARCore.IF.TowerBuildController.instance.DefenseStation_Anchor_Tr;
                                enemy.SetActive(true);
                                activation_Count++;
                                et03_Current_Count++;
                            }

                            if (activation_Count == GetActivationNumConst("APC", 3))
                            {
                                activation_Count = 0;
                                break;
                            }
                        }
                    }

                    yield return new WaitForSeconds(enemyGenerationPeriod);
                }
            }
            else
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
            LoadingManagement.instance.FillLoadingGauge(2.0f);
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

        public void StartGame()
        {
            StartCoroutine(this.ActivateEnemy());
            StartCoroutine(ActivateButterFly());
        }
    }
}