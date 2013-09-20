using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using GeneLibrary.Common;
using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;
using System.Drawing;
using System.Globalization;

namespace GeneLibrary.Items
{
    public sealed class  Study
    {
        // Interface
        public static void ProbabilityIdentification(Profiles profile, out Double probabilityResult, out Double ratioResult)
        {
            double result = 1;
            foreach (Locus innerLocus in profile.Locus.Where(n => n.CheckedAlleleCount > 0 && n.Name != "Amelogenin"))
            {
                if (innerLocus.IsHomozygotic)
                    result *= DirectlyHomozigoticProbablyOne(innerLocus.Allele.Where(n => n.Checked).First());
                else
                    result *= DirectlyHeterozigoticProbablyOne(
                                innerLocus.Allele.Where(n => n.Checked).ElementAt(0),
                                innerLocus.Allele.Where(n => n.Checked).ElementAt(1)
                                );
            }
            probabilityResult = result;
            ratioResult = 1 / result;
        }
        public static void OneParentIdentification(Profiles profileChild, out Double probabilityResult, out Double ratioResult)
        {
            probabilityResult = 1;
            ratioResult = 1;

            // Вероятность случайного совпадения
            var locusesProbably = profileChild.Locus.Where(n => n.CheckedAlleleCount > 0 && n.Name != "Amelogenin");
            foreach (Locus innerLocus in locusesProbably)
            {
                if (innerLocus.IsHomozygotic)
                    probabilityResult *= OneParentHomozigoticProbablyOne(innerLocus.Allele.Where(n => n.Checked).ElementAt(0));
                else
                    probabilityResult *= OneParenHeterozigoticProbablyOne(
                                         innerLocus.Allele.Where(n => n.Checked).ElementAt(0),
                                         innerLocus.Allele.Where(n => n.Checked).ElementAt(1)
                                         );
            }
            ratioResult = 1 / probabilityResult;
        }
        public static void TooParentIdentification(Profiles profileChild, out Double probabilityResult, out Double ratioResult)
        {
            probabilityResult = 1;
            ratioResult = 1;

            // Вероятность случайного совпадения
            var locusesProbably = profileChild.Locus.Where(n => n.CheckedAlleleCount > 0 && n.Name != "Amelogenin");
            foreach (Locus innerLocus in locusesProbably)
            {
                if (innerLocus.IsHomozygotic)
                    probabilityResult *= TooParentHomozigoticProbablyOne(innerLocus.Allele.Where(n=>n.Checked).ElementAt(0));
                else
                    probabilityResult *= TooParentHeterozigoticProbablyOne(
                                         innerLocus.Allele.Where(n => n.Checked).ElementAt(0),
                                         innerLocus.Allele.Where(n => n.Checked).ElementAt(1)
                                         );
            }
            ratioResult = 1 / probabilityResult;
        }
        public static void CollaborationIntoBlendIdentification(Profiles profileIkl, Profiles profileIk2, 
            out Double probabilityResult, out Double ratioResult)
        { 
            probabilityResult = 1;
            ratioResult = 1;

            foreach (Locus locus in profileIkl.Locus.Where(n => n.CheckedAlleleCount > 0 && n.Name != "Amelogenin").
                    Intersect(profileIk2.Locus.Where(n => n.CheckedAlleleCount > 0 && n.Name != "Amelogenin"), new EqualityLocusById()))
            {
                probabilityResult *= CollaborationIntoBlendIdentificationOne(locus);
            
            }
            ratioResult = 1 / probabilityResult;
        }
        public static void ExportToExcel(KindStudy kindStudy, Profiles profile, Point startCell)
        {
            var locuses = profile.Locus.Where(n => n.CheckedAlleleCount > 0 && n.Name != "Amelogenin");
            Excel.Application applicationExcel = Tools.ExcelApplication();
            try
            {
                Excel.Workbook workBook = applicationExcel.Workbooks.Add(Missing.Value);
                Excel.Worksheet workSheet = (Excel.Worksheet)workBook.ActiveSheet;

                // Названия столбцов
                string[] columnHeader = new string[4] { 
                    ResourceStudy.locus, 
                    ResourceStudy.frequency,
                    ResourceStudy.formula, 
                    ResourceStudy.result};

                int i = 0;
                int j = 0;
                string[,] arrExcel = new string[locuses.Count(), columnHeader.Count()];
                PrintHeader(columnHeader, workSheet, startCell);

                switch (kindStudy)
                {
                    case KindStudy.Probably:
                        #region Probably
                        foreach (Locus locus in locuses)
                        {
                            arrExcel[i, j] = locus.Name;
                            arrExcel[i, j + 1] = locus.Allele.Where(n => n.Checked).Select(n => String.Format("P{0} = {1}", n.Name, n.Frequency)).
                                Aggregate((curr, next) => curr + "; " + next);
                            if (locus.IsHomozygotic)
                            {
                                arrExcel[i, j + 2] = String.Format(ResourceStudy.formulaHomozigoticProbably, locus.Allele.Where(n => n.Checked).First().Name);
                                arrExcel[i, j + 3] = DirectlyHomozigoticProbablyOne(locus.Allele.Where(n => n.Checked).First()).ToString("E03", CultureInfo.InvariantCulture);
                            }
                            else
                            {
                                arrExcel[i, j + 2] = String.Format(ResourceStudy.formulaHeteroZigoticProbably,
                                    locus.Allele.Where(n => n.Checked).ElementAt(0).Name,
                                    locus.Allele.Where(n => n.Checked).ElementAt(1).Name);
                                arrExcel[i, j + 3] = DirectlyHeterozigoticProbablyOne(
                                    locus.Allele.Where(n => n.Checked).ElementAt(0),
                                    locus.Allele.Where(n => n.Checked).ElementAt(1)
                                    ).ToString("E03", CultureInfo.InvariantCulture);
                            }
                            j = 0;
                            i++;
                        }
                        #endregion
                        break;
                    case KindStudy.OneParent:
                        #region OneParetn
                        foreach (Locus locus in locuses)
                        {
                            arrExcel[i, j] = locus.Name;
                            arrExcel[i, j + 1] = locus.Allele.Where(n => n.Checked).Select(n => String.Format("P{0} = {1}", n.Name, n.Frequency)).
                                Aggregate((curr, next) => curr + "; " + next);
                            if (locus.IsHomozygotic)
                            {
                                arrExcel[i, j + 2] = String.Format(ResourceStudy.formulaHomozigoticOneParent, locus.Allele.Where(n => n.Checked).First().Name);
                                arrExcel[i, j + 3] = OneParentHomozigoticProbablyOne(locus.Allele.Where(n => n.Checked).First()).ToString("E03", CultureInfo.InvariantCulture);
                            }
                            else
                            {
                                arrExcel[i, j + 2] = String.Format(ResourceStudy.formula2OneParentHeterozigotic,
                                    locus.Allele.Where(n => n.Checked).ElementAt(0).Name,
                                    locus.Allele.Where(n => n.Checked).ElementAt(1).Name);
                                arrExcel[i, j + 3] = OneParenHeterozigoticProbablyOne(
                                    locus.Allele.Where(n => n.Checked).ElementAt(0),
                                    locus.Allele.Where(n => n.Checked).ElementAt(1)
                                    ).ToString("E03", CultureInfo.InvariantCulture);
                            }
                            j = 0;
                            i++;
                        }
                        #endregion
                        break;
                    case KindStudy.TooParent:
                        foreach (Locus locus in locuses)
                        {
                            arrExcel[i, j] = locus.Name;
                            arrExcel[i, j + 1] = locus.Allele.Where(n => n.Checked).Select(n => String.Format("P{0} = {1}", n.Name, n.Frequency)).
                                Aggregate((curr, next) => curr + "; " + next);
                            if (locus.IsHomozygotic)
                            {
                                arrExcel[i, j + 2] = String.Format(ResourceStudy.formulaHomozigoticTooParent, locus.Allele.Where(n => n.Checked).First().Name);
                                arrExcel[i, j + 3] = TooParentHomozigoticProbablyOne(locus.Allele.Where(n => n.Checked).First()).ToString("E03", CultureInfo.InvariantCulture);
                            }
                            else
                            {
                                arrExcel[i, j + 2] = String.Format(ResourceStudy.formulaHeterozigoticTooParent,
                                    locus.Allele.Where(n => n.Checked).ElementAt(0).Name,
                                    locus.Allele.Where(n => n.Checked).ElementAt(1).Name);
                                arrExcel[i, j + 3] = TooParentHeterozigoticProbablyOne(
                                    locus.Allele.Where(n => n.Checked).ElementAt(0),
                                    locus.Allele.Where(n => n.Checked).ElementAt(1)
                                    ).ToString("E03", CultureInfo.InvariantCulture);
                            }
                            j = 0;
                            i++;
                        }
                        break;
                    default:
                        break;
                }

                workSheet.get_Range(
                    workSheet.Cells[startCell.Y + 1, startCell.X],
                    workSheet.Cells[startCell.Y + locuses.Count(), startCell.X + columnHeader.Count() - 1]
                    ).Value2 = arrExcel;  

                applicationExcel.Visible = true;
                applicationExcel.UserControl = true;

            }
            catch (Exception e)
            {
                applicationExcel.Quit();
                throw new WFExceptions.WFException(WFExceptions.ErrType.Message, e.Message, e);
            }
        }

