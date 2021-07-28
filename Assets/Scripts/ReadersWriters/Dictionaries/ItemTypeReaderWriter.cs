using KnightsMobile;
using Mirror;
using System.Collections.Generic;

public static class DictionaryTypeItemReadWriteFunctions
{
    public static void WriteMyType(this NetworkWriter writer, Dictionary<ItemType, Item> values)
    {
        writer.WriteInt(values.Count);
        foreach (KeyValuePair<ItemType, Item> value in values)
        {
            writer.WriteInt((int)value.Key);
            writer.Write<Item>(value.Value);
        }
    }

    public static Dictionary<ItemType, Item> ReadMyType(this NetworkReader reader)
    {
        Dictionary<ItemType, Item> values = new Dictionary<ItemType, Item>();
        int count = reader.ReadInt();
        for (int i = 0; i < count; ++i)
        {
            values.Add((ItemType)reader.ReadInt(), reader.Read<Item>());
        }
        return values;
    }
}
