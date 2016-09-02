using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

/// <summary>
    /// Game Engine Management
/// Author
/// -S. White 
    /// </summary>

namespace Pipe_Game
{
    
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        
        Texture2D playingPieces;
        Texture2D backgroundScreen;
        Texture2D titleScreen;
        
        Texture2D levelIndicator;
        Texture2D ScoreIndicator;
        
        Texture2D endScreen;

        Texture2D[] leveltext =new Texture2D[26];//SOCIAL ACHIVEMENT DESCRIPTOR ARRAY
        Texture2D[] socialState = new Texture2D[26];//SOCIAL STATE (level text instead of number) ARRAY
        
        GameBoard gameBoard;
        Vector2 gameBoardDisplayOrigin = new Vector2(245, 37);//game board coordinates
        
        int playerScore = 0;
        Vector2 scorePosition = new Vector2(535, 510);//score position
        
        enum GameStates { TitleScreen, Playing, GameOver };
        
        GameStates gameState = GameStates.TitleScreen;
        Rectangle EmptyPiece = new Rectangle(1, 247, 40, 40);
        
        const float MinTimeSinceLastInput = 0.25f;
        float timeSinceLastInput = 0.0f;
        
        SpriteFont pericles36Font;
        Queue<ScoreZoom> ScoreZooms = new Queue<ScoreZoom>();
        Vector2 gameOverLocation = new Vector2(200, 260);
        
        float gameOverTimer;
        const float MaxFloodCounter = 100.0f;
        float floodCount = 0.0f;
        float timeSinceLastFloodIncrease = 0.0f;
        float timeBetweenFloodIncreases = 1.0f;
        float floodIncreaseAmount = 0.5f;

        const int MaxWaterHeight = 244;
        const int WaterWidth = 297;
        Vector2 waterOverlayStart = new Vector2(85, 245);
        Vector2 waterPosition = new Vector2(478, 338);

        int currentLevel = 0;
        int socialCount = 0;
        int linesCompletedThisLevel = 0;
        const float floodAccelerationPerLevel = 0.5f;
        Rectangle levelTextPosition = new Rectangle(150, 450,650,500);//Level text position in game (moves the social achivement pop up)                         DONE
        Rectangle levelTextOrigin = new Rectangle(153, 456, 643, 500);//level text origin in png                                                             DONE

        Rectangle socialStatePosition = new Rectangle(250, 525, 350, 250);//social state position in game (moves the social state level text)                         DONE
        Rectangle socialStateOrigin = new Rectangle(246, 516, 335, 550);//social state text origin in png                                                             DONE


        Rectangle levelTextIndicator = new Rectangle(263, 470, 70, 18);//level text indicator in game box (moves the social state label)  
        Rectangle levelindicatorOrigin = new Rectangle(0, 0, 132, 18);//level text indicator origin in png 

        Rectangle ScoreindicatorOrigin = new Rectangle(0, 0, 65, 19);//position in png
        Rectangle ScoreTextPosition = new Rectangle(533, 465, 65, 19);//position of "score" in game

        ClockTimer clock = new ClockTimer(); // POPING MECHANISM DONE!
        ClockTimer gameOverClock = new ClockTimer();//GameOver Timer
        

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            this.IsMouseVisible = true;
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 600;
            graphics.ApplyChanges();
            gameBoard = new GameBoard();
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            playingPieces = Content.Load<Texture2D>(@"Textures\Pipe Game Sprite Sheet (Final #2)");
            backgroundScreen = Content.Load<Texture2D>(@"Textures\Pipe Game Background (Final)");
            titleScreen = Content.Load<Texture2D>(@"Textures\Start Screen (Final)");
            endScreen = Content.Load<Texture2D>(@"Textures\End Screen (Final)");
            levelIndicator=Content.Load<Texture2D>(@"Textures\Text\Social State UI Text");
            ScoreIndicator=Content.Load<Texture2D>(@"Textures\Text\Score Text");

            // TODO: use this.Content to load your game content here

