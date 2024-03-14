using UnityEngine;

namespace BlastPuzzle.Scripts.Interfaces
{
    public delegate void OnDied();

    public interface IDamageable
    {
        int Health { get; set; }
        OnDied OnDied { get; set; }
        void TakeDamage(int damage);
        Transform Transform { get; }
    }
}