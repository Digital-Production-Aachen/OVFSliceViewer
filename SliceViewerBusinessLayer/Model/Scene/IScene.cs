using OpenTK;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace OVFSliceViewerBusinessLayer.Model
{
    public interface IScene: IDisposable
    {
        Task LoadFile(FileInfo fileInfo);
        void CloseFile();
        void Render();
        Vector2 GetCenter();
        IEnumerable<AbstrPart> PartsInScene { get; }
        void LoadWorkplaneToBuffer(int index);
        int GetNumberOfLinesInWorkplane();
        Vector3 LastPosition { get; }
        OVFFileInfo OVFFileInfo { get; }
        SceneSettings SceneSettings { get; }
        void ChangeNumberOfLinesToDraw(int numberOfLinesToDraw);

    }
}
