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
                List<Move> moves = MC.CalculatePossibleMoves(state, true);

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

                List<Move> moves = MC.CalculatePossibleMoves(state, true);

                Console.Write(board + "\n\n\n");
                Assert.AreEqual( exspectedNum , moves.Count );
            }

            List<byte[]> obstructions = new List<byte[]>();
            obstructions.Add( new byte[0]                          );
            obstructions.Add( new byte[1]{0x34}                    );
            obstructions.Add( new byte[2]{0x34 , 0x54}             );
            obstructions.Add( new byte[3]{0x34 , 0x54, 0x45}       );
            obstructions.Add( new byte[4]{0x34 , 0x54, 0x45, 0x43} );

                                        // 0  1    2  3  4
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

                List<Move> moves = MC.CalculatePossibleMoves(state, true);

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

                List<Move> moves = MC.CalculatePossibleMoves(state, true);

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

                List<Move> moves = MC.CalculatePossibleMoves(state, true);

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
        public void PawnMoves(){
            Assert.True(false);
        }

        
    }
}
