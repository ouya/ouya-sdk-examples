using Android.Content.Res;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using Krypton.Lights;
using Krypton.Common;

namespace Krypton
{
    public enum LightMapSize
    {
        Full = 1,
        Fourth = 2,
        Eighth = 4,
    }

    /// <summary>
    /// A GPU-based 2D lighting engine
    /// </summary>
    public class KryptonEngine : DrawableGameComponent
    {
        // The Krypton Effect
        private string mEffectAssetName;
        private Effect mEffect;
        private CullMode mCullMode = CullMode.CullCounterClockwiseFace;

        // The goods
        private List<ShadowHull> mHulls = new List<ShadowHull>();
        private List<ILight2D> mLights = new List<ILight2D>();

        // World View Projection matrix, and it's min and max view bounds
        private Matrix mWVP = Matrix.Identity;
        private bool mSpriteBatchCompatabilityEnabled = false;
        private BoundingRect mBounds = BoundingRect.MinMax;

        // Blur
        private float mBluriness = 0.25f;
        private RenderTarget2D mMapBlur;

        // Light maps
        private RenderTarget2D mMap;
        private Color mAmbientColor = new Color(35,35,35);
        private LightMapSize mLightMapSize = LightMapSize.Full;

        /// <summary>
        /// Krypton's render helper. It helps render. It also needs to be re-written.
        /// </summary>
        public KryptonRenderHelper RenderHelper { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating how Krypton should cull geometry. The default value is CullMode.CounterClockwise
        /// </summary>
        public CullMode CullMode
        {
            get { return this.mCullMode; }
            set { this.mCullMode = value; }
        }

        /// <summary>
        /// The collection of lights krypton uses to render shadows
        /// </summary>
        public List<ILight2D> Lights { get { return this.mLights; } }

        /// <summary>
        /// The collection of hulls krypton uses to render shadows
        /// </summary>
        public List<ShadowHull> Hulls { get { return this.mHulls; } }

        /// <summary>
        /// Gets or sets the matrix used to draw the light map. This should match your scene's matrix.
        /// </summary>
        public Matrix Matrix
        {
            get { return this.mWVP; }
            set
            {
                if (this.mWVP != value)
                {
                    this.mWVP = value;

                    // This is totally ghetto, but it works for now. :)
                    // Compute the world-space bounds of the given matrix
                    var inverse = Matrix.Invert(value);

                    var v1 = Vector2.Transform(new Vector2(1, 1), inverse);
                    var v2 = Vector2.Transform(new Vector2(1, -1), inverse);
                    var v3 = Vector2.Transform(new Vector2(-1, -1), inverse);
                    var v4 = Vector2.Transform(new Vector2(-1, 1), inverse);

                    this.mBounds.Min = v1;
                    this.mBounds.Min = Vector2.Min(this.mBounds.Min, v2);
                    this.mBounds.Min = Vector2.Min(this.mBounds.Min, v3);
                    this.mBounds.Min = Vector2.Min(this.mBounds.Min, v4);

                    this.mBounds.Max = v1;
                    this.mBounds.Max = Vector2.Max(this.mBounds.Max, v2);
                    this.mBounds.Max = Vector2.Max(this.mBounds.Max, v3);
                    this.mBounds.Max = Vector2.Max(this.mBounds.Max, v4);

                    this.mBounds = BoundingRect.MinMax;
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating weither or not to use SpriteBatch's matrix when drawing lightmaps
        /// </summary>
        public bool SpriteBatchCompatablityEnabled
        {
            get { return this.mSpriteBatchCompatabilityEnabled; }
            set { this.mSpriteBatchCompatabilityEnabled = value; }
        }

        /// <summary>
        /// Ambient color of the light map. Lights + AmbientColor = Final 
        /// </summary>
        public Color AmbientColor
        {
            get { return this.mAmbientColor; }
            set { this.mAmbientColor = value; }
        }

        /// <summary>
        /// Gets or sets the value used to determine light map size
        /// </summary>
        public LightMapSize LightMapSize
        {
            get { return this.mLightMapSize; }
            set
            {
                if (this.mLightMapSize != value)
                {
                    this.mLightMapSize = value;
                    this.DisposeRenderTargets();
                    this.CreateRenderTargets();
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating how much to blur the final light map. If the value is zero, the lightmap will not be blurred
        /// </summary>
        public float Bluriness
        {
            get { return this.mBluriness; }
            set { this.mBluriness = Math.Max(0, value); }
        }

        /// <summary>
        /// Constructs a new instance of krypton
        /// </summary>
        /// <param name="game">Your game object</param>
        /// <param name="effectAssetName">The asset name of Krypton's effect file, which must be included in your content project</param>
        public KryptonEngine(Game game, string effectAssetName)
            : base(game)
        {
            this.mEffectAssetName = effectAssetName;
        }

        /// <summary>
        /// Initializes Krpyton, and hooks itself to the graphics device
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            this.GraphicsDevice.DeviceReset += new EventHandler<EventArgs>(GraphicsDevice_DeviceReset);
        }

        /// <summary>
        /// Resets kryptons graphics device resources
        /// </summary>
        private void GraphicsDevice_DeviceReset(object sender, EventArgs e)
        {
            this.DisposeRenderTargets();
            this.CreateRenderTargets();
        }

		public static Android.App.Activity Activity = null;

        /// <summary>
        /// Load's the graphics related content required to draw light maps
        /// </summary>
        protected override void LoadContent()
        {
            // This needs to better handle content loading...
            // if the window is resized, Krypton needs to notice.
            //this.mEffect = this.Game.Content.Load<Effect>(this.mEffectAssetName);

			try {
				string filename = "KryptonEffect.mgfx";
				AssetManager assetManager = Activity.ApplicationContext.Assets;
				using (System.IO.Stream stream = assetManager.Open(filename, Access.Buffer))
				{
					var ms = new System.IO.MemoryStream();
					stream.CopyTo (ms);
					this.mEffect = new Effect(this.GraphicsDevice, ms.ToArray());
				}
			} catch (Exception) {
				//Log.Error (TAG, string.Format("Failed to read application key exception={0}", e));
			}

            this.RenderHelper = new KryptonRenderHelper(this.GraphicsDevice, this.mEffect);

            this.CreateRenderTargets();
        }

        /// <summary>
        /// Unload's the graphics content required to draw light maps
        /// </summary>
        protected override void UnloadContent()
        {
            this.DisposeRenderTargets();
        }

        /// <summary>
        /// Creates render targets
        /// </summary>
        private void CreateRenderTargets()
        {
            var targetWidth = GraphicsDevice.Viewport.Width / (int)(this.mLightMapSize);
            var targetHeight = GraphicsDevice.Viewport.Height / (int)(this.mLightMapSize);

            this.mMap = new RenderTarget2D(GraphicsDevice, targetWidth, targetHeight, false, SurfaceFormat.Color, DepthFormat.Depth24Stencil8, 0, RenderTargetUsage.PlatformContents);
            this.mMapBlur = new RenderTarget2D(GraphicsDevice, targetWidth, targetHeight, false, SurfaceFormat.Color, DepthFormat.Depth24Stencil8, 0, RenderTargetUsage.PlatformContents);
        }

        /// <summary>
        /// Disposes of render targets
        /// </summary>
        private void DisposeRenderTargets()
        {
            KryptonEngine.TryDispose(this.mMap);
            KryptonEngine.TryDispose(this.mMapBlur);
        }

        /// <summary>
        /// Attempts to dispose of disposable objects, and assigns them a null value afterward
        /// </summary>
        /// <param name="obj"></param>
        private static void TryDispose(IDisposable obj)
        {
            if (obj != null)
            {
                obj.Dispose();
                obj = null;
            }
        }

        /// <summary>
        /// Draws the light map to the current render target
        /// </summary>
        /// <param name="gameTime">N/A - Required</param>
        public override void Draw(GameTime gameTime)
        {
            this.LightMapPresent();
        }

        /// <summary>
        /// Prepares the light map to be drawn (pre-render)
        /// </summary>
        public void LightMapPrepare()
        {
            // Prepare and set the matrix
            var viewWidth = this.GraphicsDevice.ScissorRectangle.Width;
            var viewHeight = this.GraphicsDevice.ScissorRectangle.Height;

            // Prepare the matrix with optional settings and assign it to an effect parameter
            Matrix lightMapMatrix = this.LightmapMatrixGet();
            this.mEffect.Parameters["Matrix"].SetValue(lightMapMatrix);

            // Obtain the original rendering states
            var originalRenderTargets = this.GraphicsDevice.GetRenderTargets();

            // Set and clear the target
            this.GraphicsDevice.SetRenderTarget(this.mMap);
            this.GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.Stencil, this.mAmbientColor, 0, 1);

            // Make sure we're culling the right way!
            this.GraphicsDevice.RasterizerState = KryptonEngine.RasterizerStateGetFromCullMode(this.mCullMode);

            // put the render target's size into a more friendly format
            var targetSize = new Vector2(this.mMap.Width, this.mMap.Height);

            // Render Light Maps
            foreach (var light in this.mLights)
            {
                // Loop through each light within the view frustum
                if (light.Bounds.Intersects(this.mBounds))
                {
                    // Clear the stencil and set the scissor rect (because we're stretching geometry past the light's reach)
                    this.GraphicsDevice.Clear(ClearOptions.Stencil, Color.Black, 0, 1);
                    this.GraphicsDevice.ScissorRectangle = KryptonEngine.ScissorRectCreateForLight(light, lightMapMatrix, targetSize);
                    
                    // Draw the light!
                    light.Draw(this.RenderHelper, this.mHulls);
                }
            }

            if (this.mBluriness > 0)
            {
                // Blur the shadow map horizontally to the blur target
                this.GraphicsDevice.SetRenderTarget(this.mMapBlur);
                this.RenderHelper.BlurTextureToTarget(this.mMap, LightMapSize.Full, BlurTechnique.Horizontal, this.mBluriness);

                // Blur the shadow map vertically back to the final map
                this.GraphicsDevice.SetRenderTarget(this.mMap);
                this.RenderHelper.BlurTextureToTarget(this.mMapBlur, LightMapSize.Full, BlurTechnique.Vertical, this.mBluriness);
            }

            // Reset to the original rendering states
            this.GraphicsDevice.SetRenderTargets(originalRenderTargets);
        }

        /// <summary>
        /// Returns the final, modified matrix used to render the lightmap.
        /// </summary>
        /// <returns></returns>
        private Matrix LightmapMatrixGet()
        {
            if (this.mSpriteBatchCompatabilityEnabled)
            {
                float xScale = (this.GraphicsDevice.Viewport.Width > 0) ? (1f / this.GraphicsDevice.Viewport.Width) : 0f;
                float yScale = (this.GraphicsDevice.Viewport.Height > 0) ? (-1f / this.GraphicsDevice.Viewport.Height) : 0f;

                // This is the default matrix used to render sprites via spritebatch
                var matrixSpriteBatch = new Matrix()
                {
                    M11 = xScale * 2f,
                    M22 = yScale * 2f,
                    M33 = 1f,
                    M44 = 1f,
                    M41 = -1f - xScale,
                    M42 = 1f - yScale,
                };

                // Return krypton's matrix, compensated for use with SpriteBatch
                return this.mWVP * matrixSpriteBatch;
            }
            else
            {
                // Return krypton's matrix
                return this.mWVP;
            }
        }

        /// <summary>
        /// Gets a pixel-space rectangle which contains the light passed in
        /// </summary>
        /// <param name="light">The light used to create the rectangle</param>
        /// <param name="matrix">the WorldViewProjection matrix being used to render</param>
        /// <param name="targetSize">The rendertarget's size</param>
        /// <returns></returns>
        private static Rectangle ScissorRectCreateForLight(ILight2D light, Microsoft.Xna.Framework.Matrix matrix, Vector2 targetSize)
        {
            // This needs refining, but it works as is (I believe)
            var lightBounds = light.Bounds;

            var min = KryptonEngine.VectorToPixel(lightBounds.Min, matrix, targetSize);
            var max = KryptonEngine.VectorToPixel(lightBounds.Max, matrix, targetSize);

            var min2 = Vector2.Min(min, max);
            var max2 = Vector2.Max(min, max);

            min = Vector2.Clamp(min2, Vector2.Zero, targetSize);
            max = Vector2.Clamp(max2, Vector2.Zero, targetSize);

            return new Rectangle((int)(min.X), (int)(min.Y), (int)(max.X - min.X), (int)(max.Y - min.Y));
        }

        /// <summary>
        /// Takes a screen-space vector and puts it in to pixel space
        /// </summary>
        /// <param name="v"></param>
        /// <param name="matrix"></param>
        /// <param name="targetSize"></param>
        /// <returns></returns>
        private static Vector2 VectorToPixel(Vector2 v, Matrix matrix, Vector2 targetSize)
        {
            Vector2.Transform(ref v, ref matrix, out v);

            v.X = (1 + v.X) * (targetSize.X / 2f);
            v.Y = (1 - v.Y) * (targetSize.Y / 2f);

            return v;
        }

        /// <summary>
        /// Takes a screen-space size vector and converts it to a pixel-space size vector
        /// </summary>
        /// <param name="v"></param>
        /// <param name="matrix"></param>
        /// <param name="targetSize"></param>
        /// <returns></returns>
        private static Vector2 ScaleToPixel(Vector2 v, Matrix matrix, Vector2 targetSize)
        {
            v.X *= matrix.M11 * (targetSize.X / 2f);
            v.Y *= matrix.M22 * (targetSize.Y / 2f);

            return v;
        }

        /// <summary>
        /// Retrieves a rasterize state by using the cull mode as a lookup
        /// </summary>
        /// <param name="cullMode">The cullmode used to lookup the rasterize state</param>
        /// <returns></returns>
        private static RasterizerState RasterizerStateGetFromCullMode(CullMode cullMode)
        {
            switch (cullMode)
            {
                case (CullMode.CullCounterClockwiseFace):
                    return RasterizerState.CullCounterClockwise;

                case (CullMode.CullClockwiseFace):
                    return RasterizerState.CullClockwise;

                default:
                    return RasterizerState.CullNone;
            }
        }

        /// <summary>
        /// Presents the light map to the current render target
        /// </summary>
        private void LightMapPresent()
        {
            RenderHelper.DrawTextureToTarget(this.mMap, this.mLightMapSize, BlendTechnique.Multiply);
        }
    }
}
