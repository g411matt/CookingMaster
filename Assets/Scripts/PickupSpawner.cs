using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Spawner for pickups, pickups are spawned randomly for the designated player and need to be interacted with
/// Pickups are spawned when customers are served quickly
/// pickups are spawned in the player's color for identification
/// future: benefits are hardcoded, but could be variable instead, maybe tiers for repeated quick serves
/// future: pool pickup objects instead of creating and destroying them
/// </summary>
public class PickupSpawner : MonoBehaviour
{
    public enum PickupType
    {
        Speed,
        Time,
        Score
    }

    /// <summary>
    /// Box collider to designate the extents in which pickups can be spawned
    /// it is not possible to collide with this
    /// </summary>
    [SerializeField]
    private BoxCollider2D _spawnZone = null;
    /// <summary>
    /// speed pickup prefab
    /// </summary>
    [SerializeField]
    private Pickup _speedPrefab = null;
    /// <summary>
    /// time pickup prefab
    /// </summary>
    [SerializeField]
    private Pickup _timePrefab = null;
    /// <summary>
    /// score pickup prefab
    /// </summary>
    [SerializeField]
    private Pickup _scorePrefab = null;
    /// <summary>
    /// player 1 material reference
    /// </summary>
    [SerializeField]
    private Material _player1Mat = null;
    /// <summary>
    /// player 2 material reference
    /// </summary>
    [SerializeField]
    private Material _player2Mat = null;

    /// <summary>
    /// tracks existing pickups to clean up at the end
    /// </summary>
    private List<Pickup> _pickups = new List<Pickup>();
    
    /// <summary>
    /// spawns a random pickup for the player
    /// </summary>
    public void SpawnPickup(Player player)
    {
        PickupType type = (PickupType)Random.Range(0, 3);
        SpawnPickup(type, player);
    }
    /// <summary>
    /// Spawns the specified pickup at a random location
    /// </summary>
    public void SpawnPickup(PickupType pickupType, Player player)
    {
        Pickup pickup = null;
        switch (pickupType)
        {
            case PickupType.Speed:
                pickup = GameObject.Instantiate<Pickup>(_speedPrefab, transform, false);
                break;
            case PickupType.Time:
                pickup = GameObject.Instantiate<Pickup>(_timePrefab, transform, false);
                break;
            case PickupType.Score:
                pickup = GameObject.Instantiate<Pickup>(_scorePrefab, transform, false);
                break;
        }
        pickup.SetSpawnInfo(this, player, GameManager.Instance.IsPlayerOne(player) ? _player1Mat : _player2Mat);
        _pickups.Add(pickup);
        float xCoord = Random.Range(-_spawnZone.bounds.extents.x, _spawnZone.bounds.extents.x);
        float yCoord = Random.Range(-_spawnZone.bounds.extents.y, _spawnZone.bounds.extents.y);
        pickup.transform.localPosition = new Vector3(xCoord, yCoord, 1);
    }

    /// <summary>
    /// Uses the pickup effect, called by the pickup so the spawner knows to delete it
    /// </summary>
    public void ActivatePickup(Pickup pickup, Player player)
    {
        PickupType type = pickup.Type;
        _pickups.Remove(pickup);
        Destroy(pickup.gameObject);
        switch (type)
        {
            case PickupType.Speed:
                GameManager.Instance.BoostPlayer(player);
                break;
            case PickupType.Time:
                GameManager.Instance.AddTime(15, player);
                break;
            case PickupType.Score:
                GameManager.Instance.AddPoints(50, player);
                break;
        }
    }

    /// <summary>
    /// cleanup leftover pickups
    /// </summary>
    public void Reset()
    {
        while(_pickups.Count > 0)
        {
            Destroy(_pickups[0].gameObject);
            _pickups.RemoveAt(0);
        }
    }
}
