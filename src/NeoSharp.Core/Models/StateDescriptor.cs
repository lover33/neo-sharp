﻿using System;
using System.Linq;
using NeoSharp.BinarySerialization;
using NeoSharp.BinarySerialization.SerializationHooks;
using NeoSharp.Core.Types;

namespace NeoSharp.Core.Models
{
    public class StateDescriptor : IBinaryVerifiable
    {
        [BinaryProperty(0)]
        public StateType Type;
        [BinaryProperty(1, MaxLength = 100)]
        public byte[] Key;
        [BinaryProperty(2, MaxLength = 32)]
        public string Field;
        [BinaryProperty(3, MaxLength = 65535)]
        public byte[] Value;

        /// <summary>
        /// System Fee
        /// </summary>
        public Fixed8 SystemFee
        {
            get
            {
                switch (Type)
                {
                    case StateType.Validator: return GetSystemFee_Validator();
                    default: return Fixed8.Zero;
                }
            }
        }

        private Fixed8 GetSystemFee_Validator()
        {
            switch (Field)
            {
                case "Registered":
                    {
                        if (Value.Any(p => p != 0))
                            return Fixed8.FromDecimal(1000);
                        else
                            return Fixed8.Zero;
                    }
                default: throw new InvalidOperationException();
            }
        }

        public bool Verify()
        {
            switch (Type)
            {
                case StateType.Account:
                    {
                        if (Key.Length != 20) throw new FormatException();
                        if (Field != "Votes") throw new FormatException();

                        return VerifyAccountState();
                    }
                case StateType.Validator:
                    {
                        if (Key.Length != 33) throw new FormatException();
                        if (Field != "Registered") throw new FormatException();

                        return VerifyValidatorState();
                    }
                default: return false;
            }
        }

        private bool VerifyAccountState()
        {
            switch (Field)
            {
                case "Votes":
                    {
                        // TODO: Inject this

                        //if (Blockchain.Default == null) return false;
                        //ECPoint[] pubkeys;
                        //try
                        //{
                        //    pubkeys = Value.AsSerializableArray<ECPoint>((int)Blockchain.MaxValidators);
                        //}
                        //catch (FormatException)
                        //{
                        //    return false;
                        //}
                        //UInt160 hash = new UInt160(Key);
                        //AccountState account = Blockchain.Default.GetAccountState(hash);
                        //if (account?.IsFrozen != false) return false;
                        //if (pubkeys.Length > 0)
                        //{
                        //    if (account.GetBalance(Blockchain.GoverningToken.Hash).Equals(Fixed8.Zero)) return false;
                        //    HashSet<ECPoint> sv = new HashSet<ECPoint>(Blockchain.StandbyValidators);
                        //    DataCache<ECPoint, ValidatorState> validators = Blockchain.Default.GetStates<ECPoint, ValidatorState>();
                        //    foreach (ECPoint pubkey in pubkeys)
                        //        if (!sv.Contains(pubkey) && validators.TryGet(pubkey)?.Registered != true)
                        //            return false;
                        //}
                        return true;
                    }
                default: return false;
            }
        }

        private bool VerifyValidatorState()
        {
            switch (Field)
            {
                case "Registered": return true;
                default: return false;
            }
        }
    }
}