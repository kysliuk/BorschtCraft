namespace BorschtCraft
{
    public class ViewModel<T> where T : Model
    {
        protected T _model;

        public ViewModel(T model)
        {
            _model = model ?? throw new System.ArgumentNullException(nameof(model));
        }
    }
}
