using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IFP
{
    public class WeaponCtrl : MonoBehaviour
    {
        /// <summary>
        /// ???????? : SungJun :
        /// </summary>
        private WeaponSubject weaponSubject;

        #region Fields : Weapon __
        /// <summary>
        /// 20180515 SangBin : 
        /// </summary>
        public static WeaponCtrl instance = null;

        /// <summary>
        /// 20180515 SangBin : 
        /// </summary>
        [SerializeField]
        private Transform holeSpot;

        /// <summary>
        /// 20180515 SangBin : 
        /// </summary>
        private ParticleSystem holeSparkPS;


        /// <summary>
        /// 20180515 SangBin : 
        /// </summary>
        [SerializeField]
        private Transform shotSpot;

        #endregion

        #region Fields : Weapon Type
        /// <summary>
        /// 20180514 SangBin : Weapon Type Enum
        /// </summary>
        [HideInInspector]
        public enum WeaponTypeEnum { weaponType01, weaponType02, weaponType03, weaponType04 };

        //public List<WeaponTypeEnum> weaponList = new List<WeaponTypeEnum>();

        /// <summary>
        /// 20180515 SangBin : 
        /// </summary>
        public Dictionary<int, WeaponTypeEnum> weaponDic = new Dictionary<int, WeaponTypeEnum>();

        /// <summary>
        /// 20180514 SangBin : Weapon Type
        /// </summary>
        [HideInInspector]
        private WeaponTypeEnum currentWeaponType;

        /// <summary>
        /// 20180514 SangBin : Weapon Type
        /// </summary>
        public WeaponTypeEnum CurrentWeaponType { get { return currentWeaponType; } set{ currentWeaponType = value; } }

        #endregion

        #region Fields : Weapon Type01 Resources
        /// <summary>
        /// 20180515 SangBin : 
        /// </summary>
        [SerializeField]
        private GameObject FlareEffect;

        /// <summary>
        /// 20180515 SangBin : 
        /// </summary>
        [SerializeField]
        private GameObject weaponType01_shot_HoleEffect;

        /// <summary>
        /// 20180515 SangBin : 
        /// </summary>
        [SerializeField]
        private AudioClip weaponType01_shot_sound;
        #endregion

        #region Fields : Weapon Type02 Resources
        #endregion

        #region Fields : Weapon Type03 Resources
        /// <summary>
        /// 20180515 SangBin : 
        /// </summary>
        [SerializeField]
        private GameObject weaponType03_shot_Effect;
        #endregion

        #region Fields : Weapon Type04 Resources
        /// <summary>
        /// 20180515 SangBin : 
        /// </summary>
        [SerializeField]
        private GameObject weaponType04_shot_Effect;
        #endregion

        #region Fields : Textures & Material
        /// <summary>
        /// 20180515 SangBin : Weapon Material
        /// </summary>
        [SerializeField]
        private Material currentMaterial;

        /// <summary>
        /// 20180515 SangBin : Weapon Type01 Textures
        /// </summary>
        [SerializeField]
        private Texture weaponType01_01;
        [SerializeField]
        private Texture weaponType01_02;
        [SerializeField]
        private Texture weaponType01_03;
        [SerializeField]
        private Texture weaponType01_04;

        /// <summary>
        /// 20180515 SangBin : Weapon Type02 Textures
        /// </summary>
        [SerializeField]
        private Texture weaponType02_01;
        [SerializeField]
        private Texture weaponType02_02;
        [SerializeField]
        private Texture weaponType02_03;
        [SerializeField]
        private Texture weaponType02_04;

        /// <summary>
        /// 20180515 SangBin : Weapon Type03 Textures
        /// </summary>
        [SerializeField]
        private Texture weaponType03_01;
        [SerializeField]
        private Texture weaponType03_02;
        [SerializeField]
        private Texture weaponType03_03;
        [SerializeField]
        private Texture weaponType03_04;

        /// <summary>
        /// 20180515 SangBin : Weapon Type04 Textures
        /// </summary>
        [SerializeField]
        private Texture weaponType04_01;
        [SerializeField]
        private Texture weaponType04_02;
        [SerializeField]
        private Texture weaponType04_03;
        [SerializeField]
        private Texture weaponType04_04;
        #endregion

        #region Fields : Ammunition
        public int amm_wt01 = 0;
        public float amm_wt02 = 30.0f;
        public int amm_wt03 = 100;
        public int amm_wt04 = 100;

        public delegate void Weapon_EventHandler();
        public static event Weapon_EventHandler Display_Ammu;
        #endregion

        //----------------------------------------------------------------------------------

        /// <summary>
        /// 20180515 SangBin : 
        /// ???????? SungJun : Notify to WeaponSlots
        /// </summary>
        private void Awake()
        {
            //if (instance == null)
            //{
            //    instance = this;
            //    DontDestroyOnLoad(gameObject);
            //    weaponTypeDic.Add(1, WeaponTypeEnum.weaponType01);
            //    currentWeaponType = weaponTypeDic[1];
            //}
            //else
            //{
            //    DestroyImmediate(this);
            //}
            instance = this;
            //weaponDic.Add(0, WeaponTypeEnum.weaponType01);
            //currentWeaponType = weaponDic[0];
            //GetWeapons();
            holeSparkPS = transform.GetChild(1).gameObject.GetComponent<ParticleSystem>();

            //test
            currentWeaponType = WeaponTypeEnum.weaponType01;

 
            weaponSubject = ItemWindow.Instance.WeaponSubject;
            //weaponSubject.NotifyNewItem(WeaponTypeEnum.weaponType01);

            GetAmmu();
            StartCoroutine(SaveAmmuPerPeriod());
        }

        private void OnApplicationQuit()
        {
            SaveAmmu();
        }


        private void OnDestroy()
        {
            SaveAmmu();
        }

        private void OnEnable()
        {
            EMS_LongRange.AbsorbAmmu += this.AbsorbAmmu;
            EMS_ShortRange.AbsorbAmmu += this.AbsorbAmmu;
        }

        private void OnDisable()
        {
            StopAllCoroutines();
            EMS_LongRange.AbsorbAmmu -= this.AbsorbAmmu;
            EMS_ShortRange.AbsorbAmmu -= this.AbsorbAmmu;
        }

        IEnumerator SaveAmmuPerPeriod()
        {
            yield return new WaitForSeconds(15.0f);
            SaveAmmu();
        }

        /// <summary>
        /// 20180515 SangBin : 
        /// </summary>
        public void WeaponFunc(ref RaycastHit hitinfo)
        {
            object[] parameters = new object[2];
            switch (currentWeaponType)
            {
                case WeaponTypeEnum.weaponType01 :
                    parameters[0] = BalanceManagement.instance.CalcPlayerStrkingPower(hitinfo.collider.gameObject.tag, currentWeaponType);
                    parameters[1] = false;

                    GameObject flare = Instantiate(FlareEffect, hitinfo.collider.gameObject.transform.position + (hitinfo.collider.gameObject.transform.up *0.4f), Quaternion.identity);
                    hitinfo.collider.gameObject.SendMessage("OnDamaged", parameters, SendMessageOptions.DontRequireReceiver);
                    Destroy(flare, 3.0f);
                    break;

                case WeaponTypeEnum.weaponType02 :
                    ///Flame Thrower Attack Way 1
                    //parameters[0] = BalanceManagement.instance.PlayerStrkingPower(hitinfo.collider.gameObject.tag, currentWeaponType);
                    //parameters[1] = true;
                    //hitinfo.collider.gameObject.SendMessage("OnDamaged", parameters, SendMessageOptions.DontRequireReceiver);

                    ///Flame Thrower Attack Way 2
                    //if (!transform.GetChild(3).gameObject.activeSelf)
                    //{
                    //    transform.GetChild(3).gameObject.SetActive(true);
                    //}

                    ///Flame Thrower Attack Way 2 - change to sphere cast
                    //parameters[0] = BalanceManagement.instance.CalcPlayerStrkingPower(hitinfo.collider.gameObject.tag, currentWeaponType);
                    //parameters[1] = false;
                    //hitinfo.collider.gameObject.SendMessage("OnDamaged", parameters, SendMessageOptions.DontRequireReceiver);
                    break;
            }
        }

        /// <summary>
        /// 20180516 SangBin : 
        /// </summary>
        public void WeaponFunc()
        {
            switch (currentWeaponType)
            {
                case WeaponTypeEnum.weaponType02:
                    if (!transform.GetChild(3).gameObject.activeSelf)
                    {
                        transform.GetChild(3).gameObject.SetActive(true);
                        //amm_wt02 -= Time.deltaTime;
                    }
                    break;

                case WeaponTypeEnum.weaponType03:
                    StartCoroutine(ProjectileShot(weaponType03_shot_Effect));
                    amm_wt03--;
                    break;

                case WeaponTypeEnum.weaponType04:
                    StartCoroutine(ProjectileShot(weaponType04_shot_Effect));
                    amm_wt04--;
                    break;
            }

            Display_Ammu();
        }

        /// <summary>
        /// 20180515 SangBin : 
        /// </summary>
        public void MakeWeaponShotEffect()
        {
            //if (transform.GetChild(2).gameObject.activeSelf)
            //    transform.GetChild(2).gameObject.SetActive(false);

            switch (currentWeaponType)
            {
                case WeaponTypeEnum.weaponType01:
                    GameObject holeEffect = Instantiate(weaponType01_shot_HoleEffect, holeSpot.position, Quaternion.identity);
                    holeEffect.transform.SetParent(transform);
                    //GameObject muzzleFlash = Instantiate(weaponType01_shot_HoleEffect, shotSpot.position, Quaternion.identity);
                    GameManagement.instance.SoundEffect(transform.position, weaponType01_shot_sound);
                    Destroy(holeEffect, 1.5f);
                    //Destroy(muzzleFlash, 1.5f);
                    break;

                case WeaponTypeEnum.weaponType02:
                    if (!transform.GetChild(2).gameObject.activeSelf)
                        transform.GetChild(2).gameObject.SetActive(true);
                    break;

                case WeaponTypeEnum.weaponType03:
                    //이펙트 고민 중
                    break;

                case WeaponTypeEnum.weaponType04:
                    //이펙트 고민 중
                    break;
            }
        }

        /// <summary>
        /// 20180516 SangBin : 
        /// </summary>
        private IEnumerator ProjectileShot(GameObject eff)
        {
            GameObject effect = Instantiate(eff, shotSpot.position, shotSpot.rotation);

            yield return new WaitForSeconds(3.0f);

            if(effect.activeSelf)
                Destroy(effect, 1.0f);

        }

        /// <summary>
        /// 20180516 SangBin : 
        /// </summary>
        public void StopFlame()
        {
            transform.GetChild(3).gameObject.SetActive(false);
            transform.GetChild(2).gameObject.SetActive(false);
        }

        /// <summary>
        /// 20180702 SangBin :
        /// </summary>
        private void GetAmmu()
        {
            //if (PlayerPrefs.HasKey("ammunition_wt01"))
            //{
            //    amm_wt01 = PlayerPrefs.GetInt("ammunition_wt01", 0);
            //}

            if (PlayerPrefs.HasKey("ammunition_wt02"))
            {
                amm_wt02 = PlayerPrefs.GetFloat("ammunition_wt02", 0.0f);
            }

            if (PlayerPrefs.HasKey("ammunition_wt03"))
            {
                amm_wt03 = PlayerPrefs.GetInt("ammunition_wt03", 0);
            }

            if (PlayerPrefs.HasKey("ammunition_wt04"))
            {
                amm_wt04 = PlayerPrefs.GetInt("ammunition_wt04", 0);
            }

            Display_Ammu();
        }

        /// <summary>
        /// 20180702 SangBin :
        /// </summary>
        private void SaveAmmu()
        {
            //PlayerPrefs.SetInt("ammunition_wt01", amm_wt01);
            PlayerPrefs.SetFloat("ammunition_wt02", amm_wt02);
            PlayerPrefs.SetInt("ammunition_wt03", amm_wt03);
            PlayerPrefs.SetInt("ammunition_wt04", amm_wt04);
        }

        private void AbsorbAmmu()
        {
            switch (TempStageManagement.instance.CurrentStageLevel)
            {
                case 2:
                    amm_wt02 += 30.0f;
                    break;
                case 3:
                    amm_wt03 += 30;
                    break;
                case 4:
                    amm_wt04 += 30;
                    break;

            }

            Display_Ammu();
        }

        /// <summary>
        /// 20180515 SangBin : 
        /// 20180517 SeongJun : Item Add Routin
        /// </summary>
        private void GetWeapons()
        {
            WeaponTypeEnum newType = currentWeaponType;
            if (PlayerPrefs.HasKey("weaponType02"))
            {
                weaponDic.Add(1, newType = WeaponTypeEnum.weaponType02);
            }
            if (PlayerPrefs.HasKey("weaponType03"))
            {
                weaponDic.Add(2, newType = WeaponTypeEnum.weaponType03);
            }
            if (PlayerPrefs.HasKey("weaponType04"))
            {
                weaponDic.Add(3, newType = WeaponTypeEnum.weaponType04);
            }

            currentWeaponType = newType;
        }

        /// <summary>
        /// 20180515 SangBin : 
        /// </summary>
        public void SaveWeapons()
        {

            //중간 보스 구현 후에 연동 예정
            PlayerPrefs.SetInt("weaponType02", 1);
        }

        /// <summary>
        /// 20180515 SangBin : 
        /// </summary>
        public void SwitchWeapon(WeaponCtrl.WeaponTypeEnum selectedWeapon)
        {
            StopFlame();
            currentWeaponType = selectedWeapon;
            StartCoroutine(MakeSwitchingEffect());
            weaponSubject.Notify();
            Display_Ammu();
        }

        /// <summary>
        /// 20180515 SangBin : 
        /// </summary>
        private IEnumerator MakeSwitchingEffect()
        {
            holeSparkPS.Stop();
            var mainPS = holeSparkPS.main;
            switch (currentWeaponType)
            {
                case WeaponTypeEnum.weaponType01:
                    //transform.GetChild(1).gameObject.GetComponent<ParticleSystem>().main.startColor = new Color(8.0f, 184.0f, 72.0f, 255.0f);
                    mainPS.startColor = new Color(196.0f, 199.0f, 6.0f, 255.0f);
                    currentMaterial.SetTexture("_EmissionMap", weaponType01_01);
                    transform.GetChild(0).transform.GetChild(0).gameObject.SetActive(true);
                    yield return new WaitForSeconds(3.0f);
                    transform.GetChild(0).transform.GetChild(0).gameObject.SetActive(false);
                    break;

                case WeaponTypeEnum.weaponType02:
                    mainPS.startColor = new Color(199.0f, 6.0f, 26.0f, 255.0f);
                    currentMaterial.SetTexture("_EmissionMap", weaponType02_01);
                    transform.GetChild(0).transform.GetChild(1).gameObject.SetActive(true);
                    yield return new WaitForSeconds(3.0f);
                    transform.GetChild(0).transform.GetChild(1).gameObject.SetActive(false);
                    break;

                case WeaponTypeEnum.weaponType03:
                    mainPS.startColor = new Color(6.0f, 39.0f, 199.0f, 255.0f);
                    currentMaterial.SetTexture("_EmissionMap", weaponType03_01);
                    transform.GetChild(0).transform.GetChild(2).gameObject.SetActive(true);
                    yield return new WaitForSeconds(3.0f);
                    transform.GetChild(0).transform.GetChild(2).gameObject.SetActive(false);
                    break;

                case WeaponTypeEnum.weaponType04:
                    mainPS.startColor = new Color(8.0f, 184.0f, 72.0f, 255.0f);
                    currentMaterial.SetTexture("_EmissionMap", weaponType04_01);
                    transform.GetChild(0).transform.GetChild(3).gameObject.SetActive(true);
                    yield return new WaitForSeconds(3.0f);
                    transform.GetChild(0).transform.GetChild(3).gameObject.SetActive(false);
                    break;
            }
            //yield return null;
            holeSparkPS.Play();
        }
    }
}
