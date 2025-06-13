using System.Linq;

namespace BorschtCraft.Food
{
    public class Consumable<T> : Item, IConsumable where T : IConsumed
    {
        public virtual IConsumed Consume(IConsumed item)
        {
            if (!CanDecorate(item))
            {
                Logger.LogWarning(this, $"Cannot decorate {item?.GetType().Name} by {GetType().Name}");
                return item;
            }

            return ConsumeAbstractFactory.CreateConsumed<T>(Price, item);
        }

        public virtual bool CanDecorate(IConsumed item)
        {
            if(item == null) return false;

            if (item.HasIngredientOfType<T>())
            {
                Logger.LogInfo(this, $"Item {item.GetType().Name} already contains ingredient {typeof(T).Name}");
                return false;
            }

            return InnerCanDecorate(item);
        }

        protected virtual bool InnerCanDecorate(IConsumed item)
        {
            return true;
        }

        public Consumable(int price) : base(price) { }
    }
}