        // Методы расчета
        public static double DirectlyHomozigoticProbablyOne(Allele allele)
        {
            return Math.Pow(allele.Frequency, 2);
        }
        public static double DirectlyHeterozigoticProbablyOne(Allele alleleOne, Allele alleleSecond)
        {
            return 2 * (alleleOne.Frequency * alleleSecond.Frequency); ;
        }
        public static double OneParentHomozigoticProbablyOne(Allele allele)
        {
            return allele.Frequency * (2 - allele.Frequency);
        }
        public static double OneParenHeterozigoticProbablyOne(Allele alleleFirst, Allele alleleSecond)
        {
            return alleleFirst.Frequency * (2 - alleleFirst.Frequency) + alleleSecond.Frequency *
                (2 - alleleSecond.Frequency) - 2 * alleleFirst.Frequency * alleleSecond.Frequency;
        }
        public static double TooParentHomozigoticProbablyOne(Allele allele)
        {
            return Math.Pow(allele.Frequency * (2 - allele.Frequency), 2);
        }
        public static double TooParentHeterozigoticProbablyOne(Allele alleleFirst, Allele alleleSecond)
        {
            return 2 * alleleFirst.Frequency * (2 - alleleFirst.Frequency) * alleleSecond.Frequency *
                (2 - alleleSecond.Frequency) - 2 * alleleFirst.Frequency * alleleFirst.Frequency *
                alleleSecond.Frequency * alleleSecond.Frequency;
        }
        public static double CollaborationIntoBlendIdentificationOne(Locus locus)
        {
            return Math.Pow((from Allele allele in locus.Allele where allele.Checked select allele.Frequency).
                Aggregate((curr, next) => curr + next), 2);
        }