            //MASSIVE BLOCK OF CODE TO LOAD LEVEL TEXTS INTO ARRAY, IF BETTER WAY ID APRECIATE KNOWING.

            leveltext[0] = Content.Load<Texture2D>(@"Textures\Text\Social Achievement Sprite #1");
            leveltext[1] = Content.Load<Texture2D>(@"Textures\Text\Social Achievement Sprite #2");
            leveltext[2] = Content.Load<Texture2D>(@"Textures\Text\Social Achievement Sprite #3");
            leveltext[3] = Content.Load<Texture2D>(@"Textures\Text\Social Achievement Sprite #4");
            leveltext[4] = Content.Load<Texture2D>(@"Textures\Text\Social Achievement Sprite #5");
            leveltext[5] = Content.Load<Texture2D>(@"Textures\Text\Social Achievement Sprite #6");
            leveltext[6] = Content.Load<Texture2D>(@"Textures\Text\Social Achievement Sprite #7");
            leveltext[7] = Content.Load<Texture2D>(@"Textures\Text\Social Achievement Sprite #8");
            leveltext[8] = Content.Load<Texture2D>(@"Textures\Text\Social Achievement Sprite #9");
            leveltext[9] = Content.Load<Texture2D>(@"Textures\Text\Social Achievement Sprite #10");

            leveltext[10] = Content.Load<Texture2D>(@"Textures\Text\Social Achievement Sprite #11");
            leveltext[11] = Content.Load<Texture2D>(@"Textures\Text\Social Achievement Sprite #12");
            leveltext[12] = Content.Load<Texture2D>(@"Textures\Text\Social Achievement Sprite #13");
            leveltext[13] = Content.Load<Texture2D>(@"Textures\Text\Social Achievement Sprite #14");
            leveltext[14] = Content.Load<Texture2D>(@"Textures\Text\Social Achievement Sprite #15");
            leveltext[15] = Content.Load<Texture2D>(@"Textures\Text\Social Achievement Sprite #16");
            leveltext[16] = Content.Load<Texture2D>(@"Textures\Text\Social Achievement Sprite #17");
            leveltext[17] = Content.Load<Texture2D>(@"Textures\Text\Social Achievement Sprite #18");
            leveltext[18] = Content.Load<Texture2D>(@"Textures\Text\Social Achievement Sprite #19");
            leveltext[19] = Content.Load<Texture2D>(@"Textures\Text\Social Achievement Sprite #20");

            leveltext[20] = Content.Load<Texture2D>(@"Textures\Text\Social Achievement Sprite #21");
            leveltext[21] = Content.Load<Texture2D>(@"Textures\Text\Social Achievement Sprite #22");
            leveltext[22] = Content.Load<Texture2D>(@"Textures\Text\Social Achievement Sprite #23");
            leveltext[23] = Content.Load<Texture2D>(@"Textures\Text\Social Achievement Sprite #24");
            leveltext[24] = Content.Load<Texture2D>(@"Textures\Text\Social Achievement Sprite #25");
            leveltext[25] = Content.Load<Texture2D>(@"Textures\Text\Social Achievement Sprite #26");
            
            

            
            //END MASSIVE BLOCK OF CODE LOADING LEVEL TEXTSINTO ARRAY,
            //ANOUTHER MASSIVE BLOCK OF CODE TO LOAD SOCIALSTATE TEXTS INTO ARRAY, IF BETTER WAY ID APRECIATE KNOWING.

            socialState[0] = Content.Load<Texture2D>(@"Textures\Level\Social State Text #1");
            socialState[1] = Content.Load<Texture2D>(@"Textures\Level\Social State Text #2");
            socialState[2] = Content.Load<Texture2D>(@"Textures\Level\Social State Text #3");
            socialState[3] = Content.Load<Texture2D>(@"Textures\Level\Social State Text #4");
            socialState[4] = Content.Load<Texture2D>(@"Textures\Level\Social State Text #5");
            socialState[5] = Content.Load<Texture2D>(@"Textures\Level\Social State Text #6");
            socialState[6] = Content.Load<Texture2D>(@"Textures\Level\Social State Text #7");
            socialState[7] = Content.Load<Texture2D>(@"Textures\Level\Social State Text #8");
            socialState[8] = Content.Load<Texture2D>(@"Textures\Level\Social State Text #9");
            socialState[9] = Content.Load<Texture2D>(@"Textures\Level\Social State Text #10");

