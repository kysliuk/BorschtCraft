using System;
using UnityEngine;

namespace BorschtCraft.Food.UI
{
    public interface IManagedConsumedView
    {
        Type GetConsumedModelType();
        void AttachViewModel(IConsumedViewModel viewModel);
        void DetachViewModel();
        GameObject GetGameObject();
    }
}
