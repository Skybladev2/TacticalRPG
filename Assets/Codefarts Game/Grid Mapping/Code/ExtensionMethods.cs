namespace Codefarts.GridMapping
{
    using System;
    using System.Linq;

    public static class ExtensionMethods
    {

        public static bool TryParsePrefabName(this string text, out string name, out int layer, out int column, out int row)
        {
#if PERFORMANCE
            var perf = PerformanceTesting<PerformanceID>.Instance;
            var perfIDExists = perf.GetKeys().Any(x => x == PerformanceID.TryParsePrefabName);
            if (perfIDExists)
            {
                perf.Start(PerformanceID.TryParsePrefabName);
            }
#endif

            // setup default return values
            name = null;
            layer = 0;
            column = 0;
            row = 0;

            // check if test contains data
            if (string.IsNullOrEmpty(text))
            {
#if PERFORMANCE
                if (perfIDExists)
                {
                    perf.Stop(PerformanceID.TryParsePrefabName);
                }
#endif
                return false;
            }

            // setup parser callback
            var cb = new Func<string, int>(x =>
                {
                    // try to find last underscore
                    var index = text.LastIndexOf(string.Format("_{0}", x), StringComparison.Ordinal);
                    if (index == -1)
                    {
                        return -1;
                    }

                    // get number part
                    var value = text.Substring(index + 1 + x.Length);

                    // test to make sure that it contains only numbers
                    if (value.Any(c => "0123456789".IndexOf(c) == -1))
                    {
                        return -1;
                    }

                    // parse number value
                    int intValue;
                    if (!int.TryParse(value, out intValue))
                    {
                        return -1;
                    }

                    // crop from text
                    text = text.Substring(0, index);

                    return intValue;
                });


            // ------------------- row 
            row = cb("r");
            if (row == -1)
            {
#if PERFORMANCE
                if (perfIDExists)
                {
                    perf.Stop(PerformanceID.TryParsePrefabName);
                }
#endif
                return false;
            }

            // ------------------- column
            column = cb("c");
            if (column == -1)
            {
#if PERFORMANCE
                if (perfIDExists)
                {
                    perf.Stop(PerformanceID.TryParsePrefabName);
                }
#endif
                return false;
            }

            // ------------------- layer 
            layer = cb("l");
            if (layer == -1)
            {
#if PERFORMANCE
                if (perfIDExists)
                {
                    perf.Stop(PerformanceID.TryParsePrefabName);
                }
#endif
                return false;
            }

            // ------------------- name
            name = text;

#if PERFORMANCE
            if (perfIDExists)
            {
                perf.Stop(PerformanceID.TryParsePrefabName);
            }
#endif
            return true;
        }

    }
}
