using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.AI;

namespace IF
{
    public class BeeCtrl : EnemyBaseClass
    {
        /// <summary>
        /// 20180403 SangBin : Broken Enemy Explosion Effect Prefab;
        /// </summary>
        [SerializeField]
        private GameObject expEffectPrefab;

        /// <summary>
        /// 20180403 SangBin : Enemy Current Health Power
        /// </summary>
        private double EnemyHP = 100.0d;

        /// <summary>
        /// 20180403 SangBin : Enemy Died or not
        /// </summary>
        private bool isDie = false;

        /// <summary>
        /// 20180403 SangBin : Enemy's Transfrom
        /// </summary>
        //private Transform EnemyTr;

        /// <summary>
        /// 20180403 SangBin : Unity AI Baked Navigation Agent
        /// 20180418 SangBin : Unity AI Baked Navigation --> Thread & RigidBody.Addforce
        /// </summary>
        //private NavMeshAgent nvAgent;

        /// <summary>
        /// 20180403 SangBin : Unity AI Baked Navigation / Enemy's Enable Tracing Distance
        /// 20180418 SangBin : Unity AI Baked Navigation --> Coroutine & RigidBody.Addforce
        /// </summary>
        private float traceDistEtoP = 10.0f;

        /// <summary>
        /// 20180403 SangBin : Unity AI Baked Navigation / Enemy's Enable attack Distance
        /// 20180418 SangBin : Unity AI Baked Navigation --> Coroutine & RigidBody.Addforce
        /// </summary>
        private float attackDistEtoP = 2.0f;

        /// <summary>
        /// 20180403 SangBin : Vector From This Enemy To Player 
        /// </summary>
        private Vector3 directionVectorEtoP;

        /// <summary>
        /// 20180418 SangBin : Distance between This Enemy and Player
        /// </summary>
        private float distanceEtoP;

        /// <summary>
        /// 20180418 SangBin : Normalized Vector From This Enemy To Player 
        /// </summary>
        private Vector3 directionVector_NormalizedEtoP;

        /// <summary>
        /// 20180418 SangBin : Enemy Moving Speed
        /// </summary>
        private float MovingSpeed = 5.0f;

        /// <summary>
        /// 20180403 SangBin : Enemy Action State
        /// </summary>
        private enum EnemyState { idle, traceToDS, traceToPlayer, attackOnDS, attackOnPlayer, die };

        /// <summary>
        /// 20180403 SangBin : Enemy Present Action State
        /// </summary>
        private EnemyState myEnemyState = EnemyState.idle;

        /// <summary>
        /// 20180403 SangBin : Enemy Stinger Prefab
        /// </summary>
        [SerializeField]
        private GameObject StingerPrefab;

        /// <summary>
        /// 20180403 SangBin : Contraints of the number of Enemy Bullet
        /// </summary>
        private int MaxBullet = 5;

        /// <summary>
        /// 20180403 SangBin : Stinger Object Pool List
        /// </summary>
        private List<GameObject> StingerObjectPool = new List<GameObject>();

        /// <summary>
        /// 20180430 SangBin : stinger shoot Sound File
        /// </summary>
       [SerializeField]
        private AudioClip stingerSoundFile;

        /// <summary>
        /// 20180430 SangBin : Vector From This Enemy To Defense Station 
        /// </summary>
        private Vector3 directionVectorEtoDS;

        /// <summary>
        /// 20180430 SangBin : Distance between This Enemy and Defense Station 
        /// </summary>
        private float distanceEtoDS;

        /// <summary>
        /// 20180430 SangBin : Normalized Vector From This Enemy To Defense Station  
        /// </summary>
        private Vector3 directionVector_NormalizedEtoDS;

        /// <summary>
        /// 20180430 SangBin : 
        /// </summary>
        private bool IsDamaged = false;

        /// <summary>
        /// 20180430 SangBin : 
        /// </summary>
        private float traceDistEtoDS = 10.0f;

