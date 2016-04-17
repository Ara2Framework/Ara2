// Copyright (c) 2010-2016, Rafael Leonel Pontani. All rights reserved.
// For licensing, see LICENSE.md or http://www.araframework.com.br/license
// This file is part of AraFramework project details visit http://www.arafrework.com.br
// AraFramework - Rafael Leonel Pontani, 2016-4-14
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Ara2
{
    public class CustomBinarySerializer<T>
    {
        public byte[] Serialize2Bytes(T data)
        {
            byte[] ret = null;
 
            if (data != null)
            {
                MemoryStream streamMemory = new MemoryStream();
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(streamMemory, data);
                ret = streamMemory.GetBuffer();
			}
 
            return ret;
        }
 
        public T DeserializeFromBytes(byte[] binData)
        {
            T retorno = default(T);
            if (binData != null || binData.Length != 0)
            {
                BinaryFormatter formatter = new BinaryFormatter();
                MemoryStream ms = new MemoryStream(binData);
                retorno = (T)formatter.Deserialize(ms);
 
            }
            return retorno;
        }
    }
}
