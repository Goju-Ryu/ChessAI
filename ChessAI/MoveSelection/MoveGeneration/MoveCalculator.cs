using System.Collections.Generic;
using ChessAI.DataClasses;
using System;


namespace ChessAI.MoveSelection.MoveGeneration
{
    // Moves Implementation 
    public partial class MoveCalculator : IMoveCalculator
    {
        public List<Move> CalculatePossibleMoves(GameState state, bool calculateForWhite)
        {
            List<Move> moves = new List<Move>();


            PieceList pieces = calculateForWhite ? state.WhitePieces : state.BlackPieces;  
            //Piece[] pieces = state.State.Fields.ToArray();

            for (var index = 0; index < pieces.Length; index++)
            {
                Piece piece = pieces[index];
                moves.AddRange(CalcMovesForPiece(state, piece));
            }

            return moves;
        }

        public List<Move> CalcMovesForPiece(GameState state,Piece piece )
        {
            bool king  = (piece == Piece.King); // Also used as single distance boolean
            bool queen = (piece == Piece.Queen);

            List<Move> moves = new List<Move>();
            if(  piece.PieceType == 0 ){
                return moves;
            }

            if (king || queen || piece.PieceType == Piece.Rook)
            {
                //Console.WriteLine("LINES  ");
                moves.AddRange(GenLineMoves( state , piece , king ));
            }

            if (king || queen || piece.PieceType == Piece.Bishop)
            {
                //Console.WriteLine("DIAG   ");
                moves.AddRange(GenDiagMoves( state , piece , king ));
            }

            if( piece.PieceType == Piece.Knight){
                //Console.WriteLine("KNIGHT ");
                moves.AddRange(genHorseMoves(state, piece));
            }

            if( piece.PieceType == Piece.Pawn){
//                Console.WriteLine("PAWN   ");
                moves.AddRange(genPawnMoves(state, piece));
            }

            return moves;
        }

        private List<Move> GenLineMoves(GameState state , Piece piece , bool depthIs1)
        {
            bool moreMoves = true;
            byte tempPos;
            Direction dir;

            List<Move> moves = new List<Move>();
            
            // Create a function that returns the correct offsets to use for the given color
            Func<Direction, sbyte> directionGetter = piece.IsWhite ? Board.WhiteDirection : Board.BlackDirection;

            for (byte dirs = 0; dirs < 4; dirs++)
            {
                tempPos = piece.Position;
                moreMoves = true;
                dir = (Direction)dirs;

                while (moreMoves)
                {
                    
                    // this adds the offset 
                    tempPos = (byte)(tempPos + directionGetter(dir));
                    if (!(Board.IsIndexValid(tempPos)))
                    {
                        break;
                    }

                    // IS Field Occupied , and if it is : is it occupied by myself or enemy? if enemy end loop after finish, else end now. 
                    if (state.State.IsFieldOccupied(tempPos))
                    {
                        if ( piece.IsWhite != state.State.IsFieldOwnedByWhite(tempPos) )
                        {
                            moreMoves = false; // if it is an enemy we want to end the loop after it is finished
                        }
                        else
                        {
                            break; // if it is not an enemy then we want to end the loop NOW!
                        }
                    }

                    // createMove and Add to list 
                    moves.Add( Move.CreateSimpleMove(piece.Position, tempPos, state));

                    // for all directions. First 4, up down left right.  
                    if (depthIs1)
                        break;
                }
            }
            
            return moves;
        }
        private List<Move> GenDiagMoves( GameState state , Piece piece , bool depthIs1)
        {

            bool moreMoves = true;
            byte tempPos;

            List<Move> moves = new List<Move>();
            
            // Create a function that returns the correct offsets to use for the given color
            Func<Direction, sbyte> directionGetter = piece.IsWhite ? Board.WhiteDirection : Board.BlackDirection;

            // Create an array of all the offsets for moving diagonally
            sbyte[] dirs = new[]
            {
                (sbyte)(directionGetter(Direction.Up) + directionGetter(Direction.Left)),
                (sbyte)(directionGetter(Direction.Up) + directionGetter(Direction.Right)),
                (sbyte)(directionGetter(Direction.Down) + directionGetter(Direction.Left)),
                (sbyte)(directionGetter(Direction.Down) + directionGetter(Direction.Right)),
            };
                
            foreach (sbyte dir in dirs)
            {
                tempPos = piece.Position;
                moreMoves = true;

                while (moreMoves)
                {
                    // this adds the offset 
                    tempPos = (byte)(tempPos + dir);
                    if (!(Board.IsIndexValid(tempPos)))
                    {
                        break;
                    }

                    // IS Field Occupied , and if it is : is it occupied by myself or enemy? if enemy end loop after finish, else end now. 
                    if (state.State.IsFieldOccupied(tempPos))
                    {
                        if ( piece.IsWhite != state.State.IsFieldOwnedByWhite(tempPos) )
                        {
                            moreMoves = false; // if it is an enemy we want to end the loop after it is finished
                        }
                        else
                        {
                            break; // if it is not an enemy then we want to end the loop NOW!
                        }
                    }

                    // createMove and Add to list 
                    moves.Add( Move.CreateSimpleMove(piece.Position, tempPos, state));

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
                        if ( piece.IsWhite == state.State.IsFieldOwnedByWhite(tempPos) )
                        {
                               valid = false;// if it is an enemy we want to end the loop after it is finished
                        }
                    }
                    
                if(valid)
                    moves.Add( Move.CreateSimpleMove(piece.Position, tempPos, state));
            }

            return moves;
        
        }
        
