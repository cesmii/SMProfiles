using System;
using System.Collections.Generic;
using System.Text;

namespace CESMII.SMProfiles
{
    public interface ISMProfile
    {
        public IProfileEnvelope ProfileEnvelope { get; }
        public IProfileBody ProfileBody { get; }
    }

    public interface IProfileEnvelope
    {
        public ProfileSubjectType Subject { get; }
        public List<ProfileOriginTraceType> DeliveryStamps { get;  }
        public string FulfillmentUUID { get;  }
        public string RequestOrigin { get;  }
        public string ContentChecksum { get; }
    }

    public interface IProfileSubject
    {
        public string FQN { get; }
        public string Version { get; }
    }

    public interface IProfileOriginTrace
    {
        public string URI { get; }
        public DateTime TimeStamp { get; }
    }

    public interface IProfileBody
    {
        public ProfileHeaderType Header { get;  }
        public ProfileContentType Content { get; }
    }

    public interface IProfileHeader
    {
        public string ProfileNamespace { get; }
        public string ProfileName { get; }
        public string ProfileAuthor { get; }
        public string ProfileVersion { get; }
        public string ProfileSignature { get; }
        public string ProfileUUID { get; }
    }

}
