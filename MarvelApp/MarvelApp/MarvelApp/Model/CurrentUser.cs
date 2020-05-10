using MarvelApp.Model.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace MarvelApp.Model
{
    public class CurrentUser : ModelBase
    {
        private string name;

        public CurrentUser(string name)
        {
            Name = name;
        }

        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }

    }
}
