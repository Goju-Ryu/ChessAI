using System.Collections.Generic;

namespace ChessAI {

    public enum DirectionIndex {
        Up = 0, Down = 1, Left = 2, Right = 3,
        UpLeft = 4, DownRight=5, UpRight=7, DownLeft=8
    };

    // Base Definitions and Setups 
    public abstract partial class AMoveGenerator {


        public static readonly sbyte MaxFields = 64;
        public static readonly sbyte FieldsPrSide = 8;
        public static readonly sbyte[] DirOffsets  = { 8, -8, -1, 1, 7, -7, 9, -9 };



        private partial void Init();
        private partial void Init_moves();
        public AMoveGenerator() {
            Init();
            Init_moves();
        }

        private partial sbyte[ ] GetDirections(sbyte index);
        private partial sbyte GetFieldsToEdge(int position , int direction);
    }
    // Direction Board Implementation 
    public abstract partial class AMoveGenerator {

        sbyte[][] directionBoard;
        private partial void Init() {
            directionBoard = new sbyte[ 64 ][ ];
            for(sbyte i = 0 ; i < 64 ; i++) {
                // Stores information about how many rows there are up, down, left, right.
                directionBoard[ i ] = GetDirections(i);
            }
        }
        private partial sbyte[ ] GetDirections(sbyte index) {

            int GetUp(int v) {
                return v / FieldsPrSide;
            }
            int GetDown(int v , int up) {
                return FieldsPrSide-1 - up;
            }
            int GetLeft(int v , int up) {
                return v - (up * FieldsPrSide);
            }
            int GetRight(int v , int left) {
                return FieldsPrSide-1 - left;
            }

            sbyte[] dirs = new sbyte[8];
            int up, down, left, right;

            up      = GetUp(index);
            down    = GetDown(index , up);
            left    = GetLeft(index , up);
            right   = GetRight(index , left);

            dirs[ (int)DirectionIndex.Up ] = (sbyte)up;
            dirs[ (int)DirectionIndex.Down ] = (sbyte)down;
            dirs[ (int)DirectionIndex.Left ] = (sbyte)left;
            dirs[ (int)DirectionIndex.Right ] = (sbyte)right;

            //dirs[ (int)DirectionIndex.UpRight	] = (sbyte)(up   > right ? up	: right);
            //dirs[ (int)DirectionIndex.DownRight ] = (sbyte)(down > right ? down	: right);
            //dirs[ (int)DirectionIndex.UpLeft	] = (sbyte)(up   > left	? up	: left);
            //dirs[ (int)DirectionIndex.DownRight ] = (sbyte)(down > right ? down	: right);

            return null;
        }
        private partial sbyte GetFieldsToEdge(int position , int direction) {
            return (sbyte)directionBoard[ position ][ direction ];
        }
    }

    // Moves Implementation 
    public abstract partial class AMoveGenerator {

        private partial void Init_moves() {

        }


                  
        //public enum DirectionIndex {
        //Up = 0, Down = 1, Left = 2, Right = 3,
        //UpLeft = 4, DownRight=5, UpRight=7, DownLeft=8};

        // this is made to fit this Direction Index. 

        public static readonly byte[] X88Dirs  = { 
             0x10,  // 1 up     0 x
            -0x10,  // 1 down   0 x
             0x01,  // 0 x      1 right
            -0x01,  // 0 x      1 left
             0x0e,  // 1 up     1 left
            -0x11,  // 1 down   1 right 
             0x11,  // 1 up     1 right
            -0x0e   // 1 down   1 left 
        };

        private bool king , queen ;
        public Move GenerateMoves(Pieces piece, byte position) {

            king = ( piece == Pieces.King );
            queen = (piece == Pieces.Queen);
        
            List<Move> moves = new List<Move>();
            if( king || queen || piece == Pieces.Rook ){
                // i add king to this mehtod, because it is the only piece that has a limit of 1 distance, since "king" is a bool, i pass it as "depthIs1"
                moves.AddRange( genLineMoves(position, king) );
            }   

            if( king || queen  || piece == Pieces.Bishop ){
                 // COPY OF PREV COMMENT :::  i add king to this mehtod, because it is the only piece that has a limit of 1 distance, since "king" is a bool, i pass it as "depthIs1"
                moves.AddRange( genDiagMoves(position, king) );
            }

            return null;
        }

        bool moreMoves;
        byte dirs;
        byte tempPos;

        private List<Move> genLineMoves(byte position, bool depthIs1 ){
            List<Move> moves = new List<Move>();
            moreMoves = true;
            for(dirs = 0 ; dirs < 4 ; dirs++) {

                tempPos = position;
                
                while(moreMoves){
                    // this adds the offset 
                    tempPos += X88Dirs[ dirs ];

                    if(false) // IS OUT OF BOUNDS OF BOARD 
                        break;

                    // createMove and Add to list 
                    moves.Add( new Move( position, tempPos ) ) ;


                // for all directions. First 4, up down left right.  
                if(depthIs1)
                    break;
                }
            }
           
            return null;
        }
        private List<Move> genDiagMoves(byte position, bool depthIs1 ){
            List<Move> moves = new List<Move>();
            moreMoves = true;
            for(dirs = 4 ; dirs < 8 ; dirs++) {

                tempPos = position;
                
                while(moreMoves){
                    // this adds the offset 
                    tempPos += X88Dirs[ dirs ];

                    if(false) // IS OUT OF BOUNDS OF BOARD 
                        break;

                    if(false) // is blocked by self ?? 

                    // createMove and Add to list 
                    moves.Add( new Move( position, tempPos ) ) ;


                // for all directions. First 4, up down left right.  
                if(depthIs1)
                    break;
                }
            }
            return null;
        }
        
    }
    /*struct MoveOffset {



        public static readonly sbyte[] offsets;
        public MoveOffset(
            sbyte up = 0 , sbyte down = 0 , sbyte left = 0 , sbyte right = 0 ,
            sbyte upLeft = 0 , sbyte upRight = 0 , sbyte downLeft = 0 , sbyte downRight = 0
        ) {
            sbyte[] offs = new sbyte[8];
            offs[ (int)DirectionIndex.Up ]     = up;
            offs[ (int)DirectionIndex.Down ]   = down;
            offs[ (int)DirectionIndex.Left ]   = left;
            offs[ (int)DirectionIndex.Right ]  = right;


            offs[ (int)DirectionIndex.UpRight ]     = upRight;
            offs[ (int)DirectionIndex.DownRight ]   = downRight;
            offs[ (int)DirectionIndex.UpLeft ]      = upLeft;
            offs[ (int)DirectionIndex.DownLeft ]    = downLeft;

        }
    }*/


}
