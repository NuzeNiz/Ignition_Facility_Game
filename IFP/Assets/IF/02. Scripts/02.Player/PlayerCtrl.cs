using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

namespace IFP
{
    [RequireComponent(typeof(AudioSource))]
    public class PlayerCtrl : MonoBehaviour
    {
        #region Fields : Player Statistic

        /// <summary>
        /// 20180403 SangBin : Singletone Pattern
        /// </summary>
        public static PlayerCtrl instance = null;

        /// <summary>
        /// 20180403 SangBin : Projectile Style Bullet Prefabs
        /// </summary>
        //public GameObject bulletPref;

        /// <summary>
        /// 20180403 SangBin : Fire Sound File
        /// </summary>
        //public AudioClip fireSoundFile;

        /// <summary>
        /// 20180403 SangBin : Muzzleflash Renderer for Switching
        /// </summary>
        [HideInInspector]
        //public MeshRenderer muzzleFlash;

        /// <summary>
        /// 20180403 SangBin : Player's Transform Syncronized With The Main First Person Camera
        /// </summary>
        private Transform playerTr;

        /// <summary>
        /// 20180403 SangBin : Player's Transform Property
        /// </summary>
        public Transform PlayerTr { get { return playerTr; } set { playerTr = value; } }

        /// <summary>
        /// 20180403 SangBin : Shooting Ray Max Distance
        /// </summary>
        private float rayMaxDistance = 6.0f;

        /// <summary>
        /// 20180403 SangBin : Player's Striking Power
        /// </summary>
        //private double PlayerStrikingPower = 50.0d;

        /// <summary>
        /// 20180403 SangBin : Player's Max Health Power
        /// </summary>
        private double playerMaxHP;

        /// <summary>
        /// 20180403 SangBin : Player's Max Health Power Property
        /// </summary>
        public double PlayerMaxHP { get { return playerMaxHP; } }

        /// <summary>
        /// 20180403 SangBin : Player's Current Health Power
        /// </summary>
        private double playerHP;

        /// <summary>
        /// 20180403 SangBin : Player's Current Health Power Property
        /// </summary>
        public double PlayerHP { get { return playerHP; } set { playerHP = value; } }

        #endregion

        #region Fields : Camera Shaking
        /// <summary>
        /// 20180514 SangBin : Shaking Camera fields
        /// </summary>
        private float shakeTime = 1.5f;

        /// <summary>
        /// 20180514 SangBin : Shaking Camera fields
        /// </summary>
        private float shakeAmount = 0.1f;

        /// <summary>
        /// 20180514 SangBin : Shaking Camera fields
        /// </summary>
        private float shakeSpeed = 4.0f;
        #endregion

        /// <summary>
        /// 20180430 SangBin : 
        /// </summary>
        public Transform itemtarget;

        /// <summary>
        /// 20180530 SangBin : 
        /// </summary>
        public delegate void PlayerEventHandler();
        public static event PlayerEventHandler PlayerDamaged;
        //public static event PlayerEventHandler Player_FallDown;
        //-----------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// 20180403 SangBin : 
        /// </summary>
        private void Awake()
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
            //muzzleFlash.enabled = false;
            if (IFP.TempStageManagement.instance.CurrentStageLevel == 10)
                playerHP = 1000.0d;
            else
                playerHP = 500.0d;

            playerMaxHP = playerHP;

        }

        /// <summary>
        /// 20180403 SangBin : 
        /// </summary>
        private void Start()
        {
            playerTr = GameObject.Find("First Person Camera").GetComponent<Transform>();

            StartCoroutine(SetPositionAndRotation());
        }

        /// <summary>
        /// 20180403 SangBin : 
        /// </summary>
        void Update() {
            PlayerShoot();
        }