        // Private methods
        private static void PrintHeader(string[] columnTitles, Excel.Worksheet workSheet, Point startCell)
        {
            for (int i = 0; i < columnTitles.Length; i++)
            {
                Excel.Range range = workSheet.get_Range(workSheet.Cells[startCell.Y, startCell.X+i], workSheet.Cells[startCell.Y, startCell.X+i]);
                range.Value2 = columnTitles[i];
                range.Font.Bold = true;
                range.Orientation = 90;
                range.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            }
        }
        private static Excel.Range nextRange(Excel.Worksheet workSheet, Point startCell)
        {
            return workSheet.get_Range(workSheet.Cells[startCell.Y, startCell.X], workSheet.Cells[startCell.Y, startCell.X]);        }

        // Private constructors
        private Study() { }
    }

    public enum KindStudy { Probably, OneParent, TooParent, Blend};
    class EqualityAlleleById : IEqualityComparer<Allele>
    {
        public bool Equals(Allele allele, Allele withAllele)
        {
            return allele.Id.Equals(withAllele.Id);
        }
        public int GetHashCode(Allele obj)
        {
            return obj.Id.GetHashCode();
        }
    }
    class EqualityLocusById : IEqualityComparer<Locus>
    {
        public bool Equals(Locus locus, Locus withLocus)
        {
            return locus.Id.Equals(withLocus.Id);
        }
        public int GetHashCode(Locus obj)
        {
            return obj.Id.GetHashCode();
        }
    }
    
}
