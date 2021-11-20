using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using ChessAI.DataClasses;
using ChessAI.MoveSelection.MoveGeneration;


namespace UnitTests.MoveSelection.MoveGeneration {
    public class MoveCalculaterTest {
        
        [Test]
        public void DiagonalMoves(){
            
            void testWithobstructions(byte[] l, byte OBScolor ,int exspectedNum){

                List<Piece> pieces = new List<Piece>();
                pieces.Add ( new Piece( Piece.Bishop ^ Piece.Black  , 0x44 ) );

                for (int i = 0; i < l.Length; i++)
                {
                    pieces.Add ( new Piece(Piece.PieceMask ^ OBScolor , l[i] )) ;
                }

                Board board = TestBuilder.GenerateBoard(pieces) ;
                GameState state = new GameState(board);
                MoveCalculator MC = new MoveCalculator();
                List<Move> moves = MC.CalculatePossibleMoves(state, false);

                Assert.AreEqual( exspectedNum,moves.Count );

            }

            List<byte[]> obstructions = new List<byte[]>();
            obstructions.Add( new byte[0]                          );
            obstructions.Add( new byte[1]{0x33}                    );
            obstructions.Add( new byte[2]{0x33 , 0x35}             );
            obstructions.Add( new byte[3]{0x33 , 0x35, 0x53}       );
            obstructions.Add( new byte[4]{0x33 , 0x35, 0x53, 0x55} );

            int[] exspectedMoves = { 13, 9 , 6, 3, 0 };
            int[] exspectedMovesENEMY = { 13, 10 , 8, 6, 4 };
            for (int i = 0; i < obstructions.Count; i++)
            {
                testWithobstructions( obstructions[i] , Piece.Black , exspectedMoves[i] );
                testWithobstructions( obstructions[i] , Piece.White , exspectedMovesENEMY[i] );
            }
        }
    
        [Test]
        public void LineMoves(){
            
            void testWithobstructions(byte[] l, byte OBScolor ,int exspectedNum){
                
                List<Piece> pieces = new List<Piece>();
                pieces.Add ( new Piece( Piece.Rook ^ Piece.Black  , 0x44 ) );

                for (int i = 0; i < l.Length; i++)
                {
                    Piece p = new Piece( Piece.PieceMask ^ OBScolor , l[i] );
                    pieces.Add ( p ) ;
                    Console.WriteLine( p.PieceFlags.ToString("X4") );
                }

                Board board = TestBuilder.GenerateBoard(pieces) ;
                GameState state = new GameState(board);
                MoveCalculator MC = new MoveCalculator();

                List<Move> moves = MC.CalculatePossibleMoves(state, false);

                Console.Write(board + "\n\n\n");
                Assert.AreEqual( exspectedNum , moves.Count );
            }

            List<byte[]> obstructions = new List<byte[]>();
            obstructions.Add( new byte[0]                          );
            obstructions.Add( new byte[1]{0x34}                    );
            obstructions.Add( new byte[2]{0x34 , 0x54}             );
            obstructions.Add( new byte[3]{0x34 , 0x54, 0x45}       );
            obstructions.Add( new byte[4]{0x34 , 0x54, 0x45, 0x43} );

                                          // 0      1       2       3       4
            int[] exspectedMoves =      {   14,     10 ,    7,      4,      0 };
            int[] exspectedMovesENEMY = {   14,     11 ,    9,      7,      4 };

            for (int i = 0; i < obstructions.Count; i++)
            {
                testWithobstructions( obstructions[i] , Piece.Black , exspectedMoves[i]      );
                testWithobstructions( obstructions[i] , Piece.White , exspectedMovesENEMY[i] );
            }
          
        }

        [Test]
        public void HorseMoves(){
            void testWithobstructions(byte[] l, byte OBScolor ,int exspectedNum){
                
                List<Piece> pieces = new List<Piece>();
                pieces.Add ( new Piece( Piece.Knight ^ Piece.Black  , 0x44 ) );

                for (int i = 0; i < l.Length; i++)
                {
                    Piece p = new Piece( Piece.PieceMask ^ OBScolor , l[i] );
                    pieces.Add ( p ) ;
                }

                Board board = TestBuilder.GenerateBoard(pieces) ;
                GameState state = new GameState(board);
                MoveCalculator MC = new MoveCalculator();

                List<Move> moves = MC.CalculatePossibleMoves(state, false);

                Assert.AreEqual( exspectedNum , moves.Count );
            }

            List<byte[]> obs = new List<byte[]>();
            obs.Add( new byte[0]                                                               );
            obs.Add( new byte[]{    0x63                                                       } );
            obs.Add( new byte[]{    0x63 , 0x65                                                } );
            obs.Add( new byte[]{    0x63 , 0x65,    0x56                                       } );
            obs.Add( new byte[]{    0x63 , 0x65,    0x56, 0x36,                                } );
            obs.Add( new byte[]{    0x63 , 0x65,    0x56, 0x36,     0x25                       } );
            obs.Add( new byte[]{    0x63 , 0x65,    0x56, 0x36,     0x25, 0x23                 } );
            obs.Add( new byte[]{    0x63 , 0x65,    0x56, 0x36,     0x25, 0x23,     0x32,      } );
            obs.Add( new byte[]{    0x63 , 0x65,    0x56, 0x36,     0x25, 0x23,     0x32, 0x52 } );

            for (int i = 0; i < obs.Count; i++)
            {
                Console.WriteLine(8 - i);
                testWithobstructions( obs[i] , Piece.Black , (8 - i) );
                testWithobstructions( obs[i] , Piece.White ,  8      );
            }

        }
        
