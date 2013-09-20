using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using WFDatabase;
using System.Data.OracleClient;
using GeneLibrary.Common;
using System.Collections.ObjectModel;
using System.Globalization;

namespace GeneLibrary.Items.Find
{
    // Поиск совпадений в карточках
    public class FindCoincidence
    {
        public FindCoincidence() { }

        // Public methods
        public virtual void FindOnMinimum()
        {
            Init(MethodOfFind.direct);
            gate.Find(this);
        }
        public virtual void FindOnHalf()
        {
            Init(MethodOfFind.half);
            gate.FindHalf(this);
        }
        public virtual void FindOneParent()
        {
            Init(MethodOfFind.half);
            gate.FindOneParent(this);
        }
        public virtual void FindChildByParents()
        {
            Init(MethodOfFind.half);
            gate.FindChildByParents(this);
        }
        public virtual void FindIklForBlend()
        {
            Init(MethodOfFind.direct);
            gate.FindIklForBlend(this);
        }
        public virtual void FindTwoIklForBlend()
        {
            Init(MethodOfFind.direct);
            gate.FindTwoIklForBlend(this);
        }

        // Properties
        public virtual DataTable FindResult 
        {
            get 
            {
                return gate.FindResult;
            }
        }
        public virtual FindCondition FoundCondition { get { return findCondition; } }

        // Private methods
        private void Init(MethodOfFind methodOfFind)
        {
            DataColumn ProfileId = new DataColumn("PROFILE_ID", Type.GetType("System.Int32"));
            ProfileId.Caption = resDataNames.findCoincidenceProfile;
            gate.FindResult.Columns.Add(ProfileId);

            DataColumn CountName = new DataColumn("CNT", Type.GetType("System.Int32"));
            if (methodOfFind == MethodOfFind.half)
                CountName.Caption = resDataNames.findLocusCount;
            else
                CountName.Caption = resDataNames.findCoincidenceCount;
            gate.FindResult.Columns.Add(CountName);

            DataColumn TypeName = new DataColumn("TYPE_NAME", Type.GetType("System.String"));
            TypeName.Caption = resDataNames.findCoincidenceCardType;
            gate.FindResult.Columns.Add(TypeName);

            DataColumn CardType = new DataColumn("CARD_TYPE", Type.GetType("System.String"));
            CardType.Caption = "card_type";
            gate.FindResult.Columns.Add(CardType);

            DataColumn CardNumber = new DataColumn("CARD_NUMBER", Type.GetType("System.String"));
            CardNumber.Caption = resDataNames.findCoincidenceCardNumber;
            gate.FindResult.Columns.Add(CardNumber);

            DataColumn ExamNumber = new DataColumn("EXAM_NUM", Type.GetType("System.String"));
            ExamNumber.Caption = resDataNames.findCoincidenceExamNumber;
            gate.FindResult.Columns.Add(ExamNumber);

        }

        // Private members
        private FindCoincidenceGate gate = GateFactory.FindCoincidenceGate();
        private FindCondition findCondition = new FindCondition();
    }

    public class FindCondition
    {
        public FindField CountCoincidenceString 
        {
            set 
            {
                this.countCoincidence = value;
                for (int i = 0; i < countCoincidence.FieldName.Count(); i++)
                {
                    Tools.GetIntFromText(countCoincidence.FieldValue[i], countCoincidence.FieldName[i]);
                }
            }
        }
        public FindField CountLocusString 
        { 
            set
            {
                this.countLocus = value;
                for (int i = 0; i < countLocus.FieldName.Count(); i++)
                {
                    Tools.GetIntFromText(countLocus.FieldValue[i], countLocus.FieldName[i]);
                }
            }
        }
        public FindField CardsString 
        {
            set
            {
                this.cards = value;
                for (int i = 0; i < cards.FieldName.Count(); i++)
                {
                    Tools.GetIntFromText(cards.FieldValue[i], cards.FieldName[i]);
                }
            }
        }
        public FindField AccurencyString
        {
            set
            {
                this.accurency = value;
                for (int i = 0; i < accurency.FieldName.Count(); i++)
                {
                    Tools.GetIntFromText(accurency.FieldValue[i], accurency.FieldName[i]);
                }
            }
        }

