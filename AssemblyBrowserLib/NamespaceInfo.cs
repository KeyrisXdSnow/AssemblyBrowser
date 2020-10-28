using System.Collections.Generic;

namespace AssemblyBrowserLib
{
    public class NamespaceInfo
    {
        public string Signature { get; set; }
        public List<Container> MemberInfo { get; set; }

        public NamespaceInfo(string signature)
        {
            MemberInfo = new List<Container>();
            Signature = signature;
        }

        public NamespaceInfo(List<Container> memberInfo, string signature)
        {
            Signature = signature;
            MemberInfo = memberInfo;
        }
    }
}