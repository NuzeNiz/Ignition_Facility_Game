using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

namespace IFP
{
    [RequireComponent(typeof(AudioSource))]
    public class PlayerCtrl : MonoBehaviour
    {
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
        private float rayMaxDistance = 10.0f;

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
        public double PlayerHP { get { return playerHP; } }

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
        /// 20180530 SangBin : 
        /// </summary>
        public delegate void PlayerEventHandler();
        public static event PlayerEventHandler PlayerDamaged;
        //public static event PlayerEventHandler Player_FallDown;
        //-----------------------------------------------------------------------------------------------------------------------------

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
            playerHP = 100.0d;
            playerMaxHP = playerHP;

        }

        private void Start()
        {
            playerTr = GameObject.Find("First Person Camera").GetComponent<Transform>();

            StartCoroutine(SetPositionAndRotation());
        }

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
                    myTouch = Input.GetTouch(0);
                    if (myTouch.phase == TouchPhase.Stationary || myTouch.phase == TouchPhase.Moved)
                    {
                        WeaponCtrl.instance.MakeWeaponShotEffect();

                        ////RaycastHit hitinfo;

                        ////if (Physics.Raycast(playerTr.position, playerTr.forward, out hitinfo, rayMaxDistance))
                        //if (Physics.SphereCast(playerTr.position, 0.2f, playerTr.forward, out hitinfo, rayMaxDistance))
                        //{
                        //    if (hitinfo.collider.gameObject.layer == 8)
                        //    {
                                WeaponCtrl.instance.WeaponFunc();
                        //    }
                        //}
                    }
                    else if (myTouch.phase == TouchPhase.Ended)
                    {
                        WeaponCtrl.instance.StopFlame();
                    }
                }
                else if (WeaponCtrl.instance.CurrentWeaponType == WeaponCtrl.WeaponTypeEnum.weaponType03 || WeaponCtrl.instance.CurrentWeaponType == WeaponCtrl.WeaponTypeEnum.weaponType04)
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
        virtual protected void OnParticleCollision(GameObject other)
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
    }
}