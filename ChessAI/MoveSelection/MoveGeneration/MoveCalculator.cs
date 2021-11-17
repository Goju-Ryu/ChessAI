using System.Collections.Generic;
using ChessAI.DataClasses;

namespace ChessAI.MoveSelection.MoveGeneration
{
    public enum DirectionIndex
    {
        Up = 0,
        Down = 1,
        Left = 2,
        Right = 3,
        UpLeft = 4,
        DownRight = 5,
        UpRight = 7,
        DownLeft = 8
    };


    // Moves Implementation 
    public class MoveGenerator : IMoveCalculator
    {
        //public enum DirectionIndex {
        //Up = 0, Down = 1, Left = 2, Right = 3,
        //UpLeft = 4, DownRight=5, UpRight=7, DownLeft=8};

        // this is made to fit this Direction Index. 

        public static readonly sbyte[] X88Dirs =
        {
            0x10, // 1 up     0 x
            -0x10, // 1 down   0 x
            0x01, // 0 x      1 right
            -0x01, // 0 x      1 left
            0x0e, // 1 up     1 left
            -0x11, // 1 down   1 right 
            0x11, // 1 up     1 right
            -0x0e // 1 down   1 left 
        };

        private bool king, queen;
        private GameState currentState;
        private bool isWhite;

        public List<Move> CalculatePossibleMoves(GameState state, bool calculateForWhite)
        {
            List<Move> moves = new List<Move>();
            return moves;
        }

        public List<Move> calcMovesForPiece(Piece piece, byte position)
        {
            king = (piece == Piece.King);
            queen = (piece == Piece.Queen);

            List<Move> moves = new List<Move>();
            if (king || queen || piece == Piece.Rook)
            {
                // i add king to this mehtod, because it is the only piece that has a limit of 1 distance, since "king" is a bool, i pass it as "depthIs1"
                moves.AddRange(genLineMoves(position, king));
            }

            if (king || queen || piece == Piece.Bishop)
            {
                // COPY OF PREV COMMENT :::  i add king to this mehtod, because it is the only piece that has a limit of 1 distance, since "king" is a bool, i pass it as "depthIs1"
                moves.AddRange(genDiagMoves(position, king));
            }

            return moves;
        }

        // Datastructur og Algorithme

        // m�linger p� Cutoff - 

        // effektiv forgr�ning faktor

        // I takk med inf�ring af varialber der flytter kurven .  


        bool moreMoves;
        byte dirs;
        byte tempPos;

        private List<Move> genLineMoves(byte position, bool depthIs1)
        {
            List<Move> moves = new List<Move>();
            moreMoves = true;
            for (dirs = 0; dirs < 4; dirs++)
            {
                tempPos = position;
                moreMoves = true;
                while (moreMoves)
                {
                    // this adds the offset, to generate new position
                    tempPos = (byte)(tempPos + X88Dirs[dirs]);

                    // IS OUT OF BOUNDS OF BOARD 
                    if (!(Board.IsIndexValid(tempPos)))
                        break;

                    // IS Field Occupied , and if it is : is it occupied by myself or enemy? if enemy end loop after finish, else end now. 
                    if (currentState.State.IsFieldOccupied(tempPos))
                    {
                        // is Blocked
                        // is Enemy Piece ?? 
                        if (isWhite != currentState.State.isFieldOwnedByWhite(tempPos))
                        {
                            moreMoves = false; // if it is an enemy we want to end the loop after it is finished
                        }
                        else
                        {
                            break; // if it is not an enemy then we want to end the loop NOW!
                        }
                    }

                    // createMove and Add to list 
                    moves.Add(new Move(position, tempPos));


                    // for all directions. First 4, up down left right.  
                    if (depthIs1)
                        break;
                }
            }

            return moves;
        }

        private List<Move> genDiagMoves(byte position, bool depthIs1)
        {
            List<Move> moves = new List<Move>();
            moreMoves = true;
            for (dirs = 4; dirs < 8; dirs++)
            {
                tempPos = position;
                moreMoves = true;

                while (moreMoves)
                {
                    // this adds the offset 
                    tempPos = (byte)(tempPos + X88Dirs[dirs]);

                    // IS OUT OF BOUNDS OF BOARD 
                    if (!(Board.IsIndexValid(tempPos)))
                        break;

                    // IS Field Occupied , and if it is : is it occupied by myself or enemy? if enemy end loop after finish, else end now. 
                    if (currentState.State.IsFieldOccupied(tempPos))
                    {
                        // is Blocked
                        // is Enemy Piece ?? 
                        if (isWhite != currentState.State.isFieldOwnedByWhite(tempPos))
                        {
                            moreMoves = false; // if it is an enemy we want to end the loop after it is finished
                        }
                        else
                        {
                            break; // if it is not an enemy then we want to end the loop NOW!
                        }
                    }

                    // createMove and Add to list 
                    moves.Add(new Move(position, tempPos));

                    // for all directions. First 4, up down left right.  
                    if (depthIs1)
                        break;
                }
            }

            return moves;
        }

        // source https://learn.inside.dtu.dk/d2l/le/content/80615/viewContent/284028/View
        sbyte[] horseMoves =
        {
            0x21, // two up one right
            0x1F, // two up one left
            0x12, // one up two right
            0x0E, // one up two left
            -0x21, // two down one left
            -0x1F, // two down one right
            -0x12, // one down two left
            -0x0E
        };

        private List<Move> genHorseMoves(byte position, bool depthIs1)
        {
            List<Move> moves = new List<Move>();
            for (dirs = 0; dirs < horseMoves.Length; dirs++)
            {
                // this adds the offset 
                tempPos = (byte)(tempPos + horseMoves[dirs]);

                if (false)
                {
                    // IS OUT OF BOUNDS OF BOARD 

                    continue;
                }

                if (false)
                {
                    // is blocked by self ?? 

                    continue;
                }


                // createMove and Add to list 
                moves.Add(new Move(position, tempPos));
            }

            return moves;
        }
    }
}