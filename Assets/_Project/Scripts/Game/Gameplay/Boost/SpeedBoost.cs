using UnityEngine;

namespace _Project.Gameplay
{
    public class SpeedBoost : BaseBoost
    {
        [SerializeField, Range(0f, 5f)] private float _speedBoost;

        public float Speed => _speedBoost;
    }
}