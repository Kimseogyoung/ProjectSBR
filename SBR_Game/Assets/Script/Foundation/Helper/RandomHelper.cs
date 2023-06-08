using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class RandomHelper
{
    private static Random _random = new Random(DateTime.Now.Millisecond);

    public static ItemProto GetRandomItem()
    {
        int result =_random.Next(0,ProtoHelper.GetCount<ItemProto>());
        return ProtoHelper.GetUsingIndex<ItemProto>(result);
    }

    public static ItemProto[] GetRandomThreeItem()
    {
        ItemProto[] itemProtos = new ItemProto[3];
        int cnt = 0;
        while (cnt < 3)
        {
            int result = _random.Next(0, ProtoHelper.GetCount<ItemProto>());
            
            if(itemProtos[cnt] != null && itemProtos[cnt].Id == result)
            {
                continue;
            }
            itemProtos[cnt] = ProtoHelper.GetUsingIndex<ItemProto>(result);
            cnt++;
        }
        return itemProtos;
    }
}

