using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Collections.ObjectModel;


namespace GeneLibrary.Common
{
    public sealed class TextBoxId : TextBox
    {
        /// <summary>
        /// Idenitify from database
        /// </summary>
        public int Id { get; set; }
        public Collection<int> Ids 
        {
            get
            {
                return new Collection<int>(ids);
            }
        }

        private List<int> ids = new List<int>();
    }
}