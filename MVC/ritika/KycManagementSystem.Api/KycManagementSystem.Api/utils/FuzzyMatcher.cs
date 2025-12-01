using System;

public static class FuzzyMatcher
{
    public static int LevenshteinDistance(string s, string t)
    {
        var n = s.Length;
        var m = t.Length;
        var d = new int[n + 1, m + 1];

        for (int i = 0; i <= n; i++) d[i, 0] = i;
        for (int j = 0; j <= m; j++) d[0, j] = j;

        for (int i = 1; i <= n; i++)
        {
            for (int j = 1; j <= m; j++)
            {
                int cost = s[i - 1] == t[j - 1] ? 0 : 1;
                d[i, j] = Math.Min(
                    Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                    d[i - 1, j - 1] + cost
                );
            }
        }
        return d[n, m];
    }

    public static double Similarity(string s, string t)
    {
        int distance = LevenshteinDistance(s.ToLower(), t.ToLower());
        int maxLen = Math.Max(s.Length, t.Length);
        return maxLen == 0 ? 1.0 : 1.0 - (double)distance / maxLen;
    }

    public static bool IsMatch(string s, string t, double threshold = 0.8)
    {
        return Similarity(s, t) >= threshold;
    }
}
