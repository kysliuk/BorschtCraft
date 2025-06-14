using System.Collections.Generic;

namespace BorschtCraft.Food.UI.DisplayLogic
{
    public interface IItemLayerProcessor
    {
        List<IConsumed> GetLayersToDisplay(IConsumed overallRootItem);
    }
}
