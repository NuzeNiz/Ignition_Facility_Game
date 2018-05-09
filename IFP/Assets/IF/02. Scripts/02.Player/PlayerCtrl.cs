using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

namespace IF
{
    [RequireComponent(typeof(AudioSource))]
    public class PlayerCtrl : MonoBehaviour
    {
        /// <summary>
        /// 20180403 SangBin : Singletone Pattern
        /// </summary>
        public static PlayerCtrl PlayerInstance = null;

        /// <summary>
        /// 20180403 SangBin : Projectile Style Bullet Prefabs
        /// </summary>
        public GameObject bulletPref;

        /// <summary>
        /// 20180403 SangBin : Fire Sound File
        /// </summary>
        public AudioClip fireSoundFile;

        /// <summary>
        /// 20180403 SangBin : Muzzleflash Renderer for Switching
        /// </summary>
        public MeshRenderer muzzleFlash;

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
        private double PlayerStrikingPower = 50.0d;

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
        private double playerHP = 100.0d;

        /// <summary>
        /// 20180403 SangBin : Player's Current Health Power Property
        /// </summary>
        public double PlayerHP { get { return playerHP; } }

        /// <summary>
        /// 20180430 SangBin : Player Bullet Flare Effect
        /// </summary>
        [SerializeField]
        private GameObject FlareEffect;

        //-----------------------------------------------------------------------------------------------------------------------------

        private void Awake()
        {
            PlayerInstance = this;
            playerTr = GameObject.Find("First Person Camera").GetComponent<Transform>();
            muzzleFlash.enabled = false;
            playerMaxHP = playerHP;

        }

        void Update() {
            this.gameObject.transform.SetPositionAndRotation(playerTr.position, playerTr.rotation);

            Touch touch;
            if ((touch = Input.GetTouch(0)).phase == TouchPhase.Began)
            {
                Fire();

                /// <summary>
                /// 20180403 SangBin : Player's RayCast Shoot to Enemy
                /// 20180418 SangBin : + to Item
                /// 20180427 SangBin : + to Butterfly
                /// </summary>
                RaycastHit hitinfo;

                if (Physics.Raycast(playerTr.position, playerTr.forward, out hitinfo, rayMaxDistance))                
                {
                    if(hitinfo.collider.gameObject.layer == 8)
                    //if (hitinfo.collider.tag == "ENEMY_BEE")
                    {
                        object[] parameters = new object[2]; 
                        parameters[0] = hitinfo.point;
                        parameters[1] = PlayerStrikingPower;
                        GameObject flare = (GameObject)Instantiate(FlareEffect, hitinfo.collider.gameObject.transform.position, Quaternion.identity);
                        hitinfo.collider.gameObject.SendMessage("OnDamaged", parameters, SendMessageOptions.DontRequireReceiver);
                        Destroy(flare, 3.0f);
                    }
                    //else if (hitinfo.collider.tag == "ITEM")
                    //{
                    //    hitinfo.collider.gameObject.SendMessage("OnHit", SendMessageOptions.DontRequireReceiver);
                    //}
                    //else if (hitinfo.collider.tag == "ENEMY_BUTTERFLY")
                    //{
                    //    hitinfo.collider.gameObject.SendMessage("OnHit", SendMessageOptions.DontRequireReceiver);
                    //}
                }
            }
        }

        /// <summary>
        /// 20180403 SangBin : Collision event between player and enemy bullet
        /// </summary>
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag == "ENEMY_BEE_STINGER")
            {
                //playerHP -= 2.0d; // test
                playerHP -= collision.gameObject.GetComponent<BeeStingerCtrl>().stingerDamage;
                collision.gameObject.GetComponent<TrailRenderer>().enabled = false;
                collision.gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// 20180403 SangBin : Player's Shooting
        /// </summary>
        void Fire()
        {
            StartCoroutine(this.ShowMuzzleFlash());
            GameLogicManagement.GLM_Instance.SoundEffect(playerTr.position, fireSoundFile);
        }

        /// <summary>
        /// 20180403 SangBin : Coroutine function for muzzleflash
        /// </summary>
        IEnumerator ShowMuzzleFlash()
        {

            float scale = Random.Range(0.3f, 0.5f);
            muzzleFlash.transform.localScale = Vector3.one * scale;

            Quaternion rotate = Quaternion.Euler(0, 0, Random.Range(0, 360));
            muzzleFlash.transform.localRotation = rotate;

            muzzleFlash.enabled = true;
            yield return new WaitForSeconds(Random.Range(0.05f, 0.3f));
            muzzleFlash.enabled = false;
        }
    }
}