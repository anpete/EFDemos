// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;

namespace Performance.EFCore
{
    public class Customer
    {
        public Customer()
        {
            SalesOrderHeaders = new HashSet<SalesOrderHeader>();
        }

        public int CustomerID { get; set; }
        public int? PersonID { get; set; }
        public int? StoreID { get; set; }
        public int? TerritoryID { get; set; }
        public string AccountNumber { get; set; }
        public Guid rowguid { get; set; }
        public DateTime ModifiedDate { get; set; }

        public virtual ICollection<SalesOrderHeader> SalesOrderHeaders { get; set; }
        public virtual Person Person { get; set; }
        public virtual Store Store { get; set; }
        public virtual SalesTerritory Territory { get; set; }
    }
}
