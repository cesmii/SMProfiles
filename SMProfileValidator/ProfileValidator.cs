using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Json;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace CESMII.SMProfiles
{
    class ProfileValidator
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            if (args.Length > 0)
            {
                SMProfiles.ProfileTools _profileTools = new ProfileTools();
                string _profilePath = args[0];
                if (File.Exists(_profilePath))
                {
                    Console.WriteLine("Analyzing SMProfile at: " + _profilePath);
                    string _profileContainer = File.ReadAllText(_profilePath);
                    if (_profileTools.ValidateProfileStructure(_profileContainer))
                    {
                        if (_profileTools.ValidateProfileChecksum(_profileContainer))
                        {
                            Console.Write(((char)0x221A));
                            Console.WriteLine(" Pass: Profile integrity check OK.");
                        }
                        else
                        {
                            Console.Write(((char)0x00D7));
                            Console.WriteLine(" Error: Profile integrity check Failed!");
                            Environment.ExitCode = -5;
                        }
                        if (_profileTools.ValidateProfileOrigin(_profileContainer))
                        {
                            Console.Write(((char)0x221A));
                            Console.WriteLine(" Pass: Origin URI responded OK.");
                        }
                        else
                        {
                            Console.Write(((char)0x00D7));
                            Console.WriteLine(" Error: Profile Origin check Failed!");
                            Environment.ExitCode = -4;
                        }
                    }
                    else
                    {
                        Console.Write(((char)0x00D7));
                        Console.WriteLine( " Error: Profile structure is invalid!");
                        Environment.ExitCode = -3; 
                    }   
                }
                else
                {
                    Console.Write(((char)0x00D7));
                    Console.WriteLine(" Error: Could not find SMProfile to analyze at: " + _profilePath);
                    Environment.ExitCode = -2;
                }
            }
            else
            {
                Console.Write(((char)0x00D7));
                Console.WriteLine(" Error: Provide the SMProfile path to analyze as an argument.");
                Environment.ExitCode = -1;
            }
        }
    }
}
