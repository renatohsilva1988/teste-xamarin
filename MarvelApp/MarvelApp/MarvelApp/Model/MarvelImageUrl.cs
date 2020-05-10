using MarvelApp.Model.Base;

namespace MarvelApp.Model
{
    public class MarvelImageUrl : ModelBase
    {
        private string extension;
        private string path;

        public MarvelImageUrl(string extension, string path)
        {
            Extension = extension;
            Path = path;
        }

        public MarvelImageUrl()
        {

        }

        public string Extension
        {
            get => extension;
            set => SetProperty(ref extension, value);
        }

        public string Path
        {
            get => path;
            set => SetProperty(ref path, value);
        }

        public override string ToString()
        {
            return $"{Path}.{Extension}";
        }
    }
}
