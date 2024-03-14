using System.Collections;
using BlastPuzzle.Scripts.Managers.Map;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace BlastPuzzle.Scripts.UI
{
    public class CelebrationController : MonoBehaviour
    {
        private static readonly string PoolID = "CelebrationParticle";

        private void OnEnable()
        {
            EventSystem.StartListening(Events.Win, "CelebrationController", OnWin);
        }

        private void OnDisable()
        {
            EventSystem.StopListening(Events.Win, "CelebrationController");
        }

        private void OnWin(EventSystem.EventData obj)
        {
            var pos = GridManager.Instance.Pos;
            GetComponent<Image>().enabled = true;
            var star = PoolingSystem.Instance.Instantiate(PoolID, pos, Quaternion.identity);
            star.transform.localScale = Vector3.zero;
            star.transform.DOScale(Vector3.one * 4, 1.5f).SetEase(Ease.OutBack);
            StartCoroutine(WaitForCelebration());
        }

        private IEnumerator WaitForCelebration()
        {
            yield return new WaitForSeconds(5);
            GridManager.Instance.LevelComplete();
        }
    }
}