using System.Collections.Generic;
using MailKit;

namespace EmailWorker.Tests.UnitTests.Shared;

public class UniqueIdsShim
{
    public static List<UniqueId> Create(int numberOfItems)
    {
        List<UniqueId> uniqueIdsShim = new(numberOfItems);

        for (uint i = 0; i < numberOfItems; i++)
        {
            uniqueIdsShim.Add(new UniqueId(i+1));
        }

        return uniqueIdsShim;
    }
}