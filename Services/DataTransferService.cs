namespace MRWBlogs.Services
{
    public class DataTransferService
    {
        protected Dictionary<string , object> DataStore { get; set; } = [];

        public DataTransferService() { }

        public void StoreData(string key, object value)
        {
            DataStore.TryAdd(key, value);
        }
        public object? RetrieveData(string key)
        {
            DataStore.TryGetValue(key, out object? value);            
            return value;
        }
    }
}
