using System.Collections.Generic;
using System;

namespace BorschtCraft.Food.UI.DisplayLogic
{
    public class ItemLayerProcessor : IItemLayerProcessor
    {
        private const int MaxLayers = 10; // Safety limit

        public List<IConsumed> GetLayersToDisplay(IConsumed overallRootItem)
        {
            var layersToDisplay = new List<IConsumed>();
            if (overallRootItem == null) return layersToDisplay;

            IConsumed currentDisplayLayer = overallRootItem;
            bool topItemIsCookedResult = overallRootItem is ICooked;
            int layerProcessingCount = 0;

            while (currentDisplayLayer != null && layerProcessingCount < MaxLayers)
            {
                layerProcessingCount++;
                Type modelType = currentDisplayLayer.GetType();
                bool shouldDisplayThisLayer = true;

                if (topItemIsCookedResult &&
                    currentDisplayLayer != overallRootItem &&
                    currentDisplayLayer is ICookable)
                {
                    // Suppress underlying ICookable layers if the top item is ICooked
                    // Logger.LogInfo($"ItemLayerProcessor: Suppressing view for underlying ICookable layer '{modelType.Name}' because top item ('{overallRootItem.GetType().Name}') was ICooked.");
                    shouldDisplayThisLayer = false;
                }

                if (shouldDisplayThisLayer)
                {
                    layersToDisplay.Add(currentDisplayLayer);
                }

                if (currentDisplayLayer is Consumed consumedLayerInstance)
                {
                    currentDisplayLayer = consumedLayerInstance.WrappedItem;
                }
                else
                {
                    currentDisplayLayer = null; // Break if not a Consumed type or no wrapped item
                }
            }

            if (layerProcessingCount >= MaxLayers)
            {
                // Logger.LogWarning($"ItemLayerProcessor: Layer processing loop hit safety limit ({layerProcessingCount}). Root item: {overallRootItem.GetType().Name}");
            }
            return layersToDisplay;
        }
    }
}
