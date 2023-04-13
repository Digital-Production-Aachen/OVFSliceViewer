using OpenGlGuiLibrary;
using OpenGlGuiLibrary.GuiElements;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Diagnostics;
using OVFSliceViewerCore.Model;
using OVFSliceViewerCore.Classes;
using OpenVectorFormat.AbstractReaderWriter;

namespace SliceViewerBusinessLayer
{
    public class Display : GameWindow, ICanvas
    {
        //Shader _shader;
        Gui _gui;
        DisplaySettings _displaySettings;
        Label _label;

        MotionTracker _cameraMotion = new MotionTracker();
        public SceneController Scene { get; set; }
        //public PartSlicer Slicer { get; set; }
        private FileReader _sliceReader;

        public float Width => _displaySettings.Width;

        public float Height => _displaySettings.Height;

        public Display(FileReader sliceReader, GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSetting) : base(gameWindowSettings, nativeWindowSetting)

        {
            Debug.WriteLine(GL.GetError());
            //Slicer = slicer;
            _sliceReader = sliceReader;

            this.Title = "Slic Three R";
            this._displaySettings = new DisplaySettings(nativeWindowSetting.Size.X, nativeWindowSetting.Size.Y);

            Scene = new SceneController(this);
            Debug.WriteLine(GL.GetError());
            MakeCurrent();
            Debug.WriteLine(GL.GetError());
            Scene.LoadFile(sliceReader);
            Debug.WriteLine(GL.GetError());
            //_cameraMotion = new CameraMotion(Scene.Camera);
        }
        //bool changeOffsetValue = false;
        int _layernumber = 0;
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            KeyboardState input = KeyboardState.GetSnapshot();
            //var mouse = this.MouseState;

            if (input.IsKeyDown(Keys.Escape))
            {
                this.Close();
            }
            else if (input.IsKeyDown(Keys.Space))
            {
                //_shader.Recompile();
            }
            else if ((input.IsKeyDown(Keys.LeftAlt) || input.IsKeyDown(Keys.RightAlt)) && input.IsKeyDown(Keys.Up))
            {
            }

            else if (CheckInputForNumbersAndProcess())
            {

            }
            else if ((input.IsKeyDown(Keys.Comma) && !_lastKeyState.IsKeyDown(Keys.Comma)) || (input.IsKeyDown(Keys.KeyPadDecimal) && !_lastKeyState.IsKeyDown(Keys.KeyPadDecimal)))
            {
                _input += ",";
            }
            else if ((input.IsKeyDown(Keys.Enter) && !_lastKeyState.IsKeyDown(Keys.Enter)) || (input.IsKeyDown(Keys.KeyPadEnter) && !_lastKeyState.IsKeyDown(Keys.KeyPadEnter)))
            {
                try
                {
                    if (_input.Contains(","))
                    {
                        float.TryParse(_input, System.Globalization.NumberStyles.AllowDecimalPoint, null, out float offset);
                        _input = "";

                        if (_sliceReader.GetType().GetProperty("PartSettings") != null)
                        {
                            var type = _sliceReader.GetType();
                            bool hasPartSettings = type.GetProperty("PartSettings") != null;

                            if (hasPartSettings)
                            {
                                var part = (OpenVectorFormat.Part)type.GetProperty("PartSettings").GetValue(_sliceReader);
                                part.ProcessStrategy.ContourOffsetInMm = offset;
                            }
                        }

                        Scene.Scene.LoadWorkplaneToBuffer(_layernumber);//.LoadWorkplane(workplane.GetAwaiter().GetResult()).GetAwaiter().GetResult();
                        //Scene.SetNumberOfLines(Scene.MaxNumberOfLines);
                        //Scene.SetHighlightColors(1);
                    }
                    else
                    {
                        _layernumber = Convert.ToInt32(_input);
                        _input = "";

                        //var workplane = Slicer.SliceLayerAsync(_layernumber);
                        _label.SetText("Layer: " + _layernumber);
                        Scene.Scene.LoadWorkplaneToBuffer(_layernumber);

                        //Scene.LoadWorkplane(workplane.GetAwaiter().GetResult()).GetAwaiter().GetResult();
                        //Scene.SetNumberOfLines(Scene.MaxNumberOfLines);
                        //Scene.SetHighlightColors(1);
                    }

                }
                catch (Exception)
                {
                    _input = "";
                }
            }

            _lastKeyState = input;
            base.OnUpdateFrame(e);
        }

