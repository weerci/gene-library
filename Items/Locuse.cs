using GeneLibrary.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.OracleClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WFDatabase;

namespace GeneLibrary.Items
{
    public class LocuseVocabulary : Vocabulary
    {
        // Private members
        private LocuseVocabularyGate _gate = GateFactory.LocuseVocabularyGate();

        // Constructors
        public LocuseVocabulary() : base() { }

        // Public methods
        public override void Open()
        {
            //ID	NUMBER(10)				
            //NAME	NVARCHAR2(50)						
            base.DT = new DataTable();
            base.DT.Locale = CultureInfo.InvariantCulture;
            DataColumn Id = new DataColumn("ID", Type.GetType("System.Int32"));
            Id.Caption = resDataNames.freqTableID;
            DataColumn Name = new DataColumn("NAME", Type.GetType("System.String"));
            Name.Caption = resDataNames.freqLocusTableName;

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
            Item = new LocuseItemInner();
        }
        public override void LoadItem(int id)
        {
            Item = new LocuseItemInner(id);
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
        public LocuseItemInner Item { get; set; }

        // Nested classes
        public class LocuseItemInner : LocuseItem
        {
            public LocuseItemInner() : base() { }
            public LocuseItemInner(int id) : base(id) { }
        }

    }

    public class LocuseItem
    {
        // Private fields
        private LocuseItemGate _gate = GateFactory.LocuseItemGate();
        private List<ChangedAllele> _allelies = new List<ChangedAllele>();


        // Constructors
        protected LocuseItem() { }
        protected LocuseItem(int id)
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
        public Collection<ChangedAllele> Allelies
        {
            get
            {
                return new Collection<ChangedAllele>(_allelies);
            }
        }
        public bool LocusNameIsChanged { get; set; }
    }

    public class ChangedAllele
    {
        public int Id 
        {
            get { return _id; }
            set { _id = value;} 
        }
        public string Name { get; set; }
        public double Val { get; set; }
        public ChangedLocusState State 
        {
            get { return _state; }
            set { _state = value; } 
        }

        private int _id = 0;
        private ChangedLocusState _state = ChangedLocusState.None;
    }

    public enum ChangedLocusState { Added, Edited, Deleted, None }
    
    public abstract class LocuseVocabularyGate
    {
        public abstract void Open(DataTable dataTable);
        public abstract bool Del(int[] ids);
        public abstract string TableName();
    }
    public class LocuseVocabularyGateOracle : LocuseVocabularyGate
    {
        public override void Open(DataTable dataTable)
        {
            OracleWork.OracleSqlSimple("select id, name, id||name fhash from modern.locus order by name", dataTable, "id");
        }
        public override bool Del(int[] ids)
        {
            WFOracle.DB.StartTransaction();
            try
            {
                OracleWork.OracleDeleteSimple("modern.prk_tab.locus_del", ids, "a_id");
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
            return "modern.locus";
        }
    }

