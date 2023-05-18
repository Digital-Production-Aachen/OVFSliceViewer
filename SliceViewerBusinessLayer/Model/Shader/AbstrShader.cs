namespace SliceViewerBusinessLayer.Model.Shader
{
    public class AbstrShader
    {
        protected AbstrShader() { }
        private static AbstrShader _instance = new AbstrShader();
        public static AbstrShader GetInstance => _instance;
        public string Shader;
    }
}
