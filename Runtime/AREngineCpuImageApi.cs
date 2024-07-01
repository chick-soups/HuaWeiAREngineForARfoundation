using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARSubsystems;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Threading;

namespace UnityEngine.XR.AREngine
{

    public class AREngineCpuImageApi : XRCpuImage.Api
    {
        private static AREngineCpuImageApi s_Instance;
        public static AREngineCpuImageApi Instance
        {
            get
            {
                if (s_Instance == null)
                {
                    s_Instance = new AREngineCpuImageApi();
                }
                return s_Instance;
            }
        }

        private Dictionary<int, Task> taskDic;
        private Dictionary<int, GCHandle> dataDic;
        public AREngineCpuImageApi()
        {
            taskDic = new Dictionary<int, Task>();
            dataDic = new Dictionary<int, GCHandle>();
        }


        public override int ConvertAsync(int nativeHandle, XRCpuImage.ConversionParams conversionParams)
        {
            Debug.Log("ConvertAsync:" + nativeHandle);
            IntPtr intPtr = (IntPtr)nativeHandle;
            GCHandle gCHandle = GCHandle.FromIntPtr(intPtr);
            byte[] bytes = (byte[])gCHandle.Target;
            TextureFormat textureFormat = conversionParams.outputFormat;


            Task task = Task.Factory.StartNew(() =>
            {
                int width = conversionParams.inputRect.width;
                int height = conversionParams.inputRect.height;
                int targetWidth = conversionParams.outputDimensions.x;
                int targetHeight = conversionParams.outputDimensions.y;
                bool mirrorX = conversionParams.transformation.HasFlag(XRCpuImage.Transformation.MirrorX);
                bool mirrorY = conversionParams.transformation.HasFlag(XRCpuImage.Transformation.MirrorY);
                bytes = TextureUtils.DecodeYUV2RGB(bytes, TextureUtils.YuvType.NV12, width, height);
                bytes = TextureUtils.ResizeTexture(bytes, width, height,
                TextureUtils.ImageFilterMode.Biliner, targetWidth, targetHeight, mirrorX, mirrorY);
                switch (textureFormat)
                {
                    case TextureFormat.RGB24:

                        break;
                    case TextureFormat.RGBA32:
                        bytes = TextureUtils.RGB2RGBA32(bytes);
                        break;
                    case TextureFormat.ARGB32:
                        bytes = TextureUtils.RGB2ARGB32(bytes);
                        break;
                    case TextureFormat.BGRA32:
                        bytes = TextureUtils.RGB2BGRA32(bytes);
                        break;
                    default:
                        throw new NotImplementedException();

                }
                GCHandle handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
                if (dataDic.ContainsKey(nativeHandle))
                {
                    Debug.Log("zxh have data:" + nativeHandle);
                    dataDic[nativeHandle] = handle;
                }
                else
                {
                    dataDic.Add(nativeHandle, handle);

                }

            });
            if (taskDic.ContainsKey(nativeHandle))
            {
                Debug.Log("zxh have task:" + nativeHandle);
                taskDic[nativeHandle] = task;

            }
            else
            {
                taskDic.Add(nativeHandle, task);
            }

            return nativeHandle;
        }
        public override void DisposeAsyncRequest(int requestId)
        {
            IntPtr intPtr = (IntPtr)requestId;
            GCHandle gCHandle = GCHandle.FromIntPtr(intPtr);
            gCHandle.Free();
            Debug.Log("zxh DisposeAsyncRequest:requestId:" + requestId);
            if (taskDic.ContainsKey(requestId))
            {
                taskDic[requestId].Dispose();
                taskDic.Remove(requestId);
                Debug.Log("zxh DisposeAsyncRequest:requestId:1");
            }
            if (dataDic.ContainsKey(requestId))
            {
                dataDic[requestId].Free();
                dataDic.Remove(requestId);
                Debug.Log("zxh DisposeAsyncRequest:requestId:2");
            }

        }

