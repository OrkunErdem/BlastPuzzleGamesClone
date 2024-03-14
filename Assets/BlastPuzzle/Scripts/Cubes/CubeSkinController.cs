using UnityEngine;

namespace BlastPuzzle.Scripts.Cubes
{
    public class CubeSkinController : MonoBehaviour
    {
        private SpriteRenderer _spriteRenderer;
        [SerializeField] private Sprite sprite;
        [SerializeField] private Sprite tntSprite;
        [SerializeField] private Sprite damagedSprite;
        private bool _isDamaged;

        private SpriteRenderer SpriteRenderer =>
            _spriteRenderer ? _spriteRenderer : _spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        public void SetSpriteOrder(int order)
        {
            SpriteRenderer.sortingOrder = order;
        }

        public void ChangeSprite(bool isTnt)
        {
            if (_isDamaged)
            {
                SpriteRenderer.sprite = damagedSprite;
            }
            else
            {
                SpriteRenderer.sprite = isTnt ? tntSprite : sprite;
            }
        }

        public void ChangeDamaged()
        {
            _isDamaged = true;
        }
    }
}