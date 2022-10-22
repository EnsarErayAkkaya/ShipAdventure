using Cysharp.Threading.Tasks;
using System.Threading.Tasks;
using UnityEngine;

namespace EEA.Managers
{
    public class WindManager : MonoBehaviour
    {
        [SerializeField] private float directionLerpDuration;
        [SerializeField] private int directionChangeInterval;

        [SerializeField] private Transform windParticle;

        public static WindManager instance;

        public Vector2 wind;

        private Vector2 windTarget;
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            UpdateWindAsync().Forget();
        }
        private async UniTaskVoid UpdateWindAsync()
        {
            SetRandomWindDirection();

            Vector2 windStartPos = wind;
            float t = 0;
            float angle;
            while (t < 1)
            {
                t += Time.deltaTime / directionLerpDuration;
                wind = Vector2.Lerp(windStartPos, windTarget, t);
                wind.Normalize();

                angle = Vector3.SignedAngle(Vector3.forward, new Vector3(wind.x, 0, wind.y), Vector3.up);
                windParticle.transform.rotation = Quaternion.Euler(0, angle, 0);

                await UniTask.Yield();
            }

            wind = windTarget.normalized;

            await UniTask.Delay(directionChangeInterval * 1000); // seconds to miliseconds

            UpdateWindAsync().Forget();
        }

        private void SetRandomWindDirection()
        {
            windTarget = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)).normalized;
            Debug.Log("windTarget: " + windTarget);
        }
    }
}