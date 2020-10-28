﻿namespace AssemblyBrowserLib
{
    public class MemberInfo
    {
        public string Signature { get; set; }
        public string Class { get; set; }

        public MemberInfo(string signature, string @class)
        {
            Signature = signature;
            Class = @class;
        }
    }
}