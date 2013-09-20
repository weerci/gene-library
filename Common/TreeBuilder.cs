using System;
using System.Collections.Generic;
using System.Linq;
using GeneLibrary.Items;
using System.Text;
using System.Data;
using System.Data.OracleClient;
using System.Windows.Forms;
using WFDatabase;
using System.Globalization;

namespace GeneLibrary.Common
{
    public abstract class TreeBuilderBase
    {
        // Public method
        public abstract void CreateMainTree(TreeView treeView);
        /// <summary>
        /// Создается дерево всех прав, доступных для назначения пользователю
        /// </summary>
        /// <param name="treeView">Экземпляр класса TreeView, который заполняется правами назначенными пользователю</param>
        public abstract void CreateRightAllTree(TreeView treeView);
        /// <summary>
        /// Создается дерево всех прав назначенных пользователю
        /// </summary>
        /// <param name="treeView">Экземпляр класса TreeView, который заполняется правами назначенными пользователю</param>
        /// <param name="userId">Идентификатор пользователя</param>
        public abstract void CreateRightForAcceptTree(TreeView treeView, int userId);
    }

    class TreeBuilderOra : TreeBuilderBase
    {
        // Class interface
        public override void CreateMainTree(TreeView treeView)
        {
            treeView.Nodes.Clear();
            LogOnBase logonBase = GateFactory.LogOnBase();

            OracleCommand cmd = new OracleCommand(sqlMainTree, WFOracle.DB.OracleConnection);
            WFOracle.AddInParameter("group_id", OracleType.Number, 1, cmd, false);
            WFOracle.AddInParameter("expert", OracleType.Number, logonBase.Id, cmd, false);

            using (OracleDataReader rdr = cmd.ExecuteReader())
            {
                _isRead = rdr.Read();
                if (_isRead)
                {
                    _currNode = new TreeNode(resDataNames.formsList);
                    treeView.Nodes.Add(_currNode);
                    AddItemsToTree(rdr, _currNode, 0);
                }
            }
        }
        public override void CreateRightAllTree(TreeView treeView)
        {
            treeView.Nodes.Clear();
            LogOnBase logonBase = GateFactory.LogOnBase();

            OracleCommand cmd = new OracleCommand(sqlAllRightTree, WFOracle.DB.OracleConnection);
            using (OracleDataReader rdr = cmd.ExecuteReader())
            {
                _isRead = rdr.Read();
                if (_isRead)
                {
                    _currNode = new TreeNode(resDataNames.rightAllList);
                    treeView.Nodes.Add(_currNode);
                    AddItemsToTree(rdr, _currNode, 0);
                }
            }
        }
        public override void CreateRightForAcceptTree(TreeView treeView, int userId)
        {
            treeView.Nodes.Clear();

            OracleCommand cmd = new OracleCommand(sqlRightForAcceptTree, WFOracle.DB.OracleConnection);
            WFOracle.AddInParameter("expert", OracleType.Number, userId, cmd, false);

            using (OracleDataReader rdr = cmd.ExecuteReader())
            {
                _isRead = rdr.Read();
                if (_isRead)
                {
                    _currNode = new TreeNode(resDataNames.rightForAccept);
                    treeView.Nodes.Add(_currNode);
                    AddItemsToTree(rdr, _currNode, 0);
                }
            }
        }

        // Class realization
        private void AddItemsToTree(OracleDataReader rdr, TreeNode treeNode, int level)
        {
            if (_isRead)
            {
                if (Convert.ToInt32(rdr["lvl"], CultureInfo.InvariantCulture) == level)
                {
                    _currNode = new TreeNode(rdr["caption"].ToString());
                    _currNode.Name = rdr["name"].ToString();
                    treeNode.Nodes.Add(_currNode);
                    _isRead = rdr.Read();
                    AddItemsToTree(rdr, _currNode, level + 1);
                    return;
                }
                if (Convert.ToInt32(rdr["lvl"], CultureInfo.InvariantCulture) > level)
                {
                    AddItemsToTree(rdr, treeNode, level + 1);
                    return;
                }
                if (Convert.ToInt32(rdr["lvl"], CultureInfo.InvariantCulture) < level)
                {
                    AddItemsToTree(rdr, treeNode.Parent, level - 1);
                    return;
                }
            }
            else
                return;
        }

        // Fields
        private TreeNode _currNode;
        private bool _isRead;
        private string sqlMainTree =
            " select t.name, t.caption, t.func_id, lvl from modern.action_group ag," +
			" (select c.*, level - 1 lvl from (select * from modern.Controls" +
	        " where func_id in (select \"id\" from table(modern.pkg_scr.user_right(:expert)))) c" +
			" where c.id <> 0 connect by prior c.id = parent_id start with parent_id = 0" +
		    " order siblings by sort_ord) t where ag.res_id = t.id and ag.id = :group_id";      
        private string sqlAllRightTree =
            " select name, caption, func_id, level-1 lvl from modern.controls connect by prior id = parent_id" +
            " start with parent_id = 0 order siblings by sort_ord";
        private string sqlRightForAcceptTree =
            " select name, caption, func_id, level-1 lvl from (select * from modern.controls where func_id not in" +
            " (select \"id\" from table(modern.pkg_scr.user_right(:expert)))) t" +
            " connect by prior t.id = t.parent_id start with t.parent_id = 0"+
            " order siblings by t.sort_ord";
    }
}
