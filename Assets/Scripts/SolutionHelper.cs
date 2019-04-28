using System.Collections.Generic;
using System.Linq;
using UnityEngine;

class SolutionHelper
{
    const int MIN_COIN_DENOMINATION = 5;

    // dynamic programming solution to subset sum
    private static int[,] ComputeSubsetSumTable(IList<int> values, int target)
    {
        Debug.Assert(target % MIN_COIN_DENOMINATION == 0);

        int n = values.Count;
        // optimize the potential sums based on minimum deno
        int potentialSums = target / MIN_COIN_DENOMINATION;
        var optimizedValues = values.Select(v => v / MIN_COIN_DENOMINATION).ToList();
        int[,] solutions = new int[potentialSums + 1, n + 1];

        // there's always one solution to 0 value - no coins
        for (int j = 0; j <= n; j++)
            solutions[0, j] = 1;

        for (int sum = 1; sum <= potentialSums; sum++)
        {
            for (int coins = 1; coins <= n; coins++)
            {
                // # of solutions once we add this coin is the # of solutions we had before adding the coin
                var count = solutions[sum, coins - 1];
                var coinValue = optimizedValues[coins - 1];
                var isValidCoin = coinValue > 0;
                var canUseCoin = sum >= coinValue;
                if (isValidCoin && canUseCoin)
                {
                    // add the number of solutions for remainder of target after using it
                    var remainder = sum - optimizedValues[coins - 1];
                    count += solutions[remainder, coins - 1];
                }
                solutions[sum, coins] = count;
            }
        }

        return solutions;
    }
    public static int NumberSolutions(IList<int> values, int target)
    {
        if (target % MIN_COIN_DENOMINATION != 0)
            return 0;
        var solutions = ComputeSubsetSumTable(values, target);
        return solutions[target / MIN_COIN_DENOMINATION, values.Count];
    }

    public static int PickSumFor(IList<int> values, SolutionConstraint constraint = null)
    {
        if (constraint == null)
            constraint = SolutionConstraints.DEFAULT;
        var max = values.Sum();
        var table = ComputeSubsetSumTable(values, max);

        var foundSolution = false;
        int sumForSolution = 0;

        while (!foundSolution)
        {
            sumForSolution = Random.Range(1, table.GetLength(0));

            int sums = 0;
            for (int coins = 0; coins < table.GetLength(1); coins++)
            {
                sums += table[sumForSolution, coins];
            }

            if (sums == 0)
            {
                // only accept invalid solutions with a small chance
                if (Random.Range(0.0f, 1.0f) < constraint.ChanceNoSolution)
                {
                    foundSolution = true;
                }
            }
            else
            {
                foundSolution = true;
            }
        }


        return sumForSolution * MIN_COIN_DENOMINATION;

    }

    public static CoinConfig[] GenerateRandomValues(List<CoinConfig> coinConfigs, int? n = null, int min = 2, int max = 7)
    {
        if (!n.HasValue)
        {
            n = Random.Range(min, max);
        }
        var result = new CoinConfig[n.Value];
        for (int i = 0; i < n.Value; i++)
        {
            var choice = Random.Range(0, coinConfigs.Count);
            result[i] = coinConfigs[choice];
        }
        return result;
    }

}