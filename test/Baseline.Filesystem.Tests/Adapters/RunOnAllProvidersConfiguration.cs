using System.Collections;
using System.Collections.Generic;

namespace Baseline.Filesystem.Tests.Adapters;

public class RunOnAllProvidersConfiguration : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        return new List<object[]>
        {
            new object[] { Adapter.Memory },
            new object[] { Adapter.S3 },
            new object[] { Adapter.Local },
        }.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