    public abstract class LocuseItemGate
    {
        public abstract void Open(LocuseItem roleItem);
        public abstract int Insert(LocuseItem roleItem);
        public abstract void Update(LocuseItem roleItem);
    }
    public class LocuseItemGateOracle : LocuseItemGate
    {
        // Public interface
        public override void Open(LocuseItem locusItem)
        {
            String sql = " select l.id locus_id, l.name locus_name, a.id allele_id, a.name allele_name, a.val" +
                         " from modern.locus l left join modern.allele a on l.id = a.locus_id" +
                         " where l.id = :id order by val";

            OracleCommand cmd = new OracleCommand(sql, WFOracle.DB.OracleConnection);
            WFOracle.AddInParameter("id", OracleType.Number, locusItem.Id, cmd, false);

            using (OracleDataReader rdr = cmd.ExecuteReader())
            {
                if (rdr.Read())
                {

                    locusItem.Id = Convert.ToInt32(rdr["locus_id"], CultureInfo.InvariantCulture);
                    locusItem.Name = rdr["locus_name"].ToString();
                    locusItem.Allelies.Clear();
                    if (!String.IsNullOrEmpty(rdr["allele_id"].ToString()))
                    {
                        locusItem.Allelies.Add(new ChangedAllele() {
                                Id = Convert.ToInt32(rdr["allele_id"], CultureInfo.InvariantCulture),
                                Name = rdr["allele_name"].ToString(),
                                Val = Convert.ToDouble(rdr["val"], CultureInfo.InvariantCulture)
                            });
                        while (rdr.Read())
                        {
                            locusItem.Allelies.Add(new ChangedAllele()
                            {
                                Id = Convert.ToInt32(rdr["allele_id"], CultureInfo.InvariantCulture),
                                Name = rdr["allele_name"].ToString(),
                                Val = Convert.ToDouble(rdr["val"], CultureInfo.InvariantCulture)
                            });
                        }
                    }
                }
            }

            //LoadRoles(roleItem);
            //LoadRight(roleItem);
            //LoadAcceptRight(roleItem);
        }
        public override int Insert(LocuseItem locusItem)
        {
            string sql = "begin :res := modern.prk_tab.locus_ins(:a_name, :curr_user); end;";
            OracleParameter prmRes;

            WFOracle.DB.StartTransaction();
            try
            {

                // Создание нового локуса
                OracleCommand cmd = new OracleCommand(sql, WFOracle.DB.OracleConnection, WFOracle.DB.OracleTransaction);
                cmd.CommandType = CommandType.Text;
                WFOracle.AddInParameter("a_name", OracleType.NVarChar, locusItem.Name, cmd, false);
                WFOracle.AddInParameter("curr_user", OracleType.Number, GateFactory.LogOnBase().Id, cmd, true);
                prmRes = WFOracle.AddOutParameter("res", OracleType.Number, cmd);
                cmd.ExecuteNonQuery();

                locusItem.Id = Convert.ToInt32(prmRes.Value, CultureInfo.InvariantCulture);

                // Создание новых аллелей
                cmd.CommandText = "begin :res := modern.prk_tab.allele_ins(:a_locus_id, :a_name, :a_val); end;";
                StringBuilder note = new StringBuilder(String.Format("Изменения состава аллелей для локуса ID='{0}':\r\n", locusItem.Id));
                foreach (ChangedAllele item in locusItem.Allelies)
                {
                    cmd.Parameters.Clear();
                    WFOracle.AddInParameter("a_locus_id", OracleType.Number, locusItem.Id, cmd, false);
                    WFOracle.AddInParameter("a_name", OracleType.NVarChar, item.Name, cmd, false);
                    WFOracle.AddInParameter("a_val", OracleType.Number, item.Val, cmd, false);
                    prmRes = WFOracle.AddOutParameter("res", OracleType.Number, cmd);
                    cmd.ExecuteNonQuery();

                    int allele_id = Convert.ToInt32(prmRes.Value, CultureInfo.InvariantCulture);
                    note.Append(String.Format("Создана аллель ID={0}, Имя = {1}, Значение = {2}\r\n", allele_id, item.Name, item.Val));
                }

                // Создание истории
                cmd.CommandText = "begin :res := modern.prk_tab.set_locus_history(:a_id, :history_action, :expert_id, :note); end;";
                cmd.Parameters.Clear();
                WFOracle.AddInParameter("a_id", OracleType.Number, locusItem.Id, cmd, false);
                WFOracle.AddInParameter("history_action", OracleType.Number, 15, cmd, false);
                WFOracle.AddInParameter("expert_id", OracleType.Number, GateFactory.LogOnBase().Id, cmd, true);
                WFOracle.AddInParameter("note", OracleType.NVarChar, note.ToString(), cmd, true);
                prmRes = WFOracle.AddOutParameter("res", OracleType.Number, cmd);
                cmd.ExecuteNonQuery();

                WFOracle.DB.Commit();
            }
            catch
            {
                WFOracle.DB.Rollback();
                throw;
            }
            return locusItem.Id;
        }
        public override void Update(LocuseItem locusItem)
        {
            string sql = "modern.prk_tab.locus_upd";
            try
            {
                StringBuilder note = new StringBuilder();

                WFOracle.DB.StartTransaction();
                OracleCommand cmd = new OracleCommand(sql, WFOracle.DB.OracleConnection, WFOracle.DB.OracleTransaction);
                cmd.CommandType = CommandType.StoredProcedure;
                WFOracle.AddInParameter("a_id", OracleType.Number, locusItem.Id, cmd, false);
                WFOracle.AddInParameter("a_name", OracleType.NVarChar, locusItem.Name, cmd, false);
                WFOracle.AddInParameter("curr_user", OracleType.Number, GateFactory.LogOnBase().Id, cmd, true);

                if (locusItem.LocusNameIsChanged)
                {
                    cmd.ExecuteNonQuery();
                    note.Append(String.Format("Изменение локуса ID='{0}':\r\n", locusItem.Id));
                }

                // Добавление новых аллелей
                OracleParameter prmRes;
                note.Append(String.Format("Изменения состава аллелей для локуса ID='{0}':\r\n", locusItem.Id));
                foreach (ChangedAllele item in locusItem.Allelies.Where(n=>n.State == ChangedLocusState.Added))
                {
                    cmd.CommandText = "begin :res := modern.prk_tab.allele_ins(:a_locus_id, :a_name, :a_val); end;";
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Clear();
                    WFOracle.AddInParameter("a_locus_id", OracleType.Number, locusItem.Id, cmd, false);
                    WFOracle.AddInParameter("a_name", OracleType.NVarChar, item.Name, cmd, false);
                    WFOracle.AddInParameter("a_val", OracleType.Number, item.Val, cmd, false);
                    prmRes = WFOracle.AddOutParameter("res", OracleType.Number, cmd);
                    cmd.ExecuteNonQuery();

                    item.State = ChangedLocusState.None;
                    item.Id = Convert.ToInt32(prmRes.Value, CultureInfo.InvariantCulture);
                    note.Append(String.Format("Создана аллель ID={0}, Имя = {1}, Значение = {2}\r\n", item.Id, item.Name, item.Val));
                }

                // Редактирование существующих
                foreach (ChangedAllele item in locusItem.Allelies.Where(n => n.State == ChangedLocusState.Edited))
                {
                    cmd.CommandText = "modern.prk_tab.allele_upd";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Clear();
                    WFOracle.AddInParameter("a_id", OracleType.Number, item.Id, cmd, false);
                    WFOracle.AddInParameter("a_name", OracleType.NVarChar, item.Name, cmd, false);
                    WFOracle.AddInParameter("a_val", OracleType.Number, item.Val, cmd, false);
                    
                    cmd.ExecuteNonQuery();
                    note.Append(String.Format("Изменена аллель ID={0}, Имя = {1}, Значение = {2}\r\n", item.Id, item.Name, item.Val));
                    item.State = ChangedLocusState.None;
                }

                // Удаление несуществующих
                foreach (ChangedAllele item in locusItem.Allelies.Where(n => n.State == ChangedLocusState.Deleted))
                {
                    cmd.CommandText = "modern.prk_tab.allele_del";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Clear();
                    WFOracle.AddInParameter("a_id", OracleType.Number, item.Id, cmd, false);
                    cmd.ExecuteNonQuery();
                    note.Append(String.Format("Удалена аллель ID={0}\r\n", item.Id));

                }

                ChangedAllele[] notDeletedLocus = locusItem.Allelies.Where(n => n.State != ChangedLocusState.Deleted).ToArray();
                locusItem.Allelies.Clear();
                foreach (ChangedAllele item in notDeletedLocus)
                    locusItem.Allelies.Add(item);

                // Создание истории
                cmd.CommandText = "begin :res := modern.prk_tab.set_locus_history(:a_id, :history_action, :expert_id, :note); end;";
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Clear();
                WFOracle.AddInParameter("a_id", OracleType.Number, locusItem.Id, cmd, false);
                WFOracle.AddInParameter("history_action", OracleType.Number, 15, cmd, false);
                WFOracle.AddInParameter("expert_id", OracleType.Number, GateFactory.LogOnBase().Id, cmd, true);
                WFOracle.AddInParameter("note", OracleType.NVarChar, note.ToString(), cmd, true);
                prmRes = WFOracle.AddOutParameter("res", OracleType.Number, cmd);
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
