using System.Collections.Generic;

using SharpIpp.Protocol;
using SharpIpp.Protocol.Models;

namespace SharpIpp.Models
{
    public class GetJobAttributesResponse : IIppResponseMessage
    {
        public JobDescriptionAttributes JobAttributes { get; set; } = null!;

        public IppVersion Version { get; set; } = IppVersion.V1_1;

        public IppStatusCode StatusCode { get; set; }

        public int RequestId { get; set; }

        public List<IppSection> Sections { get; } = new List<IppSection>();
    }
}
