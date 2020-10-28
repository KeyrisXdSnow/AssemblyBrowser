using System;
using System.Reflection;
using AssemblyBrowserLib;
using NUnit.Framework;

namespace Test
{
    [TestFixture]
    public class Tests
    {
        private readonly AssemblyBrowser _browser = new AssemblyBrowser();
        private const string TestDirectory = "E:\\Sharaga\\SPP\\Laba3\\AssemblyBrowser\\Test\\TestFiles\\";

        [Test]
        public void DllBrowserWorkFinishedCorrectly()
        {
            _browser.GetAssemblyInfo(TestDirectory+"AssemblyBrowserLib.dll");
            
            Assert.True(true);
        }
        
        [Test]
        public void ExeBrowserWorkFinishedCorrectly()
        {
            _browser.GetAssemblyInfo(TestDirectory+"TestClass.exe");
            
            Assert.True(true);
        }
        
        [Test]
        public void ExeBrowserWorkFinishedCorrectly2()
        {
            _browser.GetAssemblyInfo(TestDirectory+"View.exe");
            
            Assert.True(true);
        }
        
        [Test]
        public void NameSpaceIsCorrect()
        {
            var assemblyInfo = _browser.GetAssemblyInfo(TestDirectory+"TestClass.exe");
            ;
            Assert.IsTrue(assemblyInfo[0].Signature.Equals("TestClass") );
        }
        
        [Test]
        public void ClassIsCorrect()
        {
            var assemblyInfo = _browser.GetAssemblyInfo(TestDirectory+"TestClass.exe");
            ;
            Assert.IsTrue(assemblyInfo[0].Class.Equals("public  class  A") );
        }
        
        [Test]
        public void MethodAmountIsCorrect()
        {
            var assemblyInfo = _browser.GetAssemblyInfo(TestDirectory+"TestClass.exe");
            ;
            Assert.IsTrue(assemblyInfo[0].Members.Count == 2);
        }
        
        [Test]
        public void MethodNotNull()
        {
            var assemblyInfo = _browser.GetAssemblyInfo(TestDirectory+"TestClass.exe");
            ;
            Assert.NotNull(assemblyInfo[1].Members);
        }
    }
}