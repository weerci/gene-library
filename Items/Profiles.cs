using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using WFExceptions;
using WFDatabase;
using System.Data;
using System.Globalization;
using System.Data.OracleClient;
using GeneLibrary.Common;

namespace GeneLibrary.Items
{

    public class Profiles
    {
        public Profiles(){}
        public Profiles(string name):this()
        {
            this.Name = name;
        }

        // Interface   
        public void CheckAlleleInLocus(Locus locus, Allele allele, bool condition)
        {
            findLocusId = locus.Id;
            Locus findLocus = locusList.Find(FindLocus);
            if (findLocus != null)
                findLocus.CheckAllele(allele, condition);
            
            // Вызов события
            if (CheckAllele != null)
                CheckAllele(locus, allele, condition);
        }
        public void Load()
        {
            this.Load(0, GateFactory.GLDefault().DefaultMethod);
        }
        public void Load(int profileId, int methodId)
        {
            profileGate.Load(profileId, methodId);
            int i = 0;
            LocusInner locusInner = null;
            foreach (DataRow dataRow in profileGate.DataTable.Rows)
            {
                int LocusId = Convert.ToInt32(dataRow["LOCUS_ID"], CultureInfo.InvariantCulture);
                if (LocusId != i)
                {
                    locusInner = new LocusInner(
                        LocusId,
                        dataRow["LOCUS_NAME"].ToString());
                    locusInner.AddAllele(
                        Convert.ToInt32(dataRow["ALLELE_ID"], CultureInfo.InvariantCulture),
                        dataRow["ALLELE_NAME"].ToString(),
                        Convert.ToDouble(dataRow["VAL"], CultureInfo.InvariantCulture),
                        Convert.ToBoolean(dataRow["CHK"], CultureInfo.InvariantCulture),
                        Convert.ToDouble(dataRow["FREQ"], CultureInfo.InvariantCulture)
                        );
                    if (AddAllele != null)
                        AddAllele(locusInner, locusInner.Allele.Last<Allele>());
                    i = LocusId;
                    this.locusList.Add(locusInner);
                }
                else
                {
                    if (locusInner != null)
                    {
                        locusInner.AddAllele(
                            Convert.ToInt32(dataRow["ALLELE_ID"], CultureInfo.InvariantCulture),
                            dataRow["ALLELE_NAME"].ToString(),
                            Convert.ToDouble(dataRow["VAL"], CultureInfo.InvariantCulture),
                            Convert.ToBoolean(dataRow["CHK"], CultureInfo.InvariantCulture),
                            Convert.ToDouble(dataRow["FREQ"], CultureInfo.InvariantCulture)
                            );
                        if (AddAllele != null)
                            AddAllele(locusInner, locusInner.Allele.Last<Allele>());
                    }
                }
            }

        }
        public void Save(int CardId)
        {
            profileGate.Save(CardId, this);
        }

        // Properties
        public ReadOnlyCollection<Locus> Locus
        {
            get
            {
                return new ReadOnlyCollection<Locus>(locusList);
            }
        }
        /// <summary>
        /// Возвращает локус в профиле, по его имени
        /// </summary>
        /// <param name="item">Имя локуса</param>
        /// <returns>Локус</returns>
        public Locus this[string item]
        {
            get
            {
                findLocusName = item;
                return locusList.Find(FindLocusByName);
            }
        }
        public string Name { get; set; }
 
        // Private methods
        private bool FindLocus(Locus locus)
        {
            return findLocusId == locus.Id;
        }
        private bool FindLocusByName(Locus locus)
        {
            if (findLocusName.ToUpper() == "AMEL" && locus.Name.ToUpper() == "AMELOGENIN")
                return true;
            return findLocusName.ToUpper() == locus.Name.ToUpper();
        }

        // Fields
        protected List<Locus> locusList = new List<Locus>();
        private ProfileGate profileGate = GateFactory.ProfileGate();
        private int findLocusId;
        private string findLocusName;

        // Typies
        protected class LocusInner : Locus
        {
            public LocusInner(int id, string name) : base(id, name) { }
        }

        // Events
        internal protected delegate void CheckAlleleEventHandler(Locus locus, Allele allele, bool condition);
        internal protected delegate void AddAlleleEventHandler(Locus locus, Allele allele);
        internal event CheckAlleleEventHandler CheckAllele;
        internal event AddAlleleEventHandler AddAllele;
    }

