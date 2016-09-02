using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
//S. White
//3/4/2013
namespace Pipe_Game
{
    class FadingPiece: GamePiece
    {
        public float alphaLevel = 1.0f;
        public static float alphaChangeRate = 0.02f;

        public FadingPiece(string pieceType, string suffix)
            : base(pieceType, suffix)
        {

        }
        public void UpdatePiece()
        {
            alphaLevel = MathHelper.Max(0,alphaLevel - alphaChangeRate);
        }











    }
}
