using System.Collections.Generic;

namespace Inventory
{
    public interface IInventory
    {
        List<BaseItemSlot> Slots { get; set; }
        void AddItemToInventory(BaseItem itemToAdd, int amountToAdd);
        void RemoveItemFromInventory(BaseItem itemToRemove, int amountToRemove);
    }
}
