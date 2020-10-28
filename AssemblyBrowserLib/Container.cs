﻿using System.Collections.Generic;
using System.Linq;

namespace AssemblyBrowserLib
{
    public class Container : MemberInfo
    {
        public List<MemberInfo> Members { get; set; }
        
        public Container(string @namespace, string @class, string signature, List<MemberInfo> members) : base(@namespace,@class)
        {
            Signature = signature;
            Members = members;
        }
        public Container(string @namespace, string @class) : base(@namespace, @class)
        {
            Members = new List<MemberInfo>();
        }
        
        
    }
}