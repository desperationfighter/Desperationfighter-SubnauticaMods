using SMLHelper.V2.Json;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace DAATQS_BZ.Managment
{
    public class TechTypeAllowList : ConfigFile
    {
        public TechTypeAllowList() : base("AllowList")
        {
        }

        //public List<TechType> TechType = new List<TechType>();
        public List<string> TechType = new List<string>();
    }

}