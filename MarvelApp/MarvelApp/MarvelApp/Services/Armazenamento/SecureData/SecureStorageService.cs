using MarvelApp.ServicesContracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace MarvelApp.Services.Armazenamento.SecureData
{
    public class SecureStorageService : ISecureStorageService
    {
        private readonly List<string> keysCannotDelete = new List<string>() { SecureKey.LembrarMe };

        public async Task<string> GetValue(string key)
        {
            try
            {
                return await SecureStorage.GetAsync(key);
            }
            catch (Exception ex)
            {
                throw new Exception(@"Possible that device doesn't support secure storage on device.", ex.InnerException);
            }
        }

        public async Task SetValue(string key, string value)
        {
            try
            {
                await SecureStorage.SetAsync(key, value);
            }
            catch (Exception ex)
            {
                throw new Exception(@"Possible that device doesn't support secure storage on device.", ex.InnerException);
            }
        }

        public void RemoveValue(string key)
        {
            try
            {
                if (keysCannotDelete.Contains(key))
                {
                    throw new Exception(@"You cannot delete that key.");
                }

                SecureStorage.Remove(key);
            }
            catch (Exception ex)
            {
                throw new Exception(@"Possible that device doesn't support secure storage on device.", ex.InnerException);
            }
        }

        public async Task DeleteDataAsync()
        {
            try
            {
                var dictionaryValues = new Dictionary<string, string>();
                Parallel.ForEach(keysCannotDelete, async (key) =>
                {
                    var value = await GetValue(key);
                    dictionaryValues.Add(key, value);
                });

                SecureStorage.RemoveAll();

                var tasks = new List<Task>();

                Parallel.ForEach(dictionaryValues, (item) =>
                {
                    tasks.Add(SetValue(item.Key, item.Value));
                });
                await Task.WhenAll(tasks);
            }
            catch (Exception ex)
            {
                throw new Exception(@"Possible that device doesn't support secure storage on device.", ex.InnerException);
            }
        }
    }
}
