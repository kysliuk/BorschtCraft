namespace BorschtCraft
{
    public class View<T, TModel> where T : ViewModel<TModel> where TModel : Model
    {
        protected T _viewModel;
        public View(T viewModel)
        {

            _viewModel = viewModel ?? throw new System.ArgumentNullException(nameof(viewModel));

        }
    }
}
