using System.Collections.Generic;
using System;
using System.Linq; // Added for Except andOfType

namespace BorschtCraft.Food.UI.DisplayLogic
{
    public class ItemLayerProcessor : IItemLayerProcessor
    {
        private const int MaxLayers = 10; // Safety limit

        // Updated method signature
        public List<IConsumed> GetLayersToDisplay(IConsumed overallRootItem, SlotType slotContext)
        {
            var layersToProcess = new List<IConsumed>();
            if (overallRootItem == null) return layersToProcess;

            IConsumed currentDisplayLayer = overallRootItem;
            bool topItemIsCookedResult = overallRootItem is ICooked;
            int layerProcessingCount = 0;

            while (currentDisplayLayer != null && layerProcessingCount < MaxLayers)
            {
                layerProcessingCount++;
                // Type modelType = currentDisplayLayer.GetType(); // Not used in this version of the loop logic
                bool shouldAddThisLayer = true;

                if (topItemIsCookedResult &&
                    currentDisplayLayer != overallRootItem &&
                    currentDisplayLayer is ICookable)
                {
                    // Suppress underlying ICookable layers if the top item is ICooked
                    // Logger.LogInfo($"ItemLayerProcessor: Suppressing view for underlying ICookable layer '{currentDisplayLayer.GetType().Name}' because top item ('{overallRootItem.GetType().Name}') was ICooked.");
                    shouldAddThisLayer = false;
                }

                if (shouldAddThisLayer)
                {
                    layersToProcess.Add(currentDisplayLayer);
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

            // Context-specific filtering
            if (slotContext == SlotType.Releasing)
            {
                // Exclude BreadRaw type specifically from the layers to be displayed in releasing slots
                // Assuming BreadRaw is a class that implements IConsumed.
                // If BreadRaw is not defined in this project, this line will cause a compile error.
                // For the purpose of this subtask, we assume BreadRaw is a defined type.
                var filteredLayers = layersToProcess.Where(layer => !(layer is BreadRaw)).ToList();
                // Logger.LogInfo($"ItemLayerProcessor: Releasing slot context. Original layers: {layersToProcess.Count}, Filtered layers (no BreadRaw): {filteredLayers.Count}");
                return filteredLayers;
            }

            return layersToProcess;
        }
    }
}
