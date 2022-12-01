using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel;

namespace JitSwap.Blazor.ViewModels
{
    public interface IDataLoader : INotifyPropertyChanged
    {
        LoadingState LoadingState { get; }

        DateTimeOffset? LoadedDateTime { get; }
        bool IsExpired { get; }
    }

    /// <summary>
    /// Possible loading states for the DataLoader
    /// </summary>
    public enum LoadingState
    {
        /// <summary>None</summary>
        None,
        /// <summary>Loading</summary>
        Loading,
        /// <summary>Finished</summary>
        Finished,
        /// <summary>Error</summary>
        Error
    }

    /// <summary>
    /// Helper model to easily show loaders when loading data
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial class DataLoader<T> : ObservableObject, IDataLoader where T: class, new()
    {
        // <summary>
        /// DataLoader constructors
        /// </summary>
        /// <param name="swallowExceptions">Swallows exceptions. Defaults to true. It's a more common scenario to swallow exceptions and just bind to the IsError property. You don't want to surround each DataLoader with a try/catch block. You can listen to the error callback at all times to get the error.</param>
        public DataLoader(bool swallowExceptions = true)
        {
            _swallowExceptions = swallowExceptions;
        }


        private bool _swallowExceptions;
        //private SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1, 1);

        [ObservableProperty]
        private bool isLoading;

        [ObservableProperty]
        private LoadingState loadingState;

        [ObservableProperty]
        private T? data;

        public DateTimeOffset? LoadedDateTime { get; set; }

        public DateTimeOffset? ExpireDateTime { get; set; }

        public bool IsExpired => !LoadedDateTime.HasValue || (ExpireDateTime.HasValue && ExpireDateTime < DateTimeOffset.UtcNow);

        /// <summary>
        ///  Load data. Errors will be in errorcallback
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="loadingMethod"></param>
        /// <param name="resultCallback"></param>
        /// <param name="errorCallback">optional error callback. Fires when exceptino is thrown in loadingMethod</param>
        /// <returns></returns>
        public async Task<T?> LoadAsync(Func<Task<T>> loadingMethod, Action<T>? resultCallback = null, Action<Exception>? errorCallback = null, TimeSpan? expire = null)
        {
            if (!IsExpired)
                return Data;

            //await semaphoreSlim.WaitAsync();
            //try
            //{
                //Set loading state
                LoadingState = LoadingState.Loading;

                T? result = default;

                try
                {
                    result = await loadingMethod();

                    //Set finished state
                    LoadingState = LoadingState.Finished;
                    LoadedDateTime = DateTimeOffset.UtcNow;
                    
                    if(expire.HasValue)
                        ExpireDateTime = LoadedDateTime.Value.Add(expire.Value);

                    if (resultCallback != null)
                        resultCallback(result);

                }
                catch (Exception e)
                {
                    //Set error state
                    LoadingState = LoadingState.Error;

                    if (errorCallback != null)
                        errorCallback(e);
                    else if (!_swallowExceptions) //swallow exception if _swallowExceptions is true
                        throw; //throw error if no callback is defined

                }

                Data = result;

                return result;
            //}
            //finally
            //{
            //    semaphoreSlim.Release();
            //}
        }

        public void Clear()
        {
            this.LoadedDateTime = null;
            this.ExpireDateTime = null;
            this.LoadingState = LoadingState.None;
            this.Data = null;
        }
    }
}
