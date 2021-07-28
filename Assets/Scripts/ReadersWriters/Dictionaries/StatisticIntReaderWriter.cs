using KnightsMobile;
using Mirror;
using System.Collections.Generic;

public static class DictionaryStatisticIntReadWriteFunctions
{
    public static void WriteMyType(this NetworkWriter writer, Dictionary<Statistic, int> values)
    {
        writer.WriteInt(values.Count);
        foreach (KeyValuePair<Statistic, int> value in values)
        {
            writer.WriteInt((int)value.Key);
            writer.WriteInt(value.Value);
        }
    }

    public static Dictionary<Statistic, int> ReadMyType(this NetworkReader reader)
    {
        Dictionary<Statistic, int> values = new Dictionary<Statistic, int>();
        int count = reader.ReadInt();
        for (int i = 0; i < count; ++i)
        {
            values.Add((Statistic)reader.ReadInt(), reader.ReadInt());
        }
        return values;
    }
}
