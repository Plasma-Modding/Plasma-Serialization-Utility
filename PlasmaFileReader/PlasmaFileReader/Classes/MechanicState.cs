using System;
using System.Collections.Generic;
using Sirenix.Serialization;
using UnityEngine;

namespace PlasmaFileReader.Plasma.Classes
{
    // Token: 0x02000171 RID: 369
    public class MechanicState
    {
        // Token: 0x06000E35 RID: 3637 RVA: 0x0004887D File Offset: 0x00046A7D
        public MechanicState()
        {
            this.articulations = new List<MechanicState.MechanicStateArticulation>();
            this.componentStates = new List<MechanicState.ComponentState>();
        }

        // Token: 0x06000E36 RID: 3638 RVA: 0x0004889C File Offset: 0x00046A9C
        public MechanicState(MechanicState state)
        {
            this.articulations = new List<MechanicState.MechanicStateArticulation>();
            this.componentStates = new List<MechanicState.ComponentState>();
            foreach (MechanicState.MechanicStateArticulation articulation in state.articulations)
            {
                MechanicState.MechanicStateArticulation item = new MechanicState.MechanicStateArticulation(articulation);
                this.articulations.Add(item);
            }
        }

        // Token: 0x06000E37 RID: 3639 RVA: 0x00048918 File Offset: 0x00046B18
        public void Reset(Device device)
        {
            this.articulations[0].baseRawPosition = device.rootComponent.wireframePosition;
            this.articulations[0].baseRawOrientation = device.rootComponent.wireframeRotation;
            this.articulations[0].baseRawLinearVelocity = Vector3.zero;
            this.articulations[0].baseRawAngleVelocity = Vector3.zero;
            this.articulations[0].relativeBaseAngularVelocity = Vector3.zero;
            this.articulations[0].relativeBaseLinearVelocity = Vector3.zero;
            foreach (MechanicState.ComponentState componentState in this.componentStates)
            {
                componentState.Reset();
            }
        }

        // Token: 0x06000E38 RID: 3640 RVA: 0x000489F8 File Offset: 0x00046BF8
        public void CreateDefaults(Vector3 basePosition, Quaternion baseRotation, int baseComponentGuid, ComponentHandler componentHandler = null)
        {
            MechanicState.MechanicStateArticulation mechanicStateArticulation = new MechanicState.MechanicStateArticulation();
            mechanicStateArticulation.baseRawPosition = basePosition;
            mechanicStateArticulation.baseRawOrientation = baseRotation;
            mechanicStateArticulation.baseComponentGuid = baseComponentGuid;
            if (componentHandler != null && componentHandler.subComponentsCount > 1)
            {
                this.componentStates.Add(new MechanicState.ComponentState(componentHandler));
            }
            this.articulations.Add(mechanicStateArticulation);
        }

        // Token: 0x06000E39 RID: 3641 RVA: 0x00048A54 File Offset: 0x00046C54
        public void CreateDefaults(Device device)
        {
            MechanicState.MechanicStateArticulation item = new MechanicState.MechanicStateArticulation();
            this.articulations.Add(item);
            foreach (ComponentHandler componentHandler in device.allComponentsHierarchy)
            {
                if (componentHandler.subComponentsCount > 1)
                {
                    this.componentStates.Add(new MechanicState.ComponentState(componentHandler));
                }
            }
        }

        // Token: 0x06000E3A RID: 3642 RVA: 0x00048ACC File Offset: 0x00046CCC
        public byte[] ToBytes()
        {
            return SerializationUtility.SerializeValue<MechanicState>(this, DataFormat.Binary, null);
        }

        // Token: 0x06000E3B RID: 3643 RVA: 0x00048AD6 File Offset: 0x00046CD6
        public static MechanicState Load(byte[] bytes)
        {
            if (bytes == null)
            {
                return null;
            }
            return SerializationUtility.DeserializeValue<MechanicState>(bytes, DataFormat.Binary, null);
        }

        // Token: 0x06000E3D RID: 3645 RVA: 0x00048EE0 File Offset: 0x000470E0
        public MechanicState.MechanicStateArticulation ArticulatorForBaseComponentWithGuid(int guid)
        {
            foreach (MechanicState.MechanicStateArticulation mechanicStateArticulation in this.articulations)
            {
                if (mechanicStateArticulation.baseComponentGuid == guid)
                {
                    return mechanicStateArticulation;
                }
            }
            return null;
        }

