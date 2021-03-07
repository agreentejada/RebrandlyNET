using System;
using System.Collections.Generic;
using System.Text;

namespace Rebrandly
{
    public enum OrderDir
    {
        desc,
        asc
    }

    public enum DomainType
    {
        service,
        user
    }

    public enum OrderByLinks
    {
        createdAt,
        updatedAt,
        title,
        slashtag
    }

    public enum OrderByWorkspace
    {
        name,
        createdAt,
        updatedAt
    }

    public enum OrderByDomain
    {
        fullName,
        createdAt,
        updatedAt
    }

}
