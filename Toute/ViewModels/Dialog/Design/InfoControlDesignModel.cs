using System;
using System.Collections.Generic;
using System.Text;

namespace Toute
{
    public class InfoControlDesignModel : InfoControlViewModel
    {
        public static InfoControlDesignModel Instance = new InfoControlDesignModel();

        public InfoControlDesignModel()
        {
            Message = "Design time information";
            IsError = true;
        }
    }
}
