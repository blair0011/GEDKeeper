﻿/* Extensions.cs
 * 
 * Copyright 2019 Serg V. Zhdanovskih
 * This file is part of GEDmill - A family history website creator
 * 
 * GEDmill is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * GEDmill is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with GEDmill.  If not, see <http://www.gnu.org/licenses/>.
 *
 *
 * History:  
 * 29Mar19 ZSV      Created for conversion to GKCore
 *
 */

using System;
using System.Collections.Generic;
using GEDmill.Model;
using GKCommon.GEDCOM;
using GKCore;
using GKCore.Tools;
using GKCore.Types;

namespace GEDmill
{
    public static class Extensions
    {
        private static readonly Dictionary<string, CISRecordChanges> recX = new Dictionary<string, CISRecordChanges>();

        public static void SetVisibility(this GEDCOMRecord record, bool value)
        {
            if (record == null) return;

            CISRecordChanges changes;
            if (!recX.TryGetValue(record.XRef, out changes)) {
                changes = new CISRecordChanges(true);
                recX.Add(record.XRef, changes);
            }

            changes.Visibility = value;
        }

        public static bool GetVisibility(this GEDCOMRecord record)
        {
            if (record == null) return true;

            CISRecordChanges changes;
            if (!recX.TryGetValue(record.XRef, out changes)) {
                return true;
            }

            return changes.Visibility;
        }

        public static Dictionary<object, GEDCOMIndividualRecord> MakeBackReferences(this GEDCOMRecord record)
        {
            var result = new Dictionary<object, GEDCOMIndividualRecord>();
            return result;
        }

        // Returns the n'th name and associated sources
        public static NameAndSource GetNameAndSource(this GEDCOMIndividualRecord record, int n)
        {
            if (record.PersonalNames.Count <= n || n < 0) {
                return null;
            }
            GEDCOMPersonalName pns = record.PersonalNames[n];
            NameAndSource nas = new NameAndSource(pns.StringValue);
            nas.Sources.AddRange(pns.Pieces.SourceCitations);
            return nas;
        }

        public static List<GEDCOMFamilyRecord> GetFamilyList(this GEDCOMIndividualRecord record)
        {
            var result = new List<GEDCOMFamilyRecord>();

            foreach (var link in record.SpouseToFamilyLinks) {
                var family = link.Value as GEDCOMFamilyRecord;
                if (family != null) {
                    result.Add(family);
                }
            }

            return result;
        }

        public static List<BackReference> GetBackReferences(this GEDCOMSourceRecord record)
        {
            var result = new List<BackReference>();
            return result;
        }

        public static string GetLifeDatesStr(this GEDCOMIndividualRecord record)
        {
            var lifeDates = record.GetLifeDates();
            //TODO
            //TreeChartOptions options = GlobalOptions.Instance.ChartOptions;
            //DateFormat dateFormat = (options.OnlyYears) ? DateFormat.dfYYYY : DateFormat.dfDD_MM_YYYY;
            DateFormat dateFormat = DateFormat.dfYYYY;

            string birthDate = GKUtils.GEDCOMEventToDateStr(lifeDates.BirthEvent, dateFormat, false);
            string deathDate = GKUtils.GEDCOMEventToDateStr(lifeDates.DeathEvent, dateFormat, false);
            return birthDate + " - " + deathDate;
        }

        public static bool IsPictureFormat(this GEDCOMFileReferenceWithTitle fileRef)
        {
            MultimediaKind mmKind = GKUtils.GetMultimediaKind(fileRef.MultimediaFormat);
            return (mmKind == MultimediaKind.mkImage);
        }

        private static bool PruneProc(GEDCOMIndividualRecord iRec, TreeTools.TreeWalkMode mode, object extData)
        {
            bool visible = (bool)extData;
            iRec.SetVisibility(visible);
            return true;
        }

        public static void PruneAncestors(this GEDCOMTree tree, GEDCOMIndividualRecord iRec, bool visible)
        {
            TreeTools.WalkTree(iRec, TreeTools.TreeWalkMode.twmAncestors, PruneProc, ((object)visible));
        }

        public static void PruneDescendants(this GEDCOMTree tree, GEDCOMIndividualRecord iRec, bool visible)
        {
            TreeTools.WalkTree(iRec, TreeTools.TreeWalkMode.twmDescendants, PruneProc, ((object)visible));
        }

        private static bool PruneMarkProc(GEDCOMIndividualRecord iRec, TreeTools.TreeWalkMode mode, object extData)
        {
            var marks = (List<GEDCOMRecord>)extData;
            marks.Add(iRec);
            return true;
        }

