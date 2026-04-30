using System;
using System.Collections.Generic;
using System.Globalization;
using System.Numerics;

namespace Godot.BindingsGeneration;

internal sealed class NumberDefaultValueParser<T> : DefaultValueParser where T : INumber<T>
{
    public static NumberDefaultValueParser<T> Instance { get; } = new();

    private static readonly Dictionary<string, Func<string, string>> _floatingConstants = new()
    {
        ["pi"] = type => $"{type}.Pi",
        ["-pi"] = type => $"-{type}.Pi",
        ["tau"] = type => $"{type}.Tau",
        ["-tau"] = type => $"-{type}.Tau",
        ["inf"] = type => $"{type}.PositiveInfinity",
        ["-inf"] = type => $"{type}.NegativeInfinity",
        ["nan"] = type => $"{type}.NaN",
    };

    private NumberDefaultValueParser() { }

    protected override string ParseCore(string engineDefaultValue)
    {
        // Native structures format use C/C++ syntax so default values for floats may end with 'f'
        // but that syntax is not supported by float.Parse so remove the trailing 'f' in that case.
        if (typeof(T) == typeof(float) || typeof(T) == typeof(double))
        {
            if (_floatingConstants.TryGetValue(engineDefaultValue, out var constantFunc))
            {
                if (typeof(T) == typeof(float))
                {
                    return constantFunc("float");
                }
                if (typeof(T) == typeof(double))
                {
                    return constantFunc("double");
                }
            }

            if (engineDefaultValue.EndsWith('f'))
            {
                engineDefaultValue = engineDefaultValue[..^1];
            }
        }

        T numericValue = T.Parse(engineDefaultValue, CultureInfo.InvariantCulture);
        return ToString(numericValue);
    }

    private static string ToString(T value)
    {
        // Add literal suffix or casting for the types that need it.

        if (typeof(T) == typeof(uint))
        {
            return string.Create(CultureInfo.InvariantCulture, $"{value}U");
        }
        if (typeof(T) == typeof(ulong))
        {
            return string.Create(CultureInfo.InvariantCulture, $"{value}UL");
        }
        if (typeof(T) == typeof(long))
        {
            return string.Create(CultureInfo.InvariantCulture, $"{value}L");
        }
        if (typeof(T) == typeof(Half))
        {
            return string.Create(CultureInfo.InvariantCulture, $"({KnownTypes.SystemHalf.FullNameWithGlobal})({value})");
        }
        if (typeof(T) == typeof(float))
        {
            return string.Create(CultureInfo.InvariantCulture, $"{value}f");
        }
        if (typeof(T) == typeof(double))
        {
            return string.Create(CultureInfo.InvariantCulture, $"{value}D");
        }

        return string.Create(CultureInfo.InvariantCulture, $"{value}");
    }
}
