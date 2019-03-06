namespace UnityEngine.Experimental.Rendering.HDPipeline
{
    /// <summary>Properties computed during a frame rendering.</summary>
    public struct FrameProperties
    {
        /// <summary>The size of the output in pixels.</summary>
        public readonly Vector2Int outputSize;
        /// <summary>World to camera matrix. (Right Hand Side).</summary>
        public readonly Matrix4x4 cameraToWorldMatrixRHS;
        /// <summary>Projection matrix.</summary>
        public readonly Matrix4x4 projectionMatrix;

        /// <summary>Creates a new FrameProperties.</summary>
        /// <param name="outputSize"><see cref="outputSize"/></param>
        /// <param name="cameraToWorldMatrixRhs"><see cref="cameraToWorldMatrixRHS"/></param>
        /// <param name="projectionMatrix"><see cref="projectionMatrix"/></param>
        public FrameProperties(Vector2Int outputSize, Matrix4x4 cameraToWorldMatrixRhs, Matrix4x4 projectionMatrix)
        {
            this.outputSize = outputSize;
            cameraToWorldMatrixRHS = cameraToWorldMatrixRhs;
            this.projectionMatrix = projectionMatrix;
        }

        /// <summary>Creates a new FrameProperties from an <see cref="HDCamera"/>.</summary>
        /// <param name="hdCamera">The camera to use.</param>
        public static FrameProperties From(HDCamera hdCamera)
            => new FrameProperties(
                new Vector2Int(hdCamera.actualWidth, hdCamera.actualHeight),
                hdCamera.camera.cameraToWorldMatrix,
                hdCamera.projMatrix
            );
    }
}