        public int CountCoincidence 
        { 
            get 
            {
                if (this.countCoincidence.FieldValue.Count() > 0)
                    return Convert.ToInt32(this.countCoincidence.FieldValue.ElementAt(0), CultureInfo.InvariantCulture);
                else
                    return GateFactory.GLDefault().CountCoincidence;
            } 
        }
        public int CountLocus
        {
            get
            {
                if (this.countLocus.FieldValue.Count() > 0)
                    return Convert.ToInt32(this.countLocus.FieldValue.ElementAt(0), CultureInfo.InvariantCulture);
                else
                    return GateFactory.GLDefault().CountLocus;
            } 
        }
        public int[] Cards
        {
            get
            {
                return cards.FieldValue.Select(n => Convert.ToInt32(n, CultureInfo.InvariantCulture)).ToArray<int>();
            }
        }
        public int Accurency
        {
            get
            {
                if (this.accurency.FieldValue.Count() > 0)
                    return Convert.ToInt32(this.accurency.FieldValue.ElementAt(0), CultureInfo.InvariantCulture);
                else
                    return GateFactory.GLDefault().Accurency;
            }
        }

        // Private
        private FindField countCoincidence;
        private FindField countLocus;
        private FindField cards;
        private FindField accurency;

        public struct FindField
        {
            public string[] FieldName;
            public string[] FieldValue;
        }
    }

    public class FindDecorator : FindCoincidence
    {
        // Constructors
        public FindDecorator(FindCoincidence findCoincidence)
        {
            this.findCoincidence = findCoincidence;
        }

        public override void FindChildByParents()
        {
            findCoincidence.FindChildByParents();
        }
        public override void FindOneParent()
        {
            findCoincidence.FindOneParent();
        }
        public override void FindOnHalf()
        {
            findCoincidence.FindOnHalf();
        }
        public override void FindOnMinimum()
        {
            findCoincidence.FindOnMinimum();
        }
        public override void FindIklForBlend()
        {
            findCoincidence.FindIklForBlend();
        }
        public override void FindTwoIklForBlend()        
        {
            findCoincidence.FindTwoIklForBlend();
        }

        public override DataTable FindResult
        {
            get
            {
                return findCoincidence.FindResult;
            }
        }
        public override FindCondition FoundCondition
        {
            get
            {
                return findCoincidence.FoundCondition;
            }
        }

        // Private 
        private FindCoincidence findCoincidence;
    }
    
    // Интерфейс шлюза поиска
    public abstract class FindCoincidenceGate
    {
        // Interface
        public abstract void Find(FindCoincidence findCoincidence);
        public abstract void FindHalf(FindCoincidence findCoincidence);
        public abstract void FindOneParent(FindCoincidence findCoincidence);
        public abstract void FindChildByParents(FindCoincidence findCoincidence);
        public abstract void FindIklForBlend(FindCoincidence findCoincidence);
        public abstract void FindTwoIklForBlend(FindCoincidence findCoincidence);

        //Properties
        public DataTable FindResult { get { return findResult; } }

        // Fields
        private DataTable findResult = new DataTable();
    }
    
