using System.Collections.Generic;
using MailKit;

namespace EmailWorker.Tests.UnitTests.Shared;

public class UniqueIDListShim
{
    public static List<UniqueId> Create(int numberOfItems)
    {
        List<UniqueId> uniqueIDs = new(numberOfItems);

        for (uint i = 0; i < numberOfItems; i++)
        {
            uniqueIDs.Add(new UniqueId(i+1));
        }

        return uniqueIDs;
    }
}