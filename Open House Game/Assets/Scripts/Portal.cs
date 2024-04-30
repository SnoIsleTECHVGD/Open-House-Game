using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] private Portal _destination;

    private static Dictionary<PlayerController, float> _cooldowns = new();

    private void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerController playerController))
        {
            if (!_cooldowns.ContainsKey(playerController))
                _cooldowns.Add(playerController, 0);

            if (_cooldowns[playerController] > 0)
                return;

            playerController.transform.position = _destination.transform.position;
            _cooldowns[playerController] = 1;
        }
    }

    private void OnDrawGizmos()
    {
        if (_destination)
            Gizmos.DrawLine(transform.position, _destination.transform.position);
    }
}
