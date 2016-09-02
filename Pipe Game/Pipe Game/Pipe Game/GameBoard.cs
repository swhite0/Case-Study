using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
//S . White 2/23/13
namespace Pipe_Game
{
    class GameBoard
    {
        Random rand = new Random();//fills board with random pieces
        public const int GameBoardWidth = 8;//board characteristics
        public const int GameBoardHeight = 10;
        private GamePiece[,] boardSquares =
        new GamePiece[GameBoardWidth, GameBoardHeight];//where new piece is located in gameboard
        private List<Vector2> WaterTracker = new List<Vector2>();
        public Dictionary<string, FallingPiece> fallingPieces = new Dictionary<string, FallingPiece>();
        public Dictionary<string, RotatingPiece> rotatingPieces = new Dictionary<string, RotatingPiece>();
        public Dictionary<string, FadingPiece> fadingPieces = new Dictionary<string, FadingPiece>();
        public GameBoard()
        {
            ClearBoard();
        }
        public void ClearBoard()//empties the board and fills the squares with the empty piece
        {
            for (int x = 0; x < GameBoardWidth; x++)
                for (int y = 0; y < GameBoardHeight; y++)
                    boardSquares[x, y] = new GamePiece("Empty");
        }

        public void RotatePiece(int x, int y, bool clockwise)//rotate piece function interacts with gamepiece function
        {
            boardSquares[x, y].RotatePiece(clockwise);
        }
        public Rectangle GetSourceRect(int x, int y)//gets where to draw the sprite image
        {
            return boardSquares[x, y].GetSourceRect();
        }
        public string GetSquare(int x, int y)//gets a piece type for the game piece in the relevant xy cordinate
        {
            return boardSquares[x, y].PieceType;
        }
        public void SetSquare(int x, int y, string pieceName)//sets a piece type for the game piece in the relevant xy cordinate
        {
            boardSquares[x, y].SetPiece(pieceName);
        }
        public bool HasConnector(int x, int y, string direction)//queries for if that pieces can connect in that direction
        {
            return boardSquares[x, y].HasConnector(direction);
        }
        public void RandomPiece(int x, int y)//sets the square in the board to a random empty piece
        {
            boardSquares[x, y].SetPiece(GamePiece.PieceTypes[rand.Next(0,
            GamePiece.MaxPlayablePieceIndex + 1)]);
        }
        public void FillFromAbove(int x, int y)//drops piece that was above and drops it down to the empty spot
        {
            int rowLookup = y - 1;
            while (rowLookup >= 0)
        {
            if (GetSquare(x, rowLookup) != "Empty")//checks for empty piece spots in the top rows and fills them with pieces
        {
                    SetSquare(x, y,
                    GetSquare(x, rowLookup));
                    SetSquare(x, rowLookup, "Empty");
                    AddFallingPiece(x, y, GetSquare(x, y),GamePiece.PieceHeight * (y - rowLookup));
                    rowLookup = -1;
        }
                    rowLookup--;
        }
        }
        public void GenerateNewPieces(bool dropSquares)//checks and generates pieces from the fillfrom above function
        {
            if (dropSquares)
            {
                for (int x = 0; x < GameBoard.GameBoardWidth; x++)
                {
                    for (int y = GameBoard.GameBoardHeight - 1; y >= 0;y--)
                    {
                         if (GetSquare(x, y) == "Empty")
                          {
                        FillFromAbove(x, y);
                          }
                    }
                }
            }
            for (int y = 0; y < GameBoard.GameBoardHeight; y++)
            {
                for (int x = 0; x < GameBoard.GameBoardWidth; x++)
                {
                    if (GetSquare(x, y) == "Empty")
                    {
                        AddFallingPiece(x, y, GetSquare(x, y),GamePiece.PieceHeight * GameBoardHeight);
                        RandomPiece(x, y);//generates a random piece to fill in the x,y cordinate in game board
                    }
                }
            }
        }
        public void ResetWater()//removes the water from the game piece reseting them to normal unfilled pieces
        {
            for (int y = 0; y < GameBoardHeight; y++)
                for (int x = 0; x < GameBoardWidth; x++)
                    boardSquares[x, y].RemoveSuffix("W");
        }
        public void FillPiece(int X, int Y)//adds the w suffix to the game piece indicating to use the water filled sprite tile
        {
            boardSquares[X, Y].AddSuffix("W");
        }

