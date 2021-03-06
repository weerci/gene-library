﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Windows.Forms;
using WFDatabase;
using System.Data.OracleClient;
using GeneLibrary.Common;
using System.Globalization;
using System.Collections.ObjectModel;


namespace GeneLibrary.Items
{
    public class RoleVocabulary : Vocabulary
    {
        // Private members
        private RoleVocabularyGate _gate = GateFactory.RoleDictionaryGate();

        // Constructors
        public RoleVocabulary() : base() { }
        
        // Public methods
        public override void Open()
        {
            //ID	NUMBER(10)				
            //NAME	NVARCHAR2(50)						
            base.DT = new DataTable();
            base.DT.Locale = CultureInfo.InvariantCulture;
            DataColumn Id = new DataColumn("ID", Type.GetType("System.Int32"));
            Id.Caption = resDataNames.roleTableID;
            DataColumn Name = new DataColumn("NAME", Type.GetType("System.String"));
            Name.Caption = resDataNames.roleTableName;

            DT.Columns.Add(Id);
            DT.Columns.Add(Name);

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
            Item = new RoleItemInner();
        }
        public override void LoadItem(int id)
        {
            Item = new RoleItemInner(id);
        }
        public override string[] IdAndCaption()
        {
            return new string[2] { "ID", "NAME" };
        }
        public override string TableName()
        {
            return _gate.TableName();
        }

        // Properties
        public DataTable LinTable
        {
            get
            {
                return DT;
            }
        }
        public RoleItem Item { get; set; }

        // Nested classes
        class RoleItemInner : RoleItem
        {
            public RoleItemInner() : base() { }
            public RoleItemInner(int id) : base(id) { }
        }

    }
    public class RoleItem
    {
        // Private fields
        private RoleItemGate _gate = GateFactory.RoleItemGate();
        private List<ComboBoxItem> _roles = new List<ComboBoxItem>();
        private List<ComboBoxItem> _allRight = new List<ComboBoxItem>();
        private List<ComboBoxItem> _acceptRight = new List<ComboBoxItem>();


        // Constructors
        protected RoleItem() { }
        protected RoleItem(int id)
        {
            this.Id = id;
        }

