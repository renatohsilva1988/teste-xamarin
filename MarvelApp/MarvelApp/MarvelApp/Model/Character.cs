using MarvelApp.Model.Base;

namespace MarvelApp.Model
{
    public class Character : ModelBase
    {
        private string name;
        private string description;
        private MarvelImageUrl thumbnail;

        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }

        public string Description
        {
            get => description;
            set => SetProperty(ref description, value);
        }

        public MarvelImageUrl Thumbnail
        {
            get => thumbnail;
            set => SetProperty(ref thumbnail, value);
        }
    }
}