        /// <summary>
        /// 20180430 SangBin : 
        /// </summary>
        private float attackDistEtoDS = 2.0f;

        private Vector3 DSoffset = Vector3.up * 0.5f;
        //------------------------------------------------------------------------------------------------------------------------

        private void Awake()
        {
            //EnemyTr = this.gameObject.GetComponent<Transform>();
            CreateBulletObjectPool();
        }

        private void OnEnable()
        {
            StartCoroutine(this.TracingAction()); //Finite State Machine (or Finite Automaton)
            StartCoroutine(this.CheckEnemyState());
        }

        /// <summary>
        /// 20180403 SangBin : Damage to enemy (Message Driven Method)
        /// </summary>
        void OnDamaged(object[] parameters)
        {
            IsDamaged = true;
            EnemyHP -= (double)parameters[1];

            if (EnemyHP <= 0.0d)
            {
                EnemyKilled();
            }
        }

        /// <summary>
        /// 20180403 SangBin : Check enemy action state by distance to target from enemy
        /// 20180430 SangBin : + target(Defense Station) 
        /// </summary>
        IEnumerator CheckEnemyState()
        {
            while (!isDie)
            {
                yield return new WaitForSeconds(0.2f);

                if (IsDamaged)
                {
                    Cal_DirectionEtoP();

                    if (distanceEtoP <= attackDistEtoP)
                    {
                        myEnemyState = EnemyState.attackOnPlayer;
                    }
                    else if (distanceEtoP <= traceDistEtoP)
                    {
                        myEnemyState = EnemyState.traceToPlayer;
                    }
                    else
                    {
                        myEnemyState = EnemyState.idle;
                    }
                }
                else
                {
                    Cal_DirectionEtoDS();

                    if (distanceEtoDS <= attackDistEtoDS)
                    {
                        myEnemyState = EnemyState.attackOnDS;
                    }
                    else if (distanceEtoDS <= traceDistEtoDS)
                    {
                        myEnemyState = EnemyState.traceToDS;
                    }
                    else
                    {
                        myEnemyState = EnemyState.idle;
                    }
                }
            }
        }

        /// <summary>
        /// 20180403 SangBin : Control enemy tracing 
        /// 20180430 SangBin : + Tracing Defense Station
        /// </summary>
        IEnumerator TracingAction()
        {
            while (!isDie)
            {
                switch (myEnemyState)
                {
                    case EnemyState.idle:
                        GetComponent<Rigidbody>().velocity = Vector3.zero;
                        GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                        //animator.SetBool("IsTrace", false); // later
                        break;

                    case EnemyState.traceToPlayer:
                        transform.LookAt(PlayerCtrl.PlayerInstance.PlayerTr);
                        GetComponent<Rigidbody>().AddForce(directionVector_NormalizedEtoP * MovingSpeed, ForceMode.Force);
                        //animator.SetBool("IsAttack", false); // later
                        //animator.SetBool("IsTrace", true); // later
                        break;

                    case EnemyState.attackOnPlayer:
                        transform.LookAt(PlayerCtrl.PlayerInstance.PlayerTr);
                        GetComponent<Rigidbody>().velocity = Vector3.zero;
                        GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                        //animator.SetBool("IsAttack", true); // later
                        StartCoroutine(StingerShooting(directionVector_NormalizedEtoP));
                        yield return new WaitForSeconds(2.0f);
                        break;

                    case EnemyState.traceToDS:
                        transform.LookAt(DefenseStationCtrl.DS_Instance.DefenseStationTR);
                        GetComponent<Rigidbody>().AddForce(directionVector_NormalizedEtoDS * MovingSpeed, ForceMode.Force);
                        //animator.SetBool("IsAttack", false); // later
                        //animator.SetBool("IsTrace", true); // later
                        break;

                    case EnemyState.attackOnDS:
                        transform.LookAt(DefenseStationCtrl.DS_Instance.DefenseStationTR);
                        GetComponent<Rigidbody>().velocity = Vector3.zero;
                        GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                        //animator.SetBool("IsAttack", true); // later
                        StartCoroutine(StingerShooting(directionVector_NormalizedEtoDS));
                        yield return new WaitForSeconds(2.0f);
                        break;
                }
                yield return null;
            }
        }

