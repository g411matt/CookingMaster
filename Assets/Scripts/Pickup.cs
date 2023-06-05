using UnityEngine;

/// <summary>
/// Pickup item, works like an interactable, but is destructable
/// Spawner handles creation and destruction
/// </summary>
public class Pickup : Interactable
{
    /// <summary>
    /// renderer reference for color coding
    /// </summary>
    [SerializeField]
    private MeshRenderer[] _renderers = null;
    /// <summary>
    /// type reference
    /// </summary>
    [SerializeField]
    private PickupSpawner.PickupType _type = PickupSpawner.PickupType.Score;
    /// <summary>
    /// reference back to the player
    /// </summary>
    private Player _target = null;
    /// <summary>
    /// reference back to the spawner
    /// </summary>
    private PickupSpawner _spawner = null;

    public PickupSpawner.PickupType Type { get { return _type; } }

    /// <summary>
    /// initializes the pickup in the player's color
    /// </summary>
    public void SetSpawnInfo(PickupSpawner spawner, Player target, Material mat)
    {
        _target = target;
        _spawner = spawner;
        foreach(var renderer in _renderers)
        {
            renderer.sharedMaterial = mat;
        }
    }

    /// <summary>
    /// Calls back to the spawner to handle interaction
    /// destroys the pickup
    /// </summary>
    public override void Interact(Player player)
    {
        if (player == _target)
        {
            _spawner.ActivatePickup(this, player);
        }
    }

}
