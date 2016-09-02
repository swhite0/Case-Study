using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
//S. White
//3/4/2013
namespace Pipe_Game
{
    class FallingPiece:GamePiece
    {
        public int VerticalOffset;
        public static int fallRate = 5;

        public FallingPiece(string pieceType, int verticalOffset)
            : base(pieceType)
        {
            VerticalOffset = verticalOffset;
        }

        public void UpdatePiece()
        {
            VerticalOffset = (int)MathHelper.Max(
            0,
            VerticalOffset - fallRate);
        }











    }//end of class
}//end of namespace
