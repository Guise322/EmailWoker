using System.Collections.Generic;
using MailKit;

namespace EmailWorker.Tests.UnitTests.Shared;

public class UniqueIDsShim
{
    public static List<UniqueId> Create(int numberOfItems)
    {
        List<UniqueId> uniqueIDsShim = new(numberOfItems);

        for (uint i = 0; i < numberOfItems; i++)
        {
            uniqueIDsShim.Add(new UniqueId(i+1));
        }

        return uniqueIDsShim;
    }
}