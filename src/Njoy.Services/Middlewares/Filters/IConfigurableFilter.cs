namespace Njoy.Services
{
    public interface IConfigurableFilter<T>
    {
        void Configure(T config);
    }
}