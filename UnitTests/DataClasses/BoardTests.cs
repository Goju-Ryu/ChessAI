using ChessAI.DataClasses;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;


namespace UnitTests.DataClasses
{

    
        
    public class BoardTests
    {
        [Test]
        public void IsIndexValidTest()
        {
            for (byte i = 0; i < 0x10; i++)
            {
                for (byte j = 0; j < 0x10; j++)
                {
                    
                    var index = (byte) (( i * 0x10 ) + j); // converts i and j to a combined index that should be correct
                    if (i < 8 && j < 8)
                    {
                        Assert.IsTrue(Board.IsIndexValid(index));
                    }
                    else
                    {
                        Assert.IsFalse(Board.IsIndexValid(index));
                    }
                }
            }
        }

        [Test]
        public void LookUpTests(){
            
            Piece[] pieces = {
                new Piece(  Piece.Pawn   & Piece.Black   ,   (int)BT.ColA + (int)BT.Row2   ),
                new Piece(  Piece.Pawn   & Piece.Black   ,   (int)BT.ColB + (int)BT.Row2   ),
                new Piece(  Piece.Pawn   & Piece.Black   ,   (int)BT.ColC + (int)BT.Row2   ),
                new Piece(  Piece.Pawn   & Piece.Black   ,   (int)BT.ColD + (int)BT.Row2   ),
                new Piece(  Piece.Pawn   & Piece.Black   ,   (int)BT.ColE + (int)BT.Row2   ),
                new Piece(  Piece.Pawn   & Piece.Black   ,   (int)BT.ColF + (int)BT.Row2   ),
                new Piece(  Piece.Pawn   & Piece.Black   ,   (int)BT.ColG + (int)BT.Row2   ),
                new Piece(  Piece.Pawn   & Piece.Black   ,   (int)BT.ColH + (int)BT.Row2   ),
                                                             
                new Piece(  Piece.Rook   & Piece.Black   ,   (int)BT.ColA + (int)BT.Row1   ),
                new Piece(  Piece.Knight & Piece.Black   ,   (int)BT.ColB + (int)BT.Row1   ),
                new Piece(  Piece.Bishop & Piece.Black   ,   (int)BT.ColC + (int)BT.Row1   ),
                new Piece(  Piece.Queen  & Piece.Black   ,   (int)BT.ColD + (int)BT.Row1   ),
                new Piece(  Piece.King   & Piece.Black   ,   (int)BT.ColE + (int)BT.Row1   ),
                new Piece(  Piece.Bishop & Piece.Black   ,   (int)BT.ColF + (int)BT.Row1   ),
                new Piece(  Piece.Knight & Piece.Black   ,   (int)BT.ColG + (int)BT.Row1   ),
                new Piece(  Piece.Rook   & Piece.Black   ,   (int)BT.ColH + (int)BT.Row1   ),
                                                             
                new Piece(  Piece.Pawn   & Piece.White   ,   (int)BT.ColA + (int)BT.Row7   ),
                new Piece(  Piece.Pawn   & Piece.White   ,   (int)BT.ColB + (int)BT.Row7   ),
                new Piece(  Piece.Pawn   & Piece.White   ,   (int)BT.ColC + (int)BT.Row7   ),
                new Piece(  Piece.Pawn   & Piece.White   ,   (int)BT.ColD + (int)BT.Row7   ),
                new Piece(  Piece.Pawn   & Piece.White   ,   (int)BT.ColE + (int)BT.Row7   ),
                new Piece(  Piece.Pawn   & Piece.White   ,   (int)BT.ColF + (int)BT.Row7   ),
                new Piece(  Piece.Pawn   & Piece.White   ,   (int)BT.ColG + (int)BT.Row7   ),
                new Piece(  Piece.Pawn   & Piece.White   ,   (int)BT.ColH + (int)BT.Row7   ),
                                                             
                new Piece(  Piece.Rook   & Piece.White   ,   (int)BT.ColA + (int)BT.Row8   ),
                new Piece(  Piece.Knight & Piece.White   ,   (int)BT.ColB + (int)BT.Row8   ),
                new Piece(  Piece.Bishop & Piece.White   ,   (int)BT.ColC + (int)BT.Row8   ),
                new Piece(  Piece.King   & Piece.White   ,   (int)BT.ColD + (int)BT.Row8   ),
                new Piece(  Piece.Queen  & Piece.White   ,   (int)BT.ColE + (int)BT.Row8   ),
                new Piece(  Piece.Bishop & Piece.White   ,   (int)BT.ColF + (int)BT.Row8   ),
                new Piece(  Piece.Knight & Piece.White   ,   (int)BT.ColG + (int)BT.Row8   ),
                new Piece(  Piece.Rook   & Piece.White   ,   (int)BT.ColH + (int)BT.Row8   )
            };
            
            List<int> list = new List<int>(); 
            foreach(Piece q in pieces)
                list.Add(q.Position);

            list.Sort();

            foreach(int q in list)
                Console.WriteLine(" location given is  :" + q);


            Board b = new Board(pieces.ToList());

            int[] columns ={
                (int)BT.ColA,
                (int)BT.ColB,
                (int)BT.ColC,
                (int)BT.ColD,
                (int)BT.ColE,
                (int)BT.ColF,
                (int)BT.ColG,
                (int)BT.ColH
            };            
                
            int[] rows = {
                (int)BT.Row1,   
                (int)BT.Row2,   
                (int)BT.Row3,   
                (int)BT.Row4,
                (int)BT.Row5,   
                (int)BT.Row6,   
                (int)BT.Row7,   
                (int)BT.Row8
            };

            for (int i = 0; i < columns.Length; i++){
                checkEquals( columns[i] + rows[i] );
            }
            
            void checkEquals( int INDEX ){
                Piece p = b[INDEX];
                Piece a = b.Fields[ p.Position ] ;
                
                Console.WriteLine("location Checked");
                Assert.AreEqual(a.Position , p.Position);

                Console.WriteLine("Type Checked");
                Assert.AreEqual(a.PieceType , p.PieceType);
                
                Console.WriteLine("Flags Checked");
                Assert.AreEqual(a.PieceFlags , p.PieceFlags);
                
                Console.WriteLine("inbuilt check Checked");
                Assert.AreEqual(a , p);
                Assert.True(a == p);
            }
        
        }
    }
}