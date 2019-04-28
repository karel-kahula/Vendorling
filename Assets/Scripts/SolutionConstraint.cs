using System.Collections.Generic;
using System.Linq;
using UnityEngine;

class SolutionConstraint
{
    public float ChanceNoSolution;
}

static class SolutionConstraints {
    public static readonly SolutionConstraint DEFAULT = new SolutionConstraint {
        ChanceNoSolution = 0.1f
    };
}