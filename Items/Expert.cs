using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WFDatabase;
using System.Data;
using System.Data.OracleClient;
using System.Windows.Forms;
using GeneLibrary.Common;
using System.Globalization;
using System.Collections.ObjectModel;

namespace GeneLibrary.Items
{
    public class ExpertVocabulary : Vocabulary
    {
        // Constructors
        public ExpertVocabulary() : base() { }

        // Public methods
        public override void Open()
        {
            //ID	
            //DIVISION	
            //POST	NUMBER(10)			
            //SURNAME	NVARCHAR2(50)		
            //NAME	NVARCHAR2(50)		
            //PATRONIC	NVARCHAR2(50)		
            //SIGN	NVARCHAR2(32)									
            base.DT = new DataTable();
            base.DT.Locale = CultureInfo.InvariantCulture;
            DataColumn Id = new DataColumn("ID", Type.GetType("System.Int32"));
            Id.Caption = resDataNames.expTableID;
            DataColumn Division = new DataColumn("DIVISION", Type.GetType("System.String"));
            Division.Caption = resDataNames.expTableDevision;
            DataColumn Post = new DataColumn("POST", Type.GetType("System.String"));
            Post.Caption = resDataNames.expTablePost;
            DataColumn Surname = new DataColumn("SURNAME", Type.GetType("System.String"));
            Surname.Caption = resDataNames.expTableSurname;
            DataColumn Name = new DataColumn("NAME", Type.GetType("System.String"));
            Name.Caption = resDataNames.expTableName;
            DataColumn Patronic = new DataColumn("PATRONIC", Type.GetType("System.String"));
            Patronic.Caption = resDataNames.expTablePatronic;
            DataColumn Sign = new DataColumn("SIGN", Type.GetType("System.String"));
            Sign.Caption = resDataNames.expTableSign;

            DT.Columns.Add(Id);
            DT.Columns.Add(Division);
            DT.Columns.Add(Post);
            DT.Columns.Add(Surname);
            DT.Columns.Add(Name);
            DT.Columns.Add(Patronic);
            DT.Columns.Add(Sign);

            _gate.Open(DT);
        }
        public override void Open(DataGridView dataGridView)
        {
            this.Open();
            dataGridView.Columns.Clear();
            dataGridView.DataSource = base.DT;
            foreach (DataColumn dc in DT.Columns)
                dataGridView.Columns[dc.ColumnName].HeaderText = dc.Caption;
            dataGridView.Columns["fhash"].Visible = false;
        }
        public override bool Del(int[] ids)
        {
            return _gate.Del(ids);
        }
        public override bool IsEmpty()
        {
            return DT == null || DT.Rows.Count == 0;
        }
        public override void LoadItem()
        {
            Item = new ExpItemInner();
        }
        public override void LoadItem(int id)
        {
            Item = new ExpItemInner(id);
        }
        public override string[] IdAndCaption()
        {
            return new string[3] { "ID", "SURNAME", "NAME" };
        }
        public override string TableName()
        {
            return _gate.TableName();
        }

        // Properties
        public DataTable ExpTable
        {
            get
            {
                return DT;
            }
        }
        public ExpertItem Item { get; set; }

        // Types
        class ExpItemInner : ExpertItem
        {
            public ExpItemInner() : base() { }
            public ExpItemInner(int id) : base(id) { }
        }

        // Private members
        private ExpertVocabularyGate _gate = GateFactory.ExpertDictionaryGate();

    }
    public class ExpertItem : IDisposable
    {
        // Constructors
        protected ExpertItem() { }
        protected ExpertItem(int id)
        {
            this.Id = id;
        }

        // Public methods
        public void Open()
        {
            this.Init();
            _gate.Open(this);
        }
        public int Save()
        {
            if (this.Id == 0)
                return _gate.Insert(this);
            else
            {
                _gate.Update(this);
                return this.Id;
            }
        }
        public void ChangePassword()
        {
            _gate.ChangePassword(this);
        }

