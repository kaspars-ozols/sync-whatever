using System.Text;
using Newtonsoft.Json;
using SyncWhatever.Components.FileSystem.Abstract;

namespace SyncWhatever.Components.FileSystem.Json
{
    public class JsonFileRepository<TEntity> : AbstractFileRepository<TEntity>
    {
        public override string Extension { get; set; }

        protected override byte[] Serialize(TEntity entity)
        {
            var json = JsonConvert.SerializeObject(entity);
            return Encoding.UTF8.GetBytes(json);
        }

        protected override TEntity Deserialize(byte[] content)
        {
            var json = Encoding.UTF8.GetString(content);
            return JsonConvert.DeserializeObject<TEntity>(json);
        }
    }
}