        /// <summary>
        /// 20180403 SangBin : Push broken enemy into the object pool and initialize some fields
        /// </summary>
        IEnumerator PushObjectPool()
        {
            //yield return new WaitForSeconds(4.0f); //destroy delay
            yield return null;
            isDie = false;
            EnemyHP = 100.0d;
            this.gameObject.tag = "ENEMY_BEE";
            myEnemyState = EnemyState.idle;
            GetComponent<BoxCollider>().enabled = true;
            gameObject.SetActive(false);

        }

        IEnumerator StingerShooting(Vector3 directionVector_Normalized)
        {
            foreach (GameObject bulletObj in StingerObjectPool)
            {
                if (!bulletObj.activeSelf)
                {
                    //bulletObj.GetComponent<EnemyBulletCtrl>().DirectionVetor = directionVector;
                    bulletObj.transform.SetPositionAndRotation(this.gameObject.transform.position, this.gameObject.transform.rotation);
                    //yield return new WaitForSeconds(0.3f);
                    yield return null;
                    GameLogicManagement.GLM_Instance.SoundEffect(transform.position, stingerSoundFile);
                    bulletObj.SetActive(true);
                    bulletObj.SendMessage("AddForceToBullet", directionVector_Normalized, SendMessageOptions.DontRequireReceiver);
                    break;
                }
            }
        }

        /// <summary>
        /// 20180403 SangBin : Create bullet OP
        /// </summary>
        void CreateBulletObjectPool()
        {
            for (int i = 0; i < MaxBullet; i++)
            {
                GameObject bulletObj = Instantiate(StingerPrefab, this.gameObject.transform.position, this.gameObject.transform.rotation, GoogleARCore.IF.TowerBuildController.TBController.DefenseStation_Anchor_Tr);
                bulletObj.name = this.gameObject.name + "Bee_Stinger_" + i.ToString();
                bulletObj.SetActive(false);
                StingerObjectPool.Add(bulletObj);
            }
        }

        /// <summary>
        /// 20180403 SangBin : Fall down broken enemy without conflict
        /// </summary>
        void EnemyKilled()
        {
            //Because expecting to Enemy's Falling Animation by graphic designer, I did not make Enemy deactivated at once   
            this.gameObject.tag = "Untagged";
            GameObject explosion = (GameObject)Instantiate(expEffectPrefab, this.gameObject.transform.position, Quaternion.identity);

            StopAllCoroutines();
            isDie = true;
            myEnemyState = EnemyState.die;
            GetComponent<BoxCollider>().enabled = false;
            GameUIManagement.GameUIManagerInstance.DisplayScore(50);
            IsDamaged = false;

            StartCoroutine(this.PushObjectPool());
            Destroy(explosion, 2.0f);
        }

        /// <summary>
        /// 20180418 SangBin : Calculation Direction Vector from Enemy to Player
        /// </summary>
        void Cal_DirectionEtoP()
        {
            directionVectorEtoP = PlayerCtrl.PlayerInstance.PlayerTr.position - transform.position;
            distanceEtoP = directionVectorEtoP.magnitude;
            directionVector_NormalizedEtoP = Vector3.Normalize(directionVectorEtoP);
        }

        /// <summary>
        /// 20180430 SangBin : Calculation Direction Vectorfrom Enemy to Defense Station
        /// </summary>
        void Cal_DirectionEtoDS()
        {
            directionVectorEtoDS = DefenseStationCtrl.DS_Instance.DefenseStationTR.position - transform.position;
            distanceEtoDS = directionVectorEtoDS.magnitude;
            directionVector_NormalizedEtoDS = Vector3.Normalize(directionVectorEtoDS);
        }
    }
}