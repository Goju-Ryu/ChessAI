using ChessAI.DataClasses;
using NUnit.Framework;
using System;

namespace UnitTests{

        public enum BT : int {
            ColA = 0x00,   
            ColB = 0x10,   
            ColC = 0x20,   
            ColD = 0x30,
            ColE = 0x40,   
            ColF = 0x50,   
            ColG = 0x60,   
            ColH = 0x70,

            Row1 = 0x00,   
            Row2 = 0x01,   
            Row3 = 0x02,   
            Row4 = 0x03,
            Row5 = 0x04,   
            Row6 = 0x05,   
            Row7 = 0x06,   
            Row8 = 0x07
        };
    public static class TestBuilder {
    
        public static Piece[] generatePieces(){
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
            return pieces;
        }

        public static Board generateStartingBoard(){
            return new Board(generatePieces());
        }
        
        public static Board generateEmptyBoard(){
            Piece[] p = new Piece[1];
            p[0] = new Piece(Piece.Empty, 1);
            return new Board(p);
        }




    }
}