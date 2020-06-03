using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CESMII.SMProfiles
{
    class MakeProfile
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            //  Construct the Envelope
            ProfileEnvelopeType myProfileEnvelope = new ProfileEnvelopeType
            {
                // Attach subject object
                Subject = new ProfileSubjectType
                {
                    FQN = "CESMII.EquipmentBaseType.ExtruderTypers.MyExtruderType",
                    Version = "1.0"
                },
                // Attach other info
                FulfillmentUUID = "1234",
                ContentChecksum = "4321",
                RequestOrigin = "http://customerinstance.cesmii.net",
                // Construct and attach the delivery traces
                DeliveryStamps = new List<ProfileOriginTraceType>
                {
                    new ProfileOriginTraceType { TimeStamp=DateTime.Now, URI="http://marketplace.cesmii.net"},
                    new ProfileOriginTraceType {TimeStamp=DateTime.Now, URI="http://customerinstance.cesmii.net"}
                }
            };

            //  Construct the Body
            ProfileBodyType myProfileBody = new ProfileBodyType
            {
                // Construct the header
                Header = new ProfileHeaderType
                {
                    ProfileAuthor = "Jonathan Wise",
                    ProfileName = "Jonathans Extruder Profile",
                    ProfileSignature = "Jonathans Signing Key",
                    ProfileUUID = "123",
                    ProfileVersion = "1.0",
                    ProfileNamespace = "Jonathans.Internal.Namespace"
                },
                // Construct and attach the Content
                Content = new ProfileContentType
                {
                    //  ** Profile content goes here **
                }
            };

            //Construct the Profile
            CESMII.SMProfiles.SMProfileType myProfile = new SMProfileType(myProfileEnvelope, myProfileBody);

            string jsonString;
            jsonString = JsonSerializer.Serialize(myProfile);
            Console.WriteLine(jsonString);
            Console.ReadLine();

        }
    }
}
