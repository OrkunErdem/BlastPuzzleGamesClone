using System.Collections;
using UnityEngine;

namespace BlastPuzzle.Scripts.Services
{
    public class ParticleManager : Singleton<ParticleManager>
    {
        public void StartParticle(string particlePoolId, Vector3 position)
        {
            var x = PoolingSystem.Instance.Instantiate(particlePoolId, position, Quaternion.identity,this.transform);
            var p = x.GetComponent<ParticleSystem>();
            p.Play();
            StartCoroutine(DestroyCo(x, particlePoolId));
        }

        private IEnumerator DestroyCo(GameObject gameobject, string id)
        {
            yield return new WaitForSeconds(1f);
            PoolingSystem.Instance.Destroy(id, gameobject);
        }
    }
}