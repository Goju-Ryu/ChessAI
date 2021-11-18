using System.Diagnostics;
using ChessAI.DataClasses;

namespace ChessAI.MoveSelection.StateAnalysis
{
    /// <summary>
    /// An implementation of a very simple static analysis algorithm by Kaare Danielsen
    /// See more in these
    /// <see href="https://learn.inside.dtu.dk/d2l/le/content/80615/viewContent/284028/View">slides</see>
    /// at page 11.
    /// </summary>
    /// <remarks>This class assumes the use of 0x88 based indexes to work properly</remarks>
    public class StateAnalyserSimple : IStateAnalyser
    {
        public int StaticAnalysis(GameState state, bool isWhite)
        {
            var whiteScore = 0;
            var blackScore = 0;

            foreach (var piece in state.WhitePieces)
            {
                whiteScore += PieceValue(piece) + PiecePositionPoints(piece);
            }

            foreach (var piece in state.BlackPieces)
            {
                blackScore += PieceValue(piece) + PiecePositionPoints(piece);
            }

            return isWhite
                ? whiteScore - blackScore
                : blackScore - whiteScore;
        }

        private static int PieceValue(Piece piece)
        {
            return (piece.PieceType) switch
            {
                Piece.Pawn => 100,
                Piece.Knight => 300,
                Piece.Bishop => 300,
                Piece.Rook => 500,
                Piece.Queen => 900,
                Piece.King => 10_000,
                _ => 0
            };
        }

        private static int PiecePositionPoints(Piece piece)
        {
            var position = piece.Position;

            // if black transform position to correct index index
            var index =
                (piece.PieceFlags & Piece.White) != Piece.White
                    ? position + 8
                    : position;


            return (piece.PieceType) switch
            {
                Piece.Pawn => PawnPositionPoints[index],
                Piece.Knight => KnightPositionPoints[index],
                Piece.Bishop => BishopPositionPoints[index],
                Piece.Rook => RookPositionPoints[index],
                Piece.Queen => QueenPositionPoints[index],
                Piece.King => KingPositionPoints[index],
                _ => 0
            };
        }

        //##########################//
        //  Position point boards   // 
        //##########################//

        // All boards are mirrored in the invalid indexes
        // This allows one to index the position value of white simply by PiecePositionPoints[index]
        // and to access the corresponding position value of black by PiecePositionPoints[index + 8]

        protected static readonly int[] KingPositionPoints =
        {
            //    0  1  2  3  4  5  6  7  /* */ 7  6  5  4  3  2  1  0
            /*7*/ 0, 0, 0, 0, 0, 0, 0, 0, /*0*/ 0, 0, 0, 0, 0, 0, 0, 0,
            /*6*/ 0, 0, 0, 0, 0, 0, 0, 0, /*1*/ 0, 0, 0, 0, 0, 0, 0, 0,
            /*5*/ 0, 0, 0, 0, 0, 0, 0, 0, /*2*/ 0, 0, 0, 0, 0, 0, 0, 0,
            /*4*/ 0, 0, 0, 0, 0, 0, 0, 0, /*3*/ 0, 0, 0, 0, 0, 0, 0, 0,
            /*3*/ 0, 0, 0, 0, 0, 0, 0, 0, /*4*/ 0, 0, 0, 0, 0, 0, 0, 0,
            /*2*/ 0, 0, 0, 0, 0, 0, 0, 0, /*5*/ 0, 0, 0, 0, 0, 0, 0, 0,
            /*1*/ 0, 0, 0, 0, 0, 0, 0, 0, /*6*/ 0, 0, 0, 0, 0, 0, 0, 0,
            /*0*/ 0, 0, 0, 0, 0, 0, 0, 0, /*7*/ 0, 0, 0, 0, 0, 0, 0, 0
        };

        protected static readonly int[] QueenPositionPoints =
        {
            //    0  1  2  3  4  5  6  7  /* */ 7  6  5  4  3  2  1  0
            /*7*/ 2, 3, 4, 3, 4, 3, 3, 2, /*0*/ 0, 0, 0, 0, 0, 0, 0, 0,
            /*6*/ 2, 3, 4, 4, 4, 4, 3, 2, /*1*/ 2, 2, 2, 2, 2, 2, 2, 2,
            /*5*/ 3, 4, 4, 4, 4, 4, 4, 3, /*2*/ 2, 2, 2, 3, 3, 2, 2, 2,
            /*4*/ 3, 3, 4, 4, 4, 4, 3, 3, /*3*/ 2, 3, 3, 4, 4, 3, 3, 2,
            /*3*/ 2, 3, 3, 4, 4, 3, 3, 2, /*4*/ 3, 3, 4, 4, 4, 4, 3, 3,
            /*2*/ 2, 2, 2, 3, 3, 2, 2, 2, /*5*/ 3, 4, 4, 4, 4, 4, 4, 3,
            /*1*/ 2, 2, 2, 2, 2, 2, 2, 2, /*6*/ 2, 3, 4, 4, 4, 4, 3, 2,
            /*0*/ 0, 0, 0, 0, 0, 0, 0, 0, /*7*/ 2, 3, 3, 4, 3, 4, 3, 2
        };

