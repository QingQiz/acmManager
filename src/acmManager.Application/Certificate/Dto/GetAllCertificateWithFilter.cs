﻿using System;
using System.Collections.Generic;

namespace acmManager.Certificate.Dto
{
    public class GetAllCertificateWithFilter
    {
        public IEnumerable<CertificateLevel> Levels;

        public string Name;

        public DateTime? TimeStart;

        public DateTime? TimeEnd;

        public int MaxResultCount;

        public int SkipCount;
    }
}