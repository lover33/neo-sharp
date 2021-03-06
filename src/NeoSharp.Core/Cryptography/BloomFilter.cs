﻿using System;
using System.Collections;
using System.Linq;
using NeoSharp.Core.Extensions;

namespace NeoSharp.Core.Cryptography
{
    public class BloomFilter
    {
        private readonly uint[] _seeds;
        private readonly BitArray _bits;
        private readonly ICrypto _crypto;

        public int K => _seeds.Length;
        public int M => _bits.Length;
        public readonly uint Tweak;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="crypto">ICrypto</param>
        /// <param name="m">Size</param>
        /// <param name="k">Hash iterations</param>
        /// <param name="nTweak">Seed</param>
        /// <param name="elements">Initial elements</param>
        public BloomFilter(ICrypto crypto, int m, int k, uint nTweak, byte[] elements = null)
        {
            _crypto = crypto ?? throw new ArgumentNullException(nameof(crypto));
            _seeds = Enumerable.Range(0, k).Select(p => (uint)p * 0xFBA4C795 + nTweak).ToArray();
            _bits = elements == null ? new BitArray(m) : new BitArray(elements);
            _bits.Length = m;
            Tweak = nTweak;
        }

        /// <summary>
        /// Add element to structure
        /// </summary>
        /// <param name="element">Element</param>
        public void Add(byte[] element)
        {
            foreach (uint i in _seeds.AsParallel().Select(s => _crypto.Murmur32(element, s)))
                _bits.Set((int)(i % (uint)_bits.Length), true);
        }

        /// <summary>
        /// Check element in structure
        /// </summary>
        /// <param name="element">Element</param>
        /// <returns>If probably present</returns>
        public bool Check(byte[] element)
        {
            foreach (uint i in _seeds.AsParallel().Select(s => _crypto.Murmur32(element, s)))
                if (!_bits.Get((int)(i % (uint)_bits.Length)))
                    return false;
            return true;
        }

        /// <summary>
        /// BloomFilter bit structure
        /// </summary>
        /// <param name="newBits">Bytearray to store structure</param>
        public void GetBits(byte[] newBits)
        {
            _bits.CopyTo(newBits, 0);
        }
    }
}