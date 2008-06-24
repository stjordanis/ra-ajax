using System;
using System.Threading;
using NUnit.Framework;

namespace TestsRaDOM
{
    [TestFixture]
    public class TestRaBasics : TestBase
    {
        protected override string Url
        {
            get { return "RaDOMBasics.aspx"; }
        }

        [NUnit.Framework.Test]
        public void TestRaNamespace()
        {
            Browser.Eval("checkForRa();");
            AssertSuccess("Ra namespace doesn't exists");
        }

        [NUnit.Framework.Test]
        public void TestDollar()
        {
            Browser.Eval("checkForRaDollar();");
            AssertSuccess("Ra $ doesn't work");
        }

        [NUnit.Framework.Test]
        public void TestCreateClass()
        {
            Browser.Eval("checkCreateClass();");
            AssertSuccess("Ra classCreate doesn't work");
        }

        [NUnit.Framework.Test]
        public void TestExtendExists()
        {
            Browser.Eval("checkExtend();");
            AssertSuccess("Ra extend doesn't work");
        }

        [NUnit.Framework.Test]
        public void TestExtendWorksSimple()
        {
            Browser.Eval("checkExtendFunctionalSimple();");
            AssertSuccess("Ra extend doesn't work");
        }

        [NUnit.Framework.Test]
        public void TestExtendWorksSimpleMethod()
        {
            Browser.Eval("checkExtendFunctionalSimpleMethod();");
            AssertSuccess("Ra extend doesn't work");
        }

        [NUnit.Framework.Test]
        public void TestExtendWorksSimpleMethodInvoke()
        {
            Browser.Eval("checkExtendFunctionalMethodInvoke();");
            AssertSuccess("Ra extend doesn't work");
        }

        [NUnit.Framework.Test]
        public void TestExtendWorksPrototypeInstance()
        {
            Browser.Eval("checkExtendFunctionalMethodPrototype();");
            AssertSuccess("Ra extend doesn't work");
        }

        [NUnit.Framework.Test]
        public void TestExtendWorksPrototypeInstanceInitWithArgs()
        {
            Browser.Eval("checkExtendFunctionalMethodPrototypeWithInitArguments();");
            AssertSuccess("Ra extend doesn't work");
        }

        [NUnit.Framework.Test]
        public void TestExtendWorksPrototypeInstanceInitWithMultipleArgs()
        {
            Browser.Eval("checkExtendFunctionalMethodPrototypeWithMultipleInitArguments();");
            AssertSuccess("Ra extend doesn't work");
        }

        [NUnit.Framework.Test]
        public void TestExtendWithThisCheck()
        {
            Browser.Eval("checkExtendMethodPrototypeWithThisArgument();");
            AssertSuccess("Ra extend doesn't work");
        }

        [NUnit.Framework.Test]
        public void TestExtendWithInheritanceOverride()
        {
            Browser.Eval("checkExtendInheritanceOverride();");
            AssertSuccess("Ra extend doesn't work");
        }

        [NUnit.Framework.Test]
        public void TestElementExtended()
        {
            Browser.Eval("checkRaElementExtendWorks();");
            AssertSuccess("Ra extend doesn't work");
        }

        [NUnit.Framework.Test]
        public void TestElementReplace()
        {
            Browser.Eval("checkRaElementReplaceWorks();");
            AssertSuccess("Ra.Element.replace doesn't work");
        }

        [NUnit.Framework.Test]
        public void TestElementSetVisible()
        {
            Browser.Eval("checkRaElementReplaceWorks();");
            AssertSuccess("Ra.Element.setVisible doesn't work");
        }

        [NUnit.Framework.Test]
        public void TestRemove()
        {
            Browser.Eval("checkRemove();");
            AssertSuccess("Ra.Element.remove doesn't work");
        }
    }
}
