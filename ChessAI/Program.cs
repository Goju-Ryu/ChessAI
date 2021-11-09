﻿using System;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using ChessAI;

namespace ChessAI{
	class Program {
		static void Main(string[ ] args) {
			var board = new Board();
		}
	}
	public interface IDirectionBoard{
		byte GetFieldsToEdge(int position , int direction);
		bool IsFieldOccupied(byte position);
		bool IsFieldEnemouriouslyConqueredByEvil(byte position);

	}
	public class DirectionBoard{
		
		public static readonly byte MaxFields = 64;
		public static readonly byte FieldsPrSide = 8;
		public static readonly sbyte[] DirOffsets = { 8, -8, -1, 1, 7, -7, 9, -9 };
		public enum DirectionIndex { Up = 0 , Down = 1, Left = 2, Right = 3, UpLeft = 4, DownRight=5, UpRight=7, DownLeft=8 };

		// genMoveLines (0 - 3 , 4 - 8)

		private List<sbyte[]>  directionBoardData = new List<sbyte[]>();
		
		public DirectionBoard(){
			for(sbyte i = 0 ; i< MaxFields ; i++) {
                directionBoardData.Add(GetDirections(i));
			}
		}

		private sbyte[ ] GetDirections(sbyte index) {

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

			dirs[ (int)DirectionIndex.Up		] = (sbyte)up;
			dirs[ (int)DirectionIndex.Down		] = (sbyte)down;
			dirs[ (int)DirectionIndex.Left		] = (sbyte)left;
			dirs[ (int)DirectionIndex.Right		] = (sbyte)right;

			dirs[ (int)DirectionIndex.UpRight	] = (sbyte)(up   > right ? up	: right);
			dirs[ (int)DirectionIndex.DownRight ] = (sbyte)(down > right ? down	: right);
			dirs[ (int)DirectionIndex.UpLeft	] = (sbyte)(up   > left	? up	: left);
			dirs[ (int)DirectionIndex.DownRight ] = (sbyte)(down > right ? down	: right);

			return null;
		}

		public byte GetFieldsToEdge(int position , int direction) {
			return (byte) directionBoardData[ position ][ direction ];
		}
	}
	public class Board : DirectionBoard , IDirectionBoard {

		// NOTE INHERITED MEMBERS 
		//public static readonly byte maxFields = 64;
		//public static readonly byte fieldsPrSide = 8;
		//public static readonly sbyte[] dirOffsets = { 8, -8, -1, 1, 7, -7, 9, -9 };

		Piece _pieces;
		public Board() : base() {
			_pieces = new Piece();
		}	

		public bool IsFieldOccupied(byte position){
			return false;
		}

		public bool IsFieldEnemouriouslyConqueredByEvil(byte position){
			return false;
		}

	}
}

	// EXAMPLE FOR MADS 
	public static class Extensions{
		public static int ExampleExtensionMethod(this byte b){
			return (int)b; // THIS METHOD IS NOW AVAILABLE IN BYTE CLASS OBJECTS // HURRAH
		}
	}



