using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlasmaFileReader.Plasma.Classes
{
	// Token: 0x02000226 RID: 550
	public class EasyStateMachine
	{
		// Token: 0x17000214 RID: 532
		// (get) Token: 0x06001359 RID: 4953 RVA: 0x000637B4 File Offset: 0x000619B4
		// (set) Token: 0x0600135A RID: 4954 RVA: 0x000637BC File Offset: 0x000619BC
		public EasyStateMachine.Status status { get; private set; }

		// Token: 0x17000215 RID: 533
		// (get) Token: 0x0600135B RID: 4955 RVA: 0x000637C5 File Offset: 0x000619C5
		// (set) Token: 0x0600135C RID: 4956 RVA: 0x000637CD File Offset: 0x000619CD
		public EasyState nextState { get; set; }

		// Token: 0x17000216 RID: 534
		// (get) Token: 0x0600135D RID: 4957 RVA: 0x000637D6 File Offset: 0x000619D6
		public string name
		{
			get
			{
				return this._id.ToString();
			}
		}

		// Token: 0x17000217 RID: 535
		// (get) Token: 0x0600135E RID: 4958 RVA: 0x000637E9 File Offset: 0x000619E9
		public string currentStateName
		{
			get
			{
				EasyState currentState = this._currentState;
				if (currentState == null)
				{
					return null;
				}
				return currentState.name;
			}
		}

		// Token: 0x0600135F RID: 4959 RVA: 0x000637FC File Offset: 0x000619FC
		public EasyStateMachine(EasyFSMEnum id)
		{
			this._id = id;
			this._states = new List<EasyState>();
			this._controller = Controllers.easyFSMController;
			this.status = EasyStateMachine.Status.Idling;
		}

		// Token: 0x06001360 RID: 4960 RVA: 0x00063828 File Offset: 0x00061A28
		public EasyStateMachine AddState(EasyState state)
		{
			state.owner = this;
			this._states.Add(state);
			return this;
		}

		// Token: 0x06001361 RID: 4961 RVA: 0x0006383E File Offset: 0x00061A3E
		public void SetStartState(EasyState state)
		{
			this._startState = state;
		}

		// Token: 0x06001362 RID: 4962 RVA: 0x00063848 File Offset: 0x00061A48
		public void Run()
		{
			if (this.status != EasyStateMachine.Status.Running)
			{
				if (this._startState == null)
				{
					//EasyFSM.LogWarning("StartState not set.", null);
					return;
				}
				if (this._states.Count == 0)
				{
					//EasyFSM.LogWarning("State Machine is empty.", null);
					return;
				}
				//EasyFSM.Log("Starting FSM: " + this.name, null);
				this._currentState = this._startState;
				this.status = EasyStateMachine.Status.Running;
				this._coroutine = this._controller.StartCoroutine(this.MainLoop());
			}
		}

		// Token: 0x06001363 RID: 4963 RVA: 0x000638CB File Offset: 0x00061ACB
		private IEnumerator MainLoop()
		{
			bool done = false;
			while (!done)
			{
				if (this._currentState.status == EasyState.Status.Stopped)
				{
					this._currentState.Enter();
				}
				if (this._currentState.status == EasyState.Status.Running)
				{
					yield return this._currentState.Execute();
					this.TryMovingToNextState();
				}
				if (this._currentState.status == EasyState.Status.Resting)
				{
					if (this._currentState.hasTransitions)
					{
						this.nextState = this._currentState.CheckTransitions();
						if (!this.TryMovingToNextState())
						{
							yield return null;
						}
					}
					else
					{
						this._currentState.Exit();
						done = true;
					}
				}
			}
			this.status = EasyStateMachine.Status.Finished;
			yield break;
		}

		// Token: 0x06001364 RID: 4964 RVA: 0x000638DC File Offset: 0x00061ADC
		public void Stop()
		{
			if (this._coroutine != null)
			{
				if (this._controller != null)
				{
					this._controller.StopCoroutine(this._coroutine);
				}
				this._coroutine = null;
				this.status = EasyStateMachine.Status.Idling;
				//EasyFSM.Log("Stopping FSM: " + this.name, null);
			}
		}

		// Token: 0x06001365 RID: 4965 RVA: 0x00063934 File Offset: 0x00061B34
		private bool TryMovingToNextState()
		{
			if (this.nextState != null)
			{
				this._currentState.Exit();
				this._currentState = this.nextState;
				this.nextState = null;
				return true;
			}
			return false;
		}

		// Token: 0x04000FCE RID: 4046
		private EasyFSMEnum _id;

		// Token: 0x04000FCF RID: 4047
		private List<EasyState> _states;

		// Token: 0x04000FD0 RID: 4048
		private EasyState _startState;

		// Token: 0x04000FD1 RID: 4049
		private EasyState _currentState;

		// Token: 0x04000FD2 RID: 4050
		private Coroutine _coroutine;

		// Token: 0x04000FD3 RID: 4051
		private EasyFSMController _controller;

		// Token: 0x0200043C RID: 1084
		public enum Status
		{
			// Token: 0x04001EB0 RID: 7856
			Idling,
			// Token: 0x04001EB1 RID: 7857
			Running,
			// Token: 0x04001EB2 RID: 7858
			Finished
		}
	}
}
