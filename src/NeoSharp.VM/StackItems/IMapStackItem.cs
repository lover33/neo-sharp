﻿using System;
using System.Collections;
using System.Collections.Generic;

namespace NeoSharp.VM
{
    public abstract class IMapStackItem : IStackItem, IEnumerable<KeyValuePair<IStackItem, IStackItem>>
    {
        public override bool CanConvertToByteArray => false;

        public override byte[] ToByteArray() { throw new NotImplementedException(); }

        /// <summary>
        /// Count
        /// </summary>
        public abstract int Count { get; }

        public IStackItem this[IStackItem key]
        {
            get
            {
                if (TryGetValue(key, out IStackItem value))
                    return value;

                return null;
            }
            set
            {
                Set(key, value);
            }
        }

        public IEnumerable<IStackItem> Keys => GetKeys();
        public IEnumerable<IStackItem> Values => GetValues();

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="engine">Engine</param>
        public IMapStackItem(IExecutionEngine engine) : base(engine, EStackItemType.Map) { }

        #region Write

        public abstract bool Remove(IStackItem key);

        public abstract void Set(KeyValuePair<IStackItem, IStackItem> item);

        public abstract void Set(IStackItem key, IStackItem value);

        public abstract void Clear();

        #endregion

        #region Read

        public abstract bool ContainsKey(IStackItem key);

        public abstract bool TryGetValue(IStackItem key, out IStackItem value);

        #endregion

        #region Enumerables

        public abstract IEnumerable<IStackItem> GetKeys();

        public abstract IEnumerable<IStackItem> GetValues();

        public abstract IEnumerator<KeyValuePair<IStackItem, IStackItem>> GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
        {
            IEnumerator<KeyValuePair<IStackItem, IStackItem>> v = GetEnumerator();
            return v;
        }

        #endregion
    }
}