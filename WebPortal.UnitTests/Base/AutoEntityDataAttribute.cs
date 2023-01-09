using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;

namespace WebPortal.UnitTests.Base;
public class AutoEntityDataAttribute : AutoDataAttribute
{
    public AutoEntityDataAttribute()
        : base(() => new Fixture().Customize(new AutoMoqCustomization()))
    {
        Fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
        Fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }
}