        protected static readonly int[] PawnPositionPoints =
        {
            //     0   1   2   3   4   5   6   7  /* */  7   6   5   4   3   2   1   0
            /*7*/ 00, 00, 00, 00, 00, 00, 00, 00, /*0*/ 00, 00, 00, 00, 00, 00, 00, 00,
            /*6*/ 07, 07, 13, 23, 26, 13, 07, 07, /*1*/ -1, -1, 01, 06, 05, 01, -1, -1,
            /*5*/ -2, -2, 04, 12, 15, 04, -2, -2, /*2*/ -4, -4, 00, 06, 04, 00, -4, -4,
            /*4*/ -3, -3, 02, 09, 11, 02, -3, -3, /*3*/ -4, -4, 00, 08, 06, 00, -4, -4,
            /*3*/ -4, -4, 00, 06, 08, 00, -4, -4, /*4*/ -3, -3, 02, 11, 09, 02, -3, -3,
            /*2*/ -4, -4, 00, 04, 06, 00, -4, -4, /*5*/ -2, -2, 04, 15, 12, 04, -2, -2,
            /*1*/ -1, -1, 01, 05, 06, 01, -1, -1, /*6*/ 07, 07, 13, 26, 23, 13, 07, 07,
            /*0*/ 00, 00, 00, 00, 00, 00, 00, 00, /*7*/ 00, 00, 00, 00, 00, 00, 00, 00
        };

        protected static readonly int[] RookPositionPoints =
        {
            //    0   1   2   3   4   5   6  7  /* */ 7   6   5   4   3   2   1  0
            /*7*/ 9, 09, 11, 10, 11, 09, 09, 9, /*0*/ 0, 00, 00, 00, 00, 00, 00, 0,
            /*6*/ 4, 06, 07, 09, 09, 07, 06, 4, /*1*/ 3, 04, 04, 06, 06, 04, 04, 3,
            /*5*/ 9, 10, 10, 11, 11, 10, 10, 9, /*2*/ 4, 05, 05, 05, 05, 05, 05, 4,
            /*4*/ 8, 08, 08, 09, 09, 08, 08, 8, /*3*/ 6, 06, 05, 06, 06, 05, 06, 6,
            /*3*/ 6, 06, 05, 06, 06, 05, 06, 6, /*4*/ 8, 08, 08, 09, 09, 08, 08, 8,
            /*2*/ 4, 05, 05, 05, 05, 05, 05, 4, /*5*/ 9, 10, 10, 11, 11, 10, 10, 9,
            /*1*/ 3, 04, 04, 06, 06, 04, 04, 3, /*6*/ 4, 06, 07, 09, 09, 07, 06, 4,
            /*0*/ 0, 00, 00, 00, 00, 00, 00, 0, /*7*/ 9, 09, 09, 11, 10, 11, 09, 9
        };

        protected static readonly int[] BishopPositionPoints =
        {
            //    0  1  2   3   4  5  6  7  /* */ 7  6  5   4   3  2  1  0
            /*7*/ 2, 3, 4, 04, 04, 4, 3, 2, /*0*/ 0, 0, 0, 00, 00, 0, 0, 0,
            /*6*/ 4, 7, 7, 07, 07, 7, 7, 4, /*1*/ 5, 5, 5, 03, 03, 5, 5, 5,
            /*5*/ 3, 5, 6, 06, 06, 6, 5, 3, /*2*/ 3, 5, 5, -2, -2, 5, 5, 4,
            /*4*/ 3, 5, 7, 07, 07, 7, 5, 3, /*3*/ 4, 5, 6, 08, 08, 6, 5, 4,
            /*3*/ 4, 5, 6, 08, 08, 6, 5, 4, /*4*/ 3, 5, 7, 07, 07, 7, 5, 3,
            /*2*/ 4, 5, 5, -2, -2, 5, 5, 3, /*5*/ 3, 5, 6, 06, 06, 6, 5, 3,
            /*1*/ 5, 5, 5, 03, 03, 5, 5, 5, /*6*/ 4, 7, 7, 07, 07, 7, 7, 4,
            /*0*/ 0, 0, 0, 00, 00, 0, 0, 0, /*7*/ 2, 3, 4, 04, 04, 4, 3, 2,
        };

        protected static readonly int[] KnightPositionPoints =
        {
            //     0   1   2   3   4   5   6   7  /* */  7   6   5   4   3   2   1   0
            /*7*/ -2, 02, 07, 09, 09, 07, 02, -2, /*0*/ -7, -5, -4, -2, -2, -4, -5, -7,
            /*6*/ 01, 04, 12, 13, 13, 12, 04, 01, /*1*/ -5, -3, -1, 00, 00, -1, -3, -5,
            /*5*/ 05, 11, 18, 19, 19, 18, 11, 05, /*2*/ -3, 01, 03, 04, 04, 03, 01, -3,
            /*4*/ 03, 10, 14, 14, 14, 14, 10, 03, /*3*/ 00, 05, 08, 09, 09, 08, 05, 00,
            /*3*/ 00, 05, 08, 09, 09, 08, 05, 00, /*4*/ 03, 10, 14, 14, 14, 14, 10, 03,
            /*2*/ -3, 01, 03, 04, 04, 03, 01, -3, /*5*/ 05, 11, 18, 19, 19, 18, 11, 05,
            /*1*/ -5, -3, -1, 00, 00, -1, -3, -5, /*6*/ 01, 04, 12, 13, 13, 12, 04, 01,
            /*0*/ -7, -5, -4, -2, -2, -4, -5, -7, /*7*/ -2, 02, 07, 09, 09, 07, 02, -2
        };
    }
}