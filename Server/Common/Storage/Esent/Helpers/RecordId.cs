using System;
using CSharpTest.Net.Collections;

namespace MutDood.Storage.Strategies.Esent.Helpers
{
    //public class RecordId : IStorageHandle
    //{
    //    public RecordId()
    //    {
    //        byte[] unique = Guid.NewGuid().ToByteArray();
    //        for (int i = 0; i < 8; i++) unique[i] ^= unique[i + 8];
    //        Id = BitConverter.ToInt32(unique, 0);
    //    }
    //    public int Id { get; private set; }

    //    public bool Equals(IStorageHandle other)
    //    {
    //        var handle = other as RecordId;
    //        if (handle == null)
    //        {
    //            return false;
    //        }
    //        if (handle.Id == Id)
    //        {
    //            return true;
    //        }
    //        return false;
    //    }
    //    public override int GetHashCode()
    //    {
    //        return Id.GetHashCode();
    //    }

    //    public static RecordId FirstIdentity { get { return new RecordId {Id = 1}; } }

    //    public static RecordId Recreate(int id)
    //    {
    //        return new RecordId {Id = id};
    //    }
    //}
}