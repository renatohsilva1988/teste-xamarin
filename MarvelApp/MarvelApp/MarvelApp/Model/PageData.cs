using MarvelApp.Model.Base;
using System;

namespace MarvelApp.Model
{
    public class PageData : ModelBase
    {
        private int currentPage;
        private int pageSize;

        public int CurrentPage
        {
            get => currentPage;
            set => SetProperty(ref currentPage, value);
        }

        public int PageSize
        {
            get => pageSize;
            set => SetProperty(ref pageSize, value);
        }

        public int RecordOffset => CalculateOffset(CurrentPage, PageSize);

        public PageData(int currentPage, int pageSize)
        {
            if (currentPage < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(currentPage), "The current page can't be below 1. Paging starts at 1.");
            }

            CurrentPage = currentPage;
            PageSize = pageSize;
        }

        public PageData ToNextPage() => new PageData(CurrentPage + 1, PageSize);

        private static int CalculateOffset(int currentPage, int pageSize) => (currentPage - 1) * pageSize;
    }
}
