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
        }

        void TracingAction()
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
                    if (stopwatch.ElapsedMilliseconds > 300)
                    {
                        myRigid.velocity = Vector3.zero;
                        var bullet = BulletObjectPool.Find(bObj => { return bObj.activeSelf == false; });
                        bullet.GetComponent<EnemyBulletCtrl>().DirectionVetor = (-directionVector);
                        bullet.transform.SetPositionAndRotation(this.gameObject.transform.position, this.gameObject.transform.rotation);
                        bullet.SetActive(true);
                        stopwatch.Restart();
                    }
                    break;
            }
        }

        public void CheckEnemyState()
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
}