using Domain.Common;
using Newtonsoft.Json;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Adapters.Persistance
{
    public class PostgresqlDocumentStore<T> : IDisposable where T : AggregateRoot
    {
        private NpgsqlConnection _connection;

        public PostgresqlDocumentStore(string connectionString)
        {
            _connection = new NpgsqlConnection(connectionString);
        }

        public void PrepareConnection()
        {
            if (_connection.State != ConnectionState.Open)
                _connection.Open();
        }

        public IEnumerable<T> GetById(string key)
        {
            PrepareConnection();

            var cmd = GetSelectCommand(key);

            using (var reader = cmd.ExecuteReader())
            {
                return GetAggregatesFromReader(reader).ToList();
            }
        }

        private IEnumerable<T> GetAggregatesFromReader(IDataReader reader)
        {
            while (reader.Read())
            {
                string data = reader.GetString(2);
                var agg = JsonConvert.DeserializeObject<T>(data);
                agg.SurrogateId(reader.GetInt64(0));
                agg.Version(reader.GetInt32(3));
                yield return agg;
            }
        }

        public int Add(T item)
        {
            PrepareConnection();

            return GetInsertCommand(item).ExecuteNonQuery();
        }

        public int Update(T item)
        {
            PrepareConnection();

            return GetUpdateCommand(item).ExecuteNonQuery();
        }

        private IDbCommand GetUpdateCommand(T item)
        {
            var cmd = _connection.CreateCommand();
            cmd.CommandText = "update " + item.GetType().Name + " set data = @json,version=version+1 where id=@id and version=@version";

            var paramId = cmd.CreateParameter();
            paramId.ParameterName = "@id";
            paramId.DbType = DbType.Int64;
            paramId.Value = item.SurrogateId();
            cmd.Parameters.Add(paramId);

            var paramValue = cmd.CreateParameter();
            paramValue.ParameterName = "@json";
            paramValue.NpgsqlDbType = NpgsqlDbType.Jsonb;
            paramValue.NpgsqlValue = JsonConvert.SerializeObject(item);
            cmd.Parameters.Add(paramValue);

            var paramVersion = cmd.CreateParameter();
            paramVersion.ParameterName = "@version";
            paramVersion.DbType = DbType.Int32;
            paramVersion.Value = item.Version();
            cmd.Parameters.Add(paramVersion);

            return cmd;
        }

        private IDbCommand GetSelectCommand(string key)
        {
            var cmd = _connection.CreateCommand();
            cmd.CommandText = "select id, bk, data, version from " + typeof(T).Name + " where bk = @bk";

            var paramBk = cmd.CreateParameter();
            paramBk.ParameterName = "@bk";
            paramBk.DbType = DbType.String;
            paramBk.Value = key;

            cmd.Parameters.Add(paramBk);

            return cmd;
        }

        private IDbCommand GetInsertCommand(T item)
        {
            var cmd = _connection.CreateCommand();

            cmd.CommandText = "insert into " + item.GetType().Name + " (bk,data,version) values (@bk,@json,1)";

            var paramBk = cmd.CreateParameter();
            paramBk.ParameterName = "@bk";
            paramBk.DbType = DbType.String;
            paramBk.Value = item.Id;
            cmd.Parameters.Add(paramBk);

            var paramValue = cmd.CreateParameter();
            paramValue.ParameterName = "@json";
            paramValue.NpgsqlDbType = NpgsqlDbType.Jsonb;
            paramValue.NpgsqlValue = JsonConvert.SerializeObject(item);
            paramValue.Value = JsonConvert.SerializeObject(item);
            cmd.Parameters.Add(paramValue);

            return cmd;
        }

        public void Dispose()
        {
            _connection.Dispose();
        }
    }

}
