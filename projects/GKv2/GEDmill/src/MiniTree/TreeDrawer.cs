/* 
 * Copyright 2009 Alexander Curtis <alex@logicmill.com>
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
 */

using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using GDModel;
using GKCore;
using GKCore.Logging;
using GKCore.Types;

namespace GEDmill.MiniTree
{
    /*
     * In this file, Parents are the top generation, siblings are the middle generation (including the subject of the tree),
     * and children are the bottom generation. Subject's spouses come in middle generation.
     * The data structure looks like this:
     *    ________   _____________________________________________________________________________________________________   ________
     *   | father | | siblings                                                                                            | | mother |
     *   |        |-|  _______   _______   ________   _________________   ______   _________________   _______   _______  |-|        |
     *   |________| | |sibling| |sibling| |spouse/ | |children         | |spouse| |children         | |subject| |sibling| | |________|
     *              | |       |-|       |-|subject |-|  _____   _____  |-|      |-|  _____   _____  |-|/spouse|-|       | |
     *              | |_______| |_______| |________| | |child| |child| | |______| | |child| |child| | |_______| |_______| |
     *              |                                | |_____|-|_____|--------------|_____|-|_____| |                     |
     *              |                                |_________________|          |_________________|                     |
     *              |_____________________________________________________________________________________________________|
     */

    /// <summary>
    /// Class that calculates and draws a mini tree diagram
    /// </summary>
    public class TreeDrawer
    {
        private static readonly ILogger fLogger = LogManager.GetLogger(GMConfig.LOG_FILE, GMConfig.LOG_LEVEL, typeof(TreeDrawer).Name);

        /// <summary>
        /// Data structure containing the information to put in the boxes in the tree
        /// </summary>
        private class CBoxText
        {
            // Individual's name
            public string Name;

            // Dates to put in the box
            public string Date;

            // Individual's first name
            public string FirstName;

            // Individual's surname
            public string Surname;

            // Whether the information is private
            public bool Concealed;


            public CBoxText(GDMIndividualRecord ir)
            {
                FirstName = "";
                Surname = "";
                Concealed = !GMHelper.GetVisibility(ir);
                if (Concealed && !GMConfig.Instance.UseWithheldNames) {
                    FirstName = "";
                    Surname = Name = GMConfig.Instance.ConcealedName;
                } else {
                    Name = GMHelper.CapitaliseName(ir.GetPrimaryPersonalName(), out FirstName, out Surname);
                }

                Date = Concealed ? string.Empty : GetLifeDatesStr(ir);
            }
        }

        // Total size of the tree
        private SizeF fSizeTotal;

        // Reference to the global gedcom data
        private readonly GDMTree fTree;


        // Returns the height of the whole tree diagram.
        public int Height
        {
            get { return (int)fSizeTotal.Height; }
        }


        public TreeDrawer(GDMTree tree)
        {
            fTree = tree;
        }

