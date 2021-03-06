﻿using System.Threading.Tasks;
using NeoSharp.Core.Models;
using NeoSharp.Core.Types;

namespace NeoSharp.Core.Persistence.Contexts
{
    public class BlockHeaderContext : IBlockHeaderContext
    {

        #region Private Fields 
        private readonly IDbModel _model;
        #endregion

        #region Constructor 
        public BlockHeaderContext(IDbModel model)
        {
            _model = model;
        }
        #endregion

        #region IBlockHeaderContext implementation
        public Task Add(BlockHeader blockHeader)
        {
            return _model.Create(DataEntryPrefix.DataBlock, blockHeader.Hash, blockHeader);
        }

        public Task<BlockHeader> GetBlockHeaderByHash(UInt256 blockHeaderHash)
        {
            return _model.GetByHash<BlockHeader>(DataEntryPrefix.DataBlock, blockHeaderHash);
        }

        // Is there any use case where this method is need???
        //public async Task<byte[]> GetRawBlockHeader(UInt256 blockHash)
        //{
        //    return await _dbContext.GetByHash(blockHash.BuildKey(DataEntryPrefix.DataBlock));
        //}
        #endregion
    }
}