        /// <summary>
        /// 20180403 SangBin : Player's RayCast Shoot to Enemy
        /// 20180418 SangBin : + to Item
        /// 20180427 SangBin : + to Butterfly
        /// 20180515 SangBin : ReFactorying with weapons
        /// 20180516 SangBin : Add weapon Type03,04
        /// </summary>
        private void PlayerShoot()
        {
            Touch myTouch;

            RaycastHit hitinfo;

            if (Input.touchCount > 0)
            {
                if (WeaponCtrl.instance.CurrentWeaponType == WeaponCtrl.WeaponTypeEnum.weaponType02)
                {
                    if (WeaponCtrl.instance.amm_wt02 > 0.0f)
                    {
                        myTouch = Input.GetTouch(0);
                        if (myTouch.phase == TouchPhase.Stationary || myTouch.phase == TouchPhase.Moved)
                        {
                            WeaponCtrl.instance.MakeWeaponShotEffect();

                            WeaponCtrl.instance.WeaponFunc();

                            WeaponCtrl.instance.amm_wt02 -= Time.deltaTime;

                            if (WeaponCtrl.instance.amm_wt02 < 0.0f)
                                WeaponCtrl.instance.amm_wt02 = 0.0f;

                        }
                        else if (myTouch.phase == TouchPhase.Ended)
                        {
                            WeaponCtrl.instance.StopFlame();
                        }
                    }
                    else
                    {
                        WeaponCtrl.instance.StopFlame();
                    }
                }
                else if (WeaponCtrl.instance.CurrentWeaponType == WeaponCtrl.WeaponTypeEnum.weaponType03)
                {
                    if (WeaponCtrl.instance.amm_wt03 > 0)
                    {
                        for (int i = 0; i < Input.touchCount; i++)
                        {
                            myTouch = Input.GetTouch(i);
                            if (myTouch.phase == TouchPhase.Began)
                            {
                                //WeaponClass.instance.MakeWeaponShotEffect();
                                WeaponCtrl.instance.WeaponFunc();
                            }
                        }
                    }
                    else
                    {

                    }
                }
                else if (WeaponCtrl.instance.CurrentWeaponType == WeaponCtrl.WeaponTypeEnum.weaponType04)
                {
                    if (WeaponCtrl.instance.amm_wt04 > 0)
                    {
                        for (int i = 0; i < Input.touchCount; i++)
                        {
                            myTouch = Input.GetTouch(i);
                            if (myTouch.phase == TouchPhase.Began)
                            {
                                //WeaponClass.instance.MakeWeaponShotEffect();
                                WeaponCtrl.instance.WeaponFunc();
                            }
                        }
                    }
                    else
                    {

                    }
                }
                else
                {
                    for (int i = 0; i < Input.touchCount; i++)
                    {
                        myTouch = Input.GetTouch(i);
                        if (myTouch.phase == TouchPhase.Began)
                        {
                            WeaponCtrl.instance.MakeWeaponShotEffect();

                            //RaycastHit hitinfo;

                            if (Physics.Raycast(playerTr.position, playerTr.forward, out hitinfo, rayMaxDistance))
                            {
                                if (hitinfo.collider.gameObject.layer == 8)
                                {
                                    WeaponCtrl.instance.WeaponFunc(ref hitinfo);
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 20180403 SangBin : Collision event between player and enemy bullet
        /// </summary>
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag == "ENEMY_TYPE01_PROJECTILE")
            {
                StartCoroutine(CameraShake());
                //playerHP -= 2.0d; // test
                //playerHP -= collision.gameObject.GetComponent<EnemyProjectileType01Ctrl>().projectileDamage;
                playerHP -= BalanceManagement.instance.EnemyProjectile01damage;
                PlayerDamaged();

                //collision.gameObject.GetComponent<TrailRenderer>().enabled = false;
                collision.gameObject.SetActive(false);

                if (playerHP <= 0.0d)
                {
                    GameManagement.instance.GameOver();
                }
            }
        }

        /// <summary>
        /// 20180403 SangBin : Player's Shooting
        /// </summary>
        //void Fire()
        //{
        //    //StartCoroutine(this.ShowMuzzleFlash());
        //    GameManagement.instance.SoundEffect(playerTr.position, fireSoundFile);


        //}

        /// <summary>
        /// 20180403 SangBin : Coroutine function for muzzleflash
        /// </summary>
        //IEnumerator ShowMuzzleFlash()
        //{

        //    float scale = Random.Range(0.3f, 0.5f);
        //    muzzleFlash.transform.localScale = Vector3.one * scale;

        //    Quaternion rotate = Quaternion.Euler(0, 0, Random.Range(0, 360));
        //    muzzleFlash.transform.localRotation = rotate;

        //    muzzleFlash.enabled = true;
        //    yield return new WaitForSeconds(Random.Range(0.05f, 0.3f));
        //    muzzleFlash.enabled = false;
        //}

        /// <summary>
        /// 20180514 SangBin : Shaking Camera
        /// </summary>
        private IEnumerator CameraShake()
        {
            StopCoroutine(SetPositionAndRotation());
            Vector3 originPos = playerTr.position;

            float elapsedTime = 0.0f;

            while (elapsedTime < shakeTime)
            {
                Vector3 randomPoint = originPos + Random.insideUnitSphere * shakeAmount;

                playerTr.position = Vector3.Lerp(playerTr.position, randomPoint, Time.deltaTime * shakeSpeed);

                yield return null;

                elapsedTime += Time.deltaTime;
            }

            playerTr.position = originPos;
            StartCoroutine(SetPositionAndRotation());
        }

        /// <summary>
        /// 20180514 SangBin : 
        /// </summary>
        private IEnumerator SetPositionAndRotation()
        {
            //while (GameManagement.instance.ThisStageAlive)
            while(true)
            {
                transform.SetPositionAndRotation(playerTr.position, playerTr.rotation);
                yield return null;
            }

        }

        /// <summary>
        /// 20180530 SangBin : 
        /// </summary>
        protected void OnParticleCollision(GameObject other)
        {
            StartCoroutine(CameraShake());
            //playerHP -= BalanceManagement.instance.EnemyProjectile01damage;
            playerHP -= 1.0d;
            PlayerDamaged();

            if (playerHP <= 0.0d)
            {
                GameManagement.instance.GameOver();
            }
        }

        /// <summary>
        /// 20180617 SangBin : 
        /// </summary>
        private void OnTriggerEnter(Collider coll)
        {
            if (coll.gameObject.tag == "ENEMY_TYPE03_BOX")
            {
                StartCoroutine(CameraShake());

                //playerHP -= BalanceManagement.instance.EnemyProjectile01damage;
                playerHP -= 12.0d;
                PlayerDamaged();

                if (playerHP <= 0.0d)
                {
                    GameManagement.instance.GameOver();
                }
            }
        }

        //private void OnTriggerStay(Collider coll)
        //{
        //    if (coll.gameObject.tag == "ENEMY_TYPE03_BOX")
        //    {
        //        StartCoroutine(CameraShake());

        //        //playerHP -= BalanceManagement.instance.EnemyProjectile01damage;
        //        playerHP -= 10.0d;
        //        PlayerDamaged();

        //        if (playerHP <= 0.0d)
        //        {
        //            GameManagement.instance.GameOver();
        //        }
        //    }
        //}
    }
}