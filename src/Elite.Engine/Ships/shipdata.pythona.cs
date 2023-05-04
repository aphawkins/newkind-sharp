namespace Elite.Engine.Ships
{
    using Elite.Engine.Enums;
    using Elite.Engine.Types;

    internal static partial class Ship
    {
        internal static readonly ship_point[] pythona_point =
        {
            new(new(   0,    0,  224), 31,  1,  0,  3,  2),
            new(new(   0,   48,   48), 31,  1,  0,  5,  4),
            new(new(  96,    0,  -16), 31, 15, 15, 15, 15),
            new(new( -96,    0,  -16), 31, 15, 15, 15, 15),
            new(new(   0,   48,  -32), 31,  5,  4,  9,  8),
            new(new(   0,   24, -112), 31,  8,  9, 12, 12),
            new(new( -48,    0, -112), 31, 11,  8, 12, 12),
            new(new(  48,    0, -112), 31, 10,  9, 12, 12),
            new(new(   0,  -48,   48), 31,  3,  2,  7,  6),
            new(new(   0,  -48,  -32), 31,  7,  6, 11, 10),
            new(new(   0,  -24, -112), 31, 11, 10, 12, 12),
        };

        internal static readonly ship_line[] pythona_line =
        {
            new(31,  3,  2,  0,  8),
            new(31,  2,  0,  0,  3),
            new(31,  3,  1,  0,  2),
            new(31,  1,  0,  0,  1),
            new(31,  5,  9,  2,  4),
            new(31,  5,  1,  1,  2),
            new(31,  3,  7,  2,  8),
            new(31,  4,  0,  1,  3),
            new(31,  6,  2,  3,  8),
            new(31, 10,  7,  2,  9),
            new(31,  8,  4,  3,  4),
            new(31, 11,  6,  3,  9),
            new( 7,  8,  8,  3,  5),
            new( 7, 11, 11,  3, 10),
            new( 7,  9,  9,  2,  5),
            new( 7, 10, 10,  2, 10),
            new(31, 10,  9,  2,  7),
            new(31, 11,  8,  3,  6),
            new(31, 12,  8,  5,  6),
            new(31, 12,  9,  5,  7),
            new(31, 10, 12,  7, 10),
            new(31, 12, 11,  6, 10),
            new(31,  9,  8,  4,  5),
            new(31, 11, 10,  9, 10),
            new(31,  5,  4,  1,  4),
            new(31,  7,  6,  8,  9),
        };

        internal static readonly ship_face_normal[] pythona_face_normal =
        {
            new(31, new( -27,   40,   11)),
            new(31, new(  27,   40,   11)),
            new(31, new( -27,  -40,   11)),
            new(31, new(  27,  -40,   11)),
            new(31, new( -19,   38,    0)),
            new(31, new(  19,   38,    0)),
            new(31, new( -19,  -38,    0)),
            new(31, new(  19,  -38,    0)),
            new(31, new( -25,   37,  -11)),
            new(31, new(  25,   37,  -11)),
            new(31, new(  25,  -37,  -11)),
            new(31, new( -25,  -37,  -11)),
            new(31, new(   0,    0, -112)),
        };

        private static readonly ship_face[] pythona_face =
        {
            new(GFX_COL.GFX_COL_GREY_2, new(-0x1B, 0x28, 0x0B ), new[] { 0,  1,  3 }),
            new(GFX_COL.GFX_COL_GREY_1, new( 0x1B, 0x28, 0x0B ), new[] { 2,  1,  0 }),
            new(GFX_COL.GFX_COL_GREY_1, new(-0x1B,-0x28, 0x0B ), new[] { 0,  3,  8 }),
            new(GFX_COL.GFX_COL_GREY_2, new( 0x1B,-0x28, 0x0B ), new[] { 8,  2,  0 }),

            new(GFX_COL.GFX_COL_YELLOW_1, new(-0x13, 0x26, 0x00 ), new[] { 3,  1,  4 }),
            new(GFX_COL.GFX_COL_GOLD, new( 0x13, 0x26, 0x00 ), new[] { 4,  1,  2 }),
            new(GFX_COL.GFX_COL_GOLD, new(-0x13,-0x26, 0x00 ), new[] { 3,  9,  8 }),
            new(GFX_COL.GFX_COL_YELLOW_1, new( 0x13,-0x26, 0x00 ), new[] { 8,  9,  2 }),

            new(GFX_COL.GFX_COL_GREY_2, new(-0x19, 0x25,-0x0B ), new[] { 3,  4,  5, 6 }),
            new(GFX_COL.GFX_COL_GREY_1, new( 0x19, 0x25,-0x0B ), new[] { 2,  7,  5, 4 }),
            new(GFX_COL.GFX_COL_GREY_2, new( 0x19,-0x25,-0x0B ), new[] { 2,  9, 10, 7 }),
            new(GFX_COL.GFX_COL_GREY_1, new(-0x19,-0x25,-0x0B ), new[] { 3,  6, 10, 9 }),

            new(GFX_COL.GFX_COL_GREY_3, new( 0x00, 0x00,-0x70 ), new[] { 10, 6 , 5, 7 }),
        };

        internal static ShipData pythona_data = new(
            "Python",
            5,
            0,
            6400,
            0,
            0,
            40,
            250,
            20,
            3,
            13,
            pythona_point,
            pythona_line,
            pythona_face_normal,
            pythona_face
        );
    }
}