    // Реализация интерфейса поиска для Oracle
    public class FindCoincidenceGateOracle : FindCoincidenceGate
    {
        // Interface
        public override void Find(FindCoincidence findCoincidence)
        {
            string sql =
                " select case when \"card_type\" = 0 then '"+Tools.CardKindName(CardKind.ikl)+"' else '"+
                Tools.CardKindName(CardKind.ik2)+"' end type_name, \"profile_id\", \"card_type\", \"cnt\"," +
                " \"card_number\", \"exam_num\" from table(modern.pkg_card.find(:a_card_id, :a_allele_count, :a_locus_count))"+
                " order by \"cnt\"";

            OracleCommand cmd = new OracleCommand(sql, WFOracle.DB.OracleConnection);
            WFOracle.AddInParameter("a_card_id", OracleType.Number, findCoincidence.FoundCondition.Cards[0], cmd, false);
            WFOracle.AddInParameter("a_allele_count", OracleType.Number, findCoincidence.FoundCondition.CountCoincidence + 1, cmd, false);
            WFOracle.AddInParameter("a_locus_count", OracleType.Number, findCoincidence.FoundCondition.CountLocus - 1, cmd, false);
            OracleDataAdapter da = new OracleDataAdapter(cmd);
            FindResult.Clear();
            da.Fill(FindResult);
            FindResult.PrimaryKey = new DataColumn[] { FindResult.Columns["profile_id"] };
        }
        public override void FindHalf(FindCoincidence findCoincidence)
        {
            string sql =
                " select case when \"card_type\" = 0 then '" + Tools.CardKindName(CardKind.ikl) + "' else '" + Tools.CardKindName(CardKind.ik2) + "' end type_name, \"profile_id\", \"card_type\", \"cnt\"," +
                " \"card_number\", \"exam_num\" from table(modern.pkg_card.find_half(:a_card_id, :a_locus_count, :a_accuracy))" +
                " order by \"cnt\"";
            OracleCommand cmd = new OracleCommand(sql, WFOracle.DB.OracleConnection, WFOracle.DB.OracleTransaction);
            WFOracle.AddInParameter("a_card_id", OracleType.Number, findCoincidence.FoundCondition.Cards[0], cmd, false);
            WFOracle.AddInParameter("a_locus_count", OracleType.Number, findCoincidence.FoundCondition.CountLocus - 1, cmd, false);
            WFOracle.AddInParameter("a_accuracy", OracleType.Number, findCoincidence.FoundCondition.Accurency, cmd, false);
            OracleDataAdapter da = new OracleDataAdapter(cmd);

            FindResult.Clear();
            da.Fill(FindResult);
            FindResult.PrimaryKey = new DataColumn[] { FindResult.Columns["profile_id"] };
        }
        public override void FindOneParent(FindCoincidence findCoincidence)
        {
            string sql =
                " select case when \"card_type\" = 0 then '" + Tools.CardKindName(CardKind.ikl) + "' else '" + Tools.CardKindName(CardKind.ik2) + "' end type_name, \"profile_id\", \"card_type\", \"cnt\"," +
                " \"card_number\", \"exam_num\" from table(modern.pkg_card.find_half_one_parent(:a_profile_child_id, :a_profile_parent_id, :a_locus_count))" +
                " order by \"cnt\"";
            OracleCommand cmd = new OracleCommand(sql, WFOracle.DB.OracleConnection, WFOracle.DB.OracleTransaction);
            WFOracle.AddInParameter("a_profile_child_id", OracleType.Number, findCoincidence.FoundCondition.Cards[0], cmd, false);
            WFOracle.AddInParameter("a_profile_parent_id", OracleType.Number, findCoincidence.FoundCondition.Cards[1], cmd, false);
            WFOracle.AddInParameter("a_locus_count", OracleType.Number, findCoincidence.FoundCondition.CountLocus - 1, cmd, false);
            OracleDataAdapter da = new OracleDataAdapter(cmd);

            FindResult.Clear();
            da.Fill(FindResult);
            FindResult.PrimaryKey = new DataColumn[] { FindResult.Columns["profile_id"] };
        }
        public override void FindChildByParents(FindCoincidence findCoincidence)
        {
            string sql =
                " select case when \"card_type\" = 0 then '" + Tools.CardKindName(CardKind.ikl) + "' else '" + Tools.CardKindName(CardKind.ik2) + "' end type_name, \"profile_id\", \"card_type\", \"cnt\"," +
                " \"card_number\", \"exam_num\" from table(modern.pkg_card.find_child_by_parents(:a_prfile_parent_one, :a_profile_parent_second, :a_locus_count))" +
                " order by \"cnt\"";
            OracleCommand cmd = new OracleCommand(sql, WFOracle.DB.OracleConnection, WFOracle.DB.OracleTransaction);
            WFOracle.AddInParameter("a_prfile_parent_one", OracleType.Number, findCoincidence.FoundCondition.Cards[0], cmd, false);
            WFOracle.AddInParameter("a_profile_parent_second", OracleType.Number, findCoincidence.FoundCondition.Cards[1], cmd, false);
            WFOracle.AddInParameter("a_locus_count", OracleType.Number, findCoincidence.FoundCondition.CountLocus - 1, cmd, false);
            OracleDataAdapter da = new OracleDataAdapter(cmd);

            FindResult.Clear();
            da.Fill(FindResult);
            FindResult.PrimaryKey = new DataColumn[] { FindResult.Columns["profile_id"] };
        }
        public override void FindIklForBlend(FindCoincidence findCoincidence)
        {
            string sql =
                " select case when \"card_type\" = 0 then '" + Tools.CardKindName(CardKind.ikl) + "' else '" +
                Tools.CardKindName(CardKind.ik2) + "' end type_name, \"profile_id\", \"card_type\", \"cnt\"," +
                " \"card_number\", \"exam_num\" from table(modern.pkg_card.find(:a_card_id, :a_allele_count, :a_locus_count))" +
                " where \"card_type\" = 0 order by \"cnt\"";

            OracleCommand cmd = new OracleCommand(sql, WFOracle.DB.OracleConnection);
            WFOracle.AddInParameter("a_card_id", OracleType.Number, findCoincidence.FoundCondition.Cards[0], cmd, false);
            WFOracle.AddInParameter("a_allele_count", OracleType.Number, findCoincidence.FoundCondition.CountCoincidence + 1, cmd, false);
            WFOracle.AddInParameter("a_locus_count", OracleType.Number, findCoincidence.FoundCondition.CountLocus - 1, cmd, false);
            OracleDataAdapter da = new OracleDataAdapter(cmd);
            FindResult.Clear();
            da.Fill(FindResult);
            FindResult.PrimaryKey = new DataColumn[] { FindResult.Columns["profile_id"] };
        }
        public override void FindTwoIklForBlend(FindCoincidence findCoincidence)
        {
            string sql =
                " select case when \"card_type\" = 0 then '" + Tools.CardKindName(CardKind.ikl) + "' else '" +
                Tools.CardKindName(CardKind.ik2) + "' end type_name, \"profile_id\", \"card_type\", \"cnt\"," +
                " \"card_number\", \"exam_num\" from table(modern.pkg_card.find_ik2_by_two_ikl(:a_card_ikl1, :a_card_ikl2, :a_allele_count, :a_locus_count))" +
                " where \"card_type\" = 1 order by \"cnt\"";

            OracleCommand cmd = new OracleCommand(sql, WFOracle.DB.OracleConnection);
            WFOracle.AddInParameter("a_card_ikl1", OracleType.Number, findCoincidence.FoundCondition.Cards[0], cmd, false);
            WFOracle.AddInParameter("a_card_ikl2", OracleType.Number, findCoincidence.FoundCondition.Cards[1], cmd, false);
            WFOracle.AddInParameter("a_allele_count", OracleType.Number, findCoincidence.FoundCondition.CountCoincidence + 1, cmd, false);
            WFOracle.AddInParameter("a_locus_count", OracleType.Number, findCoincidence.FoundCondition.CountLocus - 1, cmd, false);
            OracleDataAdapter da = new OracleDataAdapter(cmd);
            FindResult.Clear();
            da.Fill(FindResult);
            FindResult.PrimaryKey = new DataColumn[] { FindResult.Columns["profile_id"] };
        }

    }

    public enum MethodOfFind { direct, half}
}
