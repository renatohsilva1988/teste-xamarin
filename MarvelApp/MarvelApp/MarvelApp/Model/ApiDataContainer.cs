using MarvelApp.Model.Base;
using System.Collections.Generic;

namespace MarvelApp.Model
{
    public class ApiDataContainer<TResult> : ModelBase
    {
        private int offset;
        private int limit;
        private int total;
        private int count;        
        private IEnumerable<TResult> results;

        public int Offset
        {
            get => offset;
            set => SetProperty(ref offset, value);
        }

        public int Limit
        {
            get => limit;
            set => SetProperty(ref limit, value);
        }

        public int Total
        {
            get => total;
            set => SetProperty(ref total, value);
        }

        public int Count
        {
            get => count;
            set => SetProperty(ref count, value);
        }

        public IEnumerable<TResult> Results
        {
            get => results;
            set => SetProperty(ref results, value);
        }
    }
}
