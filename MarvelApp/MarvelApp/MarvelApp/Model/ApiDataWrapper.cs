using MarvelApp.Model.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace MarvelApp.Model
{
    public class ApiDataWrapper<TData> : ModelBase
    {
        private int codet;
        private string status;
        private string copyright;
        private string attributionText;
        private string attributionHTML;
        private TData data;
        private string etag;

        public int Code
        {
            get => codet;
            set => SetProperty(ref codet, value);
        }

        public string Status
        {
            get => status;
            set => SetProperty(ref status, value);
        }

        public string Copyright
        {
            get => copyright;
            set => SetProperty(ref copyright, value);
        }

        public string AttributionText
        {
            get => attributionText;
            set => SetProperty(ref attributionText, value);
        }

        public string AttributionHTML
        {
            get => attributionHTML;
            set => SetProperty(ref attributionHTML, value);
        }

        public TData Data
        {
            get => data;
            set => SetProperty(ref data, value);
        }

        public string Etag
        {
            get => etag;
            set => SetProperty(ref etag, value);
        }
    }

}