        // Token: 0x06000E3E RID: 3646 RVA: 0x00048F3C File Offset: 0x0004713C
        public MechanicState.ComponentState GetComponentState(int componentGuid)
        {
            foreach (MechanicState.ComponentState componentState in this.componentStates)
            {
                if (componentState.guid == componentGuid)
                {
                    return componentState;
                }
            }
            return null;
        }

        // Token: 0x06000E3F RID: 3647 RVA: 0x00048F98 File Offset: 0x00047198
        public void AddArticulation(MechanicState.MechanicStateArticulation newArticulation)
        {
            this.articulations.Add(newArticulation);
        }

        // Token: 0x06000E40 RID: 3648 RVA: 0x00048FA8 File Offset: 0x000471A8
        public void Merge(MechanicState otherMechanicState, int currentGuid, int newGuid)
        {
            foreach (MechanicState.ComponentState componentState in otherMechanicState.componentStates)
            {
                if (componentState.guid == currentGuid)
                {
                    MechanicState.ComponentState componentState2 = new MechanicState.ComponentState(componentState);
                    componentState2.guid = newGuid;
                    this.componentStates.Add(componentState2);
                }
            }
        }

        // Token: 0x04000BBD RID: 3005
        public List<MechanicState.MechanicStateArticulation> articulations;

        // Token: 0x04000BBE RID: 3006
        public List<MechanicState.ComponentState> componentStates;

        // Token: 0x020003B8 RID: 952
        public class ComponentState
        {
            // Token: 0x060020D0 RID: 8400 RVA: 0x000A1B00 File Offset: 0x0009FD00
            public ComponentState()
            {
                this.subComponentStates = new Dictionary<int, MechanicState.SubComponentState>();
            }

            // Token: 0x060020D1 RID: 8401 RVA: 0x000A1B14 File Offset: 0x0009FD14
            public ComponentState(ComponentHandler componentHandler)
            {
                this.subComponentStates = new Dictionary<int, MechanicState.SubComponentState>();
                this.guid = componentHandler.agentId.guid;
                if (componentHandler.subComponentsCount > 1)
                {
                    for (int i = 1; i < componentHandler.subComponentsCount; i++)
                    {
                        MechanicState.SubComponentState value = new MechanicState.SubComponentState();
                        this.subComponentStates.Add(i, value);
                    }
                }
            }

            // Token: 0x060020D2 RID: 8402 RVA: 0x000A1B70 File Offset: 0x0009FD70
            public ComponentState(MechanicState.ComponentState state)
            {
                this.subComponentStates = new Dictionary<int, MechanicState.SubComponentState>();
                this.guid = state.guid;
                foreach (int key in state.subComponentStates.Keys)
                {
                    MechanicState.SubComponentState value = new MechanicState.SubComponentState(state.subComponentStates[key]);
                    this.subComponentStates.Add(key, value);
                }
            }

            // Token: 0x060020D4 RID: 8404 RVA: 0x000A1C52 File Offset: 0x0009FE52
            public float[] GetRawDofVars(int index)
            {
                return this.subComponentStates[index].stateRawVelVars;
            }

            // Token: 0x060020D5 RID: 8405 RVA: 0x000A1C68 File Offset: 0x0009FE68
            public void Reset()
            {
                foreach (int key in this.subComponentStates.Keys)
                {
                    this.subComponentStates[key].Reset();
                }
            }

            // Token: 0x04001D0B RID: 7435
            public int guid;

            // Token: 0x04001D0C RID: 7436
            public Dictionary<int, MechanicState.SubComponentState> subComponentStates;
        }

        // Token: 0x020003B9 RID: 953
        public class SubComponentState
        {
            // Token: 0x060020D6 RID: 8406 RVA: 0x000A1CCC File Offset: 0x0009FECC
            public SubComponentState()
            {
                this.stateRawPosVars = new float[1];
                this.stateRawVelVars = new float[1];
                this.relativePositions = new float[1];
            }

