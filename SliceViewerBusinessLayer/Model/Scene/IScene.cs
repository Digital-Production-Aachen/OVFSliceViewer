﻿using OpenTK;
using OpenTK.Mathematics;
using OVFSliceViewerCore.Model.Part;
using OVFSliceViewerCore.Model.RenderData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace OVFSliceViewerCore.Model
{
    public interface IScene: IDisposable
    {
        Task LoadFile(FileInfo fileInfo);
        void CloseFile();
        void Render();
        Vector2 GetCenter();
        IEnumerable<AbstrPart> PartsInScene { get; }
        Task LoadWorkplaneToBuffer(int index);
        int GetNumberOfLinesInWorkplane();
        Vector3 LastPosition { get; }
        OVFFileInfo OVFFileInfo { get; }
        SceneSettings SceneSettings { get; }
        void ChangeNumberOfLinesToDraw(int numberOfLinesToDraw);

    }
}
