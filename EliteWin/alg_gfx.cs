/**
 *
 * Elite - The New Kind.
 *
 * Allegro version of Graphics routines.
 *
 * The code in this file has not been derived from the original Elite code.
 * Written by C.J.Pinder 1999-2001.
 * email: <christian@newkind.co.uk>
 *
 * Routines for drawing anti-aliased lines and circles by T.Harte.
 *
 **/

//# include <string.h>
//# include <stdlib.h>
//# include <stdio.h>
//# include <math.h>
//# include <ctype.h>

//# include "allegro.h"

//# include "config.h"
//# include "gfx.h"
//# include "alg_data.h"
//# include "elite.h"

namespace Elite
{
	using System.Diagnostics;
	using System.Drawing;
	using System.Numerics;
	using Elite.Enums;

	public class alg_gfx : IGfx
	{
        /* Allegro datafile object indexes, produced by grabber v3.9.32 (WIP), Mingw32 */
        /* Datafile: c:\usr\cprogs\NewKind\elite.dat */
        /* Date: Mon May 07 19:20:00 2001 */
        /* Do not hand edit! */
        const int BLAKE = 0;        /* BMP  */
		const int DANUBE = 1;        /* MIDI */
		const int ECM = 2;        /* BMP  */
		const int ELITE_1 = 3;        /* FONT */
		const int ELITE_2 = 4;        /* FONT */
		const int ELITETXT = 5;        /* BMP  */
		const int FRONTV = 6;        /* BMP  */
		const int GRNDOT = 7;        /* BMP  */
		const int MISSILE_G = 8;        /* BMP  */
		const int MISSILE_R = 9;        /* BMP  */
		const int MISSILE_Y = 10;       /* BMP  */
		const int REDDOT = 11;       /* BMP  */
		const int SAFE = 12;       /* BMP  */
		const int THEME = 13;       /* MIDI */

		// Screen buffer
		Bitmap _gfx_screen;
		Graphics _gfx_screen_graphics;

		// Actual screen
		Bitmap _screen;
        Graphics _screen_graphics;

        volatile static int frame_count;
		//DATAFILE* datafile;
		Bitmap scanner_image;

		const int MAX_POLYS = 100;

		static int start_poly;
		static int total_polys;

		public alg_gfx(ref Bitmap screen)
		{
			_screen = screen;
            _screen_graphics = Graphics.FromImage(_screen);
            _gfx_screen = new Bitmap(screen.Width, screen.Height);
            _gfx_screen_graphics = Graphics.FromImage(_gfx_screen);
        }

        struct poly_data
		{
			internal int z;
            internal GFX_COL face_colour;
			internal Point[] point_list;
            internal int next;
		};

		static poly_data[] poly_chain = new poly_data[MAX_POLYS];

		static void frame_timer()
		{
			frame_count++;
		}
		//END_OF_FUNCTION(frame_timer);

		public int GraphicsStartup()
		{
			Debug.WriteLine(nameof(GraphicsStartup));

			//			PALETTE the_palette;
			//			int rv;

			//#if ALLEGRO_WINDOWS

			//#if RES_512_512
			//			rv = set_gfx_mode(GFX_DIRECTX_OVL, 512, 512, 0, 0);

			//			if (rv != 0)
			//				rv = set_gfx_mode(GFX_DIRECTX_WIN, 512, 512, 0, 0);

			//			if (rv != 0)
			//				rv = set_gfx_mode(GFX_GDI, 512, 512, 0, 0);

			//			if (rv == 0)
			//				set_display_switch_mode(SWITCH_BACKGROUND);
			//#else
			//			rv = set_gfx_mode(GFX_DIRECTX, 800, 600, 0, 0);

			//			if (rv != 0)
			//				rv = set_gfx_mode(GFX_GDI, 800, 600, 0, 0);
			//#endif

			//#else
			//			rv = set_gfx_mode(GFX_AUTODETECT, 800, 600, 0, 0);
			//#endif

			//			if (rv != 0)
			//			{
			//				set_gfx_mode(GFX_TEXT, 0, 0, 0, 0);
			//				allegro_message("Unable to set graphics mode.");
			//				return 1;
			//			}

			//			datafile = load_datafile("elite.dat");
			//			if (!datafile)
			//			{
			//				set_gfx_mode(GFX_TEXT, 0, 0, 0, 0);
			//				allegro_message("Error loading %s!\n", "elite.dat");
			//				return 1;
			//			}

			// TODO: load image from resource
			scanner_image = (Bitmap)Image.FromFile(Path.Combine("gfx", "scanner.bmp"));

            if (scanner_image == null)
			{
				//set_gfx_mode(GFX_TEXT, 0, 0, 0, 0);
				//allegro_message("Error reading scanner bitmap file.\n");
				return 1;
			}

            //			/* select the scanner palette */
            //			set_palette(the_palette);

            //			/* Create the screen buffer bitmap */
            //			gfx_screen = create_bitmap(SCREEN_W, SCREEN_H);

            //			clear(gfx_screen);

			DrawScanner();

			// Draw border
            DrawLine(0, 0, 0, 384);
			DrawLine(0, 0, 511, 0);
			DrawLine(511, 0, 511, 384);

			//			/* Install a timer to regulate the speed of the game... */

			//			LOCK_VARIABLE(frame_count);
			//			LOCK_FUNCTION(frame_timer);
			//			frame_count = 0;
			//			install_int(frame_timer, speed_cap);

			return 0;
		}