        static readonly HashSet<TextureFormat> s_SupportedVideoConversionFormats = new HashSet<TextureFormat>
        {
            // TextureFormat.Alpha8,
            // TextureFormat.R8,
            // TextureFormat.R16,
            // TextureFormat.RFloat,
            TextureFormat.RGB24,
            TextureFormat.RGBA32,
            TextureFormat.ARGB32,
            TextureFormat.BGRA32,
        };
        public override bool FormatSupported(XRCpuImage image, TextureFormat format) => (((image.format == XRCpuImage.Format.AndroidYuv420_888) || (image.format == XRCpuImage.Format.DepthUint16) || (image.format == XRCpuImage.Format.OneComponent8))
                && s_SupportedVideoConversionFormats.Contains(format));
        public override void ConvertAsync(int nativeHandle, XRCpuImage.ConversionParams conversionParams, OnImageRequestCompleteDelegate callback, IntPtr context)
        {
             Debug.Log("ConvertAsync:" + nativeHandle);
            IntPtr intPtr = (IntPtr)nativeHandle;
            GCHandle gCHandle = GCHandle.FromIntPtr(intPtr);
            byte[] bytes = (byte[])gCHandle.Target;
            TextureFormat textureFormat = conversionParams.outputFormat;
             SynchronizationContext synchronizationContext = SynchronizationContext.Current;

            Task task = Task.Factory.StartNew(() =>
            {
            try
            {
                int width = conversionParams.inputRect.width;
                int height = conversionParams.inputRect.height;
                int targetWidth = conversionParams.outputDimensions.x;
                int targetHeight = conversionParams.outputDimensions.y;
                bool mirrorX = conversionParams.transformation.HasFlag(XRCpuImage.Transformation.MirrorX);
                bool mirrorY = conversionParams.transformation.HasFlag(XRCpuImage.Transformation.MirrorY);
                bytes = TextureUtils.DecodeYUV2RGB(bytes, TextureUtils.YuvType.NV12, width, height);
                bytes = TextureUtils.ResizeTexture(bytes, width, height,
                TextureUtils.ImageFilterMode.Biliner, targetWidth, targetHeight, mirrorX, mirrorY);
                switch (textureFormat)
                {
                    case TextureFormat.RGB24:

                        break;
                    case TextureFormat.RGBA32:
                        bytes = TextureUtils.RGB2RGBA32(bytes);
                        break;
                    case TextureFormat.ARGB32:
                        bytes = TextureUtils.RGB2ARGB32(bytes);
                        break;
                    case TextureFormat.BGRA32:
                        bytes = TextureUtils.RGB2BGRA32(bytes);
                        break;
                    default:
                        throw new NotImplementedException();

                }
                GCHandle handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
                if (dataDic.ContainsKey(nativeHandle))
                {
                    Debug.Log("zxh have data:" + nativeHandle);
                    dataDic[nativeHandle] = handle;
                }
                else
                {
                    dataDic.Add(nativeHandle, handle);

                }
                synchronizationContext.Post(state =>
                {
                    callback(XRCpuImage.AsyncConversionStatus.Ready, conversionParams, handle.AddrOfPinnedObject(), bytes.Length, context);
                }, null);

            }catch(Exception e){
                Debug.LogError(e.Message);
                Debug.LogError(e.StackTrace);
                   synchronizationContext.Post(state =>
                {
                    callback(XRCpuImage.AsyncConversionStatus.Failed, conversionParams,IntPtr.Zero,0, context);
                }, null);
            }
        });
            if (taskDic.ContainsKey(nativeHandle))
            {
                Debug.Log("zxh have task:" + nativeHandle);
                taskDic[nativeHandle] = task;

            }
            else
            {
                taskDic.Add(nativeHandle, task);

            }

        }



