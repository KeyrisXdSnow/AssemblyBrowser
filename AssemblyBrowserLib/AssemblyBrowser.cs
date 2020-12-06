﻿using System;
using System.Collections.Generic;
 using System.IO;
 using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using AssemblyBrowserLib.format;
using static System.Reflection.BindingFlags;

namespace AssemblyBrowserLib
{
    public class AssemblyBrowser
    {
        public List<Container> GetAssemblyInfo(string filePath)
        {
            
            var assembly = Assembly.LoadFrom(filePath);
          
            var assemblyInfo = new Dictionary<string,Container>();
            
            foreach (var type in assembly.GetTypes())
            {
                try
                {
                    if (!assemblyInfo.ContainsKey(type.Namespace))
                        assemblyInfo.Add(type.Namespace, new Container(type.Namespace, ClassFormatter.Format(type)));

                    assemblyInfo.TryGetValue(type.Namespace, out var container);

                    container.Members.Add(GetMembers(type));
                    
                    if (type.IsDefined(typeof(ExtensionAttribute), false))
                        assemblyInfo = GetExtensionNamespaces(type,assemblyInfo);
                    
                }
                catch (NullReferenceException e) { Console.WriteLine(e.StackTrace); }
            }

            return assemblyInfo.Values.ToList();
        }
        
        private static Dictionary<string,Container>  GetExtensionNamespaces(Type classType, Dictionary<string,Container> assemblyInfo)
        {
            
            var extensionClasses = new Dictionary<string, Container>();

            foreach (var method in classType.GetMethods(Static | Public | NonPublic))
            {
                if (!classType.IsDefined(typeof(ExtensionAttribute), false) ||
                    !method.IsDefined(typeof(ExtensionAttribute), false)) continue;
                
                var type = method.GetParameters()[0].ParameterType;
                
                if (!assemblyInfo.ContainsKey(type.Namespace))
                    assemblyInfo.Add(type.Namespace, new Container(type.Namespace, ClassFormatter.Format(type)));
                    
                Container @class = new Container( ClassFormatter.Format(type), ClassFormatter.Format(type));
                @class.Members.Add(new MemberInfo(MethodFormatter.Format(method) + " — метод расширения", ClassFormatter.Format(classType)));
                
                assemblyInfo.TryGetValue(type.Namespace, out var container);
                container.Members.Add(@class);
                
            }
            
            return assemblyInfo ;
        }
        
        private static Container GetMembers(Type type)
        {
            var member = new Container(ClassFormatter.Format(type),ClassFormatter.Format(type));
                
            var members = GetFields(type);
            members.AddRange(GetProperties(type));
            members.AddRange(GetMethods(type));

            member.Members = members ;

            return member;
        }

        private static IEnumerable<MemberInfo> GetMethods(Type type)
        {
            var methodInfos = new List<MemberInfo>();

            // add constructors
            methodInfos.AddRange(GetConstructors(type));
            
            // add methods
            foreach (var method in type.GetMethods(Instance | Static | Public | NonPublic | DeclaredOnly))
            {

                if ( type.IsDefined(typeof(ExtensionAttribute), false) && method.IsDefined(typeof(ExtensionAttribute), false) )
                    continue;

                var signature = MethodFormatter.Format(method);
                methodInfos.Add(new MemberInfo(signature, ClassFormatter.Format(type))); 
            }
            
            return methodInfos;
        }
        
        private static IEnumerable<MemberInfo> GetConstructors(Type type)
        {
            return type.GetConstructors().Select(constructor => new MemberInfo(ConstructorFormatter.Format(constructor), ClassFormatter.Format(type))).ToArray();
        }
        
        private static List<MemberInfo> GetFields (Type type)
        { 
            return type.GetFields().Select( field => new MemberInfo(FieldFormatter.Format(field), ClassFormatter.Format(type))).ToList(); //Instance | Static | Public | NonPublic
        }
        
        private static IEnumerable<MemberInfo> GetProperties (Type type)
        { 
            return type.GetProperties().Select( property => new MemberInfo(PropertiesFormatter.Format(property), ClassFormatter.Format(type))).ToList();} //Instance | Static | Public | NonPublic
        }
        
    }