		public void GraphicsShutdown()
		{
            Debug.WriteLine(nameof(GraphicsShutdown));

            //destroy_bitmap(scanner_image);
            //destroy_bitmap(gfx_screen);
            //unload_datafile(datafile);
        }

		/*
		 * Blit the back buffer to the screen.
		 */
		public void ScreenUpdate()
		{
            //Debug.WriteLine(nameof(ScreenUpdate));

            //while (frame_count < 1)
            //{
            //	Thread.Sleep(10);
            //	// rest(10);
            //}

            //frame_count = 0;

            //acquire_screen();
            //blit(gfx_screen, screen, gfx.GFX_X_OFFSET, gfx.GFX_Y_OFFSET, gfx.GFX_X_OFFSET, gfx.GFX_Y_OFFSET, 512, 512);
            //release_screen();

            lock (_screen)
			{
				_screen_graphics.DrawImage(_gfx_screen, gfx.GFX_X_OFFSET, gfx.GFX_Y_OFFSET);
			}

            Application.DoEvents();
        }

        public void ScreenAcquire()
		{
            Debug.WriteLine(nameof(ScreenAcquire));

            //acquire_bitmap(gfx_screen);
        }

        public void ScreenRelease()
		{
            Debug.WriteLine(nameof(ScreenRelease));

            //release_bitmap(gfx_screen);
		}

		public void PlotPixelFast(int x, int y, GFX_COL col)
		{
            Debug.WriteLine(nameof(PlotPixelFast));

            ////	_putpixel(gfx_screen, x, y, col);
            //gfx_screen.line[y][x] = col;
		}

        public void PlotPixel(int x, int y, GFX_COL col)
		{
            Debug.WriteLine(nameof(PlotPixel));

            //putpixel(gfx_screen, x + gfx.GFX_X_OFFSET, y + gfx.GFX_Y_OFFSET, col);
        }

        public void DrawCircleFilled(int cx, int cy, int radius, GFX_COL circle_colour)
		{
            Debug.WriteLine(nameof(DrawCircleFilled));

            //circlefill(gfx_screen, cx + gfx.GFX_X_OFFSET, cy + gfx.GFX_Y_OFFSET, radius, circle_colour);
        }


		const int AA_BITS = 3;
		const int AA_AND = 7;
		const int AA_BASE = 235;

//#define trunc(x) ((x) & ~65535)
//#define frac(x) ((x) & 65535)
//#define invfrac(x) (65535-frac(x))
//#define plot(x,y,c) putpixel(gfx_screen, (x), (y), (c)+AA_BASE)

  //      /*
		// * Draw anti-aliased wireframe circle.
		// * By T.Harte.
		// */
  //      static void gfx_draw_aa_circle(int cx, int cy, int radius)
		//{
		//	int x, y;
		//	int s;
		//	int sx, sy;

		//	cx += gfx.GFX_X_OFFSET;
		//	cy += gfx.GFX_Y_OFFSET;

		//	radius >>= (16 - AA_BITS);

		//	x = radius;
		//	s = -radius;
		//	y = 0;

		//	while (y <= x)
		//	{
		//		//wide pixels
		//		sx = cx + (x >> AA_BITS); sy = cy + (y >> AA_BITS);

