using System.Collections.Generic;

namespace BorschtCraft.Food.UI.DisplayLogic
{
    public interface IItemLayerProcessor
    {
        // Added SlotType context parameter
        List<IConsumed> GetLayersToDisplay(IConsumed overallRootItem, SlotType slotContext);
    }
}
