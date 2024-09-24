namespace DeltaX.Core.Common;
using System.Collections.Generic;
using System.ComponentModel;

public class Enumeration<TEnum>
    where TEnum : struct, Enum
{
    public TEnum Id { get; private set; } = default!;
    public string Name { get; private set; } = default!;
    public string Description { get; private set; } = default!;

    public Enumeration() { }

    public Enumeration(TEnum @enum)
    {
        Id = @enum;
        Name = @enum.ToString();
        Description = GetEnumDescription(@enum);
    }

    public static string GetEnumDescription(TEnum item)
    {
        return item.GetType()
            .GetField(item.ToString())?
            .GetCustomAttributes(typeof(DescriptionAttribute), false)
            .Cast<DescriptionAttribute>()
            .FirstOrDefault()?.Description ?? string.Empty;
    }

    public static List<T> GetEnumValues<T>(Func<TEnum, T> converter)
         where T : Enumeration<TEnum>
    {
        return GetValues().Select(value => converter(value)).ToList();
    }

    public static List<TEnum> GetValues()
    {
        return Enum.GetValues<TEnum>().ToList();
    }

    public static List<Enumeration<TEnum>> GetAll() => GetEnumValues(e => new Enumeration<TEnum>(e));
}
