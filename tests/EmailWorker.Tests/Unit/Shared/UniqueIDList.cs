using System.Collections.Generic;
using MailKit;

namespace EmailWorker.Tests.Unit.Shared;

public class UniqueIDList
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