        // TODO REMOVE HARD CODING. 
        private static byte[,] PawnsOrigPosition = new byte[,]{
            {0x10 ,0x11 ,0x12 ,0x13 ,0x14 ,0x15 ,0x16 ,0x17},
            {0x60 ,0x61 ,0x62 ,0x63 ,0x64 ,0x65 ,0x66 ,0x67}
        };        

        private List<Move> genPawnMoves(GameState state , Piece piece){
            
            List<Move> moves = new List<Move>();            
            bool isLowIndexed   = Board.isDIrectionPositive(piece);
            int  startIndex     = isLowIndexed ? 0x10: 0x60  ; // START INDEX IS LOW IF WHITE 

            int a               = piece.Position - startIndex ;
            bool StartPosition  = (a >= 0 && a < 0x08)? true: false;
            
            int posDoubleDelta  = isLowIndexed ? 0x20 : -0x20;
            int[] posDelta      = isLowIndexed ? new int[]{ 0x10 , 0x11 , (0x10 - 0x01) } : new int[]{ - 0x10 , - 0x11 , - (0x10 - 0x01) };
            int[] moveArr       = StartPosition ? new int[]{ // if start position then the length of possible places should be 4
                piece.Position + posDelta[0],
                piece.Position + posDelta[1],
                piece.Position + posDelta[2],
                piece.Position + posDoubleDelta,
            }:
            new int[]{   // if not the first move, then possible moves are just 3. 
                piece.Position + posDelta[0],
                piece.Position + posDelta[1],
                piece.Position + posDelta[2]
            };   
            
            void ValidatePosition(byte pos , Piece piece, GameState state)
            {
                var board = state.State;
                // diagonal Move
                if(  Math.Abs ( pos - piece.Position ) % 0x10 == 0 ){
                    // STRAIGHT LINE 
                    if(    !board.IsFieldOccupied(pos)  )
                        moves.Add(     Move.CreateSimpleMove(piece.Position,pos,state)     ); // ONLY IF ISENT OCCUPIED
                }else{
                    // DIAGONAL LINE 
                    if(     board.IsFieldOccupied(pos) && board.IsFieldOwnedByWhite(pos) && !piece.IsWhite  )// ONLY IF DIAGONAL IS OCCUPIED BY ENEMY
                        moves.Add(     Move.CreateSimpleMove(piece.Position,pos, state)     );
                }                
            }

            for (int i = 0; i < moveArr.Length; i++)
            {
                // TODO change to Board Max FIeld OR SOMETHING
                if((byte) moveArr[i] == 255 ){ 
                    break;
                }

                ValidatePosition( (byte) moveArr[i],   piece,  state);
            }

            return moves ; 
        }

        
    }

    public partial class MoveCalculator : IMoveCalculator
    {

        // A list of Functions, Functions take a Gamestate, and a Piece, and returns a Fcuntion that returns a gmaestate
        //static List< Func< GameState , Piece, Func<GameState?> >> specialRule = new List<Func<GameState, Piece, Func<GameState?>>>();
        
/*
        GameState? enPassant(GameState state, Piece piece){

            bool isPos = Board.isDIrectionPositive(piece);
            (int, int) offsets = isPos ? (0x11, 0x10 - 0x01) : ( - 0x11, -(0x10 - 0x01));

            // LEFT;
            if( Board.IsIndexValid(  (byte)(piece.Position + offsets.Item1)     )) // IS THE DIAGONAL FORWRD LEFT A VALID INDEX  
                if( state.State[piece.Position - 1].PieceType == Piece.Pawn &&     state.State[piece.Position - 1].IsWhite != piece.IsWhite )      // IS THE SPACE LEFT OF PIECE OCCUPIED BY AN ENEMY PAWN?  
                    // TODO CREATE MOVE OR GAME STATE

            // LEFT;
            if( Board.IsIndexValid(  (byte)(piece.Position + offsets.Item1)     )) // IS THE DIAGONAL FORWRD LEFT A VALID INDEX  
                if( state.State[piece.Position + 1].PieceType == Piece.Pawn &&     state.State[piece.Position - 1].IsWhite != piece.IsWhite )      // IS THE SPACE LEFT OF PIECE OCCUPIED BY AN ENEMY PAWN?  
                    // TODO CREATE MOVE OR GAME STATE
       
            // RETURN SOMETHING?!
        }
        */