            socialState[10] = Content.Load<Texture2D>(@"Textures\Level\Social State Text #11");
            socialState[11] = Content.Load<Texture2D>(@"Textures\Level\Social State Text #12");
            socialState[12] = Content.Load<Texture2D>(@"Textures\Level\Social State Text #13");
            socialState[13] = Content.Load<Texture2D>(@"Textures\Level\Social State Text #14");
            socialState[14] = Content.Load<Texture2D>(@"Textures\Level\Social State Text #15");
            socialState[15] = Content.Load<Texture2D>(@"Textures\Level\Social State Text #16");
            socialState[16] = Content.Load<Texture2D>(@"Textures\Level\Social State Text #17");
            socialState[17] = Content.Load<Texture2D>(@"Textures\Level\Social State Text #18");
            socialState[18] = Content.Load<Texture2D>(@"Textures\Level\Social State Text #19");
            socialState[19] = Content.Load<Texture2D>(@"Textures\Level\Social State Text #20");

            socialState[20] = Content.Load<Texture2D>(@"Textures\Level\Social State Text #21");
            socialState[21] = Content.Load<Texture2D>(@"Textures\Level\Social State Text #22");
            socialState[22] = Content.Load<Texture2D>(@"Textures\Level\Social State Text #23");
            socialState[23] = Content.Load<Texture2D>(@"Textures\Level\Social State Text #24");
            socialState[24] = Content.Load<Texture2D>(@"Textures\Level\Social State Text #25");
            socialState[25] = Content.Load<Texture2D>(@"Textures\Level\Social State Text #26");




