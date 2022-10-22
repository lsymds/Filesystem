using System.Collections;
using System.Collections.Generic;

namespace Baseline.Filesystem.Tests.Adapters;

public class RunOnAllProvidersConfiguration : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        return new List<object[]> { new object[] { Adapter.S3 } }.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