        // This is the main tree drawing method.
        // irSubject is the individual for whom the tree is based. 
        // nTargeWidth is the width below which the layout is free to use up space to produce a nice tree.
        public List<MiniTreeMap> CreateMiniTree(Paintbox paintbox, GDMIndividualRecord ir, string fileName, int targetWidth, ImageFormat imageFormat)
        {
            // First calculate size required for tree, by iterating through individuals and building a data structure
            MiniTreeGroup mtgParent = CreateDataStructure(ir);

            // For each individual calculate size of box required for display using helper function
            // There must be a better way to get a graphics:
            Bitmap bmp = new Bitmap(1, 1, PixelFormat.Format24bppRgb);
            Graphics g = Graphics.FromImage(bmp);
            Font f = paintbox.Font;

            // Record what font windows actually used, in case it chose a different one
            GMConfig.Instance.TreeFontName = f.Name;
            GMConfig.Instance.TreeFontSize = f.Size;

            // Recursively calculate sizes of other groups
            mtgParent.CalculateSize(g, f);

            g.Dispose();
            bmp.Dispose();

            // Now calculate sizes of each row
            // Total width includes irSubject, their spouses and their siblings.
            // Total height is always three generations

            // Now calculate how best to position each generation
            // Calculate the width of each generation
            // There are three cases : frParents widest, siblings widest, children widest
            // Plus two aims : minimise total width, get offspring centred under frParents.
            // If nTargetWidth is exceeded simply because of number of individuals in one row, that 
            // row's width becomes the new target width.
            // If nTargetWidth is exceeded otherwise, minimising total width becomes the priority
            mtgParent.CalculateLayout(0f, 0f);
            mtgParent.Compress();

            RectangleF rect = mtgParent.GetExtent();
            fSizeTotal = new SizeF(rect.Width, rect.Height);
            mtgParent.Translate(-rect.Left, -rect.Top);

            // Calculate offset for each row
            // Can't do this so create a new bitmap: bmp.Width = totalSize.Width;
            // Can't do this so create a new bitmap: bmp.Height = totalSize.Height;
            int nTotalWidth = (int)(fSizeTotal.Width + 1.0f);
            int nTotalHeight = (int)(fSizeTotal.Height + 1.0f);
            bmp = new Bitmap(nTotalWidth, nTotalHeight, PixelFormat.Format32bppArgb);
            g = Graphics.FromImage(bmp);

            // Do background fill
            if (GMConfig.Instance.FakeMiniTreeTransparency && paintbox.BrushFakeTransparency != null) {
                g.FillRectangle(paintbox.BrushFakeTransparency, 0, 0, nTotalWidth, nTotalHeight);
            } else if (imageFormat == ImageFormat.Gif && paintbox.BrushBgGif != null) {
                g.FillRectangle(paintbox.BrushBgGif, 0, 0, nTotalWidth, nTotalHeight);
            }

            List<MiniTreeMap> alMap = new List<MiniTreeMap>();
            mtgParent.DrawBitmap(paintbox, g, alMap);

            // Save the bitmap
            fLogger.WriteInfo("Saving mini tree as " + fileName);

            if (File.Exists(fileName)) {
                // Delete any current file
                File.SetAttributes(fileName, FileAttributes.Normal);
                File.Delete(fileName);
            }

            // Save using FileStream to try to avoid crash (only seen by customers)
            FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write);
            bmp.Save(fs, imageFormat);
            fs.Close();

            g.Dispose();
            bmp.Dispose();

            // For gifs we need to reload and set transparency colour
            if (imageFormat == ImageFormat.Gif && !GMConfig.Instance.FakeMiniTreeTransparency) {
                var imageGif = Image.FromFile(fileName);
                var colorPalette = imageGif.Palette;

                // Creates a new GIF image with a modified colour palette
                if (colorPalette != null) {
                    // Create a new 8 bit per pixel image
                    Bitmap bm = new Bitmap(imageGif.Width, imageGif.Height, PixelFormat.Format8bppIndexed);

                    // Get it's palette
                    ColorPalette colorpaletteNew = bm.Palette;

                    // Copy all the entries from the old palette removing any transparency
                    int n = 0;
                    foreach (Color c in colorPalette.Entries) {
                        colorpaletteNew.Entries[n++] = Color.FromArgb(255, c);
                    }

                    // Now to copy the actual bitmap data
                    // Lock the source and destination bits
                    BitmapData src = ((Bitmap)imageGif).LockBits(new Rectangle(0, 0, imageGif.Width, imageGif.Height), ImageLockMode.ReadOnly, imageGif.PixelFormat);
                    BitmapData dst = bm.LockBits(new Rectangle(0, 0, bm.Width, bm.Height), ImageLockMode.WriteOnly, bm.PixelFormat);

                    // Uses pointers so we need unsafe code.
                    // The project is also compiled with /unsafe
                    byte backColor = 0;
                    unsafe {
                        backColor = ((byte*)src.Scan0.ToPointer())[0]; // Assume transparent colour appears as first pixel.

                        byte* src_ptr = ((byte*)src.Scan0.ToPointer());
                        byte* dst_ptr = ((byte*)dst.Scan0.ToPointer());
                        // May be useful: System.Runtime.InteropServices.Marshal.Copy(IntPtr source, byte[], destination, int start, int length)
                        // May be useful: System.IO.MemoryStream ms = new System.IO.MemoryStream(src_ptr);
                        int width = imageGif.Width;
                        int src_stride = src.Stride - width;
                        int dst_stride = dst.Stride - width;
                        for (int y = 0; y < imageGif.Height; y++) {
                            // Can't convert IntPtr to byte[]: Buffer.BlockCopy( src_ptr, 0, dst_ptr, 0, width );
                            int x = width;
                            while (x-- > 0) {
                                *dst_ptr++ = *src_ptr++;
                            }
                            src_ptr += src_stride;
                            dst_ptr += dst_stride;
                        }
                    }

                    // Set the newly selected transparency
                    colorpaletteNew.Entries[backColor] = Color.FromArgb(0, Color.Magenta);

                    // Re-insert the palette
                    bm.Palette = colorpaletteNew;

                    // All done, unlock the bitmaps
                    ((Bitmap)imageGif).UnlockBits(src);
                    bm.UnlockBits(dst);

                    imageGif.Dispose();

                    // Set the new image in place
                    imageGif = bm;
                    colorPalette = imageGif.Palette;

                    fLogger.WriteInfo("Re-saving mini gif as " + fileName);

                    imageGif.Save(fileName, imageFormat);
                }
            }