		//		plot(sx, sy, AA_AND - (x & AA_AND));
		//		plot(sx + 1, sy, x & AA_AND);

		//		sy = cy - (y >> AA_BITS);

		//		plot(sx, sy, AA_AND - (x & AA_AND));
		//		plot(sx + 1, sy, x & AA_AND);

		//		sx = cx - (x >> AA_BITS);

		//		plot(sx, sy, AA_AND - (x & AA_AND));
		//		plot(sx - 1, sy, x & AA_AND);

		//		sy = cy + (y >> AA_BITS);

		//		plot(sx, sy, AA_AND - (x & AA_AND));
		//		plot(sx - 1, sy, x & AA_AND);

		//		//tall pixels
		//		sx = cx + (y >> AA_BITS); sy = cy + (x >> AA_BITS);

		//		plot(sx, sy, AA_AND - (x & AA_AND));
		//		plot(sx, sy + 1, x & AA_AND);

		//		sy = cy - (x >> AA_BITS);

		//		plot(sx, sy, AA_AND - (x & AA_AND));
		//		plot(sx, sy - 1, x & AA_AND);

		//		sx = cx - (y >> AA_BITS);

		//		plot(sx, sy, AA_AND - (x & AA_AND));
		//		plot(sx, sy - 1, x & AA_AND);

		//		sy = cy + (x >> AA_BITS);

		//		plot(sx, sy, AA_AND - (x & AA_AND));
		//		plot(sx, sy + 1, x & AA_AND);

		//		s += AA_AND + 1 + (y << (AA_BITS + 1)) + ((1 << (AA_BITS + 2)) - 2);
		//		y += AA_AND + 1;

		//		while (s >= 0)
		//		{
		//			s -= (x << 1) + 2;
		//			x--;
		//		}
		//	}
		//}

  //      /*
		// * Draw anti-aliased line.
		// * By T.Harte.
		// */
  //      static void gfx_draw_aa_line(int x1, int y1, int x2, int y2)
		//{
		//	fixed grad, xd, yd;
		//	fixed xgap, ygap, xend, yend, xf, yf;
		//	fixed brightness1, brightness2, swap;

		//	int x, y, ix1, ix2, iy1, iy2;

		//	x1 += itofix(GFX_X_OFFSET);
		//	x2 += itofix(GFX_X_OFFSET);
		//	y1 += itofix(GFX_Y_OFFSET);
		//	y2 += itofix(GFX_Y_OFFSET);

		//	xd = x2 - x1;
		//	yd = y2 - y1;

		//	if (Math.Abs(xd) > Math.Abs(yd))
		//	{
		//		if (x1 > x2)
		//		{
		//			swap = x1; x1 = x2; x2 = swap;
		//			swap = y1; y1 = y2; y2 = swap;
		//			xd = -xd;
		//			yd = -yd;
		//		}

		//		grad = fdiv(yd, xd);

		//		//end point 1

		//		xend = trunc(x1 + 32768);
		//		yend = y1 + fmul(grad, xend - x1);

		//		xgap = invfrac(x1 + 32768);

		//		ix1 = xend >> 16;
		//		iy1 = yend >> 16;

		//		brightness1 = fmul(invfrac(yend), xgap);
		//		brightness2 = fmul(frac(yend), xgap);

		//		plot(ix1, iy1, brightness1 >> (16 - AA_BITS));
		//		plot(ix1, iy1 + 1, brightness2 >> (16 - AA_BITS));

		//		yf = yend + grad;

		//		//end point 2;

		//		xend = trunc(x2 + 32768);
		//		yend = y2 + fmul(grad, xend - x2);

		//		xgap = invfrac(x2 - 32768);

		//		ix2 = xend >> 16;
		//		iy2 = yend >> 16;

		//		brightness1 = fmul(invfrac(yend), xgap);
		//		brightness2 = fmul(frac(yend), xgap);

		//		plot(ix2, iy2, brightness1 >> (16 - AA_BITS));
		//		plot(ix2, iy2 + 1, brightness2 >> (16 - AA_BITS));

		//		for (x = ix1 + 1; x <= ix2 - 1; x++)
		//		{
		//			brightness1 = invfrac(yf);
		//			brightness2 = frac(yf);

