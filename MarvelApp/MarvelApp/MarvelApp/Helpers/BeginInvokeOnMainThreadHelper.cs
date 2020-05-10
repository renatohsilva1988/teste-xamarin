using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MarvelApp.Helpers
{
    public class BeginInvokeOnMainThreadHelper
    {
        public static Task<T> BeginInvokeOnMainThreadAsync<T>(Func<T> a)
        {
            var tcs = new TaskCompletionSource<T>();
            Device.BeginInvokeOnMainThread(() =>
            {
                try
                {
                    var result = a();
                    tcs.SetResult(result);
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                }
            });
            return tcs.Task;
        }

        public static Task BeginInvokeOnMainThreadAsync(Action action)
        {
            var tcs = new TaskCompletionSource<object>();
            Device.BeginInvokeOnMainThread(
                () =>
                {
                    try
                    {
                        action();
                        tcs.SetResult(null);
                    }
                    catch (Exception e)
                    {
                        tcs.SetException(e);
                    }
                });

            return tcs.Task;
        }

        public static Task BeginInvokeOnMainThreadAsync(Task action)
        {
            var tcs = new TaskCompletionSource<object>();
            Device.BeginInvokeOnMainThread(
                async () =>
                {
                    try
                    {
                        await action;
                        tcs.SetResult(null);
                    }
                    catch (Exception e)
                    {
                        tcs.SetException(e);
                    }
                });

            return tcs.Task;
        }
    }
}
