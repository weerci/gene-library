using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeneLibrary.Common
{
    public abstract class  GLDefault
    {
        public abstract int LocusViewCount { get; set;}
        public abstract int AlleleViewCount { get; set;}
        public abstract int ColumnFormFindHeaderWidth1 { get; set; }
        public abstract int ColumnFormFindHeaderWidth2 { get; set; }
        public abstract int DefaultMethod { get; set; }
        public abstract int CountCoincidence { get; set; }
        public abstract int CountLocus { get; set; }
        public abstract int Accurency { get; set; }
    }

    public class GeneLibraryDefaultOracle : GLDefault
    {
        public override int LocusViewCount { get { return 24; } set { ;} }
        public override int AlleleViewCount { get { return 19; } set { ;} }
        public override int ColumnFormFindHeaderWidth1 { get { return 35; } set { ;} }
        public override int ColumnFormFindHeaderWidth2 { get { return 120; } set { ;} }
        public override int DefaultMethod { get { return 1; } set { ;} }
        public override int CountCoincidence { get { return 2; } set { ;} }
        public override int CountLocus { get { return 6; } set { ;} }
        public override int Accurency { get { return 0; } set { ;} }
    }

}