		//			plot(x, (yf >> 16), brightness1 >> (16 - AA_BITS));
		//			plot(x, 1 + (yf >> 16), brightness2 >> (16 - AA_BITS));

		//			yf += grad;
		//		}
		//	}
		//	else
		//	{
		//		if (y1 > y2)
		//		{
		//			swap = x1; x1 = x2; x2 = swap;
		//			swap = y1; y1 = y2; y2 = swap;
		//			xd = -xd;
		//			yd = -yd;
		//		}

		//		grad = fdiv(xd, yd);

		//		//end point 1

		//		yend = trunc(y1 + 32768);
		//		xend = x1 + fmul(grad, yend - y1);

		//		ygap = invfrac(y1 + 32768);

		//		iy1 = yend >> 16;
		//		ix1 = xend >> 16;

		//		brightness1 = fmul(invfrac(xend), ygap);
		//		brightness2 = fmul(frac(xend), ygap);

		//		plot(ix1, iy1, brightness1 >> (16 - AA_BITS));
		//		plot(ix1 + 1, iy1, brightness2 >> (16 - AA_BITS));

		//		xf = xend + grad;

		//		//end point 2;

		//		yend = trunc(y2 + 32768);
		//		xend = x2 + fmul(grad, yend - y2);

		//		ygap = invfrac(y2 - 32768);

		//		ix2 = xend >> 16;
		//		iy2 = yend >> 16;

		//		brightness1 = fmul(invfrac(xend), ygap);
		//		brightness2 = fmul(frac(xend), ygap);

		//		plot(ix2, iy2, brightness1 >> (16 - AA_BITS));
		//		plot(ix2 + 1, iy2, brightness2 >> (16 - AA_BITS));

		//		for (y = iy1 + 1; y <= iy2 - 1; y++)
		//		{
		//			brightness1 = invfrac(xf);
		//			brightness2 = frac(xf);

		//			plot((xf >> 16), y, brightness1 >> (16 - AA_BITS));
		//			plot(1 + (xf >> 16), y, brightness2 >> (16 - AA_BITS));

		//			xf += grad;
		//		}
		//	}
		//}

//#undef trunc
//#undef frac
//#undef invfrac
//#undef plot

//#undef AA_BITS
//#undef AA_AND
//#undef AA_BASE

		public void DrawCircle(int cx, int cy, int radius, GFX_COL circle_colour)
		{
            Debug.WriteLine(nameof(DrawCircle));

			//if (elite.anti_alias_gfx && (circle_colour == gfx.GFX_COL_WHITE))
			//{
			//	gfx_draw_aa_circle(cx, cy, itofix(radius));
			//}
			//else
			//{
			//	circle(gfx_screen, cx + gfx.GFX_X_OFFSET, cy + gfx.GFX_Y_OFFSET, radius, circle_colour);
			//}
		}

		public void DrawLine(int x1, int y1, int x2, int y2)
		{
            //Debug.WriteLine(nameof(DrawLine));

            //if (y1 == y2)
            //{
            //	hline(gfx_screen, x1 + gfx.GFX_X_OFFSET, y1 + gfx.GFX_Y_OFFSET, x2 + gfx.GFX_X_OFFSET, gfx.GFX_COL_WHITE);
            //	return;
            //}

            //if (x1 == x2)
            //{
            //	vline(gfx_screen, x1 + gfx.GFX_X_OFFSET, y1 + gfx.GFX_Y_OFFSET, y2 + gfx.GFX_Y_OFFSET, gfx.GFX_COL_WHITE);
            //	return;
            //}

            //if (elite.anti_alias_gfx)
            //{
            //	gfx_draw_aa_line(itofix(x1), itofix(y1), itofix(x2), itofix(y2));
            //}
            //else
            //{
            Pen pen = new(MapColor(GFX_COL.GFX_COL_WHITE), 1);
				_gfx_screen_graphics.DrawLine(pen, x1 + gfx.GFX_X_OFFSET, y1 + gfx.GFX_Y_OFFSET, x2 + gfx.GFX_X_OFFSET, y2 + gfx.GFX_Y_OFFSET);
            //}
        }

