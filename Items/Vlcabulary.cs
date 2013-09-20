using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Windows.Forms;
using System.Drawing;
using WFExceptions;
using System.Collections.ObjectModel;
using GeneLibrary.Common;
using System.Data.OracleClient;
using WFDatabase;

namespace GeneLibrary.Items
{
    public abstract class Vocabulary
    {
        protected Vocabulary(){}
        
        // Методы для работы со списком значений
        public abstract void Open();
        public abstract void Open(DataGridView dataGridView);
        public virtual void Open(DataGridView dataGridView, string filter)
        {
            throw new WFException(ErrType.Assert, ErrorsMsg.NotRealizeVirtualMethod);
        }
        public abstract bool Del(int[] ids);
        public abstract bool IsEmpty();
        public abstract string[] IdAndCaption();
        public abstract string TableName();
        public int CountItems
        {
            get
            {
                return DT.Rows.Count;
            }
        }
        public DataTable DT { get; set; }
        public virtual void ToExcel(string header, Point startCell, DataGridView dataGridView)
        {
            Tools.ToExcel(header  + ", " + Properties.Resources.DataOn + " " + DateTime.Today.ToLongDateString(), startCell, dataGridView, ExcelCellOrientation.Horizontal);
        }
        public bool CompareDictionary(int cardId, int[] cardsList)
        {
            return compareVocabulare.CompareDictionary(cardId, cardsList, this.TableName().Split('.')[1], this.TableName().Split('.')[0]);
        }
        
        // Методы для работы с элементом из списка значений
        public abstract void LoadItem();
        public abstract void LoadItem(int id);

        // Pirvate
        CompareVocabulare compareVocabulare = GateFactory.CompareVocabulare();
    }

    public abstract class CompareVocabulare
    {
        public abstract bool CompareDictionary(int cardId, int[] cardsList, string tabelName, string ownerName);
    }

    public class CompareVocabulareOracle : CompareVocabulare
    {
        public override bool CompareDictionary(int cardId, int[] cardsList, string tabelName, string ownerName)
        {
            WFOracle.DB.StartTransaction();
            try
            {
                string sql = "begin modern.compare_dictionary(:a_old_id, :a_new_id, :a_table_name, :a_primary_key, :a_owner); end;";

                OracleCommand cmd = new OracleCommand(sql, WFOracle.DB.OracleConnection, WFOracle.DB.OracleTransaction);
                cmd.Parameters.Add("a_old_id", OracleType.Number);
                WFOracle.AddInParameter("a_new_id", OracleType.Number, cardId, cmd, false);
                WFOracle.AddInParameter("a_table_name", OracleType.NVarChar, tabelName, cmd, false);
                WFOracle.AddInParameter("a_primary_key", OracleType.NVarChar, "id", cmd, false);
                WFOracle.AddInParameter("a_owner", OracleType.NVarChar, ownerName, cmd, false);

                foreach (int i in cardsList)
                {
                    cmd.Parameters["a_old_id"].Value = i;
                    cmd.ExecuteNonQuery();
                }

                WFOracle.DB.Commit();
                return true;
            }
            catch
            {
                WFOracle.DB.Rollback();
                return false;
            }
        }
    }

}
