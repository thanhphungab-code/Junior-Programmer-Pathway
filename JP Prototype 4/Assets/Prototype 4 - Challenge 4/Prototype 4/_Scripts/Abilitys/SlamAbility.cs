using System.Collections;
using JpPrototype4;
using UnityEngine;

/// <summary>
/// Active ability that makes the ball jump up, slam down hard, and create an explosion upon hitting the ground.
/// </summary>
public class SlamAbility : IEquippable
{
    private float _slamRadius = 6f;
    private float _explosionForce = 40f;
    private float _jumpForce = 15f;
    private float _slamDownForce = 40f;
    private bool _isSlamming;
    private BaseBall _owner;

    public float Duration => 10f;
    public float Cooldown => 4f;

    public void OnEquip(BaseBall ball)
    {
        _owner = ball;
        _isSlamming = false;
    }

    public void OnUnequip()
    {
        _owner = null;
    }

    public void OnUse()
    {
        if (_owner == null || _isSlamming)
        {
            return;
        }

        _owner.StartCoroutine(PerformSlamRoutine());
    }

    private IEnumerator PerformSlamRoutine()
    {
        _isSlamming = true;

        Vector3 currentVelocity = _owner.Rb.linearVelocity;
        currentVelocity.y = 0f;
        _owner.Rb.linearVelocity = currentVelocity;

        _owner.Rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);

        yield return new WaitForSeconds(0.4f);

        if (_owner == null)
        {
            yield break;
        }

        currentVelocity = _owner.Rb.linearVelocity;
        currentVelocity.y = 0f;
        _owner.Rb.linearVelocity = currentVelocity;

        _owner.Rb.AddForce(Vector3.down * _slamDownForce, ForceMode.Impulse);
    }

    public void OnCollision(Collision collision)
    {
        if (!_isSlamming || _owner == null)
        {
            return;
        }
        if (collision.contacts.Length > 0 && collision.contacts[0].normal.y > 0.5f)
        {
            _isSlamming = false;
            CreateExplosion();
        }
    }

    private void CreateExplosion()
    {
        Collider[] hits = Physics.OverlapSphere(_owner.transform.position, _slamRadius);

        foreach (Collider hit in hits)
        {
            var targetBall = hit.GetComponent<BaseBall>();

            if (targetBall != null && targetBall != _owner)
            {
                float distance = Vector3.Distance(_owner.transform.position, targetBall.transform.position);
                float attenuation = 1f - Mathf.Clamp01(distance / _slamRadius);
                Vector3 pushDirection = targetBall.transform.position - _owner.transform.position;
                pushDirection.y = 0f;

                if (pushDirection != Vector3.zero)
                {
                    pushDirection.Normalize();
                }
                else
                {
                    pushDirection = _owner.transform.forward;
                }

                float finalForce = _explosionForce * attenuation;
                targetBall.Rb.AddForce(pushDirection * finalForce, ForceMode.Impulse);
            }
        }
    }
}