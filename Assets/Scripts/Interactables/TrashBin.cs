
/// <summary>
/// Trash bin for disposing of items, negatively effects score
/// </summary>
public class TrashBin : Interactable
{
    public override void Interact(Player player)
    {
        if (player.HasItem())
        {
            var item = player.PlaceItem();
            Destroy(item.gameObject);
            // TODO: reduce score
        }
    }
}
