using System.Security;
using System.Security.Permissions;
using System.Threading;
using Xunit;

public class AssumeIdentityAttributeTests
{
    [Fact, AssumeIdentity("casper")]
    public static void AttributeChangesRoleInTestMethod()
    {
        Assert.True(Thread.CurrentPrincipal.IsInRole("casper"));
        Assert.True(Thread.CurrentPrincipal.Identity.Name == "xUnit");
    }

    [Fact, AssumeIdentity("casper", "jcs")]
    public static void AttributeChangesRoleInTestMethod2()
    {
        Assert.True(Thread.CurrentPrincipal.IsInRole("casper"));
        Assert.True(Thread.CurrentPrincipal.Identity.Name == "jcs");
    }

    [Fact]
    public static void CallingSecuredMethodWillThrow()
    {
        Assert.Throws<SecurityException>(() => DefeatVillian());
    }

    [Fact, AssumeIdentity("Q")]
    public static void CallingSecuredMethodWithWrongIdentityWillThrow()
    {
        Assert.Throws<SecurityException>(() => DefeatVillian());
    }

    [Fact, AssumeIdentity("007")]
    public static void CallingSecuredMethodWithAssumedIdentityPasses()
    {
        DefeatVillian();
    }

    [PrincipalPermission(SecurityAction.Demand, Role = "007")]
    public static void DefeatVillian() { }
}