            return alMap;
        }

        // Calculate size required for tree by iterating through individuals and building a data structure.
        protected MiniTreeGroup CreateDataStructure(GDMIndividualRecord irSubject)
        {
            // Add subject's frParents
            GDMFamilyRecord familyParents = fTree.GetParentsFamily(irSubject);
            GDMIndividualRecord husband, wife;
            fTree.GetSpouses(familyParents, out husband, out wife);

            MiniTreeGroup mtgParents = new MiniTreeGroup();
            MiniTreeIndividual mtiFather = null;
            if (familyParents != null) {
                mtiFather = AddToGroup(husband, mtgParents);
            }

            // Create a group for the subject and their siblings.
            var mtgSiblings = new MiniTreeGroup();

            // Keeps count of subject's siblings (including subject)
            int siblings = 0;

            // Keeps track of last added sibling, to hook up to next added sibling.
            MiniTreeIndividual mtiRightmostSibling = null;

            // Keeps track of last added child, to hook up to next added child.
            MiniTreeIndividual mtiRightmostChild = null;

            // For each sibling (including the subject)
            while (true)
            {
                GDMIndividualRecord irSibling = GetChild(familyParents, siblings, irSubject);
                if (irSibling == null) {
                    break;
                }

                if (irSibling == irSubject) {
                    // Add spouses and children of subject, (and subject too, if we need to put wife after them.)
                    MiniTreeGroup mtgOffspring = null;
                    bool addedSubject = false;
                    int nSpouses = 0;
                    var ecbCrossbar = MiniTreeGroup.ECrossbar.Solid;

                    foreach (var link in irSubject.SpouseToFamilyLinks) {
                        GDMFamilyRecord famRec = fTree.GetPtrValue(link);
                        GDMIndividualRecord irSpouse = fTree.GetSpouseBy(famRec, irSubject);

                        if (famRec.Husband.XRef != irSubject.XRef) {
                            mtiRightmostSibling = AddToGroup(irSpouse, mtgSiblings);
                            // Subject is female so all but last husband have dotted bars
                            ecbCrossbar = MiniTreeGroup.ECrossbar.DottedLeft;
                        } else if (Exists(irSubject) && !addedSubject) {
                            // Subject is male, so need to put them in now, before their children.
                            // (Otherwise they get added as a regular sibling later)
                            var boxtext = new CBoxText(irSubject);
                            mtiRightmostSibling = mtgSiblings.AddIndividual(irSubject, boxtext.FirstName, boxtext.Surname, boxtext.Date, false, familyParents != null, true, boxtext.Concealed, false);

                            // To stop subject being added as regular sibling.
                            addedSubject = true;
                        }

                        int grandChildren = 0;
                        GDMIndividualRecord irGrandChild = null;

                        // If we have already added an offspring box (from previous marriage) need connect this box to it as its right box.
                        if (mtgOffspring != null) {
                            mtgOffspring.RightBox = mtiRightmostSibling;
                        }

                        // Create a box for the offspring of this marriage
                        mtgOffspring = new MiniTreeGroup();

                        // Set crossbar that joins subject to spouse according to whether this is subject's first spouse.
                        mtgOffspring.fCrossbar = ecbCrossbar;

                        // Add children by this spouse                   
                        MiniTreeIndividual mtiChild = null;
                        while ((irGrandChild = GetChild(famRec, grandChildren, null)) != null) {
                            if (Exists(irGrandChild)) {
                                var boxtext = new CBoxText(irGrandChild);
                                mtiChild = mtgOffspring.AddIndividual(irGrandChild, boxtext.FirstName, boxtext.Surname, boxtext.Date, true, true, false, boxtext.Concealed, false);

                                // Hook this up to any children by previous spouses.
                                if (grandChildren == 0 && mtiRightmostChild != null) {
                                    mtiRightmostChild.RightObjectAlien = mtiChild;
                                    mtiChild.LeftObjectAlien = mtiRightmostChild;
                                }
                            }
                            grandChildren++;
                        }

                        // If we added anything, record it as the right-most child ready to hook to children by next spouse.
                        if (mtiChild != null) {
                            mtiRightmostChild = mtiChild;
                        }

                        // Add the subjects children to the siblings group
                        mtgSiblings.AddGroup(mtgOffspring);

                        // Hook the offspring group to the previous sibling
                        if (mtgOffspring != null) {
                            mtgOffspring.LeftBox = mtiRightmostSibling;
                        }

                        // If subject is husband then we need to add their wife now.
                        if (famRec.Husband.XRef == irSubject.XRef) {
                            ecbCrossbar = MiniTreeGroup.ECrossbar.DottedRight;

                            // Hook up to previous rightmost sibling and set this as new rightmost sibling.
                            mtiRightmostSibling = AddToGroup(irSpouse, mtgSiblings);

                            // Hook the wife up as box on right of offspring box.
                            if (mtgOffspring != null) {
                                mtgOffspring.RightBox = mtiRightmostSibling;
                            }
                        }

                        nSpouses++;
                    }

                    if (!addedSubject) {
                        var boxtext = new CBoxText(irSubject);
                        MiniTreeIndividual mtiWife = mtgSiblings.AddIndividual(irSubject, boxtext.FirstName, boxtext.Surname, boxtext.Date, false, familyParents != null, true, boxtext.Concealed, false);

                        if (mtgOffspring != null) {
                            mtgOffspring.fCrossbar = MiniTreeGroup.ECrossbar.Solid;
                            mtgOffspring.RightBox = mtiWife;
                        }
                    }
                } else if (Exists(irSibling)) {
                    // A sibling (not the subject).
                    var boxtext = new CBoxText(irSibling);
                    mtgSiblings.AddIndividual(irSibling, boxtext.FirstName, boxtext.Surname, boxtext.Date, true, familyParents != null, true, boxtext.Concealed, false);
                }

                siblings++;
            }

            // Add siblings group after subject's father
            mtgParents.AddGroup(mtgSiblings);

            // Hook up to subject's father
            mtgSiblings.LeftBox = mtiFather;

            // Add subject's mother
            if (familyParents != null) {
                MiniTreeIndividual mtiMother = AddToGroup(wife, mtgParents);
                mtgSiblings.RightBox = mtiMother;
            }

            // Return the parents group (which contains the other family groups).
            return mtgParents;
        }