        // Properties
        public int Id { get; set; }
        public int DivisionId { get; set; }
        public string DivisionName { get; set; }
        public int PostId { get; set; }
        public string PostName { get; set; }
        public string Surname { get; set; }
        public string Name { get; set; }
        public string Patronymic { get; set; }
        public string LogOn { get; set; }
        public string Sign { get; set; }
        public string Password { get; set; }
        public DataTable DTPost
        {
            get
            {
                return _dtPost;
            }
        }
        public DataTable DTDivision
        {
            get
            {
                return _dtDiv;
            }
        }
        public Collection<ComboBoxItem> AllRoles
        {
            get
            {
                return new Collection<ComboBoxItem>(_allRole);
            }
        }
        public Collection<ComboBoxItem> AcceptRoles
        {
            get
            {
                return new Collection<ComboBoxItem>(_acceptRole);
            }
        }
        public Collection<ComboBoxItem> AllRight
        {
            get
            {
                return new Collection<ComboBoxItem>(_allRight);
            }
        }
        public Collection<ComboBoxItem> AcceptRight
        {
            get
            {
                return new Collection<ComboBoxItem>(_acceptRight);
            }
        }
        public Collection<ComboBoxItem> RevokeRight
        {
            get
            {
                return new Collection<ComboBoxItem>(_revokeRight);
            }
        }

        // Private methods
        private void Init()
        {
            DataColumn PostId = new DataColumn("ID", Type.GetType("System.Int32"));
            DataColumn PostName = new DataColumn("NAME", Type.GetType("System.String"));
            _dtPost.Columns.Add(PostId);
            _dtPost.Columns.Add(PostName);

            DataColumn DivId = new DataColumn("ID", Type.GetType("System.Int32"));
            DataColumn DivName = new DataColumn("NAME", Type.GetType("System.String"));
            DataColumn Address = new DataColumn("ADDRESS", Type.GetType("System.String"));
            DataColumn DivPhone = new DataColumn("PHONE", Type.GetType("System.String"));
            _dtDiv.Columns.Add(DivId);
            _dtDiv.Columns.Add(DivName);
            _dtDiv.Columns.Add(Address);
            _dtDiv.Columns.Add(DivPhone);
        }

        // Fields
        private DataTable _dtPost = new DataTable() { Locale = CultureInfo.InvariantCulture };
        private DataTable _dtDiv = new DataTable() { Locale = CultureInfo.InvariantCulture };
        private ExpertItemGate _gate = GateFactory.ExpertItemGate();
        private List<ComboBoxItem> _allRight = new List<ComboBoxItem>();
        private List<ComboBoxItem> _acceptRight = new List<ComboBoxItem>();
        private List<ComboBoxItem> _revokeRight = new List<ComboBoxItem>();
        private List<ComboBoxItem> _allRole = new List<ComboBoxItem>();
        private List<ComboBoxItem> _acceptRole = new List<ComboBoxItem>();

