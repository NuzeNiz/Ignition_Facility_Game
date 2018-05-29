﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IF
{
    /// <summary>
    /// 20180502 SangBin : Architecture Enemy Base Class
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    abstract public class EnemyBaseClass : MonoBehaviour
    {
        #region Fields : Prefabs
        /// <summary>
        /// 20180403 SangBin : Enemy Death Effect Prefab;
        /// </summary>
        abstract protected GameObject DeathEffect { get; }
        #endregion

        #region Fields : Enemy Statistic
        /// <summary>
        /// 20180403 SangBin : Enemy Current Health Power
        /// </summary>
        abstract protected double CurrentHealthPower { get; set; }

        /// <summary>
        /// 20180502 SangBin : 
        /// </summary>
        private double maxHealthPower;

        /// <summary>
        /// 20180502 SangBin : 
        /// </summary>
        abstract protected string TagName { get; }

        /// <summary>
        /// 20180418 SangBin : Enemy Moving Speed
        /// </summary>
        abstract protected float MovingSpeed { get; }

        protected Animator animator;
        #endregion

        #region Fields : Enemy State
        /// <summary>
        /// 20180403 SangBin : Enemy Action State
        /// </summary>
        private enum EnemyState { idle, traceToDS, traceToPlayer, attackOnDS, attackOnPlayer, die };

        /// <summary>
        /// 20180403 SangBin : Enemy Current Action State
        /// </summary>
        private EnemyState currentEnemyState = EnemyState.idle;

        /// <summary>
        /// 20180403 SangBin : Enemy Died or not
        /// </summary>
        private bool isDie = false;

        /// <summary>
        /// 20180430 SangBin : 
        /// </summary>
        protected bool isDamaged = false;
        #endregion

        #region Fields : Tracking(Enemy To Player)
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
        protected Vector3 directionVector_NormalizedEtoP;

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
        #endregion

        #region Fields : Tracking(Enemy To Defense Tower)
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
        protected Vector3 directionVector_NormalizedEtoDS;

        /// <summary>
        /// 20180430 SangBin : Enemy's Enable Tracing Distance
        /// </summary>
        private float traceDistEtoDS = 10.0f;

        /// <summary>
        /// 20180430 SangBin : Enemy's Enable attack Distance
        /// </summary>
        private float attackDistEtoDS = 2.0f;
        #endregion

        //--------------------------------------------------------------------------------------------------

        virtual protected void Awake()
        {
            maxHealthPower = CurrentHealthPower;
        }
        protected void OnEnable()
        {
            StartCoroutine(TracingAction()); //Finite State Machine (or Finite Automaton)
            StartCoroutine(CheckEnemyState());
        }

        protected void OnDisable()
        {
            ResetTag(TagName);
        }

        /// <summary>
        /// 20180403 SangBin : Damage to enemy
        /// 20180515 SangBin : + Splash Damage to enemy
        /// </summary>
        protected void OnDamaged(object[] parameters)
        {
            animator.SetTrigger("IsHit");
            //isDamaged = true;
            CurrentHealthPower -= (double)parameters[0];
            bool IsSplashDamage = (bool)parameters[1];

            if (CurrentHealthPower <= 0.0d)
            {
                EnemyKilled();
            }

            if (IsSplashDamage)
            {
                Collider[] colls = Physics.OverlapSphere(transform.position, 2.0f);
                foreach (Collider coll in colls)
                {
                    if (coll.gameObject.layer == 8)
                    {
                        Rigidbody rbody = coll.GetComponent<Rigidbody>();
                        if (rbody != null)
                        {
                            object[] _parameters = new object[2];
                            parameters[0] = BalanceManagement.instance.CalcPlayerStrkingPower_Splash(coll.gameObject.tag);
                            parameters[1] = false;
                            coll.SendMessage("OnDamaged", _parameters, SendMessageOptions.DontRequireReceiver);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 20180403 SangBin : Check enemy action state by distance to target from enemy
        /// 20180430 SangBin : + target(Defense Station) 
        /// </summary>
        private IEnumerator CheckEnemyState()
        {
            while (!isDie)
            {
                yield return new WaitForSeconds(0.2f);

                if (isDamaged)
                {
                    Cal_DirectionEtoP();

                    if (distanceEtoP <= attackDistEtoP)
                    {
                        currentEnemyState = EnemyState.attackOnPlayer;
                    }
                    else if (distanceEtoP <= traceDistEtoP)
                    {
                        currentEnemyState = EnemyState.traceToPlayer;
                    }
                    else
                    {
                        currentEnemyState = EnemyState.idle;
                    }
                }
                else
                {
                    Cal_DirectionEtoDS();

                    if (distanceEtoDS <= attackDistEtoDS)
                    {
                        currentEnemyState = EnemyState.attackOnDS;
                    }
                    else if (distanceEtoDS <= traceDistEtoDS)
                    {
                        currentEnemyState = EnemyState.traceToDS;
                    }
                    else
                    {
                        currentEnemyState = EnemyState.idle;
                    }
                }
            }
        }

        /// <summary>
        /// 20180403 SangBin : Control enemy tracing 
        /// 20180430 SangBin : + Tracing Defense Station
        /// </summary>
        private IEnumerator TracingAction()
        {
            while (!isDie)
            {
                switch (currentEnemyState)
                {
                    case EnemyState.idle:
                        ActionA();
                        break;

                    case EnemyState.traceToPlayer:
                        ActionB();
                        break;

                    case EnemyState.attackOnPlayer:
                        ActionC();
                        yield return new WaitForSeconds(2.0f);
                        break;

                    case EnemyState.traceToDS:
                        ActionD();
                        break;

                    case EnemyState.attackOnDS:
                        ActionE();
                        yield return new WaitForSeconds(2.0f);
                        break;
                }
                yield return null;
            }
        }

        /// <summary>
        /// 20180502 SangBin : Enemy Idle Action
        /// </summary>
        virtual protected void ActionA()
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            animator.SetBool("IsTrace", false); // later
        }

        /// <summary>
        /// 20180502 SangBin : Enemy Tracing to Player Action
        /// </summary>
        virtual protected void ActionB()
        {
            transform.LookAt(PlayerCtrl.instance.PlayerTr);
            GetComponent<Rigidbody>().AddForce(directionVector_NormalizedEtoP * MovingSpeed, ForceMode.Force);
            animator.SetBool("IsAttack", false); // later
            animator.SetBool("IsTrace", true); // later
        }

        /// <summary>
        /// 20180502 SangBin : Enemy Attack to Player Action
        /// </summary>
        virtual protected void ActionC()
        {
            transform.LookAt(PlayerCtrl.instance.PlayerTr);
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            animator.SetBool("IsAttack", true); // later

        }

        /// <summary>
        /// 20180502 SangBin : Enemy Tracing to DS Action
        /// </summary>
        virtual protected void ActionD()
        {
            GetComponent<Rigidbody>().AddForce(directionVector_NormalizedEtoDS * MovingSpeed, ForceMode.Force);
            animator.SetBool("IsAttack", false); // later
            animator.SetBool("IsTrace", true); // later
        }

        /// <summary>
        /// 20180502 SangBin : Enemy Attack to DS Action
        /// </summary>
        virtual protected void ActionE()
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            animator.SetBool("IsAttack", true); // later
        }

        /// <summary>
        /// 20180418 SangBin : Calculation Direction Vector from Enemy to Player
        /// </summary>
        private void Cal_DirectionEtoP()
        {
            directionVectorEtoP = PlayerCtrl.instance.PlayerTr.position - transform.position;
            distanceEtoP = directionVectorEtoP.magnitude;
            directionVector_NormalizedEtoP = Vector3.Normalize(directionVectorEtoP);
        }

        /// <summary>
        /// 20180430 SangBin : Calculation Direction Vectorfrom Enemy to Defense Station
        /// </summary>
        private void Cal_DirectionEtoDS()
        {
            directionVectorEtoDS = DefenseStationCtrl.instance.DefenseStationTR.position - transform.position;
            distanceEtoDS = directionVectorEtoDS.magnitude;
            directionVector_NormalizedEtoDS = Vector3.Normalize(directionVectorEtoDS);
        }

        /// <summary>
        /// 20180403 SangBin : Fall down broken enemy without conflict
        /// </summary>
        protected void EnemyKilled()
        {
            //Because expecting to Enemy's Falling Animation by graphic designer, I did not make Enemy deactivated at once   
            this.gameObject.tag = "Untagged";
            GameObject explosion = (GameObject)Instantiate(DeathEffect, transform.position, Quaternion.identity);

            StopAllCoroutines();
            isDie = true;
            isDamaged = false;
            currentEnemyState = EnemyState.die;
            GetComponent<CapsuleCollider>().enabled = false;
            animator.SetTrigger("IsDie");
            GameUIManagement.instance.DisplayScore(50);
            DefenseStationCtrl.instance.AbsorbingEnergy(TagName);

            StartCoroutine(PushObjectPool());
            Destroy(explosion, 2.0f);
        }

        /// <summary>
        /// 20180403 SangBin : Push broken enemy into the object pool and initialize some fields
        /// </summary>
        private IEnumerator PushObjectPool()
        {
            yield return new WaitForSeconds(4.0f); //destroy delay
            //yield return null;
            isDie = false;
            CurrentHealthPower = maxHealthPower;
            currentEnemyState = EnemyState.idle;
            GetComponent<CapsuleCollider>().enabled = true;
            gameObject.SetActive(false);
        }

        /// <summary>
        /// 20180502 SangBin : Rest enemy tag
        /// </summary>
        private void ResetTag(string tag)
        {
            gameObject.tag = tag;
        }

        //protected void OnTriggerStay(Collider collider)
        //{
        //    if (collider.gameObject.tag == "WEAPON_TYPE02_FLAME")
        //    {
        //        CurrentHealthPower -= 10.0d; //damage per frame
        //    }

        //    //임시로 넣음
        //    if (CurrentHealthPower <= 0.0d)
        //    {
        //        EnemyKilled();
        //    }

        //}


        /// <summary>
        /// 20180516 SangBin : 
        /// </summary>
        private void OnParticleCollision(GameObject other)
        {
            //if (collision.gameObject.tag == "WEAPON_TYPE03_PROJECTILE")
            {
                CurrentHealthPower -= BalanceManagement.instance.CalcPlayerStrkingPower(TagName, WeaponCtrl.instance.CurrentWeaponType); //damage per frame
            }

            //임시로 넣음
            if (CurrentHealthPower <= 0.0d)
            {
                EnemyKilled();
            }
        }
    }
}