        string _input = "";
        KeyboardState _lastKeyState;
        bool CheckInputForNumbersAndProcess()
        {
            KeyboardState input = KeyboardState;
            if ((input.IsKeyDown(Keys.D0) && !_lastKeyState.IsKeyDown(Keys.D0)) || (input.IsKeyDown(Keys.KeyPad0) && !_lastKeyState.IsKeyDown(Keys.KeyPad0)))
            {
                _input += "0";
                return true;
            }
            else if ((input.IsKeyDown(Keys.D1) && !_lastKeyState.IsKeyDown(Keys.D1)) || (input.IsKeyDown(Keys.KeyPad1) && !_lastKeyState.IsKeyDown(Keys.KeyPad1)))
            {
                _input += "1";
                return true;
            }
            else if ((input.IsKeyDown(Keys.D2) && !_lastKeyState.IsKeyDown(Keys.D2)) || (input.IsKeyDown(Keys.KeyPad2) && !_lastKeyState.IsKeyDown(Keys.KeyPad2)))
            {
                _input += "2";
                return true;
            }
            else if ((input.IsKeyDown(Keys.D3) && !_lastKeyState.IsKeyDown(Keys.D3)) || (input.IsKeyDown(Keys.KeyPad3) && !_lastKeyState.IsKeyDown(Keys.KeyPad3)))
            {
                _input += "3";
                return true;
            }
            else if ((input.IsKeyDown(Keys.D4) && !_lastKeyState.IsKeyDown(Keys.D4)) || (input.IsKeyDown(Keys.KeyPad4) && !_lastKeyState.IsKeyDown(Keys.KeyPad4)))
            {
                _input += "4";
                return true;
            }
            else if ((input.IsKeyDown(Keys.D5) && !_lastKeyState.IsKeyDown(Keys.D5)) || (input.IsKeyDown(Keys.KeyPad5) && !_lastKeyState.IsKeyDown(Keys.KeyPad5)))
            {
                _input += "5";
                return true;
            }
            else if ((input.IsKeyDown(Keys.D6) && !_lastKeyState.IsKeyDown(Keys.D6)) || (input.IsKeyDown(Keys.KeyPad6) && !_lastKeyState.IsKeyDown(Keys.KeyPad6)))
            {
                _input += "6";
                return true;
            }
            else if ((input.IsKeyDown(Keys.D7) && !_lastKeyState.IsKeyDown(Keys.D7)) || (input.IsKeyDown(Keys.KeyPad7) && !_lastKeyState.IsKeyDown(Keys.KeyPad7)))
            {
                _input += "7";
                return true;
            }
            else if ((input.IsKeyDown(Keys.D8) && !_lastKeyState.IsKeyDown(Keys.D8)) || (input.IsKeyDown(Keys.KeyPad8) && !_lastKeyState.IsKeyDown(Keys.KeyPad8)))
            {
                _input += "8";
                return true;
            }
            else if ((input.IsKeyDown(Keys.D9) && !_lastKeyState.IsKeyDown(Keys.D9)) || (input.IsKeyDown(Keys.KeyPad9) && !_lastKeyState.IsKeyDown(Keys.KeyPad9)))
            {
                _input += "9";
                return true;
            }
            return false;
        }
        protected override void OnLoad()
        {
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);

            Console.WriteLine(GL.GetError().ToString());


            _gui = new Gui(_displaySettings, this);

            _label = new Label(new Vector2i(160, 30), new Vector2i(10, 10), _gui.LabelShader, _displaySettings, ElementAnchors.TopRight);
            _label.SetText("Layer: 0");
            _gui.AddGuiElement(_label);

            //int temp;
            base.OnLoad();
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.Disable(EnableCap.Blend);

            if (Scene != null)
            {
                Scene.Render();
            }

            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            _gui.RenderAll();
            SwapBuffers();
            base.OnRenderFrame(args);
        }
        protected override void OnResize(ResizeEventArgs e)
        {
            //Scene.Painter.CanvasResize(e.Width, e.Height); // maybe this adjusts the camera
            Scene.Camera.Resize(e.Width, e.Height);
            _displaySettings.Resize(e.Width, e.Height);
            GL.Viewport(0, 0, e.Width, e.Height);
            ((ICanvas)this).Resize(e.Width, e.Height);
            base.OnResize(e);
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            if (e.Offset.Y > 0)
            {
                Scene.Camera.Zoom(true);
            }
            else
            {
                Scene.Camera.Zoom(false);
            }

            base.OnMouseWheel(e);
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            CursorVisible = false;
            CursorGrabbed = true;
            var position = new Vector2(MouseState.X, MouseState.Y);

            if (e.Button == MouseButton.Button3 || e.Button == MouseButton.Button1)
            {
                _cameraMotion.Start(new Vector2(position.X, position.Y));
            }
            else if (e.Button == MouseButton.Button2)
            {
                Scene.CenterView();
            }
            base.OnMouseDown(e);
        }
        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            CursorVisible = true;
            CursorGrabbed = false;

            _cameraMotion.Stop();

            base.OnMouseUp(e);
        }

        protected override void OnMouseMove(MouseMoveEventArgs e)
        {
            var position = new Vector2(MouseState.X, MouseState.Y);

            if (MouseState.IsButtonDown(MouseButton.Button3))
            {
                _cameraMotion.Move(new Vector2(position.X, position.Y), Scene.Camera.Move);
            }
            else if (MouseState.IsButtonDown(MouseButton.Button1))
            {
                _cameraMotion.Move(new Vector2(position.X, position.Y), Scene.Camera.Rotate);
            }
            base.OnMouseMove(e);
        }

        void ICanvas.Resize(int width, int height)
        {
            Scene.Camera.Resize(width, height);
            _displaySettings.Resize(width, height);
            GL.Viewport(0, 0, width, height);

            //throw new NotImplementedException();
        }

        public Tuple<int, int> GetCanvasArea()
        {
            throw new NotImplementedException();
        }
    }
}