        // Dispose
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                DTDivision.Dispose();
                DTPost.Dispose();
            }
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }

    public abstract class ExpertVocabularyGate
    {
        public abstract void Open(DataTable dataTable);
        public abstract bool Del(int[] ids);
        public abstract string TableName();
    }
    public class ExpertVocabularyGateOracle : ExpertVocabularyGate
    {
        public override void Open(DataTable dataTable)
        {
            OracleWork.OracleSqlSimple(
                "select id, division, post, surname, name, patronic, sign, id||division||post||surname||name||patronic||sign fhash from modern.v_expert order by surname",
                dataTable, "id");
        }
        public override bool Del(int[] ids)
        {
            WFOracle.DB.StartTransaction();
            try
            {
                OracleCommand cmd = new OracleCommand("modern.prk_tab.exp_del", WFOracle.DB.OracleConnection, WFOracle.DB.OracleTransaction);
                cmd.CommandType = CommandType.StoredProcedure;

                foreach (int i in ids)
                {
                    cmd.Parameters.Clear();
                    WFOracle.AddInParameter("a_id", OracleType.Number, i, cmd, false);
                    WFOracle.AddInParameter("a_curr_user", OracleType.Number, GateFactory.LogOnBase().Id, cmd, false);
                    cmd.ExecuteNonQuery();
                }

                WFOracle.DB.Commit();
                return true;
            }
            catch
            {
                WFOracle.DB.Rollback();
                throw;
            }
        }
        public override string TableName()
        {
            return "modern.v_expert";
        }
    }

    public abstract class ExpertItemGate
    {
        public abstract void Open(ExpertItem expertItem);
        public abstract int Insert(ExpertItem expertItem);
        public abstract void Update(ExpertItem expertItem);
        public abstract void ChangePassword(ExpertItem expertItem);
    }
    public class ExpertItemGateOracle : ExpertItemGate
    {
        // Private methods
        private static void LoadAllRight(ExpertItem expertItem)
        {
            string sql =
            " select t.id, (select note from modern.protect_function where id = t.id) note" +
            " from (select pf.id from modern.protect_function pf where id <> 1 minus" +
            " (select func_id from modern.action_expet where expert_id = :expert_id))t";

            OracleCommand cmd = new OracleCommand(sql, WFOracle.DB.OracleConnection);
            WFOracle.AddInParameter("expert_id", OracleType.Number, expertItem.Id, cmd, false);

            using (OracleDataReader rdr = cmd.ExecuteReader())
            {
                expertItem.AllRight.Clear();
                while (rdr.Read())
                {
                    expertItem.AllRight.Add(new ComboBoxItem(Convert.ToInt32(rdr["id"], CultureInfo.InvariantCulture), rdr["note"].ToString()));
                }
            }
        }
        private static void LoadAcceptRight(ExpertItem expertItem, int permission)
        {
            string sql =
                " select pf.id, pf.note from modern.action_expet ae, modern.protect_function pf" +
                " where ae.expert_id = :expert_id and ae.func_id = pf.id and ae.permission = :a_permission";

            OracleCommand cmd = new OracleCommand(sql, WFOracle.DB.OracleConnection);
            WFOracle.AddInParameter("expert_id", OracleType.Number, expertItem.Id, cmd, false);
            WFOracle.AddInParameter("a_permission", OracleType.Number, permission, cmd, false);

            using (OracleDataReader rdr = cmd.ExecuteReader())
            {
                if (permission == 1)
                    expertItem.AcceptRight.Clear();
                else
                    expertItem.RevokeRight.Clear();
                while (rdr.Read())
                {
                    if (permission == 1)
                        expertItem.AcceptRight.Add(new ComboBoxItem(Convert.ToInt32(rdr["id"], CultureInfo.InvariantCulture), rdr["note"].ToString()));
                    else
                        expertItem.RevokeRight.Add(new ComboBoxItem(Convert.ToInt32(rdr["id"], CultureInfo.InvariantCulture), rdr["note"].ToString()));
                }
            }
        }
        private static void LoadAllRoles(ExpertItem expertItem)
        {
            string sql =
                " select t.id, (select name from modern.roles where id = t.id) name" +
                " from (select id from modern.roles minus (select role_id from modern.roles_expert re " +
                " where re.expert_id = :expert_id)) t order by name";
            OracleCommand cmd = new OracleCommand(sql, WFOracle.DB.OracleConnection);
            WFOracle.AddInParameter("expert_id", OracleType.Number, expertItem.Id, cmd, false);

            using (OracleDataReader rdr = cmd.ExecuteReader())
            {
                expertItem.AllRoles.Clear();
                while (rdr.Read())
                {
                    expertItem.AllRoles.Add(new ComboBoxItem(Convert.ToInt32(rdr["id"], CultureInfo.InvariantCulture), rdr["name"].ToString()));
                }
            }
        }
        private static void LoadAcceptRoles(ExpertItem expertItem)
        {
            string sql =
            " select r.id, r.name from modern.roles_expert re, modern.roles r" +
            " where r.id = re.role_id and re.expert_id = :expert_id";
            OracleCommand cmd = new OracleCommand(sql, WFOracle.DB.OracleConnection);
            WFOracle.AddInParameter("expert_id", OracleType.Number, expertItem.Id, cmd, false);

            using (OracleDataReader rdr = cmd.ExecuteReader())
            {
                expertItem.AcceptRoles.Clear();
                while (rdr.Read())
                {
                    expertItem.AcceptRoles.Add(new ComboBoxItem(Convert.ToInt32(rdr["id"], CultureInfo.InvariantCulture), rdr["name"].ToString()));
                }
            }

        }
        private static void SavePermissionExpert(ExpertItem expertItem)
        {
            // Удаление существующих разрешений
            OracleCommand cmd = new OracleCommand("begin modern.prk_tab.right_user_del(:a_user_id); end;",
                WFOracle.DB.OracleConnection, WFOracle.DB.OracleTransaction);
            cmd.CommandType = CommandType.Text;
            OracleParameter user_id = cmd.Parameters.Add("a_user_id", OracleType.Number);
            user_id.Value = expertItem.Id;
            cmd.ExecuteNonQuery();

            // Новые роли
            cmd.CommandText = "begin modern.prk_tab.role_user_ins(:a_user_id, :a_role_id); end;";
            OracleParameter role_id = cmd.Parameters.Add("a_role_id", OracleType.Number);
            foreach (ComboBoxItem cbi in expertItem.AcceptRoles)
            {
                role_id.Value = cbi.Id;
                cmd.ExecuteNonQuery();
            }

            // Индивидуальные разрешения
            cmd.Parameters.Remove(role_id);
            cmd.CommandText = "begin modern.prk_tab.right_user_ins(:a_user_id, :a_func_id, :a_permission); end;";
            OracleParameter permission = cmd.Parameters.Add("a_permission", OracleType.Number);
            OracleParameter func_id = cmd.Parameters.Add("a_func_id", OracleType.Number);
            foreach (ComboBoxItem cbi in expertItem.AcceptRight)
            {
                func_id.Value = cbi.Id;
                permission.Value = 1;
                cmd.ExecuteNonQuery();
            }

            // Индивидуальные запрещения
            foreach (ComboBoxItem cbi in expertItem.RevokeRight)
            {
                func_id.Value = cbi.Id;
                permission.Value = -1;
                cmd.ExecuteNonQuery();
            }
        }

        // Public interface
        public override void Open(ExpertItem expertItem)
        {
            String sql =
                " select id, division_id, division_name, post_id, post, surname, name, patronic," +
                " login, sign from modern.v_expert where id = :id";
            OracleCommand cmd = new OracleCommand(sql, WFOracle.DB.OracleConnection);
            WFOracle.AddInParameter("id", OracleType.Number, expertItem.Id, cmd, false);

            using (OracleDataReader rdr = cmd.ExecuteReader())
            {
                if (rdr.Read())
                {
                    expertItem.Id = Convert.ToInt32(rdr["id"], CultureInfo.InvariantCulture);
                    expertItem.DivisionId = Convert.ToInt32(rdr["division_id"], CultureInfo.InvariantCulture);
                    expertItem.DivisionName = rdr["division_name"].ToString();
                    expertItem.PostId = Convert.ToInt32(rdr["post_id"], CultureInfo.InvariantCulture);
                    expertItem.PostName = rdr["post"].ToString();
                    expertItem.Surname = rdr["surname"].ToString();
                    expertItem.Name = rdr["name"].ToString();
                    expertItem.Patronymic = rdr["patronic"].ToString();
                    expertItem.LogOn = rdr["login"].ToString();
                    expertItem.Sign = rdr["sign"].ToString();
                }
            }

            OracleWork.OracleSqlSimple("select id, name from modern.post order by name", expertItem.DTPost, "id");
            OracleWork.OracleSqlSimple("select id, name, address, phone from modern.division order by name", expertItem.DTDivision, "id");

            // Список прав и ролей
            LoadAllRight(expertItem);
            LoadAcceptRight(expertItem, 1);
            LoadAcceptRight(expertItem, -1);
            LoadAllRoles(expertItem);
            LoadAcceptRoles(expertItem);
        }
        public override int Insert(ExpertItem expertItem)
        {
            string sql = "begin :res := modern.prk_tab.exp_ins(:a_div_id, :a_post_id, :a_surname, :a_name, :a_patronic, :a_login, :a_sign); end;";
            OracleParameter prmRes;

            WFOracle.DB.StartTransaction();
            try
            {
                OracleCommand cmd = new OracleCommand(sql, WFOracle.DB.OracleConnection, WFOracle.DB.OracleTransaction);
                cmd.CommandType = CommandType.Text;

                WFOracle.AddInParameter("a_div_id", OracleType.Number, expertItem.DivisionId, cmd, false);
                WFOracle.AddInParameter("a_post_id", OracleType.Number, expertItem.PostId, cmd, false);
                WFOracle.AddInParameter("a_surname", OracleType.NVarChar, expertItem.Surname, cmd, false);
                WFOracle.AddInParameter("a_name", OracleType.NVarChar, expertItem.Name, cmd, false);
                WFOracle.AddInParameter("a_patronic", OracleType.NVarChar, expertItem.Patronymic, cmd, false);
                WFOracle.AddInParameter("a_login", OracleType.NVarChar, expertItem.LogOn, cmd, false);
                WFOracle.AddInParameter("a_sign", OracleType.NVarChar, expertItem.Sign, cmd, false);
                prmRes = WFOracle.AddOutParameter("res", OracleType.Number, cmd);

                cmd.ExecuteNonQuery();

                // Назначение прав доступа для пользователя
                expertItem.Id = Convert.ToInt32(prmRes.Value, CultureInfo.InvariantCulture);
                SavePermissionExpert(expertItem);

                WFOracle.DB.Commit();
            }
            catch
            {
                WFOracle.DB.Rollback();
                throw;
            }
            return Convert.ToInt32(prmRes.Value, CultureInfo.InvariantCulture);
        }
        public override void Update(ExpertItem expertItem)
        {
            string sql = "modern.prk_tab.exp_upd";
            try
            {
                WFOracle.DB.StartTransaction();
                OracleCommand cmd = new OracleCommand(sql, WFOracle.DB.OracleConnection, WFOracle.DB.OracleTransaction);
                cmd.CommandType = CommandType.StoredProcedure;

                WFOracle.AddInParameter("a_id", OracleType.Number, expertItem.Id, cmd, false);
                WFOracle.AddInParameter("a_div_id", OracleType.Number, expertItem.DivisionId, cmd, false);
                WFOracle.AddInParameter("a_post_id", OracleType.Number, expertItem.PostId, cmd, false);
                WFOracle.AddInParameter("a_surname", OracleType.NVarChar, expertItem.Surname, cmd, false);
                WFOracle.AddInParameter("a_name", OracleType.NVarChar, expertItem.Name, cmd, false);
                WFOracle.AddInParameter("a_patronic", OracleType.NVarChar, expertItem.Patronymic, cmd, false);
                WFOracle.AddInParameter("a_login", OracleType.NVarChar, expertItem.LogOn, cmd, false);
                WFOracle.AddInParameter("a_sign", OracleType.NVarChar, expertItem.Sign, cmd, false);

                cmd.ExecuteNonQuery();

                // Назначение прав доступа для пользователя
                SavePermissionExpert(expertItem);

                WFOracle.DB.Commit();
            }
            catch
            {
                WFOracle.DB.Rollback();
                throw;
            }
        }
        public override void ChangePassword(ExpertItem expertItem)
        {
            string sql = "modern.prk_tab.exp_change_password";
            try
            {
                WFOracle.DB.StartTransaction();
                OracleCommand cmd = new OracleCommand(sql, WFOracle.DB.OracleConnection, WFOracle.DB.OracleTransaction);
                cmd.CommandType = CommandType.StoredProcedure;
                WFOracle.AddInParameter("a_id", OracleType.Number, expertItem.Id, cmd, false);
                WFOracle.AddInParameter("a_password", OracleType.NVarChar, expertItem.Password, cmd, true);

                cmd.ExecuteNonQuery();

                WFOracle.DB.Commit();
            }
            catch
            {
                WFOracle.DB.Rollback();
                throw;
            }
        }
    }
}
