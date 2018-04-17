using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using UnityEngine;

namespace IF
{
    public class GameLogicWithThread : GameLogicManagement
    {
        private Thread enemiesGenerater;
        private Thread enemiesCaculator;
        private CancellationTokenSource cts;

        private new void Start()
        {
            CreateEnemyObjectPool();

            TempSpawnpoints = GameObject.Find("EnemySpawnPoints").GetComponentsInChildren<Transform>();

            cts = new CancellationTokenSource();
            enemiesGenerater = new Thread(() => GenerateEnemy(cts.Token));
            enemiesCaculator = new Thread(() => TraceActionEnemy(cts.Token));
            enemiesGenerater.Start();
            enemiesCaculator.Start();
        }

        private void GenerateEnemy(CancellationToken ct)
        {
            try
            {
                while (thisLevelState)
                {
                    ct.ThrowIfCancellationRequested();
                    Thread.Sleep((int)enemyGenerationPeriod * 1000);
                    var enemy = enemyObjectPool.Find(a => { return a.activeSelf == false; });
                    if (enemy == null)
                        continue;
                    int index = Random.Range(1, TempSpawnpoints.Length);
                    enemy.transform.position = TempSpawnpoints[index].position;
                    enemy.transform.parent = GoogleARCore.IF.TowerBuildController.TBController.DefenseStation_Anchor_Tr;
                    enemy.SetActive(true);
                }
            }
            catch
            {

            }
        }

        private void TraceActionEnemy(CancellationToken ct)
        {
            try
            {
                while (true)
                {
                    ct.ThrowIfCancellationRequested();
                    enemyObjectPool
                        .Where(a => { return a.activeSelf == true; })
                        .ToList()
                        .ForEach(a =>{
                            var pointer = PlayerCtrl.PlayerInstance.transform.position - a.transform.position;
                            var dist = pointer.magnitude;
                            var enemy = a.GetComponent<EnemyCtrlLight>();
                            enemy.playerCompass = pointer.normalized;

                            enemy.CheckEnemyState();
                        });
                }
            }
            catch
            {

            }
        }

        private void ThreadDispose()
        {
            cts.Cancel();
            enemiesGenerater.Join();
            enemiesCaculator.Join();
            cts.Dispose();
        }

        private new void GameOver()
        {
            ThreadDispose();
            base.GameOver();
        }

        ~GameLogicWithThread()
        {
            ThreadDispose();
        }
    }
}