		public void DrawLine(int x1, int y1, int x2, int y2, GFX_COL line_colour)
		{
            //Debug.WriteLine(nameof(DrawLine));

            //if (y1 == y2)
            //{
            //	hline(gfx_screen, x1 + gfx.GFX_X_OFFSET, y1 + gfx.GFX_Y_OFFSET, x2 + gfx.GFX_X_OFFSET, line_colour);
            //	return;
            //}

            //if (x1 == x2)
            //{
            //	vline(gfx_screen, x1 + gfx.GFX_X_OFFSET, y1 + gfx.GFX_Y_OFFSET, y2 + gfx.GFX_Y_OFFSET, line_colour);
            //	return;
            //}

            //if (elite.anti_alias_gfx && (line_colour == gfx.GFX_COL_WHITE))
            //{
            //	gfx_draw_aa_line(itofix(x1), itofix(y1), itofix(x2), itofix(y2));
            //}
            //else
            //{
            Pen pen = new(MapColor(line_colour), 1);
				_gfx_screen_graphics.DrawLine(pen, x1 + gfx.GFX_X_OFFSET, y1 + gfx.GFX_Y_OFFSET, x2 + gfx.GFX_X_OFFSET, y2 + gfx.GFX_Y_OFFSET);
			//}
		}

        public void DrawLineXor(int x1, int y1, int x2, int y2, GFX_COL line_colour)
		{
            Debug.WriteLine(nameof(DrawLineXor));

            //xor_mode(true);
            //gfx_draw_colour_line(x1, y1, x2, y2, line_colour);
            //xor_mode(false);
        }

        public void DrawTriangle(int x1, int y1, int x2, int y2, int x3, int y3, GFX_COL col)
		{
            Debug.WriteLine(nameof(DrawTriangle));

         //   triangle(gfx_screen, x1 + gfx.GFX_X_OFFSET, y1 + gfx.GFX_Y_OFFSET, x2 + gfx.GFX_X_OFFSET, y2 + gfx.GFX_Y_OFFSET,
						   //x3 + gfx.GFX_X_OFFSET, y3 + gfx.GFX_Y_OFFSET, col);
		}

		public void DisplayText(int x, int y, string txt)
		{
            Debug.WriteLine(nameof(DisplayText));

			//text_mode(-1);
			//textout(gfx_screen, datafile[ELITE_1].dat, txt, (x / (2 / gfx.GFX_SCALE)) + gfx.GFX_X_OFFSET, (y / (2 / gfx.GFX_SCALE)) + gfx.GFX_Y_OFFSET, gfx.GFX_COL_WHITE);
		}

		public void DisplayText(int x, int y, string txt, GFX_COL col)
		{
            Debug.WriteLine(nameof(DisplayText));

			//text_mode(-1);
			//textout(gfx_screen, datafile[ELITE_1].dat, txt, (x / (2 / gfx.GFX_SCALE)) + gfx.GFX_X_OFFSET, (y / (2 / gfx.GFX_SCALE)) + gfx.GFX_Y_OFFSET, col);
		}

		public void DisplayTextCentre(int y, string str, int psize, GFX_COL col)
		{
            Debug.WriteLine(nameof(DisplayTextCentre));

			//int txt_size;
			//int txt_colour;

			//if (psize == 140)
			//{
			//	txt_size = ELITE_2;
			//	txt_colour = -1;
			//}
			//else
			//{
			//	txt_size = ELITE_1;
			//	txt_colour = col;
			//}

			//text_mode(-1);
			//textout_centre(gfx_screen, datafile[txt_size].dat, str, (128 * gfx.GFX_SCALE) + gfx.GFX_X_OFFSET, (y / (2 / gfx.GFX_SCALE)) + gfx.GFX_Y_OFFSET, txt_colour);
		}

		public void ClearDisplay()
		{
            //Debug.WriteLine(nameof(ClearDisplay));

            //rectfill(gfx_screen, gfx.GFX_X_OFFSET + 1, gfx.GFX_Y_OFFSET + 1, 510 + gfx.GFX_X_OFFSET, 383 + gfx.GFX_Y_OFFSET, gfx.GFX_COL_BLACK);

            Brush brush = new SolidBrush(Color.Black);
            _gfx_screen_graphics.FillRectangle(brush, gfx.GFX_X_OFFSET + 1, gfx.GFX_Y_OFFSET + 1, 510 + gfx.GFX_X_OFFSET, 383 + gfx.GFX_Y_OFFSET);
		}

