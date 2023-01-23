namespace Elite.Ships
{
    using Elite.Enums;
    using Elite.Structs;

    internal static partial class shipdata
    {
        internal static readonly ship_point[] transp_point =
        {
            new(new(   0,   10,  -26), 31,  0,  6,  7,  7),
            new(new( -25,    4,  -26), 31,  0,  1,  7,  7),
            new(new( -28,   -3,  -26), 31,  0,  1,  2,  2),
            new(new( -25,   -8,  -26), 31,  0,  2,  3,  3),
            new(new(  26,   -8,  -26), 31,  0,  3,  4,  4),
            new(new(  29,   -3,  -26), 31,  0,  4,  5,  5),
            new(new(  26,    4,  -26), 31,  0,  5,  6,  6),
            new(new(   0,    6,   12), 19, 15, 15, 15, 15),
            new(new( -30,   -1,   12), 31,  1,  7,  8,  9),
            new(new( -33,   -8,   12), 31,  1,  2,  3,  9),
            new(new(  33,   -8,   12), 31,  3,  4,  5, 10),
            new(new(  30,   -1,   12), 31,  5,  6, 10, 11),
            new(new( -11,   -2,   30), 31,  8,  9, 12, 13),
            new(new( -13,   -8,   30), 31,  3,  9, 13, 13),
            new(new(  14,   -8,   30), 31,  3, 10, 13, 13),
            new(new(  11,   -2,   30), 31, 10, 11, 12, 13),
            new(new(  -5,    6,    2),  7,  7,  7,  7,  7),
            new(new( -18,    3,    2),  7,  7,  7,  7,  7),
            new(new(  -5,    7,   -7),  7,  7,  7,  7,  7),
            new(new( -18,    4,   -7),  7,  7,  7,  7,  7),
            new(new( -11,    6,  -14),  7,  7,  7,  7,  7),
            new(new( -11,    5,   -7),  7,  7,  7,  7,  7),
            new(new(   5,    7,  -14),  7,  6,  6,  6,  6),
            new(new(  18,    4,  -14),  7,  6,  6,  6,  6),
            new(new(  11,    5,   -7),  7,  6,  6,  6,  6),
            new(new(   5,    6,   -3),  7,  6,  6,  6,  6),
            new(new(  18,    3,   -3),  7,  6,  6,  6,  6),
            new(new(  11,    4,    8),  7,  6,  6,  6,  6),
            new(new(  11,    5,   -3),  7,  6,  6,  6,  6),
            new(new( -16,   -8,  -13),  6,  3,  3,  3,  3),
            new(new( -16,   -8,   16),  6,  3,  3,  3,  3),
            new(new(  17,   -8,  -13),  6,  3,  3,  3,  3),
            new(new(  17,   -8,   16),  6,  3,  3,  3,  3),
            new(new( -13,   -3,  -26),  8,  0,  0,  0,  0),
            new(new(  13,   -3,  -26),  8,  0,  0,  0,  0),
            new(new(   9,    3,  -26),  5,  0,  0,  0,  0),
            new(new(  -8,    3,  -26),  5,  0,  0,  0,  0),
        };

        internal static readonly ship_line[] transp_line =
        {
            new(31,  0,  7,  0,  1),
            new(31,  0,  1,  1,  2),
            new(31,  0,  2,  2,  3),
            new(31,  0,  3,  3,  4),
            new(31,  0,  4,  4,  5),
            new(31,  0,  5,  5,  6),
            new(31,  0,  6,  0,  6),
            new(16,  6,  7,  0,  7),
            new(31,  1,  7,  1,  8),
            new(11,  1,  2,  2,  9),
            new(31,  2,  3,  3,  9),
            new(31,  3,  4,  4, 10),
            new(11,  4,  5,  5, 10),
            new(31,  5,  6,  6, 11),
            new(17,  7,  8,  7,  8),
            new(17,  1,  9,  8,  9),
            new(17,  5, 10, 10, 11),
            new(17,  6, 11,  7, 11),
            new(19, 11, 12,  7, 15),
            new(19,  8, 12,  7, 12),
            new(16,  8,  9,  8, 12),
            new(31,  3,  9,  9, 13),
            new(31,  3, 10, 10, 14),
            new(16, 10, 11, 11, 15),
            new(31,  9, 13, 12, 13),
            new(31,  3, 13, 13, 14),
            new(31, 10, 13, 14, 15),
            new(31, 12, 13, 12, 15),
            new( 7,  7,  7, 16, 17),
            new( 7,  7,  7, 18, 19),
            new( 7,  7,  7, 19, 20),
            new( 7,  7,  7, 18, 20),
            new( 7,  7,  7, 20, 21),
            new( 7,  6,  6, 22, 23),
            new( 7,  6,  6, 23, 24),
            new( 7,  6,  6, 24, 22),
            new( 7,  6,  6, 25, 26),
            new( 7,  6,  6, 26, 27),
            new( 7,  6,  6, 25, 27),
            new( 7,  6,  6, 27, 28),
            new( 6,  3,  3, 29, 30),
            new( 6,  3,  3, 31, 32),
            new( 8,  0,  0, 33, 34),
            new( 5,  0,  0, 34, 35),
            new( 5,  0,  0, 35, 36),
            new( 5,  0,  0, 36, 33),
        };

        internal static readonly ship_face_normal[] transp_face_normal =
        {
            new ship_face_normal(31, new(   0,    0, -103)),
            new ship_face_normal(31, new(-111,   48,   -7)),
            new ship_face_normal(31, new(-105,  -63,  -21)),
            new ship_face_normal(31, new(   0,  -34,    0)),
            new ship_face_normal(31, new( 105,  -63,  -21)),
            new ship_face_normal(31, new( 111,   48,   -7)),
            new ship_face_normal(31, new(   8,   32,    3)),
            new ship_face_normal(31, new(  -8,   32,    3)),
            new ship_face_normal(19, new(  -8,   34,   11)),
            new ship_face_normal(31, new( -75,   32,   79)),
            new ship_face_normal(31, new(  75,   32,   79)),
            new ship_face_normal(19, new(   8,   34,   11)),
            new ship_face_normal(31, new(   0,   38,   17)),
            new ship_face_normal(31, new(   0,    0,  121)),
        };

        /* Two lines on base & detail of IB DB initials added back in :-) */
        private static readonly ship_face[] transporter_face =
        {
            new(GFX_COL.GFX_COL_GREY_3, new( 0x00, 0x00,-0x67), 7,  5,  4,  3, 2,  1 ,0, 6, 0),

            new(GFX_COL.GFX_COL_BLUE_1, new(-0x6F, 0x30,-0x07), 4,  9,  8,  1,  2, 0,  0, 0, 0),
            new(GFX_COL.GFX_COL_BLUE_2, new(-0x69,-0x3F,-0x15), 3,  3,  9,  2,  0, 0,  0, 0, 0),
            new(GFX_COL.GFX_COL_BLUE_4, new( 0x00,-0x22, 0x00), 6, 14, 13,  9,  3, 4, 10, 0, 0),
            new(GFX_COL.GFX_COL_BLUE_2, new( 0x69,-0x3F,-0x15), 3,  5, 10,  4,  0, 0,  0, 0, 0),
            new(GFX_COL.GFX_COL_BLUE_1, new( 0x6F, 0x30,-0x07), 4, 11, 10,  5,  6, 0,  0, 0, 0),

            new(GFX_COL.GFX_COL_GREY_1, new( 0x08, 0x20, 0x03), 4, 6,  0,  7, 11, 0,  0, 0, 0),
            new(GFX_COL.GFX_COL_GREY_2, new(-0x08, 0x20, 0x03), 4,  8,  7,  0,  1, 0,  0, 0, 0),

            new(GFX_COL.GFX_COL_BLUE_1, new(-0x4B, 0x20, 0x4F), 4, 13, 12,  8,  9, 0,  0, 0, 0),
            new(GFX_COL.GFX_COL_BLUE_1, new( 0x4B, 0x20, 0x4F), 4, 15, 14, 10, 11, 0,  0, 0, 0),

            new(GFX_COL.GFX_COL_GREY_1, new(-0x08, 0x22, 0x0B), 3,  8, 12,  7,  0, 0,  0, 0, 0),
            new(GFX_COL.GFX_COL_GREY_2, new( 0x08, 0x22, 0x0B), 3, 7, 15,  11,  0, 0,  0, 0, 0),
            new(GFX_COL.GFX_COL_GREY_4, new( 0x00, 0x26, 0x11), 3, 7, 12,  15,  0, 0,  0, 0, 0),

            new(GFX_COL.GFX_COL_WHITE_2, new( 0x00, 0x00, 0x79), 4, 15, 12, 13, 14, 0,  0, 0, 0),
            new(GFX_COL.GFX_COL_DARK_RED, new( 0x00, 0x00,-0x67), 4, 35, 34, 33, 36, 0,  0, 0, 0),

            new(GFX_COL.GFX_COL_WHITE, new( 0x00,-0x22, 0x00), 2, 30, 29, 31,  0, 0,  0, 0, 0),
            new(GFX_COL.GFX_COL_WHITE, new( 0x00,-0x22, 0x00), 2, 31, 32, 29,  0, 0,  0, 0, 0),

            new(GFX_COL.GFX_COL_WHITE, new(-0x08, 0x20, 0x03), 2, 17, 16, 18,  0, 0,  0, 0, 0),
            new(GFX_COL.GFX_COL_WHITE, new(-0x08, 0x20, 0x03), 2, 18, 19, 16,  0, 0,  0, 0, 0),
            new(GFX_COL.GFX_COL_WHITE, new(-0x08, 0x20, 0x03), 2, 18, 20, 19,  0, 0,  0, 0, 0),
            new(GFX_COL.GFX_COL_WHITE, new(-0x08, 0x20, 0x03), 2, 20, 21, 18,  0, 0,  0, 0, 0),
            new(GFX_COL.GFX_COL_WHITE, new(-0x08, 0x20, 0x03), 2, 20, 19, 21,  0, 0,  0, 0, 0),

            new(GFX_COL.GFX_COL_WHITE, new( 0x08, 0x20, 0x03), 2, 23, 22, 26,  0, 0,  0, 0, 0),
            new(GFX_COL.GFX_COL_WHITE, new( 0x08, 0x20, 0x03), 2, 25, 26, 23,  0, 0,  0, 0, 0),
            new(GFX_COL.GFX_COL_WHITE, new( 0x08, 0x20, 0x03), 2, 24, 22, 25,  0, 0,  0, 0, 0),
            new(GFX_COL.GFX_COL_WHITE, new( 0x08, 0x20, 0x03), 2, 24, 23, 22,  0, 0,  0, 0, 0),
            new(GFX_COL.GFX_COL_WHITE, new( 0x08, 0x20, 0x03), 2, 28, 27, 23,  0, 0,  0, 0, 0),
            new(GFX_COL.GFX_COL_WHITE, new( 0x08, 0x20, 0x03), 2, 25, 27, 22,  0, 0,  0, 0, 0),
            new(GFX_COL.GFX_COL_WHITE, new( 0x08, 0x20, 0x03), 2, 27, 26, 22,  0, 0,  0, 0, 0),
        };

        internal static ship_data transp_data = new(
            "Transporter",
            0,
            0,
            2500,
            12,
            0,
            16,
            32,
            10,
            0,
            0,
            transp_point,
            transp_line,
            transp_face_normal,
            transporter_face
        );
    }
}