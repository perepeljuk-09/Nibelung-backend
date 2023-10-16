using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nibelung.Infrastructure.Configs
{
    public class FilesConfig
    {
        public static string? RootFolder { get; } = "files";
        public static int MaxLenght { get; } = 10485760;
    }
}