		public void ClearTextArea()
		{
            Debug.WriteLine(nameof(ClearTextArea));

            //rectfill(gfx_screen, gfx.GFX_X_OFFSET + 1, gfx.GFX_Y_OFFSET + 340, 510 + gfx.GFX_X_OFFSET, 383 + gfx.GFX_Y_OFFSET, gfx.GFX_COL_BLACK);
		}

		public void ClearArea(int tx, int ty, int bx, int by)
		{
            Debug.WriteLine(nameof(ClearArea));

            //rectfill(gfx_screen, tx + gfx.GFX_X_OFFSET, ty + gfx.GFX_Y_OFFSET, bx + gfx.GFX_X_OFFSET, by + gfx.GFX_Y_OFFSET, gfx.GFX_COL_BLACK);
		}

		public void DrawRectangle(int tx, int ty, int bx, int by, GFX_COL col)
		{
            Debug.WriteLine(nameof(DrawRectangle));

            //rectfill(gfx_screen, tx + gfx.GFX_X_OFFSET, ty + gfx.GFX_Y_OFFSET, bx + gfx.GFX_X_OFFSET, by + gfx.GFX_Y_OFFSET, col);
		}

		public void DisplayTextPretty(int tx, int ty, int bx, int by, string txt)
		{
            Debug.WriteLine(nameof(DisplayTextPretty));

			//char strbuf[100];
			//char* str;
			//char* bptr;
			//int len;
			//int pos;
			//int maxlen;

			//maxlen = (bx - tx) / 8;

			//str = txt;
			//len = strlen(txt);

			//while (len > 0)
			//{
			//	pos = maxlen;
			//	if (pos > len)
			//		pos = len;

			//	while ((str[pos] != ' ') && (str[pos] != ',') &&
			//		   (str[pos] != '.') && (str[pos] != '\0'))
			//	{
			//		pos--;
			//	}

			//	len = len - pos - 1;

			//	for (bptr = strbuf; pos >= 0; pos--)
			//		*bptr++ = *str++;

			//	*bptr = '\0';

			//	text_mode(-1);
			//	textout(gfx_screen, datafile[ELITE_1].dat, strbuf, tx + gfx.GFX_X_OFFSET, ty + gfx.GFX_Y_OFFSET, gfx.GFX_COL_WHITE);
			//	ty += (8 * GFX_SCALE);
			//}
		}

		public void DrawScanner()
		{
            // Debug.WriteLine(nameof(DrawScanner));

            _gfx_screen_graphics.DrawImage(scanner_image, gfx.GFX_X_OFFSET, 385 + gfx.GFX_Y_OFFSET);
        }

		public void SetClipRegion(int tx, int ty, int bx, int by)
		{
            Debug.WriteLine(nameof(SetClipRegion));

            //set_clip(gfx_screen, tx + gfx.GFX_X_OFFSET, ty + gfx.GFX_Y_OFFSET, bx + gfx.GFX_X_OFFSET, by + gfx.GFX_Y_OFFSET);
		}

		public void RenderStart()
		{
			start_poly = 0;
			total_polys = 0;
		}

		public void DrawPolygon(Vector2[] point_list, GFX_COL face_colour, int zavg)
		{
			int i;
			int nx;

			if (total_polys == MAX_POLYS)
			{
				return;
			}

			int x = total_polys;
			total_polys++;

			poly_chain[x].face_colour = face_colour;
			poly_chain[x].z = zavg;
			poly_chain[x].next = -1;
            poly_chain[x].point_list = new Point[point_list.Length];

            for (i = 0; i < point_list.Length; i++)
			{
				poly_chain[x].point_list[i].X = (int)point_list[i].X;
                poly_chain[x].point_list[i].Y = (int)point_list[i].Y;
            }

			if (x == 0)
			{
				return;
			}

			if (zavg > poly_chain[start_poly].z)
			{
				poly_chain[x].next = start_poly;
				start_poly = x;
				return;
			}

			for (i = start_poly; poly_chain[i].next != -1; i = poly_chain[i].next)
			{
				nx = poly_chain[i].next;

				if (zavg > poly_chain[nx].z)
				{
					poly_chain[i].next = x;
					poly_chain[x].next = nx;
					return;
				}
			}

			poly_chain[i].next = x;
		}

		public void DrawLine(int x1, int y1, int x2, int y2, int dist, GFX_COL col)
		{
			Vector2[] point_list = new Vector2[2];

			point_list[0].X = x1;
			point_list[0].Y = y1;
			point_list[1].X = x2;
			point_list[1].Y = y2;

			DrawPolygon(point_list, col, dist);
		}

