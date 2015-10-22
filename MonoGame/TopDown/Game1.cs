using Android.Content.Res;
using Java.Util.Zip;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using TV.Ouya.Sdk;

namespace TopDown
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        private const string TAG = "Game1";

        public static GraphicsDeviceManager _sGraphics;
        SpriteBatch _mSpriteBatch;
        SpriteFont _mFont;

        RenderTarget2D _mRenderTarget = null;
        RenderTarget2D _mLightRenderTarget = null;

        private Effect _mLightingEffect = null;
        private EffectParameter _mLightingAlpha = null;

        /// <summary>
        /// Static random instance
        /// </summary>
        public static Random _sRandom = new Random();

        /// <summary>
        /// Static list of players
        /// </summary>
        List<Player> _mPlayers = new List<Player>();

        /// <summary>
        /// Static list of projectiles
        /// </summary>
        public static List<Projectile> _sProjectiles = new List<Projectile>();

        public Game1()
        {
            _sGraphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";

            _sGraphics.IsFullScreen = true;
            _sGraphics.PreferredBackBufferWidth = 1920;
            _sGraphics.PreferredBackBufferHeight = 1080;
            _sGraphics.SupportedOrientations = DisplayOrientation.LandscapeLeft;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // Create players
            _mPlayers.Add(new Player(0, Color.Orange, 75, 75));
            _mPlayers.Add(new Player(1, Color.Cyan, 75, 75));

            base.Initialize();
        }

        private Effect LoadEffect(string path)
        {
            byte[] bytes = null;
            try
            {
                AssetManager assetManager = Activity.ApplicationContext.Assets;
                System.IO.Stream inputStream = assetManager.Open(path, Access.Buffer);

                byte[] buffer = new byte[1024];
                ZipInputStream zis = new ZipInputStream(inputStream);
                ZipEntry ze = zis.NextEntry;
                while (ze != null)
                {
                    String filePath = ze.Name;
                    //Log.i(TAG, "Found="+ze.getName());
                    //Log.i(TAG, "Open Output="+newFile.getAbsoluteFile());
                    int totalLength = 0;
                    int len;
                    List<byte[]> chunks = new List<byte[]>();
                    while ((len = zis.Read(buffer)) > 0)
                    {
                        Android.Util.Log.Info(TAG, string.Format("Read: {0} bytes", len));
                        totalLength += len;
                        byte[] chunk = new byte[len];
                        Array.Copy(buffer, chunk, len);
                        chunks.Add(chunk);
                    }
                    bytes = new byte[totalLength];
                    int index = 0;
                    foreach (byte[] chunk in chunks)
                    {
                        Array.Copy(chunk, 0, bytes, index, chunk.Length);
                        index += chunk.Length;
                    }
                    ze = zis.NextEntry;
                }
                zis.Close();
                inputStream.Close();
            }
            catch (Exception ex)
            {
                Android.Util.Log.Error(TAG, string.Format("Exception reading={0}", ex));
            }

            Effect effect = null;
            if (null != bytes)
            {
                try
                {
                    effect = new Effect(GraphicsDevice, bytes);
                }
                catch (Exception ex)
                {
                    Android.Util.Log.Error(TAG, string.Format("Loading Effect Exception={0}", ex));
                }
            }

            return effect;
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            _mSpriteBatch = new SpriteBatch(GraphicsDevice);

            // Load the font
            _mFont = Content.Load<SpriteFont>("spriteFont1");

            int width = 320;
            int height = 240;

            width = _sGraphics.PreferredBackBufferWidth;
            height = _sGraphics.PreferredBackBufferHeight;

            // Create the render target
            _mRenderTarget = new RenderTarget2D(GraphicsDevice, width, height, false,
                _sGraphics.PreferredBackBufferFormat, DepthFormat.Depth24);

            // Create the light render target
            _mLightRenderTarget = new RenderTarget2D(GraphicsDevice, width, height, false,
                _sGraphics.PreferredBackBufferFormat, DepthFormat.Depth24);

            try
            {
                Android.Util.Log.Info(TAG, "...Loading Lighting Effect");
                _mLightingEffect = LoadEffect(@"LightingEffectOuya.zip");
                _mLightingAlpha = _mLightingEffect.Parameters["Alpha"];
                _mLightingAlpha.SetValue(0.2f);
            }
            catch (Exception ex)
            {
                Android.Util.Log.Error(TAG, string.Format("Exception loading lighting: {0}", ex));
            }

            // Load the players
            foreach (Player player in _mPlayers)
            {
                player.LoadContent(GraphicsDevice);
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            #region Render to Light Render Texture
            GraphicsDevice.SetRenderTarget(_mLightRenderTarget);

            _sGraphics.GraphicsDevice.Clear(Color.White);

            _mSpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend,
                SamplerState.LinearClamp, DepthStencilState.Default,
                RasterizerState.CullNone);

            foreach (Projectile projectile in _sProjectiles)
            {
                projectile.DrawLight(_mSpriteBatch);
            }

            foreach (Player player in _mPlayers)
            {
                player.DrawLight(_mSpriteBatch);
            }

            _mSpriteBatch.End();
            #endregion

            #region Render to Render Texture
            GraphicsDevice.SetRenderTarget(_mRenderTarget);

            _sGraphics.GraphicsDevice.Clear(Color.Green);

            _mSpriteBatch.Begin();
            foreach (Projectile projectile in _sProjectiles)
            {
                projectile.Draw(_mSpriteBatch, _mFont, gameTime);
            }
            foreach (Player player in _mPlayers)
            {
                player.Draw(_mSpriteBatch, _mFont, gameTime);
            }
            _mSpriteBatch.End();
            #endregion

            GraphicsDevice.SetRenderTarget(null);

            // draw through effect
            _mSpriteBatch.Begin(SpriteSortMode.Texture, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, _mLightingEffect, Matrix.Identity);
            _mSpriteBatch.Draw(_mRenderTarget, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            _mSpriteBatch.End();

            //draw light source
            _mSpriteBatch.Begin();
            _mSpriteBatch.Draw(_mRenderTarget, new Vector2(200, 200), new Rectangle(0, 0, 320, 200), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            _mSpriteBatch.Draw(_mLightRenderTarget, new Vector2(600, 200), new Rectangle(0, 0, 320, 200), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            _mSpriteBatch.End();

            // set sampler
            GraphicsDevice.Textures[1] = _mLightRenderTarget; // some how the _light Texture2D is being disposed when this is set
            _mLightingEffect.Parameters["LightSampler"].SetValue(_mLightRenderTarget); // this also causes the _light texture2D to be disposed

            _mSpriteBatch.Begin();
            _mSpriteBatch.DrawString(_mFont, 
                string.Format("Hello from MonoGame: Projectiles={0}", _sProjectiles.Count),
                new Vector2(Window.ClientBounds.Width / 2 - 200, Window.ClientBounds.Height / 2 - 40), Color.White);
            _mSpriteBatch.End();

            OuyaInput.ClearButtonStates();

            base.Draw(gameTime);

            while (_sProjectiles.Count > 0 &&
                _sProjectiles[0]._mLifeTime < 0)
            {
                _sProjectiles.RemoveAt(0);
            }
        }
    }
}