        // MAKE CASTLE MOVES FOR TOWERS AND ROYALS. s
        List<Move> CastleSide(GameState state, Piece piece){
            
            // in the corners 
            List<Move> moves = new List<Move>();

            bool isTower = ( piece.PieceType == Piece.Rook );           // BREAK IF NOT- POSSIBLE 
            bool isKing =  ( piece.PieceType == Piece.King );
            if (    !( isTower || isKing)     )               
                return null;
            
            bool isPos = Board.isDIrectionPositive(piece);              // TO CHECK FOR CASTLE MOVES WE MUST KNOW WICH SIDE OF THE BOARD TO LOOK FOR PIECES 
            int left   = isPos? 0x00 : Board.PositionConverter(0x00);   // the moveis only possible if the tower and king has not been moved, so assuming starting posionts    
            int right  = isPos? 0x07 : Board.PositionConverter(0x07); 
            int kingPosition = isPos ? 0x04: Board.PositionConverter(0x04);

            if(state.State[kingPosition].hasMoved)
                return moves;
            

            bool LEFT_POSSIBLE = ( state.State[left].hasMoved  );
            bool RIGHT_POSSIBLE = ( state.State[right].hasMoved  );

            int RookPos ;
            int RoyalPos;
            
            // CHECKING LEFT STATICLY 
            if(LEFT_POSSIBLE)
                LEFT_POSSIBLE = 
                (state.State[left + 1].PieceType == Piece.Empty) && 
                (state.State[left + 2].PieceType == Piece.Empty) && 
                (state.State[left + 3].PieceType == Piece.Empty) ;
            if(LEFT_POSSIBLE)
            {
                RookPos = isPos ?  0x03 : Board.PositionConverter(0x03) ;
                RoyalPos= isPos ?  0x02 : Board.PositionConverter(0x02) ;
                if( !(state.State[left].hasMoved || state.State[kingPosition].hasMoved) ){
                // TODO create Move CASTLE 
                    /*moves.Add( Move.CreateMoveTwoPieces(   
                        (byte)RookPos,(byte)RoyalPos,
                        state.State[left], state.State[kingPosition], 
                        state ) );*/
                }
            }

            // CHECKING RIGHT STATICALLY 
            if(RIGHT_POSSIBLE)
                RIGHT_POSSIBLE = 
                (state.State[right - 1].PieceType == Piece.Empty) && 
                (state.State[right - 2].PieceType == Piece.Empty) ;
            if(RIGHT_POSSIBLE)
            {
                RookPos = isPos ?  0x05 : Board.PositionConverter(0x05) ;
                RoyalPos= isPos ?  0x06 : Board.PositionConverter(0x06) ;
                if( !(state.State[right].hasMoved || state.State[0x04].hasMoved) ){
                    // TODO create Move CASTLE 
                    /*moves.Add( Move.CreateMoveTwoPieces(
                        (byte)RookPos,(byte)RoyalPos,
                        state.State[left], state.State[kingPosition],
                        state ) );*/
                        }
            }
                

            return moves;
        }
        /*
        GameState? PawnPromotion(GameState state, Piece piece){

            bool isPos = Board.isDIrectionPositive(piece);
            (int, int) offsets = isPos ? (0x11, 0x10 - 0x01) : ( - 0x11, -(0x10 - 0x01));

            // LEFT;
            if( Board.IsIndexValid(  (byte)(piece.Position + offsets.Item1)     )) // IS THE DIAGONAL FORWRD LEFT A VALID INDEX  
                if( state.State[piece.Position - 1].PieceType == Piece.Pawn &&     state.State[piece.Position - 1].IsWhite != piece.IsWhite )      // IS THE SPACE LEFT OF PIECE OCCUPIED BY AN ENEMY PAWN?  
                    // TODO CREATE MOVE OR GAME STATE

            // LEFT;
            if( Board.IsIndexValid(  (byte)(piece.Position + offsets.Item1)     )) // IS THE DIAGONAL FORWRD LEFT A VALID INDEX  
                if( state.State[piece.Position + 1].PieceType == Piece.Pawn &&     state.State[piece.Position - 1].IsWhite != piece.IsWhite )      // IS THE SPACE LEFT OF PIECE OCCUPIED BY AN ENEMY PAWN?  
                    // TODO CREATE MOVE OR GAME STATE

        }*/
    }
}