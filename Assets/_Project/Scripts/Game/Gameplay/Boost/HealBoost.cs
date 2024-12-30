using UnityEngine;

namespace _Project.Gameplay
{
    public class HealBoost : BaseBoost
    {
        [SerializeField, Range(0, 100)] private int _healAmount;

        public int HealAmount => _healAmount;
    }
}