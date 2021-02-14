﻿using System.Collections.Generic;
using acmManager.Certificate.Dto;
using acmManager.Problem.Dto;

namespace acmManager.Web.Models.Shared
{
    public class ChartViewModel
    {
        public IEnumerable<ProblemTypeDto> ProblemTypes { get; set; }
        public IEnumerable<GetAllProblemSolutionList> ProblemSolutions { get; set; }
        public IEnumerable<GetAllCertificateSummary> Certificates { get; set; }
    }
}