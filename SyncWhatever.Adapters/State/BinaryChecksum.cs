using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using SyncWhatever.Core.Interfaces;
using SyncWhatever.Core.State;

namespace SyncWhatever.Components.State
{
    public class BinaryChecksum : ISyncState
    {
        private BinaryChecksum()
        {
        }

        public static BinaryChecksum Calculate(object value, object key, string syncTaskId)
        {
            using (var stream = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, value);
                var objectBytes = stream.ToArray();

                var md5 = MD5.Create();
                var hashBytes = md5.ComputeHash(objectBytes);

                return new BinaryChecksum
                {
                    EntityKey = key.ToString(),
                    SyncTaskId = syncTaskId,
                    EntityState = BitConverter.ToString(hashBytes)
                };
            }
        }

        public override string ToString()
        {
            return $"EntityKey = '{EntityKey}', EntityState = '{EntityState}'";
        }

        public string SyncTaskId { get; set; }
        public string EntityKey { get; set; }
        public string EntityState { get; set; }
    }
}