﻿using System;
using System.Linq;
using NeoSharp.BinarySerialization;
using NeoSharp.Core.Cryptography;
using NeoSharp.Core.Types;
using Newtonsoft.Json;

namespace NeoSharp.Core.Models
{
    [Serializable]
    public class Witness : IEquatable<Witness>
    {
        private UInt160 _hash = null;

        [JsonProperty("txid")]
        public UInt160 Hash => _hash;

        [BinaryProperty(0, MaxLength = 65536)]
        [JsonProperty("invocation")]
        public byte[] InvocationScript;

        [BinaryProperty(1, MaxLength = 65536)]
        [JsonProperty("verification")]
        public byte[] VerificationScript;

        /// <summary>
        /// Check if is equal to other
        /// </summary>
        /// <param name="other">Other</param>
        /// <returns>Return true if equal</returns>
        public bool Equals(Witness other)
        {
            if (ReferenceEquals(this, other)) return true;
            if (other is null) return false;

            return InvocationScript.SequenceEqual(other.InvocationScript) && VerificationScript.SequenceEqual(other.VerificationScript);
        }

        /// <summary>
        /// Check if is equal to other
        /// </summary>
        /// <param name="other">Other</param>
        /// <returns>Return true if equal</returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj)) return true;
            if (obj is null) return false;

            if (!(obj is Witness other)) return false;

            return InvocationScript.SequenceEqual(other.InvocationScript) && VerificationScript.SequenceEqual(other.VerificationScript);
        }

        /// <summary>
        /// Get HashCode
        /// </summary>
        /// <returns>Return hashcode</returns>
        public override int GetHashCode()
        {
            var l = 0;

            if (InvocationScript.Length >= 4)
            {
                l += BitConverter.ToInt32(InvocationScript, 0);
            }
            else
            {
                l += InvocationScript.Length;
            }

            if (VerificationScript.Length >= 4)
            {
                l += BitConverter.ToInt32(VerificationScript, 0);
            }
            else
            {
                l += VerificationScript.Length;
            }

            return l;
        }

        /// <summary>
        /// Update Hash
        /// </summary>
        /// <param name="serializer">Serializer</param>
        /// <param name="crypto">Crypto</param>
        public void UpdateHash(IBinarySerializer serializer, ICrypto crypto)
        {
            _hash = new UInt160(crypto.Hash160(GetHashData(serializer)));
        }

        byte[] GetHashData(IBinarySerializer serializer)
        {
            return VerificationScript;
        }
    }
}