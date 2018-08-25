using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Waiter
{
    /*
     * This Will be used to Help with the Parameter Interaction of Waiter API.
     */
    public class ParamData
    {
        // Global Variables.
        protected string param { get; set; }
        private object data;

        // Usable Constructor.
        public ParamData(string paramName)
        {
            this.param = paramName;
        }

        // Setters and Getters.
        public ParamData Put(object objData)
        {
            this.data = objData;
            return this;
        }

        internal string GetParam()
        {
            return param;
        }

        internal object GetData()
        {
            return data;
        }
    }
}