        // Gets the n'th child in the fr, or returns the default individual if first child requested and no fr.
        private GDMIndividualRecord GetChild(GDMFamilyRecord famRec, int childIndex, GDMIndividualRecord irDefault)
        {
            GDMIndividualRecord irChild = null;
            if (famRec != null && childIndex < famRec.Children.Count) {
                // The ordering of children in the tree can be selected to be the same as it is in the GEDCOM file. This 
                // is because the file should be ordered as the user chose to order the fr when entering the data in 
                // their fr history app, regardless of actual birth dates. 
                if (GMConfig.Instance.KeepSiblingOrder) {
                    irChild = fTree.GetPtrValue(famRec.Children[childIndex]);
                } else {
                    irChild = fTree.GetPtrValue(famRec.Children[childIndex]);
                }
            } else {
                // Return the default individual as first and only child of fr.
                if (childIndex == 0) {
                    irChild = irDefault;
                }
            }
            return irChild;
        }

        // Add a box for the individual to the specified group.
        private static MiniTreeIndividual AddToGroup(GDMIndividualRecord ir, MiniTreeGroup mtg)
        {
            MiniTreeIndividual mti;
            if (Exists(ir)) {
                CBoxText boxtext = new CBoxText(ir);
                mti = mtg.AddIndividual(ir, boxtext.FirstName, boxtext.Surname, boxtext.Date, true, false, false, boxtext.Concealed, true);
            } else {
                mti = mtg.AddIndividual(null, "", GMConfig.Instance.UnknownName, " ", false, false, false, false, true);
            }
            return mti;
        }

        // Returns true if the supplied record is valid for inclusion in the tree
        private static bool Exists(GDMIndividualRecord ir)
        {
            return GMHelper.GetVisibility(ir);
        }

        private static string GetLifeDatesStr(GDMIndividualRecord record)
        {
            var lifeDates = record.GetLifeDates();
            string birthDate = GKUtils.GEDCOMEventToDateStr(lifeDates.BirthEvent, DateFormat.dfYYYY, false);
            string deathDate = GKUtils.GEDCOMEventToDateStr(lifeDates.DeathEvent, DateFormat.dfYYYY, false);
            return birthDate + " - " + deathDate;
        }
    }
}
