using NBlast.Rest.Model.Dto;
using NBlast.Rest.Services.Write;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBlast.Rest.Model.Converters
{

    public interface IJObjectLogModelConverter : IConverter<JObject, LogEvent>
    {
    }

    public class JObjectLogModelConverter : IJObjectLogModelConverter
    {
        public LogEvent Convert(JObject logModel)
        {
            throw new NotImplementedException();
        }
    }
}
