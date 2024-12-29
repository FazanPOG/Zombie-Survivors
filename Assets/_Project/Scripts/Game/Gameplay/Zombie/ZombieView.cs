using System;
using System.Collections;
using R3;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Project.Gameplay
{
    [RequireComponent(typeof(Animator))]
    public class ZombieView : MonoBehaviour
    {
        private const string MOVE_SPEED_KEY = "MoveSpeed";
        private const string ATTACK_1_KEY = "Attack";
        private const string ATTACK_2_KEY = "Attack 2";
        private const string DIED_1_KEY = "Died";
        private const string DIED_2_KEY = "Died 2";

        private Animator _animator;
        private Action _onDiedAnimationEndedCallback;

        private void Awake() => _animator = GetComponent<Animator>();

        public void Init(Action onDiedAnimationEndedCallback, float moveSpeed, ReadOnlyReactiveProperty<int> health, ParticleSystem bloodFX)
        {
            _onDiedAnimationEndedCallback = onDiedAnimationEndedCallback;
            
            _animator.SetFloat(MOVE_SPEED_KEY, moveSpeed);

            health.Skip(1).Subscribe(_ => bloodFX.Play());
        }

        public void PlayDiedAnimation()
        {
            PlayRandomDiedAnimation();
            StartCoroutine(DeathDelay());
        }

        private void PlayRandomAttackAnimation()
        {
            int index = Random.Range(0, 1);
            
            if(index == 0)
                _animator.SetTrigger(ATTACK_1_KEY);
            if(index == 1)
                _animator.SetTrigger(ATTACK_2_KEY);
        }

        private void PlayRandomDiedAnimation()
        {
            int index = Random.Range(0, 1);
            
            if(index == 0)
                _animator.SetTrigger(DIED_1_KEY);
            if(index == 1)
                _animator.SetTrigger(DIED_2_KEY);
        }

        private IEnumerator DeathDelay()
        {
            yield return new WaitForSecondsRealtime(3f);
            _onDiedAnimationEndedCallback?.Invoke();
        }
    }
}