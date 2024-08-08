using Aiba.Enums;
using Aiba.Plugin.Scanner;

namespace Aiba.Scanners
{
    public class ScannerFactory
    {
        public ScannerFactory(IEnumerable<IScanner> scanners)
        {
            foreach (IScanner scanner in scanners)
            {
                if (_scanners.TryAdd(scanner.Name, scanner))
                    _scannerList.Add(scanner);
            }
        }

        private readonly List<IScanner> _scannerList = [];

        private readonly Dictionary<string, IScanner> _scanners = [];

        public IEnumerable<IScanner> GetScannersByMediaType(MediaTypeFlag flag)
        {
            return _scannerList.Where(x =>
                (flag.HasFlag(MediaTypeFlag.MANGA) && x.SupportedMediaType.HasFlag(MediaTypeFlag.MANGA))
                || (flag.HasFlag(MediaTypeFlag.VIDEO) && x.SupportedMediaType.HasFlag(MediaTypeFlag.VIDEO)));
        }

        public IScanner? GetScanner(string scannerName)
        {
            _scanners.TryGetValue(scannerName, out IScanner? scanner);
            return scanner;
        }
    }
}