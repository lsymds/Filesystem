using System.Threading;
using System.Threading.Tasks;

namespace Baseline.Filesystem;

/// <summary>
/// An <see cref="IAdapter"/> implementation for interacting with files and directories on a local disk (or one
/// masquerading as one).
/// </summary>
public partial class LocalAdapter : IAdapter { }
