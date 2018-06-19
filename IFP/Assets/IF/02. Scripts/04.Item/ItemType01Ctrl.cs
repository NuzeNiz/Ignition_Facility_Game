using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IFP
{
    public class ItemType01Ctrl : ItemBaseClass
    {


        /// <summary>
        /// 20180418 SangBin : Item Type
        /// </summary>
        [HideInInspector]
        override public ItemTypeEnum ItemType { get { return ItemTypeEnum.boom; } }

        /// <summary>
        /// 20180418 SangBin : Sound File when item is collided
        /// </summary>
        [SerializeField]
        private AudioClip collsionSF;

        /// <summary>
        /// 20180418 SangBin : Sound File when item is collided
        /// </summary>
        override protected AudioClip CollsionSF
        {
            get
            {
                return collsionSF;
            }
        }

        /// <summary>
        /// 20180418 SangBin : Effect Prefab when item is collided
        /// </summary>
        [SerializeField]
        private GameObject collEffectPrefab;

        /// <summary>
        /// 20180418 SangBin : Effect Prefab when item is collided
        /// </summary>
        override protected GameObject CollEffectPrefab
        {
            get
            {
                return collEffectPrefab;
            }
        }

        /// <summary>
        /// 20180430 SangBin : Sound File when item is used
        /// </summary>
        [SerializeField]
        private AudioClip itemSoundFile;

        /// <summary>
        /// 20180430 SangBin : Sound File when item is used
        /// </summary>
        override protected AudioClip ItemSoundFile
        {
            get
            {
                return itemSoundFile;
            }
        }

        /// <summary>
        /// 20180430 SangBin : Effect Prefab when item is used
        /// </summary>
        [SerializeField]
        private GameObject itemEffectPrefab;

        /// <summary>
        /// 20180430 SangBin : Effect Prefab when item is used
        /// </summary>
        override protected GameObject ItemEffectPrefab
        {
            get
            {
                return itemEffectPrefab;
            }
        }

        /// <summary>
        /// 20180430 SangBin : (Parabolic Motion : item origin pos -> target point) Time
        /// </summary>
        float parabolicTime = 0.0f;

        //---------------------------------------------------------------------------------------------------------
        /// <summary>
        /// 20180418 SangBin : Being Hit By Player
        /// </summary>
        new private void OnHit()
        {
            base.OnHit();
        }

        /// <summary>
        /// 20180430 SangBin : Item Boom function
        /// 20180516 SeongJun : If item used notify
        /// </summary>
        override public IEnumerator ItemFunction()
        {
            while (gameObject.activeSelf)
            {
                GameManagement.instance.SoundEffect(transform.position, ItemSoundFile);
                transform.SetPositionAndRotation(PlayerCtrl.instance.PlayerTr.position + (PlayerCtrl.instance.PlayerTr.forward), PlayerCtrl.instance.PlayerTr.rotation);
                GetComponent<MeshRenderer>().enabled = true;

                //GetComponent<Rigidbody>().velocity = CalcParabolicMotionVelocity(transform, PlayerCtrl.instance.PlayerTr.GetChild(1).gameObject.transform, 45.0f);
                GetComponent<Rigidbody>().velocity = CalcParabolicMotionVelocity(transform, PlayerCtrl.instance.itemtarget, 45.0f);
                GetComponent<Rigidbody>().useGravity = true;

                yield return new WaitForSeconds(parabolicTime);

                GameObject itemEffect = (GameObject)Instantiate(ItemEffectPrefab, transform.position, Quaternion.identity);
                Destroy(itemEffect, 2.0f);

                Collider[] colls = Physics.OverlapSphere(transform.position, 10.0f);
                foreach (Collider coll in colls)
                {
                    if (coll.gameObject.layer == 8)
                    {
                        Rigidbody rbody = coll.GetComponent<Rigidbody>();
                        if (rbody != null)
                        {
                            coll.SendMessage("EnemyKilled", SendMessageOptions.DontRequireReceiver);
                        }
                    }
                }

                GetComponent<Rigidbody>().useGravity = false;
                //GetComponent<SphereCollider>().enabled = true;
                gameObject.SetActive(false);
                //ItemWindow.Instance.ItemSubject.CheakConsumedItem();
            }
            yield break;
        }

        /// <summary>
        /// 20180430 SangBin : Calculation Active Item Parabolic Motion Veclocity
        /// </summary>
        private Vector3 CalcParabolicMotionVelocity(Transform origin, Transform target, float angle)
        {
            //타겟점은 출발점과 이미 수평상태
            Vector3 direction = target.position - origin.position; //직 방향

            Vector3 motionDir = direction;
            motionDir.y = 0; // 혹시나 모르니까 관계를 수평으로 강제
            float distance = motionDir.magnitude;  //수평거리

            float radian = angle * Mathf.Deg2Rad;

            float speed = Mathf.Sqrt(distance * Physics.gravity.magnitude / Mathf.Sin(2 * radian));  // 수평 도달 거리를 통한 속력구하기

            parabolicTime = 2 * speed * Mathf.Sin(radian) / Physics.gravity.magnitude; // 도달 시간 계산

            motionDir.y = distance * Mathf.Tan(radian);//던지는 각도를 고려해서 방향 다시 고치기

            return speed * motionDir.normalized; //속도값 리턴

        }
    }
}
