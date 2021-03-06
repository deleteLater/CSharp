﻿
/* Printing
 *
 * GtkPrintOperation offers a simple API to support printing in a cross-platform way.
 * from:
 *  https://github.com/mono/gtk-sharp/blob/master/sample/GtkDemo/DemoPrinting.cs
 * 
 * Make some modification...
 */

using System;
using System.IO;
using System.Reflection;
using Gtk;
using Cairo;

namespace GtkDemo
{
    public class MyPrinter
    {
        private static double headerHeight = (10 * 72 / 25.4);
        private static double headerGap = (3 * 72 / 25.4);
        private static int pangoScale = 1024;

        private PrintOperation print;

        private string fileName;
        private double fontSize = 12.0;
        private int linesPerPage;
        private string[] lines;
        private int numLines;
        private int numPages;

        public MyPrinter(string fileName)
        {
            print = new PrintOperation();
            this.fileName = fileName;
            print.BeginPrint += new BeginPrintHandler(OnBeginPrint);
            print.DrawPage += new DrawPageHandler(OnDrawPage);
            print.EndPrint += new EndPrintHandler(OnEndPrint);
        }

        public void Run()
        {
            print.Run(PrintOperationAction.PrintDialog,null);
        }

        private void OnBeginPrint(object obj, Gtk.BeginPrintArgs args)
        {
            string contents;
            double height;

            PrintContext context = args.Context;
            height = context.Height;

            linesPerPage = (int)Math.Floor(height / fontSize);
            contents = System.IO.File.ReadAllText(fileName);

            lines = contents.Split('\n');

            numLines = lines.Length;
            numPages = (numLines - 1) / linesPerPage + 1;

            print.NPages = numPages;
        }

        private void OnDrawPage(object obj, Gtk.DrawPageArgs args)
        {
            PrintContext context = args.Context;

            Cairo.Context cr = context.CairoContext;
            double width = context.Width;

            cr.Rectangle(0, 0, width, headerHeight);
            cr.SetSourceRGB(0.8, 0.8, 0.8);
            cr.FillPreserve();

            cr.SetSourceRGB(0, 0, 0);
            cr.LineWidth = 1;
            cr.Stroke();

            Pango.Layout layout = context.CreatePangoLayout();

            Pango.FontDescription desc = Pango.FontDescription.FromString("sans 14");
            layout.FontDescription = desc;

            layout.SetText(fileName);
            layout.Width = (int)width;
            layout.Alignment = Pango.Alignment.Center;

            int layoutWidth, layoutHeight;
            layout.GetSize(out layoutWidth, out layoutHeight);
            double textHeight = (double)layoutHeight / (double)pangoScale;

            cr.MoveTo(width / 2, (headerHeight - textHeight) / 2);
            Pango.CairoHelper.ShowLayout(cr, layout);

            string pageStr = String.Format("{0}/{1}", args.PageNr + 1, numPages);
            layout.SetText(pageStr);
            layout.Alignment = Pango.Alignment.Right;

            cr.MoveTo(width - 2, (headerHeight - textHeight) / 2);
            Pango.CairoHelper.ShowLayout(cr, layout);

            layout = null;
            layout = context.CreatePangoLayout();

            desc = Pango.FontDescription.FromString("mono");
            desc.Size = (int)(fontSize * pangoScale);
            layout.FontDescription = desc;

            cr.MoveTo(0, headerHeight + headerGap);
            int line = args.PageNr * linesPerPage;
            for (int i = 0; i < linesPerPage && line < numLines; i++)
            {
                layout.SetText(lines[line]);
                Pango.CairoHelper.ShowLayout(cr, layout);
                cr.RelMoveTo(0, fontSize);
                line++;
            }
            (cr as IDisposable).Dispose();
            layout = null;
        }

        private void OnEndPrint(object obj, Gtk.EndPrintArgs args)
        {
        }
    }
}
