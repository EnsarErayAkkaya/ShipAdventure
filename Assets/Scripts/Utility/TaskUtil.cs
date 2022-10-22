using System.Threading;
namespace EEA.Utils
{
    public static class TaskUtil
    {
        public static CancellationTokenSource RefreshToken(ref CancellationTokenSource tokenSource)
        {
            tokenSource?.Cancel();
            tokenSource?.Dispose();
            tokenSource = new CancellationTokenSource();
            return tokenSource;
        }

        public static CancellationTokenSource RefreshToken(ref CancellationTokenSource tokenSource, CancellationToken onDestroyToken)
        {
            tokenSource?.Cancel();
            tokenSource?.Dispose();
            tokenSource = new CancellationTokenSource();
            tokenSource = CancellationTokenSource.CreateLinkedTokenSource(tokenSource.Token, onDestroyToken);
            return tokenSource;
        }
    }
}