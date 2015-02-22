using Domain.Common;
using Domain.Common.Infrastructure;
using Newtonsoft.Json;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Infrastructure.Stores
{
    public class RelationalDocumentRepository<T> : IRepository<T> where T : AggregateRoot
    {
        private NpgsqlFactory _providerFactory;
        private string _connectionString;

        public RelationalDocumentRepository(string connectionString)
        {
            _providerFactory = NpgsqlFactory.Instance;
            _connectionString = connectionString;
        }

        public T GetById(string key)
        {
            using (var _connection = _providerFactory.CreateConnection())
            {
                _connection.ConnectionString = _connectionString;
                _connection.Open();

                var cmd = _connection.CreateCommand();

                cmd.CommandText = "select id, bk, data, version from " + typeof(T).Name + " where bk = @bk";

                var paramBk = cmd.CreateParameter();
                paramBk.ParameterName = "@bk";
                paramBk.DbType = DbType.String;
                paramBk.Value = key;

                cmd.Parameters.Add(paramBk);

                using (var reader = cmd.ExecuteReader())
                {
                    reader.Read();
                    string data = reader.GetString(2);
                    var agg = JsonConvert.DeserializeObject<T>(data);
                    agg.SurrogateId(reader.GetInt64(0));
                    agg.Version(reader.GetInt32(3));
                    return agg;
                }
            }
        }

        public void Save(T item)
        {
            using (var _connection = _providerFactory.CreateConnection())
            {
                _connection.ConnectionString = _connectionString;
                _connection.Open();

                if (item.IsNew())
                    GetInsertCommand(item, _connection).ExecuteNonQuery();
                else
                {
                    int rowsupdated = GetUpdateCommand(item, _connection).ExecuteNonQuery();

                    if (rowsupdated == 0)
                        throw new DBConcurrencyException();
                }

                item.Version(item.Version() + 1);   // increase the version for the item, in case it is used again
            }
        }

        private IDbCommand GetInsertCommand(T item, IDbConnection connection)
        {
            var cmd = new NpgsqlCommand();
            cmd.Connection = connection as NpgsqlConnection;

            cmd.CommandText = "insert into " + item.GetType().Name + " (bk,data,version) values (@bk,@json,1)";

            var paramBk = cmd.CreateParameter();
            paramBk.ParameterName = "@bk";
            paramBk.DbType = DbType.String;
            paramBk.Value = item.Id;
            cmd.Parameters.Add(paramBk);

            var paramValue = cmd.CreateParameter();
            paramValue.ParameterName = "@json";
            paramValue.NpgsqlDbType = NpgsqlDbType.Json;
            paramValue.NpgsqlValue = JsonConvert.SerializeObject(item);
            paramValue.Value = JsonConvert.SerializeObject(item);
            cmd.Parameters.Add(paramValue);

            return cmd;
        }

        private IDbCommand GetUpdateCommand(T item, IDbConnection connection)
        {
            var cmd = new NpgsqlCommand();
            cmd.Connection = connection as NpgsqlConnection;

            cmd.CommandText = "update " + item.GetType().Name + " set data = @json,version=version+1 where id=@id and version=@version";

            var paramId = cmd.CreateParameter();
            paramId.ParameterName = "@id";
            paramId.DbType = DbType.Int64;
            paramId.Value = item.SurrogateId();
            cmd.Parameters.Add(paramId);

            var paramValue = cmd.CreateParameter();
            paramValue.ParameterName = "@json";
            paramValue.NpgsqlDbType = NpgsqlDbType.Json;
            paramValue.NpgsqlValue = JsonConvert.SerializeObject(item);
            cmd.Parameters.Add(paramValue);

            var paramVersion = cmd.CreateParameter();
            paramVersion.ParameterName = "@version";
            paramVersion.DbType = DbType.Int32;
            paramVersion.Value = item.Version();
            cmd.Parameters.Add(paramVersion);

            return cmd;
        }



    }
}
