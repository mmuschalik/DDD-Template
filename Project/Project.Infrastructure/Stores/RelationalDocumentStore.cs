using Domain.Common;
using Domain.Common.Infrastructure;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Infrastructure.Stores
{
    public class RelationalDocumentStore<T> : IRepository<T> where T : AggregateRoot
    {
        private IDbConnectionFactory _connectionFactory;
        private string _connectionString;

        public RelationalDocumentStore(IDbConnectionFactory connectionFactory, string connectionString)
        {
            _connectionFactory = connectionFactory;
            _connectionString = connectionString;
        }

        public T GetById(object key)
        {
            using (var _connection = _connectionFactory.CreateConnection(_connectionString))
            {
                _connection.Open();

                var cmd = _connection.CreateCommand();

                cmd.CommandText = "select id, bk, data, version from " + typeof(T).Name + " where bk = @id";

                var paramId = cmd.CreateParameter();
                paramId.ParameterName = "@id";
                paramId.DbType = DbType.String;
                paramId.Value = key;

                var data = cmd.ExecuteScalar();
                var agg = JsonConvert.DeserializeObject<T>(data.ToString());

                //agg.SurrogateId(0)

                return agg;
            }
        }

        public void Save(T item)
        {
            using (var _connection = _connectionFactory.CreateConnection(_connectionString))
            {
                _connection.Open();

                var cmd = _connection.CreateCommand();

                if (item.IsNew())
                    cmd.CommandText = "insert into " + item.GetType().Name + " (data) values (@json)"; // insert bk seperately

                else
                {
                    cmd.CommandText = "update " + item.GetType().Name + " set data = @json where id=@id and version=@version";

                    var paramId = cmd.CreateParameter();
                    paramId.ParameterName = "@id";
                    paramId.DbType = DbType.Int64;  // maybe need to handle multiple types
                    paramId.Value = item.SurrogateId();
                    cmd.Parameters.Add(paramId);
                }

                var paramValue = cmd.CreateParameter();
                paramValue.ParameterName = "@json";
                paramValue.DbType = DbType.String;
                paramValue.Value = JsonConvert.SerializeObject(item);
                
                cmd.Parameters.Add(paramValue);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
