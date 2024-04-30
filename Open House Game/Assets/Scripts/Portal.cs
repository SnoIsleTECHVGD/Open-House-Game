using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] private Portal _destination;

    private static Dictionary<Rigidbody2D, float> _cooldowns = new();

    private void Update()
    {
        Rigidbody2D[] allRB2Ds = FindObjectsOfType<Rigidbody2D>();

        foreach (Rigidbody2D rb2D in allRB2Ds)
        {
            _cooldowns[rb2D] -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Rigidbody2D rb2D))
        {
            if (!_cooldowns.ContainsKey(rb2D))
                _cooldowns.Add(rb2D, 0);

            if (_cooldowns[rb2D] > 0)
                return;

            rb2D.transform.position = _destination.transform.position;
            _cooldowns[rb2D] = 1;
        }
    }

    private void OnDrawGizmos()
    {
        if (_destination)
            Gizmos.DrawLine(transform.position, _destination.transform.position);
    }
}
