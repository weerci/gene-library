using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OracleClient;
using System.Collections;
using WFExceptions;
using WFDatabase;
using System.Security;
using System.Globalization;


namespace GeneLibrary.Items
{
    public abstract class LogOnBase
    {
        // Fields
        private HashSet<int> hashSet = new HashSet<int>();

        // Method
        public abstract void Enter(string name, string password, string server);
        public abstract void ReEnter(string name, string password, string server);
        public abstract void ChangePassword(string password);
        public abstract bool CheckPassword(string password);

        // Properties
        /// <summary>
        /// Имя пользователя базы
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Login пользователя
        /// </summary>
        public string LogOn { get; set; }
        /// <summary>
        /// Роль пользователя
        /// </summary>
        public string Role { get; set; }
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Таблица доступа
        /// </summary>
        public HashSet<int> HashSet
        {
            get
            {
                return hashSet;
            }
        }
    }
    public class LogOnOracle : LogOnBase
    {
        // Field
        private LogOnOracle() { }
        static private LogOnOracle _oracleLogin;

        /// <summary>
        /// Подключение к базе данных
        /// </summary>
        /// <param name="name">Имя пользователя</param>
        /// <param name="password">Пароль пользователя</param>
        /// <param name="server">Сервер подключения</param>
        public override void Enter(string name, string password, string server)
        {
            OracleConnectionStringBuilder builder = new OracleConnectionStringBuilder();
            builder.DataSource = Properties.Settings.Default.defServer;
            builder.UserID = Properties.Settings.Default.defLogin;
            builder.Password = Properties.Settings.Default.defPassword;

            try
            {
                OracleConnection connect = new OracleConnection(builder.ConnectionString);
                connect.Open();
                try
                {
                    OracleCommand cmd = new OracleCommand("auth.logon", connect);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("a_user_name", OracleType.NVarChar);
                    cmd.Parameters["a_user_name"].Value = name;
                    
                    cmd.Parameters.Add("a_user_password", OracleType.NVarChar);
                    cmd.Parameters["a_user_password"].Value = password;
                    
                    cmd.Parameters.Add("name_out", OracleType. NVarChar, 50);
                    cmd.Parameters["name_out"].Direction = ParameterDirection.Output;
                    
                    cmd.Parameters.Add("passwors_out", OracleType.NVarChar, 20);
                    cmd.Parameters["passwors_out"].Direction = ParameterDirection.Output;

                    cmd.Parameters.Add("a_user_id_out", OracleType.Number);
                    cmd.Parameters["a_user_id_out"].Direction = ParameterDirection.Output;
                    
                    cmd.Parameters.Add("a_user_name_out", OracleType.NVarChar, 50);
                    cmd.Parameters["a_user_name_out"].Direction = ParameterDirection.Output;
                    
                    cmd.Parameters.Add("a_user_login_out", OracleType.NVarChar, 20);
                    cmd.Parameters["a_user_login_out"].Direction = ParameterDirection.Output;

                    cmd.ExecuteNonQuery();

                    base.Id = Convert.ToInt32(cmd.Parameters["a_user_id_out"].Value, CultureInfo.InvariantCulture);
                    base.LogOn = cmd.Parameters["a_user_login_out"].Value.ToString();
                    base.Name = cmd.Parameters["a_user_name_out"].Value.ToString();

                    if (base.Id != 0)
                    {
                        GateFactory.DB().Connect(
                            server,
                            cmd.Parameters["name_out"].Value.ToString(),
                            cmd.Parameters["passwors_out"].Value.ToString());

                        // Получаем таблицу с разрешениями для пользователя
                        string sql =
                            " select func_id from (select ar.func_id, 1 perm from modern.roles_expert rle, modern.action_roles ar" +
                            " where ar.role_id = rle.role_id and rle.expert_id = :expert_id union all select re.func_id, re.permission perm" +
                            " from modern.action_expet re where expert_id = :expert_id) group by func_id having sum(perm) > 0";

                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = sql;
                        cmd.Parameters.Clear();
                        cmd.Parameters.Add("expert_id", OracleType.Number);
                        cmd.Parameters["expert_id"].Value = base.Id;

                        base.HashSet.Clear();
                        using (OracleDataReader rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                                base.HashSet.Add(Convert.ToInt32(rdr["func_id"], CultureInfo.InvariantCulture));
                        }
                    }
                    else
                    {
                        throw new WFException(ErrType.Message, ErrorsMsg.ConnectError);
                    }
                }
                finally
                {
                    connect.Close();
                }
            }
            catch
            {
                throw new WFException(ErrType.Message, ErrorsMsg.ConnectError);
            }
        }
        /// <summary>
        /// Закрытие текущего соединения и открытие нового
        /// </summary>
        /// <param name="name">Имя пользователя</param>
        /// <param name="password">Пароль</param>
        /// <param name="server">Серевер подключения</param>
        public override void ReEnter(string name, string password, string server) 
        {
            try
            {
                OracleCommand cmd = new OracleCommand("auth.logon", WFOracle.DB.OracleConnection, WFOracle.DB.OracleTransaction);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("a_user_name", OracleType.NVarChar);
                cmd.Parameters["a_user_name"].Value = name;

                cmd.Parameters.Add("a_user_password", OracleType.NVarChar);
                cmd.Parameters["a_user_password"].Value = password;

                cmd.Parameters.Add("name_out", OracleType.NVarChar, 50);
                cmd.Parameters["name_out"].Direction = ParameterDirection.Output;

                cmd.Parameters.Add("passwors_out", OracleType.NVarChar, 20);
                cmd.Parameters["passwors_out"].Direction = ParameterDirection.Output;

                cmd.Parameters.Add("a_user_id_out", OracleType.Number);
                cmd.Parameters["a_user_id_out"].Direction = ParameterDirection.Output;

                cmd.Parameters.Add("a_user_name_out", OracleType.NVarChar, 50);
                cmd.Parameters["a_user_name_out"].Direction = ParameterDirection.Output;

                cmd.Parameters.Add("a_user_login_out", OracleType.NVarChar, 20);
                cmd.Parameters["a_user_login_out"].Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();

                base.Id = Convert.ToInt32(cmd.Parameters["a_user_id_out"].Value, CultureInfo.InvariantCulture);
                base.LogOn = cmd.Parameters["a_user_login_out"].Value.ToString();
                base.Name = cmd.Parameters["a_user_name_out"].Value.ToString();

                if (base.Id != 0)
                {
                    // Получаем таблицу с разрешениями для пользователя
                    string sql =
                        " select func_id from (select ar.func_id, 1 perm from modern.roles_expert rle, modern.action_roles ar" +
                        " where ar.role_id = rle.role_id and rle.expert_id = :expert_id union all select re.func_id, re.permission perm" +
                        " from modern.action_expet re where expert_id = :expert_id) group by func_id having sum(perm) > 0";

                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = sql;
                    cmd.Parameters.Clear();
                    cmd.Parameters.Add("expert_id", OracleType.Number);
                    cmd.Parameters["expert_id"].Value = base.Id;

                    base.HashSet.Clear();
                    using (OracleDataReader rdr = cmd.ExecuteReader())
                        while (rdr.Read())
                            base.HashSet.Add(Convert.ToInt32(rdr["func_id"], CultureInfo.InvariantCulture));
                }
                else
                {
                    throw new WFException(ErrType.Message, ErrorsMsg.ConnectError);
                }
            }
            catch
            {
                throw new WFException(ErrType.Message, ErrorsMsg.ConnectError);
            }
        }
        public override void ChangePassword(string password)
        {
            string sql = "modern.prk_tab.exp_change_password";
            try
            {
                WFOracle.DB.StartTransaction();
                OracleCommand cmd = new OracleCommand(sql, WFOracle.DB.OracleConnection, WFOracle.DB.OracleTransaction);
                cmd.CommandType = CommandType.StoredProcedure;
                WFOracle.AddInParameter("a_id", OracleType.Number, base.Id, cmd, false);
                WFOracle.AddInParameter("a_password", OracleType.NVarChar, password, cmd, true);

                cmd.ExecuteNonQuery();

                WFOracle.DB.Commit();
            }
            catch
            {
                WFOracle.DB.Rollback();
                throw;
            }
        }
        public override bool CheckPassword(string password)
        {
            string sql = "begin :res := modern.prk_tab.exp_check_password(:a_id, :a_password);end;";
            OracleParameter prmRes;
            try
            {
                WFOracle.DB.StartTransaction();
                OracleCommand cmd = new OracleCommand(sql, WFOracle.DB.OracleConnection, WFOracle.DB.OracleTransaction);
                cmd.CommandType = CommandType.Text;

                WFOracle.AddInParameter("a_id", OracleType.Number, base.Id, cmd, false);
                WFOracle.AddInParameter("a_password", OracleType.NVarChar, password, cmd, false);
                prmRes = WFOracle.AddOutParameter("res", OracleType.Number, cmd);

                cmd.ExecuteNonQuery();
                WFOracle.DB.Commit();
            }
            catch
            {
                WFOracle.DB.Rollback();
                throw;
            }

            if (Convert.ToInt32(prmRes.Value, CultureInfo.InvariantCulture) == 1)
                return true;
            else
                return false;
        }

        static public LogOnBase LogOntoSystem()
        {
            if (_oracleLogin == null)
            {
                _oracleLogin = new LogOnOracle();
                return _oracleLogin;
            }
            else
            {
                return _oracleLogin;
            }
        }
    }
}

