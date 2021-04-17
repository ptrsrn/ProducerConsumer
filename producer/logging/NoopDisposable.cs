using System;

    public class NoopDisposable : IDisposable
{
    public static NoopDisposable Instance = new NoopDisposable();
 
    public void Dispose()
    {
    }
}