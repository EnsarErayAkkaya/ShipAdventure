using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace EEA.Enemy.Kraken
{
    public class KrakenTentacle : MonoBehaviour
    {
        [Header("Debug")]
        [SerializeField] private bool debug;

        [Header("Settings")]
        [SerializeField] private Transform target;
        [SerializeField] private Transform pole;
        
        [SerializeField] private Transform moveCenter;
        [SerializeField] private float moveCenterOffsetArea;

        [SerializeField] private Vector3 moveSpace;
        [SerializeField] private float moveSpaceOffsetArea;

        [SerializeField] private float maxReachableDistance;
        
        [Header("Attacking")]
        [SerializeField] private float damage;
        [SerializeField] private float tentacleLastAttackTime;
 
        private WaitForEndOfFrame waitEndOfFrame;
        private bool isAttacking;
        private IEnumerator idleRoutine;

        public bool IsAttacking => isAttacking;

        private void Start()
        {
            moveCenter.localPosition = moveCenter.localPosition + (Random.insideUnitSphere * moveCenterOffsetArea);
            moveSpace = Random.insideUnitSphere * moveSpaceOffsetArea;

            StartIdle();
        }

        private IEnumerator Idle()
        {
            float idleMoveSpeed = 0;
            while (true)
            {
                idleMoveSpeed = Mathf.Clamp01(idleMoveSpeed + Time.deltaTime);
                Vector3 offset = Mathf.Sin(Time.time) * moveSpace;
                target.localPosition =  Vector3.Lerp(target.localPosition, moveCenter.localPosition + offset, idleMoveSpeed);

                yield return waitEndOfFrame;
            }
        }
        public void AttackToPosition(Vector3 attackPos)
        {
            if (!isAttacking)
            {
                isAttacking = true;

                StopCoroutine(idleRoutine);

                attackPos = transform.worldToLocalMatrix.MultiplyVector(attackPos);

                Vector3 attackPosDir = attackPos - target.localPosition;

                Vector3 fPos = target.localPosition + (attackPosDir * 0.3f);
                Vector3 sPos = target.localPosition - (attackPosDir * 0.15f);

                Sequence sequence = DOTween.Sequence();
                sequence.Append(target.DOLocalJump(fPos, .5f, 1, 1.5f));
                sequence.Append(target.DOLocalJump(sPos, .5f, 1, 1.0f));
                sequence.Append(target.DOLocalJump(attackPos, .5f, 1, .5f));

                sequence.Play();

                sequence.OnComplete(() =>
                {
                    isAttacking = false;
                    StartIdle();
                });
            }
        }

        private void StartIdle()
        {
            idleRoutine = Idle();
            StartCoroutine(idleRoutine);
        }

        private void OnDrawGizmos()
        {
            if(debug)
            {
                Gizmos.DrawWireSphere(transform.position, maxReachableDistance);
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(target.position, .5f);
            }
        }
    }
}