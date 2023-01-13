/*
 * Elite - The New Kind.
 *
 * Reverse engineered from the BBC disk version of Elite.
 * Additional material by C.J.Pinder.
 *
 * The original Elite code is (C) I.Bell & D.Braben 1984.
 * This version re-engineered in C by C.J.Pinder 1999-2001.
 *
 * email: <christian@newkind.co.uk>
 *
 *
 */

namespace Elite
{
	using System.Numerics;
	using Elite.Enums;

	internal static class Stars
	{
		internal static bool warp_stars;

		static Vector3[] stars = new Vector3[20];

		internal static void create_new_stars()
		{
			for (int i = 0; i < stars.Length; i++)
			{
				stars[i].X = (random.rand255() - 128) | 8;
				stars[i].Y = (random.rand255() - 128) | 4;
				stars[i].Z = random.rand255() | 0x90;
			}

            warp_stars = false;
		}

		static void front_starfield()
		{
			float delta = warp_stars ? 50 : elite.flight_speed;
			float alpha = elite.flight_roll;
			float beta = elite.flight_climb;

			alpha /= 256.0f;
			delta /= 2.0f;

			for (int i = 0; i < stars.Length; i++)
			{
				/* Plot the stars in their current locations... */
				Vector2 star = new();
                star.Y = stars[i].Y;
                star.X = stars[i].X;
				float zz = stars[i].Z;

                star.X += 128;
                star.Y += 96;

                star.X *= gfx.GFX_SCALE;
                star.Y *= gfx.GFX_SCALE;

				if ((!warp_stars) &&
					(star.X >= gfx.GFX_VIEW_TX) && (star.X <= gfx.GFX_VIEW_BX) &&
					(star.Y >= gfx.GFX_VIEW_TY) && (star.Y <= gfx.GFX_VIEW_BY))
				{
                    elite.alg_gfx.PlotPixel(star, GFX_COL.GFX_COL_WHITE);

					if (zz < 0xC0)
					{
                        elite.alg_gfx.PlotPixel(new(star.X + 1, star.Y), GFX_COL.GFX_COL_WHITE);
					}

					if (zz < 0x90)
					{
                        elite.alg_gfx.PlotPixel(new(star.X, star.Y + 1), GFX_COL.GFX_COL_WHITE);
                        elite.alg_gfx.PlotPixel(new(star.X + 1, star.Y + 1), GFX_COL.GFX_COL_WHITE);
					}
				}


				/* Move the stars to their new locations...*/
				float Q = delta / stars[i].Z;

				stars[i].Z -= delta;
				float yy = stars[i].Y + (stars[i].Y * Q);
				float xx = stars[i].X + (stars[i].X * Q);
				zz = stars[i].Z;

				yy = yy + (xx * alpha);
				xx = xx - (yy * alpha);

				/*
						tx = yy * beta;
						xx = xx + (tx * tx * 2);
				*/
				yy = yy + beta;

				stars[i].Y = yy;
				stars[i].X = xx;


				if (warp_stars)
				{
                    elite.alg_gfx.DrawLine(star.X, star.Y, (xx + 128) * gfx.GFX_SCALE, (yy + 96) * gfx.GFX_SCALE);
				}

                star.X = xx;
                star.Y = yy;

				if ((star.X > 120) || (star.X < -120) ||
					(star.Y > 120) || (star.Y < -120) || (zz < 16))
				{
					stars[i].X = (random.rand255() - 128) | 8;
					stars[i].Y = (random.rand255() - 128) | 4;
					stars[i].Z = random.rand255() | 0x90;
					continue;
				}

			}

            warp_stars = false;
		}

		static void rear_starfield()
		{
			float delta = warp_stars ? 50 : elite.flight_speed;
			float alpha = -elite.flight_roll;
			float beta = -elite.flight_climb;

			alpha /= 256.0f;
			delta /= 2.0f;

			for (int i = 0; i < stars.Length; i++)
			{
				/* Plot the stars in their current locations... */
				Vector2 star = new();
                star.Y = stars[i].Y;
                star.X = stars[i].X;
				float zz = stars[i].Z;

                star.X += 128;
                star.Y += 96;

                star.X *= gfx.GFX_SCALE;
                star.Y *= gfx.GFX_SCALE;

				if ((!warp_stars) &&
					(star.X >= gfx.GFX_VIEW_TX) && (star.X <= gfx.GFX_VIEW_BX) &&
					(star.Y >= gfx.GFX_VIEW_TY) && (star.Y <= gfx.GFX_VIEW_BY))
				{
                    elite.alg_gfx.PlotPixel(star, GFX_COL.GFX_COL_WHITE);

					if (zz < 0xC0)
					{
                        elite.alg_gfx.PlotPixel(new(star.X + 1, star.Y), GFX_COL.GFX_COL_WHITE);
					}

					if (zz < 0x90)
					{
                        elite.alg_gfx.PlotPixel(new(star.X, star.Y + 1), GFX_COL.GFX_COL_WHITE);
                        elite.alg_gfx.PlotPixel(new(star.X + 1, star.Y + 1), GFX_COL.GFX_COL_WHITE);
					}
				}


				/* Move the stars to their new locations...*/

				float Q = delta / stars[i].Z;

				stars[i].Z += delta;
				float yy = stars[i].Y - (stars[i].Y * Q);
				float xx = stars[i].X - (stars[i].X * Q);
				zz = stars[i].Z;

				yy = yy + (xx * alpha);
				xx = xx - (yy * alpha);

				/*
						tx = yy * beta;
						xx = xx + (tx * tx * 2);
				*/
				yy = yy + beta;

				if (warp_stars)
				{
					float ey = yy;
					float ex = xx;
					ex = (ex + 128) * gfx.GFX_SCALE;
					ey = (ey + 96) * gfx.GFX_SCALE;

					if ((star.X >= gfx.GFX_VIEW_TX) && (star.X <= gfx.GFX_VIEW_BX) &&
					   (star.Y >= gfx.GFX_VIEW_TY) && (star.Y <= gfx.GFX_VIEW_BY) &&
					   (ex >= gfx.GFX_VIEW_TX) && (ex <= gfx.GFX_VIEW_BX) &&
					   (ey >= gfx.GFX_VIEW_TY) && (ey <= gfx.GFX_VIEW_BY))
					{
                        elite.alg_gfx.DrawLine(star.X, star.Y, (xx + 128) * gfx.GFX_SCALE, (yy + 96) * gfx.GFX_SCALE);
					}
				}

				stars[i].Y = yy;
				stars[i].X = xx;

				if ((zz >= 300) || (Math.Abs(yy) >= 110))
				{
					stars[i].Z = (random.rand255() & 127) + 51;

					if ((random.rand255() & 1) == 1)
					{
						stars[i].X = random.rand255() - 128;
						stars[i].Y = ((random.rand255() & 1) == 1) ? -115 : 115;
					}
					else
					{
						stars[i].X = ((random.rand255() & 1) == 1) ? -126 : 126;
						stars[i].Y = random.rand255() - 128;
					}
				}

			}

            warp_stars = false;
		}

