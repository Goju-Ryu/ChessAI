using System.Collections.Generic;
using ChessAI.DataClasses;
using System;

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
    public class MoveCalculator : IMoveCalculator
    {
        //public enum DirectionIndex {
        //Up = 0, Down = 1, Left = 2, Right = 3,
        //UpLeft = 4, DownRight=5, UpRight=7, DownLeft=8};

        // this is made to fit this Direction Index. 

        public static readonly sbyte[] X80Dirs =
        {
            +0x10, // 1 up     0 x
            -0x10, // 1 down   0 x
            +0x01, // 0 x      1 right
            -0x01, // 0 x      1 left
            +0x11, // 1 up     1 left
            -0x11, // 1 down   1 right 
            +0x0f, // 1 up     1 right
            -0x0f  // 1 down   1 left 
        };

        public List<Move> CalculatePossibleMoves(GameState state, bool calculateForWhite)
        {
            List<Move> moves = new List<Move>();


            //Piece[] pieces = calculateForWhite ? state.WhitePieces : state.BlackPieces;  
            Piece[] pieces = state.State.Fields;

            foreach (Piece piece in pieces)
            {
                moves.AddRange(calcMovesForPiece(state,piece));
            }

            return moves;
        }

        public List<Move> calcMovesForPiece(GameState state,Piece piece)
        {
            bool king  = (piece == Piece.King); // Also used as single distance boolean
            bool queen = (piece == Piece.Queen);

            List<Move> moves = new List<Move>();

            if (king || queen || piece == Piece.Rook)
            {
                //Console.WriteLine("LINES  ");
                moves.AddRange(genLineMoves( state , piece , king ));
            }

            if (king || queen || piece == Piece.Bishop)
            {
                //Console.WriteLine("DIAG   ");
                moves.AddRange(genDiagMoves( state , piece , king ));
            }

            if( piece == Piece.Knight){
                //Console.WriteLine("KNIGHT ");
                moves.AddRange(genHorseMoves(state, piece));
            }

            if( piece == Piece.Pawn){
                //Console.WriteLine("PAWN   ");
                moves.AddRange(genHorseMoves(state, piece));
            }

            return moves;
        }

        private List<Move> genLineMoves(GameState state , Piece piece , bool depthIs1)
        {
            bool moreMoves = true;
            byte dirs;
            byte tempPos;

            List<Move> moves = new List<Move>();
            moreMoves = true;

            for (dirs = 0; dirs < 4; dirs++)
            {
                tempPos = piece.Position;
                moreMoves = true;

                while (moreMoves)
                {
                    // this adds the offset 
                    tempPos = (byte)(tempPos + X80Dirs[dirs]);
                    if (!(Board.IsIndexValid(tempPos)))
                    {
                        break;
                    }

                    // IS Field Occupied , and if it is : is it occupied by myself or enemy? if enemy end loop after finish, else end now. 
                    if (state.State.IsFieldOccupied(tempPos))
                    {
                        if ( piece.isWhite() != state.State.isFieldOwnedByWhite(tempPos) )
                        {
                            moreMoves = false; // if it is an enemy we want to end the loop after it is finished
                        }
                        else
                        {
                            break; // if it is not an enemy then we want to end the loop NOW!
                        }
                    }

                    // createMove and Add to list 
                    moves.Add(new Move(piece.Position, tempPos));

                    // for all directions. First 4, up down left right.  
                    if (depthIs1)
                        break;
                }
            }
            
            return moves;
        }
        private List<Move> genDiagMoves( GameState state , Piece piece , bool depthIs1)
        {

            bool moreMoves = true;
            byte dirs;
            byte tempPos;

            List<Move> moves = new List<Move>();
            moreMoves = true;

            for (dirs = 4; dirs < 8; dirs++)
            {
                tempPos = piece.Position;
                moreMoves = true;

                while (moreMoves)
                {
                    // this adds the offset 
                    tempPos = (byte)(tempPos + X80Dirs[dirs]);
                    if (!(Board.IsIndexValid(tempPos)))
                    {
                        break;
                    }

                    // IS Field Occupied , and if it is : is it occupied by myself or enemy? if enemy end loop after finish, else end now. 
                    if (state.State.IsFieldOccupied(tempPos))
                    {
                        if ( piece.isWhite() != state.State.isFieldOwnedByWhite(tempPos) )
                        {
                            moreMoves = false; // if it is an enemy we want to end the loop after it is finished
                        }
                        else
                        {
                            break; // if it is not an enemy then we want to end the loop NOW!
                        }
                    }

                    // createMove and Add to list 
                    moves.Add(new Move(piece.Position, tempPos));

                    // for all directions. First 4, up down left right.  
                    if (depthIs1)
                        break;
                }
            }

            return moves;
        }
        // source https://learn.inside.dtu.dk/d2l/le/content/80615/viewContent/284028/View
        static sbyte[] horseMoves ={
                 0x21,    // two up   one Left
                 0x1F,    // two up   one Right
                 0x12,    // one up   two right
                 0x0E,    // one up   two left
                -0x21,    // two down one left
                -0x1F,    // two down one right
                -0x12,    // one down two left
                -0x0E     // one down Two Right
        };
        private List<Move> genHorseMoves( GameState state , Piece piece  )
        {

            byte tempPos = piece.Position;
            bool valid;

            List<Move> moves = new List<Move>();
            for (int i = 0; i < horseMoves.Length; i++)
            {
                valid = true;
                tempPos = (byte)(piece.Position + horseMoves[i]);
                
                if( !Board.IsIndexValid(tempPos) )// isnot Valid
                    break;
            
                if (state.State.IsFieldOccupied(tempPos))
                    {
                        if ( piece.isWhite() == state.State.isFieldOwnedByWhite(tempPos) )
                        {
                               valid = false;// if it is an enemy we want to end the loop after it is finished
                        }
                    }
                    
                if(valid)
                    moves.Add(new Move(piece.Position, tempPos));
            }

            return moves;
        
        }
        
        // TODO REMOVE HARD CODING. 
        private static byte[] PawnsOrigPosition = new byte[]{
                0x10 ,
                0x11 ,
                0x12 ,
                0x13 ,
                0x14 ,
                0x15 ,
                0x16 ,
                0x17,
            
                0x60 ,
                0x61 ,
                0x62 ,
                0x63 ,
                0x64 ,
                0x65 ,
                0x66 ,
                0x67
        };


        private List<Move> genPawnMoves(GameState state , Piece piece){
            /*
            bool isForwaard = true;
            int startIndex = isForwaard ? 0x10 : 0x60 ;

            bool isStartPosition(bool isFwrd , int startI ){
                    if
            }

            return new List<Move>();*/
            return null;
        }
    }
}