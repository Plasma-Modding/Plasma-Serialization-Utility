using System;
using System.Runtime.CompilerServices;

namespace Plasma.Classes
{
        public sealed class VideoClip : Object
        {
            // Token: 0x06000021 RID: 33 RVA: 0x000024F5 File Offset: 0x000006F5
            private VideoClip()
            {
            }

            // Token: 0x17000001 RID: 1
            // (get) Token: 0x06000022 RID: 34
            public extern string originalPath { [MethodImpl(MethodImplOptions.InternalCall)] get; }

            // Token: 0x17000002 RID: 2
            // (get) Token: 0x06000023 RID: 35
            public extern ulong frameCount { [MethodImpl(MethodImplOptions.InternalCall)] get; }

            // Token: 0x17000003 RID: 3
            // (get) Token: 0x06000024 RID: 36
            public extern double frameRate { [MethodImpl(MethodImplOptions.InternalCall)] get; }

            // Token: 0x17000004 RID: 4
            // (get) Token: 0x06000025 RID: 37
            public extern double length { [MethodImpl(MethodImplOptions.InternalCall)] get; }

            // Token: 0x17000005 RID: 5
            // (get) Token: 0x06000026 RID: 38
            public extern uint width { [MethodImpl(MethodImplOptions.InternalCall)] get; }

            // Token: 0x17000006 RID: 6
            // (get) Token: 0x06000027 RID: 39
            public extern uint height { [MethodImpl(MethodImplOptions.InternalCall)] get; }

            // Token: 0x17000007 RID: 7
            // (get) Token: 0x06000028 RID: 40
            public extern uint pixelAspectRatioNumerator { [MethodImpl(MethodImplOptions.InternalCall)] get; }

            // Token: 0x17000008 RID: 8
            // (get) Token: 0x06000029 RID: 41
            public extern uint pixelAspectRatioDenominator { [MethodImpl(MethodImplOptions.InternalCall)] get; }

            // Token: 0x17000009 RID: 9
            // (get) Token: 0x0600002A RID: 42
            //public extern bool sRGB { [NativeName("IssRGB")][MethodImpl(MethodImplOptions.InternalCall)] get; }

            // Token: 0x1700000A RID: 10
            // (get) Token: 0x0600002B RID: 43
            public extern ushort audioTrackCount { [MethodImpl(MethodImplOptions.InternalCall)] get; }

            // Token: 0x0600002C RID: 44
            [MethodImpl(MethodImplOptions.InternalCall)]
            public extern ushort GetAudioChannelCount(ushort audioTrackIdx);

            // Token: 0x0600002D RID: 45
            [MethodImpl(MethodImplOptions.InternalCall)]
            public extern uint GetAudioSampleRate(ushort audioTrackIdx);

            // Token: 0x0600002E RID: 46
            [MethodImpl(MethodImplOptions.InternalCall)]
            public extern string GetAudioLanguage(ushort audioTrackIdx);
        }
}
