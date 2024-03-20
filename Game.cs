using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using System.Drawing.Imaging;

namespace Proiect
{
    public class Game
    {
        double theta1 = 0.0;
        double theta = 0.0;
        float rotationSpeed = 2.0f;
        //int textureID;
        GameWindow window;
        List<int> textureIDs = new List<int>();
        public Game(GameWindow window)
        {
            this.window = window;
            Start();
        }

        void Start()
        {
            window.Load += loaded;
            window.Resize += resize;
            window.RenderFrame += renderF;

           
            window.KeyDown += keyDown;

            window.Run(1.0 / 60.0);
        }

        void keyDown(object sender, OpenTK.Input.KeyboardKeyEventArgs e)
        {
            
            switch (e.Key)
            {
                case OpenTK.Input.Key.Left:
                    theta += rotationSpeed;
                    break;
                case OpenTK.Input.Key.Right:
                    theta -= rotationSpeed;
                    break;
                case OpenTK.Input.Key.Up:
                    theta1 += rotationSpeed;
                    break;
                case OpenTK.Input.Key.Down:
                    theta1 -= rotationSpeed;
                    break;
            }
        }

        void resize(Object o, EventArgs e)
        {
            GL.Viewport(0, 0, window.Width, window.Height);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            Matrix4 matrix = Matrix4.CreatePerspectiveFieldOfView(1.0f, (float)window.Width / (float)window.Height, 1.0f, 100.0f);
            GL.LoadMatrix(ref matrix);
            GL.MatrixMode(MatrixMode.Modelview);
        }
        void renderF(Object o, EventArgs e)
        {
            GL.LoadIdentity();
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Translate(0.0, 0.0, -45.0);

            float distanceBetweenCubes = 12.0f;

            int cubeIndex = 1; 
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    GL.PushMatrix();
                    GL.Translate(i * distanceBetweenCubes, j * distanceBetweenCubes, 0.0);
                    GL.Rotate(theta, 0.0, 1.0, 0.0);
                    GL.Rotate(theta1, 1.0, 0.0, 0.0);
                    DrawCube(cubeIndex);
                    GL.PopMatrix();

                    cubeIndex++;
                }
            }

            window.SwapBuffers();
        }



        void DrawCube(int cubeIndex)
        {
            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, textureIDs[cubeIndex]);
            GL.Begin(BeginMode.Quads);

            GL.Color3(1.0, 1.0, 1.0);
            //front
            GL.Normal3(-1.0, 0.0, 0.0);
            GL.TexCoord2(0.0, 0.0);
            GL.Vertex3(-4.0, 4.0, 4.0);
            GL.TexCoord2(1.0, 0.0);
            GL.Vertex3(-4.0, 4.0, -4.0);
            GL.TexCoord2(1.0, 1.0);
            GL.Vertex3(-4.0, -4.0, -4.0);
            GL.TexCoord2(0.0, 1.0);
            GL.Vertex3(-4.0, -4.0, 4.0);
            //back
            GL.Normal3(1.0, 0.0, 0.0);
            GL.Vertex3(4.0, 4.0, 4.0);
            GL.Vertex3(4.0, 4.0, -4.0);
            GL.Vertex3(4.0, -4.0, -4.0);
            GL.Vertex3(4.0, -4.0, 4.0);
            //top
            GL.Normal3(0.0, -1.0, 0.0);
            GL.Vertex3(4.0, -4.0, 4.0);
            GL.Vertex3(4.0, -4.0, -4.0);
            GL.Vertex3(-4.0, -4.0, -4.0);
            GL.Vertex3(-4.0, -4.0, 4.0);
            //bottom
            GL.Normal3(0.0, 1.0, 0.0);
            GL.Vertex3(4.0, 4.0, 4.0);
            GL.Vertex3(4.0, 4.0, -4.0);
            GL.Vertex3(-4.0, 4.0, -4.0);
            GL.Vertex3(-4.0, 4.0, 4.0);
            //right
            GL.Normal3(0.0, 0.0, -1.0);
            GL.Vertex3(4.0, 4.0, -4.0);
            GL.Vertex3(4.0, -4.0, -4.0);
            GL.Vertex3(-4.0, -4.0, -4.0);
            GL.Vertex3(-4.0, 4.0, -4.0);
            //left
            GL.Normal3(0.0, 0.0, 1.0);
            GL.Vertex3(4.0, 4.0, 4.0);
            GL.Vertex3(4.0, -4.0, 4.0);
            GL.Vertex3(-4.0, -4.0, 4.0);
            GL.Vertex3(-4.0, 4.0, 4.0);
            GL.End();
            ApplySaltAndPepperNoise(textureIDs[cubeIndex]);

            GL.End();

            GL.Disable(EnableCap.Texture2D);
        }

        void loaded(object o, EventArgs e)
        {
            GL.ClearColor(0.0f, 0.0f, 0.0f, 0.0f);
            GL.Enable(EnableCap.DepthTest);

            
            int numCubes = 10; 
            for (int i = 1; i <= numCubes; i++)
            {
                string filePath = $"C:\\Users\\crist\\OneDrive\\Desktop\\PozeProiectGC\\imagine{i}a.jpg";
                int textureID = LoadTexture(filePath);
                textureIDs.Add(textureID);
            }
            //sa fie lumina
            GL.Enable(EnableCap.Lighting);
            GL.Enable(EnableCap.ColorMaterial);

            float[] light_position = { 20, 20, 80 };
            float[] light_diffuse = { 1.0f, 0.0f, 0.0f };
            float[] light_ambient = { 1.0f, 0.0f, 0.0f };

            GL.Light(LightName.Light0, LightParameter.Position, light_position);
            //GL.Light(LightName.Light0, LightParameter.Diffuse, light_diffuse);
            GL.Light(LightName.Light0, LightParameter.Ambient, light_ambient);

            GL.Enable(EnableCap.Light0);
        }
        int LoadTexture(string filePath)
        {
            int id = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, id);

            Bitmap bmp = new Bitmap(filePath);
            BitmapData data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height),
                ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0,
                OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);

            bmp.UnlockBits(data);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            return id;
        }
        void ApplySaltAndPepperNoise(int textureID)
        {
            GL.BindTexture(TextureTarget.Texture2D, textureID);

            // Obtine dimensiunile texturii
            int textureWidth, textureHeight;
            GL.GetTexLevelParameter(TextureTarget.Texture2D, 0, GetTextureParameter.TextureWidth, out textureWidth);
            GL.GetTexLevelParameter(TextureTarget.Texture2D, 0, GetTextureParameter.TextureHeight, out textureHeight);

            Random rand = new Random();

            for (int i = 0; i < textureHeight; i++)
            {
                for (int j = 0; j < textureWidth; j++)
                {
                    if (rand.NextDouble() < 0.02) // 2% probability for salt and pepper noise
                    {
                        // Adauga zgomot de sare ai piper
                        byte value = (byte)(rand.NextDouble() < 0.5 ? 0 : 255);
                        GL.TexSubImage2D(TextureTarget.Texture2D, 0, j, i, 1, 1,
                            OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, new byte[] { value, value, value, 255 });
                    }
                }
            }
        }
    }
}
