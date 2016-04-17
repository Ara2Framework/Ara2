// Copyright (c) 2010-2016, Rafael Leonel Pontani. All rights reserved.
// For licensing, see LICENSE.md or http://www.araframework.com.br/license
// This file is part of AraFramework project details visit http://www.arafrework.com.br
// AraFramework - Rafael Leonel Pontani, 2016-4-14
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace Ara2.StreamMD5
{
    class ClassStreamMD5
    {
        private byte[] _emptyBuffer = new byte[0];

        public string CalculateMD5(Stream stream)
        {
            return CalculateMD5(stream, 64 * 1024);
        }

        public string CalculateMD5(Stream stream, int bufferSize)
        {
            MD5 md5Hasher = MD5.Create();

            byte[] buffer = new byte[bufferSize];
            int readBytes;

            while ((readBytes = stream.Read(buffer, 0, bufferSize)) > 0)
            {
                md5Hasher.TransformBlock(buffer, 0, readBytes, buffer, 0);
            }

            md5Hasher.TransformFinalBlock(_emptyBuffer, 0, 0);

            return ByteArrayToString(md5Hasher.Hash);
        }

        private string ByteArrayToString(byte[] arrInput)
        {
            int i;
            StringBuilder sOutput = new StringBuilder(arrInput.Length);
            for (i = 0; i < arrInput.Length - 1; i++)
            {
                sOutput.Append(arrInput[i].ToString("X2"));
            }
            return sOutput.ToString();
        }

    }
}