        public override void DisposeImage(int nativeHandle)
        {
            Debug.Log("DisposeImage:" + nativeHandle);
            IntPtr intPtr = (IntPtr)nativeHandle;
            GCHandle gCHandle = GCHandle.FromIntPtr(intPtr);
            gCHandle.Free();
            if (taskDic.ContainsKey(nativeHandle))
            {
                taskDic[nativeHandle].Dispose();
                taskDic.Remove(nativeHandle);
            }
            if (dataDic.ContainsKey(nativeHandle))
            {
                dataDic[nativeHandle].Free();
                dataDic.Remove(nativeHandle);
            }
        }
        public override XRCpuImage.AsyncConversionStatus GetAsyncRequestStatus(int requestId)
        {
            Debug.Log("GetAsyncRequestStatus:" + requestId);
            if (taskDic.ContainsKey(requestId))
            {
                Task task = taskDic[requestId];
                if (task.IsCompletedSuccessfully)
                {
                    return XRCpuImage.AsyncConversionStatus.Ready;
                }
                else if (task.IsFaulted)
                {
                    return XRCpuImage.AsyncConversionStatus.Failed;
                }
                else if (task.IsCanceled)
                {
                    return XRCpuImage.AsyncConversionStatus.Disposed;
                }
                else
                {
                    return XRCpuImage.AsyncConversionStatus.Processing;
                }

            }
            else
            {
                return XRCpuImage.AsyncConversionStatus.Disposed;
            }
        }
        public override bool NativeHandleValid(int nativeHandle)
        {
            Debug.Log("zxh NativeHandleValid:" + nativeHandle);
            try
            {
                IntPtr intPtr = (IntPtr)nativeHandle;
                GCHandle gCHandle = GCHandle.FromIntPtr(intPtr);
                byte[] bytes = (byte[])gCHandle.Target;
                if (bytes != null)
                {
                    Debug.Log("zxh NativeHandleValid:1");
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
                Debug.LogError(e.StackTrace);
                return false;
            }


        }
        public override bool TryConvert(int nativeHandle, XRCpuImage.ConversionParams conversionParams, IntPtr destinationBuffer, int bufferLength)
        {

            try
            {
                IntPtr intPtr = (IntPtr)nativeHandle;
                GCHandle gCHandle = GCHandle.FromIntPtr(intPtr);
                byte[] bytes = (byte[])gCHandle.Target;
                int width = conversionParams.inputRect.width;
                int height = conversionParams.inputRect.height;
                int targetWidth = conversionParams.outputDimensions.x;
                int targetHeight = conversionParams.outputDimensions.y;
                bool mirrorX = conversionParams.transformation.HasFlag(XRCpuImage.Transformation.MirrorX);
                bool mirrorY = conversionParams.transformation.HasFlag(XRCpuImage.Transformation.MirrorY);
                bytes = TextureUtils.DecodeYUV2RGB(bytes, TextureUtils.YuvType.NV12, width, height);
                bytes = TextureUtils.ResizeTexture(bytes, width, height,
                TextureUtils.ImageFilterMode.Biliner, targetWidth, targetHeight, mirrorX, mirrorY);
                TextureFormat textureFormat = conversionParams.outputFormat;
                switch (textureFormat)
                {
                    case TextureFormat.RGB24:

                        break;
                    case TextureFormat.RGBA32:
                        bytes = TextureUtils.RGB2RGBA32(bytes);
                        break;
                    case TextureFormat.ARGB32:
                        bytes = TextureUtils.RGB2ARGB32(bytes);
                        break;
                    case TextureFormat.BGRA32:
                        bytes = TextureUtils.RGB2BGRA32(bytes);
                        break;
                    default:
                        throw new NotImplementedException();
                }
                Marshal.Copy(bytes, 0, destinationBuffer, bytes.Length);
                return true;

            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
                Debug.LogError(e.StackTrace);
                return false;
            }

        }
        public override bool TryGetAsyncRequestData(int requestId, out IntPtr dataPtr, out int dataLength)
        {
            Debug.Log("TryGetAsyncRequestData:" + requestId);
            if (dataDic.ContainsKey(requestId))
            {
                GCHandle handle = dataDic[requestId];
                dataPtr = handle.AddrOfPinnedObject();
                byte[] bytes = (byte[])dataDic[requestId].Target;
                dataLength = bytes.Length;
                return true;
            }
            else
            {
                dataPtr = IntPtr.Zero;
                dataLength = 0;
                return false;
            }
        }
        public override bool TryGetConvertedDataSize(int nativeHandle, Vector2Int dimensions, TextureFormat format, out int size)
        {
            if (s_SupportedVideoConversionFormats.Contains(format))
            {
                size = dimensions.x * dimensions.y;
                int bytePerPixel = 0;
                switch (format)
                {
                    case TextureFormat.R16:
                        bytePerPixel = 2;
                        break;
                    case TextureFormat.RGB24:
                        bytePerPixel = 3;
                        break;
                    case TextureFormat.RGBA32:
                    case TextureFormat.ARGB32:
                    case TextureFormat.BGRA32:
                    case TextureFormat.RFloat:
                        bytePerPixel = 4;
                        break;
                    default:
                        bytePerPixel = 1;
                        break;
                }

                size *= bytePerPixel;
                return true;
            }
            else
            {
                size = 0;
                return false;
            }


        }
        public override bool TryGetPlane(int nativeHandle, int planeIndex, out XRCpuImage.Plane.Cinfo planeCinfo)
        {
            try
            {
                HwArFrame hwArFrame = UnityEngine.XR.AREngine.Utility.LastARFrame;
                if (hwArFrame == null)
                {
                    planeCinfo = default;
                    return false;
                }
                HwArImage hwArImage = hwArFrame.AcquireCameraImage();
                AImage aImage = hwArImage.GetNdkImage();
                int width = aImage.GetWidth();
                int height = aImage.GetHeight();
                hwArImage.Dispose();
                IntPtr intPtr = (IntPtr)nativeHandle;
                GCHandle gCHandle = GCHandle.FromIntPtr(intPtr);
                IntPtr dataPtr = gCHandle.AddrOfPinnedObject();
                byte[] bytes = (byte[])gCHandle.Target;
                int dataLength = 0;
                int rowStride = width;
                int pixelStride = 1;
                int size = sizeof(byte);
                switch (planeIndex)
                {
                    case 0:
                        pixelStride = 1;
                        rowStride = width;
                        dataLength = width * height;
                        break;
                    case 1:
                        dataPtr += (width * height) * size;
                        rowStride = width;
                        pixelStride = 2;
                        dataLength = width * height / 4;
                        break;
                    case 2:
                        dataPtr += (width * height + 1) * size;
                        rowStride = width;
                        pixelStride = 2;
                        dataLength = width * height / 4;
                        break;
                }
                planeCinfo = new XRCpuImage.Plane.Cinfo(dataPtr, dataLength, rowStride, pixelStride);
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
                Debug.LogError(e.StackTrace);
                planeCinfo = default;
                return false;
            }


        }
    }
}