        [Test]
        public void QueenMoves(){
            
            void testWithobstructions(byte[] l, byte OBScolor ,int exspectedNum){
                
                List<Piece> pieces = new List<Piece>();
                pieces.Add ( new Piece( Piece.Queen ^ Piece.Black  , 0x00 ) );

                for (int i = 0; i < l.Length; i++)
                {
                    Piece p = new Piece( Piece.PieceMask ^ OBScolor , l[i] );
                    pieces.Add ( p ) ;
                    Console.WriteLine( p.PieceFlags.ToString("X4") );
                }

                Board board = TestBuilder.GenerateBoard(pieces);
                GameState state = new GameState(board);
                MoveCalculator MC = new MoveCalculator();

                List<Move> moves = MC.CalculatePossibleMoves(state, false);

                Console.Write(board + "\n\n\n");
                Assert.AreEqual( exspectedNum , moves.Count );
            }

            List<byte[]> obstructions = new List<byte[]>();
            obstructions.Add( new byte[0]                          );
            obstructions.Add( new byte[1]{0x30}                    );
            obstructions.Add( new byte[2]{0x30 , 0x55}             );
            obstructions.Add( new byte[3]{0x30 , 0x55, 0x03}       );

            int[] exspectedMoves =      {   21,     2 + 7 + 7  ,    7 + 2 + 4 ,      2 + 4 + 2 };
            

            for (int i = 0; i < obstructions.Count; i++)
            {
                Console.WriteLine("ITERATION" + i);
                testWithobstructions( obstructions[i] , Piece.Black , exspectedMoves[i]     );
                 Console.WriteLine("ITERATION" + i);
                testWithobstructions( obstructions[i] , Piece.White , exspectedMoves[i] + i );
            }
          
        }

        [Test]
        public void KingMoves(){
            
            void testWithobstructions(byte[] l, byte OBScolor ,int exspectedNum){
                
                List<Piece> pieces = new List<Piece>();
                pieces.Add ( new Piece( Piece.King ^ Piece.Black  , 0x44 ) );

                for (int i = 0; i < l.Length; i++)
                {
                    Piece p = new Piece( Piece.PieceMask ^ OBScolor , l[i] );
                    pieces.Add ( p ) ;
                    Console.WriteLine( p.PieceFlags.ToString("X4") );
                }

                Board board = TestBuilder.GenerateBoard(pieces);
                GameState state = new GameState(board);
                MoveCalculator MC = new MoveCalculator();

                List<Move> moves = MC.CalculatePossibleMoves(state, false);

                Console.Write(board + "\n\n\n");
                Assert.AreEqual( exspectedNum , moves.Count );
            }

            List<byte[]> obstructions = new List<byte[]>();
            obstructions.Add( new byte[0]                          );
            obstructions.Add( new byte[]{ 0x53                                                   });
            obstructions.Add( new byte[]{ 0x53 , 0x43                                            });
            obstructions.Add( new byte[]{ 0x53 , 0x43 , 0x33                                     });
            obstructions.Add( new byte[]{ 0x53 , 0x43 , 0x33 , 0x34                              });
            obstructions.Add( new byte[]{ 0x53 , 0x43 , 0x33 , 0x34 , 0x54                       });
            obstructions.Add( new byte[]{ 0x53 , 0x43 , 0x33 , 0x34 , 0x54, 0x55                 });
            obstructions.Add( new byte[]{ 0x53 , 0x43 , 0x33 , 0x34 , 0x54, 0x55 , 0x45          });
            obstructions.Add( new byte[]{ 0x53 , 0x43 , 0x33 , 0x34 , 0x54, 0x55 , 0x45 , 0x35   });

            int[] exspectedMoves =      { 8 , 7 , 6 ,5 ,4 ,3, 2, 1 , 0};
            

            for (int i = 0; i < obstructions.Count; i++)
            {
                testWithobstructions( obstructions[i] , Piece.Black , exspectedMoves[i]     );
                testWithobstructions( obstructions[i] , Piece.White , exspectedMoves[i] + i );
            }
          
        }

