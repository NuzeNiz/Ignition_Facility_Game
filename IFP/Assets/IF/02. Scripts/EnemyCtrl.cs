using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.AI;

namespace IF
{
    public class EnemyCtrl : MonoBehaviour
    {
        /// <summary>
        /// 20180403 SangBin : Broken Enemy Explosion Effect Prefab;
        /// </summary>
        public GameObject expEffectPrefab;

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
        private Transform EnemyTr;

        /// <summary>
        /// 20180403 SangBin : Unity AI Baked Navigation Agent
        /// 20180418 SangBin : Unity AI Baked Navigation --> Thread & RigidBody.Addforce
        /// </summary>
        //private NavMeshAgent nvAgent;

        /// <summary>
        /// 20180403 SangBin : Unity AI Baked Navigation / Enemy's Enable Tracing Distance
        /// 20180418 SangBin : Unity AI Baked Navigation --> Thread & RigidBody.Addforce
        /// </summary>
        private float traceDist = 10.0f;

        /// <summary>
        /// 20180403 SangBin : Unity AI Baked Navigation / Enemy's Enable attack Distance
        /// 20180418 SangBin : Unity AI Baked Navigation --> Thread & RigidBody.Addforce
        /// </summary>
        private float attackDist = 2.0f;

        /// <summary>
        /// 20180403 SangBin : Vector From This Enemy To Player 
        /// </summary>
        private Vector3 directionVector;

        /// <summary>
        /// 20180403 SangBin : Direction Vector Property
        /// </summary>
        public Vector3 DirectionVector { get { return directionVector; } set { directionVector = value; } }

        /// <summary>
        /// 20180418 SangBin : Distance between This Enemy and Player
        /// </summary>
        private float distance;

        /// <summary>
        /// 20180418 SangBin : Distance Property
        /// </summary>
        public float Distance { get { return distance; } set { distance = value; } }

        /// <summary>
        /// 20180418 SangBin : Normalized Vector From This Enemy To Player 
        /// </summary>
        private Vector3 directionVector_Normalized;

        /// <summary>
        /// 20180418 SangBin : Normalized Direction Vector Property
        /// </summary>
        public Vector3 DirectionVector_Normalized { get { return directionVector_Normalized; } set { directionVector_Normalized = value; } }

        /// <summary>
        /// 20180403 SangBin : Maximum Closing Distance Vector Between Player and This Enemy 
        /// </summary>
        private Vector3 maximumCloseVector;

        /// <summary>
        /// 20180418 SangBin : Enemy Moving Speed
        /// </summary>
        private float MovingSpeed = 5.0f;

        /// <summary>
        /// 20180403 SangBin : Enemy Action State
        /// </summary>
        private enum EnemyState { idle, trace, attack, die };

        /// <summary>
        /// 20180403 SangBin : Enemy Present Action State
        /// </summary>
        private EnemyState myEnemyState = EnemyState.idle;

        /// <summary>
        /// 20180403 SangBin : Enemy Bullet Prefab
        /// </summary>
        public GameObject bulletPrefab;

        /// <summary>
        /// 20180403 SangBin : Contraints of the number of Enemy Bullet
        /// </summary>
        private int MaxBullet = 5;

        /// <summary>
        /// 20180403 SangBin : Bullet Object Pool List
        /// </summary>
        private List<GameObject> BulletObjectPool = new List<GameObject>();

        //------------------------------------------------------------------------------------------------------------------------

        private void Awake()
        {
            EnemyTr = this.gameObject.GetComponent<Transform>();
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
            EnemyHP -= (double)parameters[1];

            if (EnemyHP <= 0.0d)
            {
                EnemyKilled();
            }
        }

        /// <summary>
        /// 20180403 SangBin : Check enemy action state by distance to target from enemy
        /// </summary>
        IEnumerator CheckEnemyState()
        {
            while (!isDie)
            {
                yield return new WaitForSeconds(0.2f);

                Cal_Direction();

                if (Distance <= attackDist)
                {
                    myEnemyState = EnemyState.attack;
                }
                else if (Distance <= traceDist)
                {
                    myEnemyState = EnemyState.trace;
                }
                else
                {
                    myEnemyState = EnemyState.idle;
                }
            }
        }

        /// <summary>
        /// 20180403 SangBin : Control enemy tracing
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

                    case EnemyState.trace:
                        transform.LookAt(PlayerCtrl.PlayerInstance.PlayerTr);
                        GetComponent<Rigidbody>().AddForce(directionVector_Normalized * MovingSpeed, ForceMode.Force);
                        //animator.SetBool("IsAttack", false); // later
                        //animator.SetBool("IsTrace", true); // later
                        break;

                    case EnemyState.attack:
                        transform.LookAt(PlayerCtrl.PlayerInstance.PlayerTr);
                        GetComponent<Rigidbody>().velocity = Vector3.zero;
                        GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                        //animator.SetBool("IsAttack", true); // later
                        foreach (GameObject bulletObj in BulletObjectPool)
                        {
                            if (!bulletObj.activeSelf)
                            {
                                bulletObj.GetComponent<EnemyBulletCtrl>().DirectionVetor = directionVector;
                                bulletObj.transform.SetPositionAndRotation(this.gameObject.transform.position, this.gameObject.transform.rotation);
                                yield return new WaitForSeconds(0.3f);
                                bulletObj.SetActive(true);
                                break;
                            }
                        }
                        yield return new WaitForSeconds(1.5f);
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
            this.gameObject.tag = "ENEMY_TYPE01";
            myEnemyState = EnemyState.idle;
            GetComponent<BoxCollider>().enabled = true;
            gameObject.SetActive(false);

        }

        /// <summary>
        /// 20180403 SangBin : Create bullet OP
        /// </summary>
        void CreateBulletObjectPool()
        {
            for (int i = 0; i < MaxBullet; i++)
            {
                GameObject bulletObj = Instantiate(bulletPrefab, this.gameObject.transform.position, this.gameObject.transform.rotation, transform);
                bulletObj.name = this.gameObject.name + " bulletObj " + i.ToString();
                bulletObj.SetActive(false);
                BulletObjectPool.Add(bulletObj);
            }
        }

        /// <summary>
        /// 20180403 SangBin : Fall down broken enemy without conflict
        /// </summary>
        void EnemyKilled()
        {

            this.gameObject.tag = "Untagged";
            GameObject explosion = (GameObject)Instantiate(expEffectPrefab, this.gameObject.transform.position, Quaternion.identity);

            StopAllCoroutines();
            isDie = true;
            myEnemyState = EnemyState.die;
            GetComponent<BoxCollider>().enabled = false;
            GameUIManagement.GameUIManagerInstance.DisplayScore(50);

            StartCoroutine(this.PushObjectPool());
            Destroy(explosion, 2.0f);
        }

        /// <summary>
        /// 20180418 SangBin : Calculation Direction Vector
        /// </summary>
        void Cal_Direction()
        {
            directionVector = PlayerCtrl.PlayerInstance.PlayerTr.position - transform.position;
            distance = directionVector.magnitude;
            directionVector_Normalized = Vector3.Normalize(directionVector);
        }
    }
}