        public static void PruneMarkConnected(this GEDCOMTree tree, GEDCOMIndividualRecord iRec, List<GEDCOMRecord> marks)
        {
            TreeTools.WalkTree(iRec, TreeTools.TreeWalkMode.twmAll, PruneMarkProc, marks);
        }

        public static void PruneUnmarked(this GEDCOMTree tree, List<GEDCOMRecord> marks)
        {
            var treeEnum = tree.GetEnumerator(GEDCOMRecordType.rtIndividual);
            GEDCOMRecord record;
            while (treeEnum.MoveNext(out record)) {
                if (marks.IndexOf(record) < 0) {
                    record.SetVisibility(false);
                }
            }
        }

        public static string MakeLinkNumber(this GEDCOMSourceCitation sourCit, uint uSourceCount, bool bComma)
        {
            string sComma = bComma ? "," : "";
            return string.Concat("<span class=\"reference\">", sComma, uSourceCount.ToString(), "</span>");
        }

        // Returns a string to use in the list of references at the bottom of the page
        public static string MakeLinkText(this GEDCOMSourceCitation sourCit, uint uSourceCount)
        {
            var sourRec = sourCit.Value as GEDCOMSourceRecord;
            return string.Concat(uSourceCount.ToString(), ". ", /*m_sSourceDescription*/sourRec.ShortTitle);
        }

        public static void RestrictAssociatedSources(this GEDCOMTree tree, GEDCOMIndividualRecord iRec)
        {
            // Restrict sources connected with individual directly
            foreach (GEDCOMSourceCitation sc in iRec.SourceCitations) {
                RestrictSource(sc, true);
            }

            // Restrict sources connected with name
            foreach (GEDCOMPersonalName pns in iRec.PersonalNames) {
                foreach (GEDCOMSourceCitation sc in pns.Pieces.SourceCitations) {
                    RestrictSource(sc, true);
                }
            }

            // Restrict sources connected with events
            foreach (GEDCOMCustomEvent ies in iRec.Events) {
                foreach (GEDCOMSourceCitation sc in ies.SourceCitations) {
                    RestrictSource(sc, true);
                }
            }

            // Restrict sources connected with m_associationStructures
            foreach (GEDCOMAssociation ass in iRec.Associations) {
                foreach (GEDCOMSourceCitation sc in ass.SourceCitations) {
                    RestrictSource(sc, true);
                }
            }
        }

        // Marks the given source citation as (un)restricted
        public static void RestrictSource(GEDCOMSourceCitation sc, bool visible)
        {
            if (sc != null) {
                GEDCOMSourceRecord sr = sc.Value as GEDCOMSourceRecord;
                if (sr != null) {
                    sr.SetVisibility(visible);
                }
            }
        }

        public static int SetAllMFRsVisible(this GEDCOMRecord record, bool visible)
        {
            int nChanged = 0;
            foreach (GEDCOMMultimediaLink mfr in record.MultimediaLinks) {
                var mmRec = mfr.Value as GEDCOMMultimediaRecord;
                if (mmRec != null && mmRec.GetVisibility() != visible) {
                    mmRec.SetVisibility(visible);
                    nChanged++;
                }
            }
            return nChanged;
        }

        public static int CountVisibleMFRs(this GEDCOMSourceRecord sourRec)
        {
            return 0;
        }

        public static int CountAllMFRs(this GEDCOMSourceRecord sourRec)
        {
            return 0;
        }

        public static int CountVisibleMFRs(this GEDCOMIndividualRecord iRec)
        {
            return 0;
        }

        public static int CountAllMFRs(this GEDCOMIndividualRecord iRec)
        {
            return 0;
        }

        public static string GetName(this GEDCOMIndividualRecord iRec, int i)
        {
            return (i >= 0 && i < iRec.PersonalNames.Count) ? iRec.PersonalNames[i].StringValue : "";
        }

        public static int GetChronologicalYear(GEDCOMDateValue dateVal)
        {
            return (dateVal == null) ? 0 : dateVal.GetChronologicalYear();
        }

        public static int GetEventsYearsDiff(GEDCOMDateValue ev1, GEDCOMDateValue ev2, bool currentEnd = true)
        {
            int result = -1;

            try {
                int dt1 = GetChronologicalYear(ev1);
                int dt2 = GetChronologicalYear(ev2);

                if (currentEnd && dt2 == 0) {
                    dt2 = DateTime.Now.Year;
                }

                if (dt1 != 0 && dt2 != 0) {
                    result = Math.Abs(dt2 - dt1);
                }
            } catch (Exception ex) {
                Logger.LogWrite("GKUtils.GetEventsYearsDiff(): " + ex.Message);
            }

            return result;
        }
    }
}
