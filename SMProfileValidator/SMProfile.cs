using System;
using System.Collections.Generic;
using System.Text;

namespace CESMII.SMProfiles
{
    public class SMProfileType : ISMProfile
    {
        ProfileEnvelopeType _profileEnvelope;
        ProfileBodyType _profileBody;
        public SMProfileType(ProfileEnvelopeType envelope, ProfileBodyType body)
        {
            _profileEnvelope = envelope;
            _profileBody = body;
        }

        public IProfileEnvelope ProfileEnvelope 
        {
            get
            {
                return _profileEnvelope;
            }
        }
        public IProfileBody ProfileBody
        {
            get
            {
                return _profileBody;
            }
        }
    }
    public class ProfileEnvelopeType: IProfileEnvelope
    {
        public ProfileSubjectType Subject { get; set; }
        public List<ProfileOriginTraceType> DeliveryStamps { get; set; }
        public string FulfillmentUUID { get; set; }
        public string RequestOrigin { get; set; }
        public string ContentChecksum { get; set; }
    }
    public class ProfileBodyType: IProfileBody
    {
        public ProfileHeaderType Header { get; set; }
        public ProfileContentType Content { get; set; }
    }
    public class ProfileSubjectType : IProfileSubject
    {
        public string FQN { get; set; }
        public string Version { get; set; }
    }
    public class ProfileOriginTraceType : IProfileOriginTrace
    {
        public string URI { get; set;  }
        public DateTime TimeStamp { get; set; }
    }
    public class ProfileHeaderType : IProfileHeader
    {
        public string ProfileNamespace { get; set; }
        public string ProfileName { get; set; }
        public string ProfileAuthor { get; set; }
        public string ProfileVersion { get; set; }
        public string ProfileSignature { get; set; }
        public string ProfileUUID { get; set; }
    }
    public class ProfileContentType
    {
        ProfileClassDefinitionType ClassDefinition;
    }
    public class ProfileClassDefinitionType
    {
        List<ProfileClassDefinitionMemberType> ProfileTypeMembers;
    }
    public enum ProfileClassDefinitionMemberTypeList
    {
        Attribute,
        Child,
        Meta
    }
    public class ProfileClassDefinitionMemberType
    {
        ProfileClassDefinitionMemberTypeList MemberType;
        string DisplayName;
        MeasurementType? MeasurementType;
    }
    public class MeasurementType
    {

    }
}