		public void RenderFinish()
		{
			if (total_polys == 0)
            {
                return;
            }

            for (int i = start_poly; i != -1; i = poly_chain[i].next)
			{
                Point[] points = poly_chain[i].point_list;
				GFX_COL col = poly_chain[i].face_colour;

				if (points.Length == 2)
				{
					DrawLine(points[0].X, points[0].Y, points[1].X, points[1].Y, col);
					continue;
				}

				gfx_polygon(points, col);
			};
		}

		private void gfx_polygon(Point[] points, GFX_COL face_colour)
		{
            // Debug.WriteLine(nameof(gfx_polygon));

            for (int i = 0; i < points.Length; i++)
			{
				points[i].X += gfx.GFX_X_OFFSET;
				points[i].Y += gfx.GFX_Y_OFFSET;
            }

			Brush brush = new SolidBrush(MapColor(face_colour));
			_gfx_screen_graphics.FillPolygon(brush, points);
		}

		public void DrawSprite(IMG sprite_no, int x, int y)
		{
            Debug.WriteLine(nameof(DrawSprite));

			//BITMAP* sprite_bmp;

			//switch (sprite_no)
			//{
			//	case gfx.IMG_GREEN_DOT:
			//		sprite_bmp = datafile[GRNDOT].dat;
			//		break;

			//	case gfx.IMG_RED_DOT:
			//		sprite_bmp = datafile[REDDOT].dat;
			//		break;

			//	case gfx.IMG_BIG_S:
			//		sprite_bmp = datafile[SAFE].dat;
			//		break;

			//	case gfx.IMG_ELITE_TXT:
			//		sprite_bmp = datafile[ELITETXT].dat;
			//		break;

			//	case gfx.IMG_BIG_E:
			//		sprite_bmp = datafile[ECM].dat;
			//		break;

			//	case gfx.IMG_BLAKE:
			//		sprite_bmp = datafile[BLAKE].dat;
			//		break;

			//	case gfx.IMG_MISSILE_GREEN:
			//		sprite_bmp = datafile[MISSILE_G].dat;
			//		break;

			//	case gfx.IMG_MISSILE_YELLOW:
			//		sprite_bmp = datafile[MISSILE_Y].dat;
			//		break;

			//	case gfx.IMG_MISSILE_RED:
			//		sprite_bmp = datafile[MISSILE_R].dat;
			//		break;

			//	default:
			//		return;
			//}

			//if (x == -1)
			//{
			//	x = ((256 * gfx.GFX_SCALE) - sprite_bmp.w) / 2;
			//}

			//draw_sprite(gfx_screen, sprite_bmp, x + gfx.GFX_X_OFFSET, y + gfx.GFX_Y_OFFSET);
		}

		public bool RequestFile(string title, string path, string ext)
		{
            Debug.WriteLine(nameof(RequestFile));

			bool okay = false;

			//show_mouse(screen);
			//okay = file_select(title, path, ext);
			//show_mouse(null);

			return okay;
		}

		private static Color MapColor(GFX_COL col)
		{
			switch (col)
			{
				case GFX_COL.GFX_COL_WHITE:
					return Color.White;

				case GFX_COL.GFX_COL_BLACK:
					return Color.Black;

                case GFX_COL.GFX_COL_GOLD:
                    return Color.Gold;

                case GFX_COL.GFX_COL_YELLOW_1:
                    return Color.Yellow;

                case GFX_COL.GFX_COL_RED:
                    return Color.Red;

                case GFX_COL.GFX_COL_DARK_RED:
                    return Color.DarkRed;

				case GFX_COL.GFX_COL_BLUE_1:
                    return Color.Blue;

                case GFX_COL.GFX_COL_BLUE_2:
                    return Color.AliceBlue;

                case GFX_COL.GFX_COL_BLUE_3:
                    return Color.DodgerBlue;

                case GFX_COL.GFX_COL_GREY_1:
                    return Color.Gray;

                case GFX_COL.GFX_COL_GREY_2:
                    return Color.SlateGray;

                case GFX_COL.GFX_COL_GREY_3:
                    return Color.DarkGray;

                default:
					Debug.Assert(false);
					return Color.Magenta;
			}
		}
	}
}