        public void PropagateWater(int x, int y, string fromDirection)// water(friendship chain) path finding based apon information gathered from functions in gamepiece class
        {
            if ((y >= 0) && (y < GameBoardHeight) &&
            (x >= 0) && (x < GameBoardWidth))
            {
                if (boardSquares[x, y].HasConnector(fromDirection) &&
                !boardSquares[x, y].Suffix.Contains("W"))
                {
                    FillPiece(x, y);
                    WaterTracker.Add(new Vector2(x, y));
                    foreach (string end in
                    boardSquares[x, y].
                    GetOtherEnds(fromDirection))
                        switch (end)
                        {
                            case "Left": PropagateWater(x - 1, y,"Right");
                                break;
                            case "Right": PropagateWater(x + 1, y,"Left");
                                break;
                            case "Top": PropagateWater(x, y - 1,"Bottom");
                                break;
                            case "Bottom": PropagateWater(x, y + 1,"Top");
                                break;
                        }
                }
            }
        }
        public List<Vector2> GetWaterChain(int y)//water (friendship )chain info return function from propagate water, information is usefull for score, and piece replacement/removal
        {
            WaterTracker.Clear();
            PropagateWater(0, y, "Left");
            return WaterTracker;
        }

        public void AddFallingPiece(int X, int Y,string PieceName, int VerticalOffset)
        {
            fallingPieces[X.ToString() + "_" + Y.ToString()] = new FallingPiece(PieceName, VerticalOffset);
        }
        public void AddRotatingPiece(int X, int Y,string PieceName, bool Clockwise)
        {
            rotatingPieces[X.ToString() + "_" + Y.ToString()] = new RotatingPiece(PieceName, Clockwise);
        }
        public void AddFadingPiece(int X, int Y, string PieceName)
        {
            fadingPieces[X.ToString() + "_" + Y.ToString()] = new FadingPiece(PieceName, "W");
        }

        public bool ArePiecesAnimating()
        {
            if ((fallingPieces.Count == 0) && (rotatingPieces.Count == 0) && (fadingPieces.Count == 0))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private void UpdateFadingPieces()
        {
            Queue<string> RemoveKeys = new Queue<string>();
            foreach (string thisKey in fadingPieces.Keys)
            {
                fadingPieces[thisKey].UpdatePiece();
                if (fadingPieces[thisKey].alphaLevel == 0.0f)
                    RemoveKeys.Enqueue(thisKey.ToString());
            }
            while (RemoveKeys.Count > 0)
                fadingPieces.Remove(RemoveKeys.Dequeue());
        }

        private void UpdateFallingPieces()
        {
            Queue<string> RemoveKeys = new Queue<string>();
            foreach (string thisKey in fallingPieces.Keys)
            {
                fallingPieces[thisKey].UpdatePiece();
                if (fallingPieces[thisKey].VerticalOffset == 0)
                    RemoveKeys.Enqueue(thisKey.ToString());
            }
            while (RemoveKeys.Count > 0)
                fallingPieces.Remove(RemoveKeys.Dequeue());
        }

        private void UpdateRotatingPieces()
        {
            Queue<string> RemoveKeys = new Queue<string>();
            foreach (string thisKey in rotatingPieces.Keys)
            {
                rotatingPieces[thisKey].UpdatePiece();
                if (rotatingPieces[thisKey].rotationTicksRemaining == 0)
                    RemoveKeys.Enqueue(thisKey.ToString());
            }
            while (RemoveKeys.Count > 0)
                rotatingPieces.Remove(RemoveKeys.Dequeue());
        }

        public void UpdateAnimatedPieces()
        {
            if (fadingPieces.Count == 0)
            {
                UpdateFallingPieces();
                UpdateRotatingPieces();
            }
            else
            {
                UpdateFadingPieces();
            }
        }

   }//end of class
}//end of namespace