        [Test]
        public void PawnMoves_01 (){

            List<Piece> pieces = new List<Piece>();
            Board board;
            GameState state;
            MoveCalculator MC;
            List<Move> moves ;

            void test(byte color , int exspected, byte Row , bool isWhite){
                pieces.Add ( new Piece( Piece.Pawn ^ color, Row + 0x00 ) );
                pieces.Add ( new Piece( Piece.Pawn ^ color, Row + 0x01 ) );
                pieces.Add ( new Piece( Piece.Pawn ^ color, Row + 0x02 ) );
                pieces.Add ( new Piece( Piece.Pawn ^ color, Row + 0x03 ) );
                pieces.Add ( new Piece( Piece.Pawn ^ color, Row + 0x04 ) );
                pieces.Add ( new Piece( Piece.Pawn ^ color, Row + 0x05 ) );
                pieces.Add ( new Piece( Piece.Pawn ^ color, Row + 0x06 ) );
                pieces.Add ( new Piece( Piece.Pawn ^ color, Row + 0x07 ) );
                
                board = TestBuilder.GenerateBoard(pieces);
                state = new GameState(board);

                MC = new MoveCalculator();
                moves = MC.CalculatePossibleMoves(state, isWhite);

                Console.WriteLine(board);
                Assert.AreEqual(exspected,moves.Count);
            }

            Console.WriteLine("TEST 1");
            test(Piece.White, 16, 0x10, true);
            Console.WriteLine("TEST 2");
            test(Piece.Black, 8 , 0x10, false);

            pieces.Clear();

            Console.WriteLine("TEST 3");
            test(Piece.Black, 16, 0x60, false);
            Console.WriteLine("TEST 4");
            test(Piece.White, 8 , 0x60, true);
        }

        [Test]
        public void PawnMoves_02 (){

            List<Piece> pieces = new List<Piece>();
            Board board;
            GameState state;
            MoveCalculator MC;
            List<Move> moves ;

            pieces.Add ( new Piece( Piece.Pawn ^ Piece.Black, 0x43 ) );
            pieces.Add ( new Piece( Piece.Pawn ^ Piece.White, 0x34 ) );
            
            board = TestBuilder.GenerateBoard(pieces);
            state = new GameState(board);

            MC = new MoveCalculator();

            moves = MC.CalculatePossibleMoves(state, false);
            moves.AddRange( MC.CalculatePossibleMoves(state, true) );
            //Assert.AreEqual(2,moves.Count);


            Console.WriteLine(board);
       

        }

        [Test]
        public void testConvertion(){
            
            List<(int,int)> convertions = new List<(int, int)>();
            int Row1 = 0x00; 
            int Row2 = 0x70;
            int RowOffset = 0x10;

            for (int i = 0; i < 7; i++)
            {
                Row1 += RowOffset; Row2 -= RowOffset;
                convertions.Add( ( Row1 + 0, Row2 + 7 ) );
                convertions.Add( ( Row1 + 1, Row2 + 6 ) );
                convertions.Add( ( Row1 + 2, Row2 + 5 ) );
                convertions.Add( ( Row1 + 3, Row2 + 4 ) );
                convertions.Add( ( Row1 + 4, Row2 + 3 ) );
                convertions.Add( ( Row1 + 5, Row2 + 2 ) );
                convertions.Add( ( Row1 + 6, Row2 + 1 ) );
                convertions.Add( ( Row1 + 7, Row2 + 0 ) );
            }
            
            convertions.Add( ( 0x11 , 0x66 ) );
            convertions.Add( ( 0x16 , 0x61 ) );

            testConverstions(convertions);
            void testConverstions(List<(int,int)> list){
            
                List<Piece> pieces = new List<Piece>();
                foreach ((int,int) I in list)
                {
                    CheckReversedPosition(I.Item1, I.Item2);
                    pieces.Add ( new Piece( Piece.Pawn ^ Piece.Black  , I.Item1                 ) );
                    pieces.Add ( new Piece( Piece.Pawn ^ Piece.White  , I.Item2                 ) );
                }

                //List<int> temp = new List<int>();
                void CheckReversedPosition( int pos , int ePos ){
                    int temp;
                    for (int q= 0; q < 2; q++)
                    {
                        temp = pos; pos = ePos; ePos = temp;
                        for (int i = 0; i < convertions.Count ; i++)
                        {
                            int CONV = Board.PositionConverter((byte)pos);
                            pieces.Add ( new Piece( Piece.Pawn ^ Piece.Black  , pos   ) );
                            pieces.Add ( new Piece( Piece.Pawn ^ Piece.White  , CONV                    ) );
                            Assert.AreEqual(  CONV  ,   ePos    );
                        }
                    }
                }

                Board board = TestBuilder.GenerateBoard(pieces);
                Console.WriteLine("BOARD \n" + board);

            }           
        }
    }
}