		static void side_starfield()
		{
			float delta = warp_stars ? 50 : elite.flight_speed;
			float alpha = elite.flight_roll;
			float beta = elite.flight_climb;

			if (elite.current_screen == SCR.SCR_LEFT_VIEW)
			{
				delta = -delta;
				alpha = -alpha;
				beta = -beta;
			}

			for (int i = 0; i < stars.Length; i++)
			{
                Vector2 star = new()
                {
                    Y = stars[i].Y,
                    X = stars[i].X
                };
                float zz = stars[i].Z;

				star.X += 128;
                star.Y += 96;

                star.X *= gfx.GFX_SCALE;
                star.Y *= gfx.GFX_SCALE;

				if ((!warp_stars) &&
					(star.X >= gfx.GFX_VIEW_TX) && (star.X <= gfx.GFX_VIEW_BX) &&
					(star.Y >= gfx.GFX_VIEW_TY) && (star.Y <= gfx.GFX_VIEW_BY))
				{
                    elite.alg_gfx.PlotPixel(star, GFX_COL.GFX_COL_WHITE);

					if (zz < 0xC0)
					{
                        elite.alg_gfx.PlotPixel(new(star.X + 1, star.Y), GFX_COL.GFX_COL_WHITE);
					}

					if (zz < 0x90)
					{
                        elite.alg_gfx.PlotPixel(new(star.X, star.Y + 1), GFX_COL.GFX_COL_WHITE);
                        elite.alg_gfx.PlotPixel(new(star.X + 1, star.Y + 1), GFX_COL.GFX_COL_WHITE);
					}
				}

				float yy = stars[i].Y;
				float xx = stars[i].X;
				zz = stars[i].Z;

				float delt8 = delta / (zz / 32);
				xx = xx + delt8;

				xx += (yy * (beta / 256));
				yy -= (xx * (beta / 256));

				xx += ((yy / 256) * (alpha / 256)) * (-xx);
				yy += ((yy / 256) * (alpha / 256)) * (yy);

				yy += alpha;

				stars[i].Y = yy;
				stars[i].X = xx;

				if (warp_stars)
				{
                    elite.alg_gfx.DrawLine(star.X, star.Y, (xx + 128) * gfx.GFX_SCALE, (yy + 96) * gfx.GFX_SCALE);
				}

				if (Math.Abs(stars[i].X) >= 116)
				{
					stars[i].Y = random.rand255() - 128;
					stars[i].X = (elite.current_screen == SCR.SCR_LEFT_VIEW) ? 115 : -115;
					stars[i].Z = random.rand255() | 8;
				}
				else if (Math.Abs(stars[i].Y) >= 116)
				{
					stars[i].X = random.rand255() - 128;
					stars[i].Y = (alpha > 0) ? -110 : 110;
					stars[i].Z = random.rand255() | 8;
				}

			}

            warp_stars = false;
		}

		/// <summary>
		/// When we change view, flip the stars over so they look like other stars.
		/// </summary>
		internal static void flip_stars()
		{
			for (int i = 0; i < stars.Length; i++)
			{
                float y = stars[i].Y;
                float x = stars[i].X;
                stars[i].X = y;
                stars[i].Y = x;
			}
		}

		internal static void update_starfield()
		{
			switch (elite.current_screen)
			{
				case SCR.SCR_FRONT_VIEW:
				case SCR.SCR_INTRO_ONE:
				case SCR.SCR_INTRO_TWO:
				case SCR.SCR_ESCAPE_POD:
					front_starfield();
					break;

				case SCR.SCR_REAR_VIEW:
				case SCR.SCR_GAME_OVER:
					rear_starfield();
					break;

				case SCR.SCR_LEFT_VIEW:
				case SCR.SCR_RIGHT_VIEW:
					side_starfield();
					break;
			}
		}
	}
}