using System.Collections.Generic;
using MailKit;

namespace EmailWorker.Tests.Integration;

internal class UniqueIdFactory
{
    public static List<UniqueId> Create(int numberOfItems)
    {
        var uniqueIDs = new List<UniqueId> (numberOfItems);

        for (uint i = 0; i < numberOfItems; i++)
        {
            uniqueIDs.Add(new UniqueId(i+1));
        }

        return uniqueIDs;
    }
}