    /// <summary>
    /// Класс предоставляющий фукнциональность локуса, имеет закрытые конструкторы, так что создать его можно 
    /// только при создании профиля
    /// </summary>
    public class Locus
    { 
        // Constructors
        protected Locus(int id, string name) 
        {
            this.Id = id;
            this.Name = name;
        }

        // Interface
        public void AddAllele(int id, string name, double value, bool check)
        {
            this.alleleList.Add(new AlleleInner(id, name, value, check));   
        }
        public void AddAllele(int id, string name, double value, bool check, double frequency)
        {
            this.alleleList.Add(new AlleleInner(id, name, value, check, frequency));
        }
        public void AddAllele(Allele allele)
        {
            this.alleleList.Add(allele);
        }
        /// <summary>
        /// Select/Unselect аллели по ее идентификатору
        /// </summary>
        /// <param name="id">Идентификатор аллели</param>
        /// <param name="condition">Если ture-аллель выбирается, если false - отказ от выбора аллели</param>
        public void CheckAllele(int id, bool condition)
        {
            this.findAlleleId = id;
            Allele checkedAllele = alleleList.Find(FindAllele);
            if (checkedAllele != null)
                checkedAllele.Checked = condition;
        }
        /// <summary>
        /// Проверка выбранности аллели, по ее идентификатору
        /// </summary>
        /// <param name="id">Идентификатор аллели</param>
        /// <returns>Если аллель в локусе выбрана - возвращается ture, если нет - false</returns>
        public bool CheckedAllele(int id)
        {
            this.findAlleleId = id;
            return alleleList.Find(FindCheckedAllele) != null;
        }
        public void CheckAllele(Allele allele, bool condition)
        {
            CheckAllele(allele.Id, condition);
        }

        // Properties
        public int Id { get; set; }
        public string Name { get; set; }
        public ReadOnlyCollection<Allele> Allele
        {
            get
            {
                return new ReadOnlyCollection<Allele>(alleleList);
            }
        }
        public Allele this[string item]
        {
            get
            {
                findAlleleName = item;
                return alleleList.Find(FindAlleleByName);
            }
        }
        public int CheckedAlleleCount
        {
            get 
            {
                return alleleList.Count(n => n.Checked);
            }
        }
        public bool IsHomozygotic { get { return alleleList.Count(n => n.Checked) == 1; } }
        
        // Private methods
        private bool FindAllele(Allele allele)
        {
            return allele.Id == findAlleleId;
        }
        private bool FindAlleleByName(Allele allele)
        {
            return findAlleleName.ToUpper() == allele.Name.ToUpper();
        }
        private bool FindCheckedAllele(Allele allele)
        {
            return allele.Id == findAlleleId && allele.Checked;
        }

        // Fields
        private List<Allele> alleleList = new List<Allele>();
        private int findAlleleId;
        private string findAlleleName;

        // Inner typies
        class AlleleInner : Allele
        {
            public AlleleInner(int id, string name, double value, bool check) : base(id, name, value, check) { }
            public AlleleInner(int id, string name, double value, bool check, double frequency) 
                : base(id, name, value, check, frequency) { }
        }
    }
    
    /// <summary>
    /// Класс предоставляющий функциональность аллелей, класс имеет закрытые конструкторы, так что создать его 
    /// можно только при создании локуса
    /// </summary>
    public class Allele
    { 
        // Constructors
        protected Allele(int id, string name, double value, bool check)
        {
            this.Id = id;
            this.Name = name;
            this.Value = value;
            this.Checked = check;
        }
        protected Allele(int id, string name, double value, bool check, double frequency) : this(id, name, value, check)
        {
            this.Frequency = frequency;
        }

        // Properties
        public int Id { get; set; }
        public string Name { get; set; }
        public double Value { get; set; }
        public double Frequency { get; set; }
        public bool Checked { get; set; }
    }

    /// <summary>
    /// Шлюзы для ИКЛ и ИК-2
    /// </summary>
    public abstract class ProfileGate
    {
        // Interface
        public abstract void Load(int profileId, int methodId);
        public abstract void Save(int cardId, Profiles profiles);

        //Properties
        public DataTable DataTable { get { return dataTable; } }

