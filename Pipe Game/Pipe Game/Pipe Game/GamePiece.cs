using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
//S. White 2/27/13

namespace Pipe_Game
{
    class GamePiece
    {
        public static string[] PieceTypes =// string array for rotational types of pieces in sprite sheet
        {
        "Left,Right",
        "Top,Bottom",
        "Left,Top",
        "Top,Right",
        "Right,Bottom",
        "Bottom,Left",
        "Empty"
        };
        public const int PieceHeight = 40;//sprite sheet characteristics
        public const int PieceWidth = 40;
        public const int MaxPlayablePieceIndex = 5;
        public const int EmptyPieceIndex = 6;
        private const int textureOffsetX = 1;
        private const int textureOffsetY = 1;
        private const int texturePaddingX = 1;
        private const int texturePaddingY = 1;
        private string pieceType = "";
        private string pieceSuffix = "";

        public string PieceType//returns piecetype
        {
            get { return pieceType; }
        }
        public string Suffix//returns suffix
        {
            get { return pieceSuffix; }
        }
        public GamePiece(string type, string suffix)//constructor for gamepiece with type and suffix
        {
            pieceType = type;
            pieceSuffix = suffix;
        }
        public GamePiece(string type)//constructor for gamepiece with type
        {
            pieceType = type;
            pieceSuffix = "";
        }
        public void SetPiece(string type, string suffix)//sets gamepieces type and suffix
        {
            pieceType = type;
            pieceSuffix = suffix;
        }
        public void SetPiece(string type)//sets gamepieces type
        {
            SetPiece(type, "");
        }
        public void AddSuffix(string suffix)//changes suffix for piece,if there is no suffix papplied to that piece
        {
            if (!pieceSuffix.Contains(suffix))
                pieceSuffix += suffix;
        }
        public void RemoveSuffix(string suffix)//removes suffix from piece
        {
            pieceSuffix = pieceSuffix.Replace(suffix, "");
        }
        public void RotatePiece(bool Clockwise)//allows the pieces to rotate when clicked on
        {
            switch (pieceType)
            {
                case "Left,Right":
                    pieceType = "Top,Bottom";
                    break;
                case "Top,Bottom":
                    pieceType = "Left,Right";
                    break;
                case "Left,Top":
                    if (Clockwise)
                        pieceType = "Top,Right";
                    else
                        pieceType = "Bottom,Left";
                    break;
                case "Top,Right":
                    if (Clockwise)
                        pieceType = "Right,Bottom";
                    else
                        pieceType = "Left,Top";
                    break;
                case "Right,Bottom":
                    if (Clockwise)
                        pieceType = "Bottom,Left";
                    else
                        pieceType = "Top,Right";
                    break;
                case "Bottom,Left":
                    if (Clockwise)
                        pieceType = "Left,Top";
                    else
                        pieceType = "Right,Bottom";
                    break;
                case "Empty":
                    break;
            }
        }
        public string[] GetOtherEnds(string startingEnd)//allows the program to preform water pathfinding
        {
            List<string> opposites = new List<string>();
            foreach (string end in pieceType.Split(','))
            {
                if (end != startingEnd)
                    opposites.Add(end);
            }
            return opposites.ToArray();
        }
        public bool HasConnector(string direction)//tells the program to check if theres a connector and returns the relevant info
        {
            return pieceType.Contains(direction);
        }
        public Rectangle GetSourceRect()//tells the program what to draw from the sprite sheet
        {
            int x = textureOffsetX;
            int y = textureOffsetY;
            if (pieceSuffix.Contains("W"))
                x += PieceWidth + texturePaddingX;
            y += (Array.IndexOf(PieceTypes, pieceType) *
            (PieceHeight + texturePaddingY));
            return new Rectangle(x, y, PieceWidth, PieceHeight);
        }

    }
}
