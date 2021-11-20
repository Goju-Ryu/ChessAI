using ChessAI.DataClasses;
using NUnit.Framework;
using System;

using System.Collections.Generic;

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

            List<Piece> pieces = new List<Piece>();
            for (int i = 0; i < 0x80; i++)
            {
             pieces.Add( new Piece(Piece.Empty , i ) );   
            }

            Board board = new Board(pieces);
            return board;
        }

        public static Board GenerateBoard(List<Piece> pieces){

            Board board = generateEmptyBoard();

            List<Piece> newPieces = new List<Piece>(board.Fields);
            Assert.AreEqual(newPieces.Count , 0x80);
            
            foreach(Piece piece in pieces){
                newPieces[piece.Position] = piece;
            }

            Board newBoard = new Board(newPieces);
            return newBoard;
        }





    }
}