using SMLHelper.V2.Json;
using System.Collections.Generic;

namespace DAATQS.Managment
{
    public class TechTypeAllowList : ConfigFile
    {
        public TechTypeAllowList() : base("AllowList")
        {
        }

        public List<TechType> TechType = new List<TechType>();
    }

}