        // Fields
        private DataTable dataTable = new DataTable();
    }
    public class ProfileGateOracle : ProfileGate
    {
        // Interface
        public override void Load(int profileId, int methodId)
        {
            string sql =
                " select l.id locus_id, l.name locus_name, a.id allele_id, a.name allele_name, a.val," +
                " nvl((select ca.allele_id from modern.chk_allele ca where ca.profile_id = :ProfileId" +
                " and ca.locus_id = l.id and ca.allele_id = a.id), 0) chk, t.freq from modern.allele a, modern.locus l," +
                " (select a.id, nvl(freq, m.def_freq) freq from modern.allele a left join modern.allele_freq af" +
                " on a.id = af.allele_id and af.method_id = :MethodId, modern.method m where m.id = :MethodId) t where l.id = a.locus_id" +
                " and t.id = a.id order by locus_name";

            OracleCommand cmd = new OracleCommand(sql, WFOracle.DB.OracleConnection);
            WFOracle.AddInParameter("ProfileId", OracleType.Number, profileId, cmd, false);
            WFOracle.AddInParameter("MethodId", OracleType.Number, methodId, cmd, false);
            OracleDataAdapter da = new OracleDataAdapter(cmd);
            base.DataTable.Clear();
            da.Fill(base.DataTable);
            base.DataTable.PrimaryKey = new DataColumn[] { base.DataTable.Columns["LOCUS_ID"], base.DataTable.Columns["ALLELE_ID"] };
        }
        public override void Save(int cardId, Profiles profiles)
        {
            var result = from Locus locus in profiles.Locus
                         where locus.CheckedAlleleCount > 0
                         select locus;
            if (result.Count() == 0)
                return;

            string sql = "begin :res := modern.pkg_card.set_card_history(:a_id, :history_action, :expert_id, :note); end;";
            OracleParameter prmRes;

            // Обновляется таблица истории карточки
            OracleCommand cmd = new OracleCommand(sql, WFOracle.DB.OracleConnection, WFOracle.DB.OracleTransaction);
            cmd.CommandType = CommandType.Text;

            WFOracle.AddInParameter("a_id", OracleType.Number, cardId, cmd, false);
            WFOracle.AddInParameter("history_action", OracleType.Number, 7, cmd, false);
            WFOracle.AddInParameter("expert_id", OracleType.NVarChar, GateFactory.LogOnBase().Id, cmd, false);
            StringBuilder stringBuilder = new StringBuilder();
            foreach (Locus locus in result)
                stringBuilder.AppendLine(String.Format(resDataNames.ChangeCardProfile, cardId, locus.Name + ";" + (from Allele allele in locus.Allele where allele.Checked select allele.Name).Aggregate((first, second) => first + ";" + second)));
            WFOracle.AddInParameter("note", OracleType.NClob, stringBuilder.ToString(), cmd, false);
            prmRes = WFOracle.AddOutParameter("res", OracleType.Number, cmd);
            cmd.ExecuteNonQuery();

            // Обновление номера истории в таблице карточки
            cmd.CommandText = "begin modern.pkg_card.updHistory(:a_card_id, :a_hist_id); end;";
            cmd.Parameters.Clear();
            WFOracle.AddInParameter("a_card_id", OracleType.Number, cardId, cmd, false);
            WFOracle.AddInParameter("a_hist_id", OracleType.Number, Convert.ToInt32(prmRes.Value, CultureInfo.InvariantCulture), cmd, false);
            cmd.ExecuteNonQuery();

            // Очистка таблиц профиля
            cmd.CommandText = "begin modern.pkg_card.UpdataProfile(:a_profile_id); end;";
            cmd.Parameters.Clear();
            WFOracle.AddInParameter("a_profile_id", OracleType.Number, cardId, cmd, false);
            cmd.ExecuteNonQuery();

            // Создание профиля
            cmd.CommandText = "begin modern.pkg_card.AddAllele(:a_profile_id, :a_locus_id, :a_allele_id); end;";
            cmd.Parameters.Clear();
            WFOracle.AddInParameter("a_profile_id", OracleType.Number, cardId, cmd, false);
            cmd.Parameters.Add("a_locus_id", OracleType.Number);
            cmd.Parameters.Add("a_allele_id", OracleType.Number);
            foreach (Locus locus in result)
            {
                cmd.Parameters["a_locus_id"].Value = locus.Id;
                foreach (Allele allele in locus.Allele)
                    if (allele.Checked)
                    {
                        cmd.Parameters["a_allele_id"].Value = allele.Id;
                        cmd.ExecuteNonQuery();
                    }
            }
        }
    }

}
