using Aiba.Plugin.Scanner;

namespace Aiba.Scanners
{
    public class ScannerFactory
    {
        public ScannerFactory(IEnumerable<IScanner> scanners)
        {
            foreach (IScanner scanner in scanners)
            {
                _scanners.TryAdd(scanner.Name, scanner);
            }
        }

        private readonly Dictionary<string, IScanner> _scanners = [];

        public IScanner? GetScanner(string scannerName)
        {
            _scanners.TryGetValue(scannerName, out IScanner? scanner);
            return scanner;
        }
    }
}