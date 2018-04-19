using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using UnityEngine;

namespace IF
{
    public class EnemyCtrlLight : EnemyCtrl
    {
        private Rigidbody myRigid;
        private Stopwatch stopwatch=new Stopwatch();
        public Vector3 playerCompass { get; set; }
        public static float speed = 1.0f;

        new void Awake()
        {
            EnemyTr = this.gameObject.GetComponent<Transform>();
            CreateBulletObjectPool();

            myRigid = gameObject.GetComponent<Rigidbody>();
        }

        new void OnEnable()
        {
            
        }

        private void Start()
        {
            stopwatch.Start(); 
        }

        private void Update()
        {
            TracingAction();
            gameObject.transform.LookAt(GameObject.Find("ARCore Device").transform);
        }

        void TracingAction()
        {
            if (!isDie)
            {
                switch (myEnemyState)
                {
                    case EnemyState.idle:
                        myRigid.velocity = Vector3.zero;
                        break;
                    case EnemyState.trace:
                        myRigid.velocity = speed * playerCompass;
                        //GetComponent<Rigidbody>().AddForce(playerCompass * speed, ForceMode.Force);
                        break;
                    case EnemyState.attack:
                        myRigid.velocity = Vector3.zero;
                        var bullet = BulletObjectPool.Find(bObj => { return bObj.activeSelf == false; });
                        bullet.GetComponent<EnemyBulletCtrl>().DirectionVetor = (-directionVector);
                        bullet.transform.SetPositionAndRotation(this.gameObject.transform.position, this.gameObject.transform.rotation);
                        if (stopwatch.ElapsedMilliseconds > 5000)
                        {
                            bullet.SetActive(true);
                            stopwatch.Restart();
                        }
                        break;
                }
            }
        }

        public void CheckEnemyState()
        {
            if (!isDie)
            {
                directionVector = EnemyTr.position - PlayerCtrl.PlayerInstance.PlayerTr.position;
                float dist = directionVector.magnitude;

                maximumCloseVector = (Vector3.Normalize(directionVector) * 0.7f);

                if (dist <= attackDist)
                {
                    myEnemyState = EnemyState.attack;
                }
                else if (dist <= traceDist)
                {
                    myEnemyState = EnemyState.trace;
                }
                else
                {
                    myEnemyState = EnemyState.idle;
                }
            }
        }

        private void ReSetting()
        {
            isDie = false;
            EnemyHP = 100.0d;
            gameObject.tag = "ENEMY_TYPE01";
            myEnemyState = EnemyState.idle;
            gameObject.GetComponent<BoxCollider>().enabled = true;
            gameObject.SetActive(false);
        }

        private new void EnemyKilled()
        {
            gameObject.tag = "Untagged";
            GameObject explosion = (GameObject)Instantiate(expEffectPrefab, this.gameObject.transform.position, Quaternion.identity);
            
            isDie = true;
            myEnemyState = EnemyState.die;
            this.GetComponent<BoxCollider>().enabled = false;
            GameUIManagement.GameUIManagerInstance.DisplayScore(50);
            ReSetting();
            
            Destroy(explosion, 2.0f);
        }
    }
}