            // Token: 0x060020D7 RID: 8407 RVA: 0x000A1CF8 File Offset: 0x0009FEF8
            public SubComponentState(MechanicState.SubComponentState state)
            {
                this.subComponentIndex = state.subComponentIndex;
                this.stateRawPosVars = state.stateRawPosVars;
                this.stateRawVelVars = state.stateRawVelVars;
                this.supportsScaling = state.supportsScaling;
                this.relativePositions = state.relativePositions;
                this.axisA = state.axisA;
                this.axisB = state.axisB;
                this.localRotation = state.localRotation;
            }

            // Token: 0x060020D8 RID: 8408 RVA: 0x000A1D6C File Offset: 0x0009FF6C
            public void Reset()
            {
                for (int i = 0; i < this.stateRawPosVars.Length; i++)
                {
                    if (i == 3)
                    {
                        this.stateRawPosVars[i] = 1f;
                    }
                    else
                    {
                        this.stateRawPosVars[i] = 0f;
                    }
                }
                for (int j = 0; j < this.stateRawVelVars.Length; j++)
                {
                    this.stateRawVelVars[j] = 0f;
                }
                for (int k = 0; k < this.relativePositions.Length; k++)
                {
                    this.relativePositions[k] = 0f;
                }
            }

            // Token: 0x04001D0D RID: 7437
            public int subComponentIndex;

            // Token: 0x04001D0E RID: 7438
            public float[] stateRawPosVars;

            // Token: 0x04001D0F RID: 7439
            public float[] stateRawVelVars;

            // Token: 0x04001D10 RID: 7440
            public bool supportsScaling;

            // Token: 0x04001D11 RID: 7441
            public Vector3 axisA;

            // Token: 0x04001D12 RID: 7442
            public Vector3 axisB;

            // Token: 0x04001D13 RID: 7443
            public float[] relativePositions;

            // Token: 0x04001D14 RID: 7444
            public Quaternion localRotation;
        }

        // Token: 0x020003BA RID: 954
        public class MechanicStateArticulation
        {
            // Token: 0x060020D9 RID: 8409 RVA: 0x000A1DEC File Offset: 0x0009FFEC
            public MechanicStateArticulation()
            {
            }

            // Token: 0x060020DA RID: 8410 RVA: 0x000A1DF4 File Offset: 0x0009FFF4
            public MechanicStateArticulation(ComponentHandler componentHandler)
            {
                this.baseComponentGuid = componentHandler.agentId.guid;
                this.baseRawPosition = componentHandler.wireframePosition;
                this.baseRawOrientation = componentHandler.wireframeRotation;
            }

            // Token: 0x060020DB RID: 8411 RVA: 0x000A1E28 File Offset: 0x000A0028
            public MechanicStateArticulation(MechanicState.MechanicStateArticulation articulation)
            {
                this.baseComponentGuid = articulation.baseComponentGuid;
                this.baseRawPosition = articulation.baseRawPosition;
                this.baseRawLinearVelocity = articulation.baseRawLinearVelocity;
                this.baseRawAngleVelocity = articulation.baseRawAngleVelocity;
                this.baseRawOrientation = articulation.baseRawOrientation;
                this.relativeBaseLinearVelocity = articulation.relativeBaseLinearVelocity;
                this.relativeBaseAngularVelocity = articulation.relativeBaseAngularVelocity;
            }

            // Token: 0x04001D15 RID: 7445
            public int baseComponentGuid;

            // Token: 0x04001D16 RID: 7446
            public Vector3 baseRawPosition;

            // Token: 0x04001D17 RID: 7447
            public Vector3 baseRawLinearVelocity;

            // Token: 0x04001D18 RID: 7448
            public Vector3 baseRawAngleVelocity;

            // Token: 0x04001D19 RID: 7449
            public Quaternion baseRawOrientation;

            // Token: 0x04001D1A RID: 7450
            public Vector3 relativeBaseLinearVelocity;

            // Token: 0x04001D1B RID: 7451
            public Vector3 relativeBaseAngularVelocity;
        }

        // Token: 0x020003BB RID: 955
        private struct ChildLinkAndItsStateDesc
        {
            // Token: 0x04001D1C RID: 7452
            public int m_linkIndex;

            // Token: 0x04001D1D RID: 7453
            public int m_dofCount;

            // Token: 0x04001D1E RID: 7454
            public int m_dofOffset;

            // Token: 0x04001D1F RID: 7455
            public float position;

            // Token: 0x04001D20 RID: 7456
            public float velocity;
        }
    }

}