            //END ANOUTHER MASSIVE BLOCK OF CODE TO LOAD SOCIALSTATE TEXTS INTO ARRAY
            pericles36Font=Content.Load<SpriteFont>(@"Font\Pericles36");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here
            switch (gameState)
            {
                case GameStates.TitleScreen://
                    if (Keyboard.GetState().IsKeyDown(Keys.Space))
                    {
                        gameBoard.ClearBoard();
                        gameBoard.GenerateNewPieces(false);
                        playerScore = 0;
                        currentLevel = 0;
                        floodIncreaseAmount = 0.0f;
                        StartNewLevel();
                        gameState = GameStates.Playing;

                    }
                    break;
                case GameStates.Playing:
                    timeSinceLastInput += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    timeSinceLastFloodIncrease +=
                    (float)gameTime.ElapsedGameTime.TotalSeconds;

                    if (timeSinceLastFloodIncrease >= timeBetweenFloodIncreases)
                    {
                        floodCount += floodIncreaseAmount;
                        timeSinceLastFloodIncrease = 0.0f;
                        if (floodCount >= MaxFloodCounter)
                        {
                           // gameOverTimer = 8.0f;                 //old game over logic
                           // gameState = GameStates.GameOver;
                        }
                    }
                    if (gameBoard.ArePiecesAnimating())
                    {
                        gameBoard.UpdateAnimatedPieces();
                    }
                    else
                    {
                        gameBoard.ResetWater();
                        for (int y = 0; y < GameBoard.GameBoardHeight; y++)
                        {
                        CheckScoringChain(gameBoard.GetWaterChain(y));
                        }
                        gameBoard.GenerateNewPieces(true);
                        if (timeSinceLastInput >= MinTimeSinceLastInput)
                        {
                            HandleMouseInput(Mouse.GetState());
                        }
                    }
                    UpdateScoreZooms();

                    //Count Down timer for poping of social states
                    if (clock.isRunning == true)
                    {
                         
                        clock.checkTime(gameTime);
                    }
                     
                    break;//end of playing game state
               case GameStates.GameOver:
                    
                    if (!gameOverClock.isRunning == true)
                    {

                        gameOverClock.start(5);
                    }
                   
                   if (gameOverClock.isRunning == true)
                    {
                    gameOverClock.checkTime(gameTime);

                    }
                    if (gameOverClock.isFinished == true)
                         {
                           gameState = GameStates.TitleScreen;
                         }

                break;
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            if (gameState == GameStates.TitleScreen)
            {
                spriteBatch.Begin();
                spriteBatch.Draw(titleScreen,
                new Rectangle(0, 0,
                this.Window.ClientBounds.Width,
                this.Window.ClientBounds.Height),
                Color.White);
                //spriteBatch.DrawString(pericles36Font, "Press space to begin!", gameOverLocation, Color.Yellow);
                spriteBatch.End();
            }

            if ((gameState == GameStates.Playing) ||(gameState == GameStates.GameOver))//draws different things based upon game state values
            {
                spriteBatch.Begin();
                spriteBatch.Draw(backgroundScreen,
                new Rectangle(0, 0,
                this.Window.ClientBounds.Width,
                this.Window.ClientBounds.Height),
                Color.White);
                for (int x = 0; x < GameBoard.GameBoardWidth; x++)
                {
                    for (int y = 0; y < GameBoard.GameBoardHeight; y++)
                    {
                        int pixelX = (int)gameBoardDisplayOrigin.X +
                        (x * GamePiece.PieceWidth);
                        int pixelY = (int)gameBoardDisplayOrigin.Y +
                        (y * GamePiece.PieceHeight);
                        DrawEmptyPiece(pixelX, pixelY);
                        bool pieceDrawn = false;
                        string positionName = x.ToString() + "_" + y.ToString();
                        if (gameBoard.rotatingPieces.ContainsKey(positionName))
                        {
                            DrawRotatingPiece(pixelX, pixelY, positionName);
                            pieceDrawn = true;
                        }
                        if (gameBoard.fadingPieces.ContainsKey(positionName))
                        {
                            DrawFadingPiece(pixelX, pixelY, positionName);
                            pieceDrawn = true;
                        }
                        if (gameBoard.fallingPieces.ContainsKey(positionName))
                        {
                            DrawFallingPiece(pixelX, pixelY, positionName);
                            pieceDrawn = true;
                        }
                        if (!pieceDrawn)
                        {
                            DrawStandardPiece(x, y, pixelX, pixelY);
                        }
                    }
                }
                foreach (ScoreZoom zoom in ScoreZooms)
                {
                    spriteBatch.DrawString(pericles36Font, zoom.Text,
                    new Vector2(this.Window.ClientBounds.Width / 2,
                    this.Window.ClientBounds.Height / 2),zoom.DrawColor, 0.0f,new Vector2(pericles36Font.MeasureString(zoom.Text).X / 2,
                    pericles36Font.MeasureString(zoom.Text).Y / 2),
                    zoom.Scale, SpriteEffects.None, 0.0f);
                }
                int waterHeight = (int)(MaxWaterHeight * (floodCount / 100));
                //spriteBatch.Draw(backgroundScreen,new Rectangle((int)waterPosition.X,(int)waterPosition.Y + (MaxWaterHeight - waterHeight),WaterWidth,waterHeight),
                  //  new Rectangle((int)waterOverlayStart.X,(int)waterOverlayStart.Y + (MaxWaterHeight - waterHeight),WaterWidth,waterHeight),new Color(255, 255, 255, 180)); // draw rising water levels

                spriteBatch.DrawString(pericles36Font,playerScore.ToString(),scorePosition,Color.Black);//draws score
               
                //social state poping mechanism
                if (!clock.isFinished)
                {

                    socialCount = currentLevel - 1;
                    if (socialCount == 27)    //game over clock timer function
                    {
                            gameState = GameStates.GameOver;
                        /*if (!gameOverClock.isRunning == true)
                        {

                            gameOverClock.start(5);//time social achievement text pops
                        }*/
                        
                    }
                    if (currentLevel > 0||currentLevel < 25)
                    {
                        spriteBatch.Draw(leveltext[socialCount], levelTextPosition, levelTextOrigin, Color.White);//draws SOCIAL ACHIVEMENT text
                    }
                    else if (currentLevel < 25)
                    {
                        spriteBatch.Draw(socialState[socialCount], socialStatePosition, socialStateOrigin, Color.White);//draws SOCIAL State text

                    }
                    else
                    {
                        //socialCount = 24;
                        spriteBatch.Draw(socialState[socialCount], socialStatePosition, socialStateOrigin, Color.White);//draws SOCIAL State text

                    }/*
                    {
                        spriteBatch.Draw(socialState[socialCount], socialStatePosition, socialStateOrigin, Color.White);//draws SOCIAL State text

                    }*/
                }else if (currentLevel < 25)
                {
                    spriteBatch.Draw(socialState[socialCount], socialStatePosition, socialStateOrigin, Color.White);//draws SOCIAL State text

                }
                else
                {
                    socialCount = 24;
                    spriteBatch.Draw(socialState[socialCount], socialStatePosition, socialStateOrigin, Color.White);//draws SOCIAL State text

                }
                
                spriteBatch.Draw(levelIndicator, levelTextIndicator, levelindicatorOrigin, Color.White);//draws level text label
                spriteBatch.Draw(ScoreIndicator,ScoreTextPosition,ScoreindicatorOrigin,Color.White);//draws score text label
                spriteBatch.End();
            }
            // TODO: Add your drawing code here
            if (gameState == GameStates.GameOver)
            {
                spriteBatch.Begin();
                spriteBatch.Draw(endScreen,new Rectangle(0, 0,
                this.Window.ClientBounds.Width,
                this.Window.ClientBounds.Height),
                Color.White);
                //if (!gameOverClock.isRunning == true)
                //{
                  //  gameState = GameStates.TitleScreen;
                //}
                spriteBatch.End();
            }
            base.Draw(gameTime);
        }//end of draw
        private int DetermineScore(int SquareCount)//checks score
        {
            return (int)((Math.Pow((SquareCount / 5), 2) + SquareCount) * 10);
        }
        private void CheckScoringChain(List<Vector2> WaterChain)//checks  using waterchain
        {
            if (WaterChain.Count > 0)
            {
                Vector2 LastPipe = WaterChain[WaterChain.Count - 1];
                if (LastPipe.X == GameBoard.GameBoardWidth - 1)
                {
                    if (gameBoard.HasConnector(
                    (int)LastPipe.X, (int)LastPipe.Y, "Right"))
                    {
                        playerScore += DetermineScore(WaterChain.Count);
                        linesCompletedThisLevel++;
                        floodCount = MathHelper.Clamp(floodCount - 
                            (DetermineScore(WaterChain.Count)/10), 0.0f, 100.0f);
                        ScoreZooms.Enqueue(new ScoreZoom("+" + DetermineScore(WaterChain.Count).ToString(),new Color(1.0f, 0.0f, 0.0f, 0.4f)));
                        foreach (Vector2 ScoringSquare in WaterChain)
                        {
                            gameBoard.AddFadingPiece((int)ScoringSquare.X,(int)ScoringSquare.Y,gameBoard.GetSquare((int)ScoringSquare.X,(int)ScoringSquare.Y));
                            gameBoard.SetSquare((int)ScoringSquare.X,(int)ScoringSquare.Y, "Empty");
                        }
                        if (linesCompletedThisLevel >= 1) // edit this for amount of chains to start next level
                        {
                            StartNewLevel();
                        }
                    }
                }
            }
        }
        private void HandleMouseInput(MouseState mouseState)//mouse controls
        {
            int x = ((mouseState.X -
            (int)gameBoardDisplayOrigin.X) / GamePiece.PieceWidth);
            int y = ((mouseState.Y -
            (int)gameBoardDisplayOrigin.Y) / GamePiece.PieceHeight);
            if ((x >= 0) && (x < GameBoard.GameBoardWidth) &&
            (y >= 0) && (y < GameBoard.GameBoardHeight))
            {
                if (mouseState.LeftButton == ButtonState.Pressed)
                {
                    gameBoard.AddRotatingPiece(x, y,gameBoard.GetSquare(x, y), false);
                    gameBoard.RotatePiece(x, y, false);
                    timeSinceLastInput = 0.0f;
                }
                if (mouseState.RightButton == ButtonState.Pressed)
                {
                    gameBoard.AddRotatingPiece(x, y,gameBoard.GetSquare(x, y), true);
                    gameBoard.RotatePiece(x, y, true);
                    timeSinceLastInput = 0.0f;
                }
            }
        }

