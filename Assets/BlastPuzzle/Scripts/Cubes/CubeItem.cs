using System;
using BlastPuzzle.Scripts.Services;
using UnityEngine;

namespace BlastPuzzle.Scripts.Cubes
{
    [RequireComponent(typeof(CubeSkinController))]
    public abstract class CubeItem : MonoBehaviour, IPoolable
    {
        private bool _isObstacle;

        public bool IsObstacle
        {
            get => _isObstacle;
            set => _isObstacle = value;
        }

        private CubeSkinController _cubeSkinController;
        private int _health;

        public int Health
        {
            get => _health;
            set => _health = value;
        }

        public CubeSkinController CubeSkinController
        {
            get
            {
                if (!_cubeSkinController)
                    _cubeSkinController = GetComponentInChildren<CubeSkinController>();
                return _cubeSkinController;
            }
        }

        public void TakeDamage(Action onDied = null, int damage = 1)
        {
            _health -= damage;

            if (_health <= 0)
            {
                PoolingSystem.Instance.Destroy(PoolInstanceId, gameObject);
                var x = PoolInstanceId + "DestroyParticle";
                ParticleManager.Instance.StartParticle(x, transform.position);
                if (_isObstacle)
                {
                    EventSystem.TriggerEvent(Events.ObstacleDestroyed, PoolInstanceId);
                }

                onDied?.Invoke();
            }
            else if (PoolInstanceId == "VaseItem")
            {
                gameObject.GetComponentInChildren<CubeSkinController>().ChangeDamaged();
            }
        }

        public abstract string Id { get; }

        public virtual void OnGetFromPool()
        {
        }

        public virtual void OnReturnToPool()
        {
        }


        public abstract string PoolInstanceId { get; set; }
    }
}