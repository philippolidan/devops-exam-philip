using System.Collections.Generic;

namespace aspnet_core_dotnet_core.Helper
{
    public class JsonResponser
    {
        public string message { get; set; }
        public object data { get; set; }

        public object response(string message, object data = null)
        {
            this.message = message;
            this.data = data;
            return this;
        }
    }
}