        // Public interface
        public void Open()
        {
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
        
        // Property
        public int Id { get; set; }
        public string Name { get; set; }
        public Collection<ComboBoxItem> Roles 
        {
            get
            {
                return new Collection<ComboBoxItem>(_roles);
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

    }

    public abstract class RoleVocabularyGate
    {
        public abstract void Open(DataTable dataTable);
        public abstract bool Del(int[] ids);
        public abstract string TableName();
    }
    public class RoleVocabularyGateOracle : RoleVocabularyGate
    {
        public override void Open(DataTable dataTable)
        {
            OracleWork.OracleSqlSimple("select id, name, id||name fhash from modern.roles order by name", dataTable, "id");
        }
        public override bool Del(int[] ids)
        {
            WFOracle.DB.StartTransaction();
            try
            {
                OracleWork.OracleDeleteSimple("modern.prk_tab.role_del", ids, "a_id");
                WFOracle.DB.Commit();
            }
            catch
            {
                WFOracle.DB.Rollback();
                throw;
            }
            return true;
        }
        public override string TableName()
        {
            return "modern.roles";
        }
    }

    public abstract class RoleItemGate
    {
        public abstract void Open(RoleItem roleItem);
        public abstract int Insert(RoleItem roleItem);
        public abstract void Update(RoleItem roleItem);
    }
    public class RoleItemGateOracle : RoleItemGate
    {
        // Private members
        private static void LoadRoles(RoleItem roleItem)
        {
            String sql = "select id, name from modern.roles where id <> :id";
            OracleCommand cmd = new OracleCommand(sql, WFOracle.DB.OracleConnection);
            WFOracle.AddInParameter("id", OracleType.Number, roleItem.Id, cmd, false);

            using (OracleDataReader rdr = cmd.ExecuteReader())
            {
                roleItem.Roles.Clear();
                while (rdr.Read())
                {
                    roleItem.Roles.Add(new ComboBoxItem(Convert.ToInt32(rdr["id"], CultureInfo.InvariantCulture), rdr["name"].ToString()));
                }
            }
        }
        private static void LoadRight(RoleItem roleItem)
        {
            string sql =
                " select t.id, (select note from modern.protect_function where id = t.id) note from (" +
                " select pf.id from modern.protect_function pf where id <> 1" +
                " minus (select func_id from modern.action_roles where role_id = :role_id)) t";
            OracleCommand cmd = new OracleCommand(sql, WFOracle.DB.OracleConnection);
            WFOracle.AddInParameter("role_id", OracleType.Number, roleItem.Id, cmd, false);

            using (OracleDataReader rdr = cmd.ExecuteReader())
            {
                roleItem.AllRight.Clear();
                while (rdr.Read())
                {
                    roleItem.AllRight.Add(new ComboBoxItem(Convert.ToInt32(rdr["id"], CultureInfo.InvariantCulture), rdr["note"].ToString()));
                }
            }
        }
        private static void LoadAcceptRight(RoleItem roleItem)
        {
            String sql =
                " select pf.id, pf.note from modern.action_roles ar, modern.protect_function pf" +
                " where ar.role_id = :role_id and ar.func_id = pf.id";
            OracleCommand cmd = new OracleCommand(sql, WFOracle.DB.OracleConnection);
            WFOracle.AddInParameter("role_id", OracleType.Number, roleItem.Id, cmd, false);

            using (OracleDataReader rdr = cmd.ExecuteReader())
            {
                roleItem.AcceptRight.Clear();
                while (rdr.Read())
                {
                    roleItem.AcceptRight.Add(new ComboBoxItem(Convert.ToInt32(rdr["id"], CultureInfo.InvariantCulture), rdr["note"].ToString()));
                }
            }
        }
        private static void LoadRightInDb(RoleItem roleItem, int roleId)
        {
            OracleCommand cmd = new OracleCommand("begin modern.prk_tab.role_right_del(:a_role_id); end;", WFOracle.DB.OracleConnection, WFOracle.DB.OracleTransaction);
            cmd.CommandType = CommandType.Text;
            WFOracle.AddInParameter("a_role_id", OracleType.Number, roleId, cmd, false);
            cmd.ExecuteNonQuery();

            cmd.Parameters.Clear();
            cmd.CommandText = "begin modern.prk_tab.role_right_ins(:a_role_id, :a_func_id); end;";
            OracleParameter role_id = cmd.Parameters.Add("a_role_id", OracleType.Number);
            role_id.Value = roleId;
            OracleParameter func_id = cmd.Parameters.Add("a_func_id", OracleType.Number);
            foreach (ComboBoxItem cbi in roleItem.AcceptRight)
            {
                func_id.Value = cbi.Id;
                cmd.ExecuteNonQuery();
            }
        }

        // Public interface
        public override void Open(RoleItem roleItem)
        {
            String sql = "select id, name from modern.roles where id = :id";
            OracleCommand cmd = new OracleCommand(sql, WFOracle.DB.OracleConnection);
            WFOracle.AddInParameter("id", OracleType.Number, roleItem.Id, cmd, false);

            using (OracleDataReader rdr = cmd.ExecuteReader())
            {
                if (rdr.Read())
                {
                    roleItem.Id = Convert.ToInt32(rdr["id"], CultureInfo.InvariantCulture);
                    roleItem.Name = rdr["name"].ToString();
                }
            }

            LoadRoles(roleItem);
            LoadRight(roleItem);
            LoadAcceptRight(roleItem);
        }
        public override int Insert(RoleItem roleItem)
        {
            string sql = "begin :res := modern.prk_tab.role_ins(:a_name); end;";
            OracleParameter prmRes;

            WFOracle.DB.StartTransaction();
            try
            {
                // Создание новой роли
                OracleCommand cmd = new OracleCommand(sql, WFOracle.DB.OracleConnection, WFOracle.DB.OracleTransaction);
                cmd.CommandType = CommandType.Text;
                WFOracle.AddInParameter("a_name", OracleType.NVarChar, roleItem.Name, cmd, false);
                prmRes = WFOracle.AddOutParameter("res", OracleType.Number, cmd);
                cmd.ExecuteNonQuery();

                // Назначение прав роли
                LoadRightInDb(roleItem, Convert.ToInt32(prmRes.Value, CultureInfo.InvariantCulture));
                WFOracle.DB.Commit();
            }
            catch
            {
                WFOracle.DB.Rollback();
                throw;
            }
            return Convert.ToInt32(prmRes.Value, CultureInfo.InvariantCulture);
        }
        public override void Update(RoleItem roleItem)
        {
            string sql = "modern.prk_tab.role_upd";
            try
            {
                WFOracle.DB.StartTransaction();
                OracleCommand cmd = new OracleCommand(sql, WFOracle.DB.OracleConnection, WFOracle.DB.OracleTransaction);
                cmd.CommandType = CommandType.StoredProcedure;
                WFOracle.AddInParameter("a_id", OracleType.Number, roleItem.Id, cmd, false);
                WFOracle.AddInParameter("a_name", OracleType.NVarChar, roleItem.Name, cmd, false);
                cmd.ExecuteNonQuery();

                // Назначение прав роли
                LoadRightInDb(roleItem, roleItem.Id);                

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
