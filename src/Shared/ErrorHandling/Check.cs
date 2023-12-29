namespace Example.Shared.ErrorHandling;

public interface IClause { }
public interface ICheckClause : IClause { }
public class CheckClause : ICheckClause { }

public static class Check
{
    public static ICheckClause Is { get; } = new CheckClause();
}

public interface IGuardClause : IClause { }
public class GuardClause : IGuardClause { }

public static class Guard
{
    public static IGuardClause Against { get; } = new GuardClause();
}
