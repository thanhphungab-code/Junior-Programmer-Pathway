using System.Collections;
using UnityEngine;

namespace JpPrototype4
{
    public class EntityAbilityManager : MonoBehaviour
    {
        private IEquippable _currentAbility;
        private BaseBall _myBall;

        private Coroutine _durationCoroutine;
        private float _nextReadyTime;

        private void Awake()
        {
            _myBall = GetComponent<BaseBall>();
        }

        /// <summary>
        /// Equips a new ability, replacing the current one if any.
        /// </summary>
        /// <param name="newAbility">The ability to equip.</param>
        public void EquipAbility(IEquippable newAbility)
        {
            if (_currentAbility != null)
            {
                _currentAbility.OnUnequip();
            }

            if (_durationCoroutine != null)
            {
                StopCoroutine(_durationCoroutine);
            }

            _currentAbility = newAbility;
            _currentAbility.OnEquip(_myBall);

            _nextReadyTime = Time.time;

            _durationCoroutine = StartCoroutine(LoseAbilityAfterTime(_currentAbility.Duration));
        }

        /// <summary>
        /// Attempts to activate the current ability if it is off cooldown.
        /// </summary>
        public void TryUseAbility()
        {
            if (_currentAbility == null) return;

            if (Time.time >= _nextReadyTime)
            {
                _currentAbility.OnUse();

                if (_currentAbility.Cooldown > 0)
                {
                    _nextReadyTime = Time.time + _currentAbility.Cooldown;
                }
            }
        }

        private IEnumerator LoseAbilityAfterTime(float duration)
        {
            yield return new WaitForSeconds(duration);

            if (_currentAbility != null)
            {
                _currentAbility.OnUnequip();
                _currentAbility = null;
            }

            _durationCoroutine = null;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (_currentAbility != null)
            {
                _currentAbility.OnCollision(collision);
            }
        }
    }
}