        private void DrawEmptyPiece(int pixelX, int pixelY)
        {
            spriteBatch.Draw(
            playingPieces,
            new Rectangle(pixelX, pixelY,
            GamePiece.PieceWidth, GamePiece.PieceHeight), EmptyPiece,
            Color.White);
        }
        private void DrawStandardPiece(int x, int y,int pixelX, int pixelY)
        {
            spriteBatch.Draw(
            playingPieces, new Rectangle(pixelX, pixelY,
            GamePiece.PieceWidth, GamePiece.PieceHeight),
            gameBoard.GetSourceRect(x, y),
            Color.White);
        }
        private void DrawFallingPiece(int pixelX, int pixelY,string positionName)
        {
            spriteBatch.Draw(
            playingPieces,
            new Rectangle(pixelX, pixelY -
            gameBoard.fallingPieces[positionName].VerticalOffset,
            GamePiece.PieceWidth, GamePiece.PieceHeight),
            gameBoard.fallingPieces[positionName].GetSourceRect(),
            Color.White);
        }
        private void DrawFadingPiece(int pixelX, int pixelY,string positionName)
        {
            spriteBatch.Draw(
            playingPieces,
            new Rectangle(pixelX, pixelY,
            GamePiece.PieceWidth, GamePiece.PieceHeight),
            gameBoard.fadingPieces[positionName].GetSourceRect(),
            Color.White *
            gameBoard.fadingPieces[positionName].alphaLevel);
        }
        private void DrawRotatingPiece(int pixelX, int pixelY,string positionName)
        {
            spriteBatch.Draw(
            playingPieces,
            new Rectangle(pixelX + (GamePiece.PieceWidth / 2),
            pixelY + (GamePiece.PieceHeight / 2),
            GamePiece.PieceWidth,
            GamePiece.PieceHeight),
            gameBoard.rotatingPieces[positionName].GetSourceRect(),
            Color.White,
            gameBoard.rotatingPieces[positionName].RotationAmount, new Vector2(GamePiece.PieceWidth / 2,
            GamePiece.PieceHeight / 2),
            SpriteEffects.None, 0.0f);
        }
        private void UpdateScoreZooms()
        {
            int dequeueCounter = 0;
            foreach (ScoreZoom zoom in ScoreZooms)
            {
                zoom.Update();  
                if (zoom.IsCompleted)
                    dequeueCounter++;
            }
            for (int d = 0; d < dequeueCounter; d++)
                ScoreZooms.Dequeue();
        }



        private void StartNewLevel()
        {
            currentLevel++;
            floodCount = 0.0f;
            
            linesCompletedThisLevel = 0;
            floodIncreaseAmount += floodAccelerationPerLevel;
            gameBoard.ClearBoard();
            gameBoard.GenerateNewPieces(false);
            clock.reset();
            if (currentLevel == 26)
            {
                gameState = GameStates.GameOver;
                
                //gameState = GameStates.GameOver;
            }
            if (!clock.isRunning == true)
            {
                 
                clock.start(5);//time social achievement text pops
            }
            
        }






    }//end of game 1
}//end of name space
