// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;

namespace Performance.EFCore
{
    public class Currency
    {
        public Currency()
        {
            CountryRegionCurrency = new HashSet<CountryRegionCurrency>();
            CurrencyRateFromCurrencyCodeNavigation = new HashSet<CurrencyRate>();
            CurrencyRateToCurrencyCodeNavigation = new HashSet<CurrencyRate>();
        }

        public string CurrencyCode { get; set; }
        public string Name { get; set; }
        public DateTime ModifiedDate { get; set; }

        public virtual ICollection<CountryRegionCurrency> CountryRegionCurrency { get; set; }
        public virtual ICollection<CurrencyRate> CurrencyRateFromCurrencyCodeNavigation { get; set; }
        public virtual ICollection<CurrencyRate> CurrencyRateToCurrencyCodeNavigation { get; set; }
    }
}
