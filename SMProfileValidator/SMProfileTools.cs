using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Json;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace CESMII.SMProfiles
{
    public class ProfileTools
    {

        /// <summary>
        /// Check the SMProfile Structure to ensure it can be parsed
        /// </summary>
        /// <param name="profileContent">String contents of the Profile</param>
        /// <returns>True if the profile can be parsed.</returns>
        public bool ValidateProfileStructure(string profileContent)
        {
            JsonValue _profileContainerJson;
            try
            {
                _profileContainerJson = JsonValue.Parse(profileContent);
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Profile JSON could not be parsed.");
                Debug.WriteLine(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Check the SMProfile Origin for a valid response
        /// </summary>
        /// <param name="profileContent">String contents of the Profile</param>
        /// <returns>True if the origin responds as expected.</returns>
        public bool ValidateProfileOrigin(string profileContent)
        {
            JsonValue _profileContainerJson;
            try
            {
                _profileContainerJson = JsonValue.Parse(profileContent);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Profile JSON could not be parsed.");
                Debug.WriteLine(ex.Message);
                return false;
            }

            //Validate Origin
            if (_profileContainerJson.ContainsKey("Origin"))
            {
                foreach (KeyValuePair<string, JsonValue> value in _profileContainerJson["Origin"])
                {
                    if (value.Key.ToLower() == "uri")
                    {
                        if (_checkOriginURI(value.Value))
                        {
                            return true;
                        }
                        else
                        {
                            Debug.WriteLine("Error: No response from Origin URI");
                        }
                    }
                }
            }
            else
            {
                Debug.WriteLine("Profile JSON did not contain Origin information.");
                return false;
            }
            return false;
        }

        /// <summary>
        /// Check the integrity of the Profile by comparing the embedded hash with a computed one
        /// </summary>
        /// <param name="profileContent">String contents of the Profile</param>
        /// <returns>True if the checksums match.</returns>
        public bool ValidateProfileChecksum(string profileContent)
        {
            JsonValue _profileContainerJson;
            try
            {
                _profileContainerJson = JsonValue.Parse(profileContent);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Profile JSON could not be parsed.");
                Debug.WriteLine(ex.Message);
                return false;
            }

            //Validate Content Checksum
            string _profileChecksum = string.Empty;
            string _computedChecksum = string.Empty;
            if (_profileContainerJson.ContainsKey("Content-Checksum"))
            {
                _profileChecksum = _profileContainerJson["Content-Checksum"];
                Debug.WriteLine("Profile Checksum: " + _profileChecksum);
            }
            else
            {
                Debug.WriteLine("Error: Profile does not include a Content Checksum");
                return false;
            }
            if (_profileContainerJson.ContainsKey("Profile-Content"))
            {
                string _profileContent = _profileContainerJson["Profile-Content"].ToString();
                Debug.WriteLine("Analyzing SMProfile content as follows...");
                Debug.WriteLine(_profileContent);
                _computedChecksum = _computeSha256Hash(_profileContent);
                Debug.WriteLine("Computed Hash: " + _computedChecksum);
            }
            else
            {
                Debug.WriteLine("Error: Profile Content not found in specified SMProfile file.");
                return false;
            }
            if (!string.IsNullOrEmpty(_profileChecksum) && !string.IsNullOrEmpty(_computedChecksum) && _profileChecksum == _computedChecksum)
            {
                return true;
            }
            else
            {
                Debug.WriteLine("Error: Profile checksum failure.");
                return false;
            }
        }

        private bool _checkOriginURI(string URI)
        {
            try
            {
                WebClient client = new WebClient();
                client.DownloadData(URI);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private string _computeSha256Hash(string dataToHash)
        {
            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(dataToHash));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }

}
