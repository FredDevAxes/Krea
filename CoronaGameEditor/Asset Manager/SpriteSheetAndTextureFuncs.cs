#region License

/**
     * Copyright (C) 2010 Rafael Vasco (rafaelvasco87@gmail.com)
     * 
     *
     * This program is free software; you can redistribute it and/or
     * modify it under the terms of the GNU General Public License
     * as published by the Free Software Foundation; either version 2
     * of the License, or (at your option) any later version.

     * This program is distributed in the hope that it will be useful,
     * but WITHOUT ANY WARRANTY; without even the implied warranty of
     * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
     * GNU General Public License for more details.

     * You should have received a copy of the GNU General Public License
     * along with this program; if not, write to the Free Software
     * Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.
     */

#endregion


using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace Krea.Asset_Manager
{
    public static class SpriteSheetAndTextureFuncs
    {

        public static List<Rectangle> CutByAlpha(Bitmap tex)
        {
           return CutByAlpha(tex, 0, 0, 0.12, 5);
        }

        public static List<Rectangle> CutByAlpha(Bitmap tex, double transparencyLimit, int minimumAcceptedSize)
        {
           return CutByAlpha(tex, 0, 0, transparencyLimit, minimumAcceptedSize);
        }

        public static List<Rectangle> CutByAlpha(Bitmap tex, int scanStartX, int scanStartY, double transparencyLimit,int minimumAcceptedSize)
        {
            var frameRects = FindFrameRects(tex, scanStartX, scanStartY, transparencyLimit);

            ProcessFrameRects(frameRects,minimumAcceptedSize);
            return frameRects;
        }

      

        
        /// <summary>
        /// Returns a list of sprite rectangles cut from an image by alpha channel.
        /// Modified from ClanLib SDK (http://clanlib.org)
        /// </summary>
        /// <param name="tex"></param>
        /// <param name="scanStartX"></param>
        /// <param name="scanStartY"></param>
        /// <param name="transparencyLimit"></param>
        /// <returns></returns>
        public static List<Rectangle> FindFrameRects(Bitmap tex,int scanStartX, int scanStartY, double transparencyLimit)
        {
            int width = (int)tex.Size.Width;
            int height = (int)tex.Size.Height;

            List<Rectangle> frameRects = new List<Rectangle>();

            bool[] explored = new bool[width * height];

            Color[] texData = new Color[width * height];


            for (int j = scanStartY; j < tex.Height; j++)
            {
                for (int i = scanStartX; i < tex.Width; i++)
                {
                    int index = j * tex.Width + i;
                    texData[index] = tex.GetPixel(i, j);
                }
            }

            for (int y = scanStartY; y < height; y++)
            {
                for (int x = scanStartX; x < width; x++)
                {
                    int texIndex = y * width + x;

                    if (explored[texIndex]) continue;

                    explored[y * width + x] = true;
                    if ((texData[texIndex].A) <= transparencyLimit * 255) continue;

                    int x2;
                    int x1 = x2 = x;
                    int y2;
                    int y1 = y2 = y;

                    bool more = true;

                    while (more)
                    {
                        more = false;

                        for (int i = x1; i <= x2; i++)
                        {
                            if (y2 + 1 < height)
                            {
                                explored[(y2 + 1) * width] = true;
                                if ((texData[(y2 + 1) * width + i].A) > transparencyLimit * 255)
                                {
                                    more = true;
                                    y2 += 1;
                                }
                            }
                        }

                        for (int j = y1; j <= y2; j++)
                        {
                            if (x2 + 1 < width)
                            {
                                explored[j * width + x2 + 1] = true;
                                if ((texData[j * width + x2 + 1].A) > transparencyLimit * 255)
                                {
                                    more = true;
                                    x2 += 1;
                                }
                            }

                            if (x1 - 1 >= 0)
                            {
                                explored[j * width + x1 - 1] = true;
                                if ((texData[j * width + x1 - 1].A) > transparencyLimit * 255)
                                {
                                    more = true;
                                    x1 -= 1;
                                }
                            }
                        }
                    }

                    for (int i = x1; i <= x2; i++)
                    {
                        for (int j = y1; j <= y2; j++)
                        {
                            explored[j * width + i] = true;
                        }
                    }

                    frameRects.Add(new Rectangle(x1, y1, x2 + 1 - x1, y2 + 1 - y1));
                }
            }

            return frameRects;
        }

        private static void ProcessFrameRects(List<Rectangle> frameRects, int minimumAcceptedSize)
        {
           

            var toRemove = new List<Rectangle>();

            for (var i = 0; i < frameRects.Count; i++)
            {
                if (frameRects[i].Width <= minimumAcceptedSize && frameRects[i].Height <= minimumAcceptedSize)
                {
                    toRemove.Add(frameRects[i]);
                }
            }

            foreach (var frame in toRemove)
            {
                frameRects.Remove(frame);
            }

            SortFramesLeftToRightTopToDown(frameRects);
            

        }

        public static void SortFramesLeftToRightTopToDown(List<Rectangle> frameRects)
        {

          


            SortFramesVertically(frameRects);


            var frameLines = new List<List<Rectangle>>();

            var lastMarginIndex = 0;


            if (frameRects.Count > 0)
            {
                for (var i = 0; i < frameRects.Count - 1; i++)
                {
                    if (frameRects[i + 1].Top <= frameRects[i].Top + frameRects[i].Height) continue;

                    frameLines.Add(frameRects.GetRange(lastMarginIndex, (i - lastMarginIndex) + 1));
                    lastMarginIndex = i + 1;
                }

                frameLines.Add(frameRects.GetRange(lastMarginIndex, frameRects.Count - lastMarginIndex));

                foreach (var frameLine in frameLines)
                {
                    SortFramesHorizontally(frameLine);
                }

                frameRects.Clear();

                foreach (var frameLine in frameLines)
                {
                    foreach (var frame in frameLine)
                    {
                        frameRects.Add(frame);
                    }
                }
            }

        }

        private static void SortFramesHorizontally(IList<Rectangle> frames)
        {
            SortFramesHorizontally(frames, 0, frames.Count - 1);
        }

        private static void SortFramesHorizontally(IList<Rectangle> frames, int start, int end)
        {
            var i = start;
            var j = end;
            var x = frames[(int)((start + end) * 0.5f)];

            do
            {
                while ((frames[i].Left < x.Left) && (i < end)) i++;
                while ((x.Left < frames[j].Left) && (j > start)) j--;

                if (i > j) continue;

                var y = frames[i];
                frames[i] = frames[j];
                frames[j] = y;
                i++;
                j--;
            } while (i <= j);

            if (start < j) SortFramesHorizontally(frames, start, j);
            if (i < end) SortFramesHorizontally(frames, i, end);
        }

        private static void SortFramesVertically(IList<Rectangle> frames)
        {
            SortFramesVertically(frames,0, frames.Count - 1);
        }

        private static void SortFramesVertically(IList<Rectangle> frames, int start, int end)
        {
            var i = start;
            var j = end;
            var x = frames[(int)((start + end) * 0.5f)];

            do
            {
                while ((frames[i].Top < x.Top) && (i < end)) i++;
                while ((x.Top < frames[j].Top) && (j > start)) j--;

                if (i > j) continue;

                var y = frames[i];
                frames[i] = frames[j];
                frames[j] = y;
                i++;
                j--;
            } while (i <= j);

            if (start < j) SortFramesVertically(frames, start, j);
            if (i < end) SortFramesVertically(frames, i, end);
        }
        

        //-------------------------------------------------------------------------------------------------------------------------------------


        public static Rectangle ShrinkFrameSelect(Bitmap tex, Rectangle selectionRect, double alphaTolerance)
        {



            Color[] selectionData = new Color[selectionRect.Width * selectionRect.Height];

            for (int i = 0; i < tex.Width; i++)
            {
                for (int j = 0; j < tex.Height; j++)
                {
                    selectionData[i * tex.Height + j] = tex.GetPixel(i, j);
                }
            }
        

            int x1 = 0, y1 = 0, x2 = 0, y2 = 0;

            int first = 0;

            bool found = false;

            for (int i = 0; i < selectionData.Length; i++)
            {
                if (selectionData[i].A >= alphaTolerance*255)
                {
                    found = true;
                    x1 = i % selectionRect.Width;
                    y1 = i / selectionRect.Width;
                    x2 = x1;
                    y2 = y1;

                    first = i;
                    break;
                }
            }
            if (found)
            {
                for (int i = first; i < selectionData.Length; i++)
                {
                    if (selectionData[i].A >= alphaTolerance*255)
                    {
                        int x = i % selectionRect.Width;
                        int y = i / selectionRect.Width;

                        if(x < x1) x1 = x;
                        if(y < y1) y1 = y;
                        if(x > x2) x2 = x;
                        if(y > y2) y2 = y;

                    }
                }
                x1 += selectionRect.X;
                y1 += selectionRect.Y;

                x2 += selectionRect.X + 1;
                y2 += selectionRect.Y + 1;

                
                return new Rectangle(x1,y1,x2,y2);
            }

            return Rectangle.Empty;
        }

        //public static Bitmap TrimmByAlpha(Bitmap tex)
        //{
        //    Rectangle dest = new Rectangle(0, 0, tex.Width, tex.Height);

        //    Rectangle optimizedRect = ShrinkFrameSelect(tex, dest, 0.12);

        //    Color[] optimizedSpTexData = new Color[optimizedRect.Width * optimizedRect.Height];

        //    for (int i = 0; i < tex.Width; i++)
        //    {
        //        for (int j = 0; j < tex.Height; j++)
        //        {
        //            optimizedSpTexData[i * tex.Height + j] = tex.GetPixel(i, j);
        //        }
        //    }
        //    tex.Surface.GetData(optimizedRect, optimizedSpTexData,PixelFormat.Alpha);

        //    var finalTex = new Bitmap(optimizedRect.Width, optimizedRect.Height, PixelFormat.Alpha);

        //    finalTex.Surface.SetData(optimizedSpTexData, PixelFormat.Alpha);
            
            
        //    tex.Dispose();

        //    return finalTex;
        //}
        

        
    }
}
