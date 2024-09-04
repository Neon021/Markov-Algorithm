using System.Text;

internal class Program
{
    private static void Main(string[] args1)
    {
        string[] args = ["101", "|0,0||;1,0|;0,"];
        string alphabet = args[0];
        string ruleset = args[1];
        bool lastPattern = false;

        Dictionary<string, string> rules = [];
        var indivRules = ruleset.Split(';');

        int i = 0;
        while (i < indivRules.Length)
        {
            string[] ruless = indivRules[i].Split(',');
            rules.Add(ruless[0], ruless[1]);
            i++;
        }

        while (true)
        {
            foreach (var kv in rules)
            {
                if (kv.Key == rules.Keys.Last())
                    lastPattern = true;
                string pattern = kv.Key;
                string newAlphabet = ReplaceWithPattern(alphabet, kv);
                if (newAlphabet != alphabet)
                {
                    alphabet = newAlphabet;
                    lastPattern = false;
                    break;
                }
                if (lastPattern && (newAlphabet == alphabet))
                    goto End;
            }
            Console.WriteLine($"{alphabet}\r\n");
        }
    End:;
    }

    public static string ReplaceWithPattern(string set, KeyValuePair<string, string> kv)
    {
        string pattern = kv.Key;
        string newPattern = kv.Value;

        int maxSize = pattern.Length;
        string result = set;
        for (int i = 0; i < set.Length - maxSize; i++)
        {
            if (pattern.Length > 1)
            {
                while (i + maxSize < set.Length)
                {
                    ReadOnlySpan<char> asSpan = set.AsSpan();
                    string slice = new(asSpan.Slice(i, maxSize));
                    if (slice == pattern)
                    {
                        StringBuilder sb = new(set);
                        sb.Remove(i, maxSize);
                        sb.Insert(i, newPattern);
                        result = sb.ToString();
                        return result;
                    }
                    i++;
                }
            }
            else
            {
                if (set.Contains(pattern))
                {
                    int position = set.IndexOf(pattern);
                    if (position < 0)
                        break;
                    set = set.Remove(position, 1);
                    if (position == set.Length)
                    {
                        result = set + newPattern;
                    }
                    else if(position == 0)
                    {
                        result = newPattern + set;
                    }
                    else
                        result = string.Concat(set.AsSpan(0, position), newPattern, set.AsSpan(position, set.Length));
                }
                return result;
            }
        }

        return result;
    }
}