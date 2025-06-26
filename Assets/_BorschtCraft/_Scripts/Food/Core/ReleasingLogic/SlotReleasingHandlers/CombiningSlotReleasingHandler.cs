using System.Linq;
using System.Threading.Tasks;
using UniRx;

namespace BorschtCraft.Food
{
    public class CombiningSlotReleasingHandler : SlotReleasingHandlerBase<ReleasingCombiningSlotStrategy>
    {
        //private readonly Subject<ItemDeliveredSignal> _itemDeliveredSubject = new Subject<ItemDeliveredSignal>();

        //protected override async Task<bool> ProcessItemReleasing(ISlot slot)
        //{
        //    var item = slot.Item.Value;

        //    var deliverySignal = new CustomerDeliverySignal(item);
        //    _signalBus.Fire(deliverySignal);

        //    var deliveryResult = await _itemDeliveredSubject
        //        .Where(signal => signal.DeliverySignal == deliverySignal)
        //        .First()
        //        .ToTask();

        //    return deliveryResult.Delivered;
        //}


        //protected override void OnInitialize()
        //{
        //    base.OnInitialize();
        //    _signalBus.Subscribe<ItemDeliveredSignal>(signal =>
        //    {
        //        _itemDeliveredSubject.OnNext(signal);
        //    });
        //}

        //protected override void OnDispose()
        //{
        //    base.OnDispose();
        //    _signalBus.Unsubscribe<ItemDeliveredSignal>(signal =>
        //    {
        //        _itemDeliveredSubject.OnNext(signal);
        //    });
        //}
    }
}
