using UnityEngine;

#if UNITY_6000_3_OR_NEWER
using ObjectId = UnityEngine.EntityId;
#else
using ObjectId = System.Int32;
#endif

namespace Coffee.UIEffectInternal
{
    internal static class ObjectIdExtensions
    {
        internal static ObjectId GetObjectId(this Object obj)
        {
#if UNITY_6000_3_OR_NEWER
            return obj.GetEntityId();
#else
            return obj.GetInstanceID();
#endif
        }

        internal static uint GetObjectId32(this Object obj)
        {
#if UNITY_6000_3_OR_NEWER
            return unchecked((uint)EntityId.ToULong(obj.GetEntityId()));
#else
            return unchecked((uint)obj.GetInstanceID());
#endif
        }
    }
}
