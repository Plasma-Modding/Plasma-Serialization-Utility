using System;
using System.Collections;
using System.Collections.Generic;

namespace Plasma.Classes
{
	// Token: 0x02000225 RID: 549
	public class EasyState
	{
		// Token: 0x17000210 RID: 528
		// (get) Token: 0x06001349 RID: 4937 RVA: 0x00063576 File Offset: 0x00061776
		public string name { get; }

		// Token: 0x17000211 RID: 529
		// (get) Token: 0x0600134A RID: 4938 RVA: 0x0006357E File Offset: 0x0006177E
		// (set) Token: 0x0600134B RID: 4939 RVA: 0x00063586 File Offset: 0x00061786
		public EasyStateMachine owner { get; set; }

		// Token: 0x17000212 RID: 530
		// (get) Token: 0x0600134C RID: 4940 RVA: 0x0006358F File Offset: 0x0006178F
		// (set) Token: 0x0600134D RID: 4941 RVA: 0x00063597 File Offset: 0x00061797
		public EasyState.Status status { get; private set; }

		// Token: 0x17000213 RID: 531
		// (get) Token: 0x0600134E RID: 4942 RVA: 0x000635A0 File Offset: 0x000617A0
		public bool hasTransitions
		{
			get
			{
				return this._transitions.Count > 0;
			}
		}

		// Token: 0x0600134F RID: 4943 RVA: 0x000635B0 File Offset: 0x000617B0
		public EasyState(string theName)
		{
			this.name = theName;
			this._onEnterTasks = new List<EasyState.Task>();
			this._onUpdateTasks = new List<EasyState.Task>();
			this._onExitTasks = new List<EasyState.Task>();
			this._transitions = new List<EasyTransition>();
			this.status = EasyState.Status.Stopped;
		}

		// Token: 0x06001350 RID: 4944 RVA: 0x000635FD File Offset: 0x000617FD
		public void AddInstantTask(EasyState.InstantTask instantTask, EasyState.Stages stage = EasyState.Stages.OnUpdate)
		{
			this.AddTask(new EasyState.Task(instantTask), stage);
		}

		// Token: 0x06001351 RID: 4945 RVA: 0x0006360C File Offset: 0x0006180C
		public void AddCoroutineTask(Func<IEnumerator> coroutineTask, EasyState.Stages stage = EasyState.Stages.OnUpdate)
		{
			this.AddTask(new EasyState.Task(coroutineTask), stage);
		}

		// Token: 0x06001352 RID: 4946 RVA: 0x0006361B File Offset: 0x0006181B
		public void AddCoroutineTask(Func<object, IEnumerator> coroutineTask, object coroutineParameter, EasyState.Stages stage = EasyState.Stages.OnUpdate)
		{
			this.AddTask(new EasyState.Task(coroutineTask, coroutineParameter), stage);
		}

		// Token: 0x06001353 RID: 4947 RVA: 0x0006362B File Offset: 0x0006182B
		private void AddTask(EasyState.Task task, EasyState.Stages stage)
		{
			switch (stage)
			{
			case EasyState.Stages.OnEnter:
				this._onEnterTasks.Add(task);
				return;
			case EasyState.Stages.OnUpdate:
				this._onUpdateTasks.Add(task);
				return;
			case EasyState.Stages.OnExit:
				this._onExitTasks.Add(task);
				return;
			default:
				return;
			}
		}

		// Token: 0x06001354 RID: 4948 RVA: 0x00063666 File Offset: 0x00061866
		public void AddTransition(EasyTransition transition)
		{
			this._transitions.Add(transition);
		}

		// Token: 0x06001355 RID: 4949 RVA: 0x00063674 File Offset: 0x00061874
		public void Enter()
		{
			//EasyFSM.Log(string.Concat(new string[]
			//{
			//	"Entering state: ",
			//	this.name,
			//	" [",
			//	this.owner.name,
			//	"]"
			//}), this.owner.name);
			this.status = ((this._onUpdateTasks.Count > 0) ? EasyState.Status.Running : EasyState.Status.Resting);
		}

		// Token: 0x06001356 RID: 4950 RVA: 0x000636E3 File Offset: 0x000618E3
		public IEnumerator Execute()
		{
			foreach (EasyState.Task task in this._onUpdateTasks)
			{
				if (task.instantTask != null)
				{
					string[] array = new string[5];
					array[0] = "Executing instant task: ";
					int num = 1;
					EasyState.InstantTask instantTask = task.instantTask;
					array[num] = ((instantTask != null) ? instantTask.ToString() : null);
					array[2] = " [";
					array[3] = this.owner.name;
					array[4] = "]";
					//EasyFSM.Log(string.Concat(array), this.owner.name);
					task.instantTask();
				}
				else if (task.coroutineTask != null || task.coroutineTaskWithParameter != null)
				{
					IEnumerator guestCoroutine;
					if (task.coroutineTask != null)
					{
						guestCoroutine = task.coroutineTask();
						string[] array2 = new string[5];
						array2[0] = "Executing coroutine task: ";
						int num2 = 1;
						Func<IEnumerator> coroutineTask = task.coroutineTask;
						array2[num2] = ((coroutineTask != null) ? coroutineTask.ToString() : null);
						array2[2] = " [";
						array2[3] = this.owner.name;
						array2[4] = "]";
						//EasyFSM.Log(string.Concat(array2), this.owner.name);
					}
					else
					{
						guestCoroutine = task.coroutineTaskWithParameter(task.coroutineParameter);
						string[] array3 = new string[7];
						array3[0] = "Executing coroutine task: ";
						int num3 = 1;
						Func<object, IEnumerator> coroutineTaskWithParameter = task.coroutineTaskWithParameter;
						array3[num3] = ((coroutineTaskWithParameter != null) ? coroutineTaskWithParameter.ToString() : null);
						array3[2] = " with parameter: ";
						int num4 = 3;
						object coroutineParameter = task.coroutineParameter;
						array3[num4] = ((coroutineParameter != null) ? coroutineParameter.ToString() : null);
						array3[4] = " [";
						array3[5] = this.owner.name;
						array3[6] = "]";
						//EasyFSM.Log(string.Concat(array3), this.owner.name);
					}
					while (guestCoroutine.MoveNext())
					{
						object obj = guestCoroutine.Current;
						yield return obj;
						this.owner.nextState = this.CheckTransitions();
						if (this.owner.nextState != null)
						{
							yield break;
						}
					}
					guestCoroutine = null;
				}
			}
			List<EasyState.Task>.Enumerator enumerator = default(List<EasyState.Task>.Enumerator);
			this.status = EasyState.Status.Resting;
			yield break;
			yield break;
		}

		// Token: 0x06001357 RID: 4951 RVA: 0x000636F4 File Offset: 0x000618F4
		public void Exit()
		{
			this.status = EasyState.Status.Stopped;
			//EasyFSM.Log(string.Concat(new string[]
			//{
			//	"Exiting state: ",
			//	this.name,
			//	" [",
			//	this.owner.name,
			//	"]"
			//}), this.owner.name);
		}

		// Token: 0x06001358 RID: 4952 RVA: 0x00063754 File Offset: 0x00061954
		public EasyState CheckTransitions()
		{
			foreach (EasyTransition easyTransition in this._transitions)
			{
				if (easyTransition.Check())
				{
					return easyTransition.targetState;
				}
			}
			return null;
		}

		// Token: 0x04000FC7 RID: 4039
		private List<EasyState.Task> _onEnterTasks;

		// Token: 0x04000FC8 RID: 4040
		private List<EasyState.Task> _onUpdateTasks;

		// Token: 0x04000FC9 RID: 4041
		private List<EasyState.Task> _onExitTasks;

		// Token: 0x04000FCA RID: 4042
		private List<EasyTransition> _transitions;

		// Token: 0x02000437 RID: 1079
		// (Invoke) Token: 0x0600225E RID: 8798
		public delegate void InstantTask();

		// Token: 0x02000438 RID: 1080
		private class Task
		{
			// Token: 0x06002261 RID: 8801 RVA: 0x000A4E31 File Offset: 0x000A3031
			public Task(EasyState.InstantTask instantTask)
			{
				this.instantTask = instantTask;
			}

			// Token: 0x06002262 RID: 8802 RVA: 0x000A4E40 File Offset: 0x000A3040
			public Task(Func<IEnumerator> coroutineTask)
			{
				this.coroutineTask = coroutineTask;
			}

			// Token: 0x06002263 RID: 8803 RVA: 0x000A4E4F File Offset: 0x000A304F
			public Task(Func<object, IEnumerator> coroutineTaskWithParameter, object coroutineParameter)
			{
				this.coroutineTaskWithParameter = coroutineTaskWithParameter;
				this.coroutineParameter = coroutineParameter;
			}

			// Token: 0x04001E9E RID: 7838
			public EasyState.InstantTask instantTask;

			// Token: 0x04001E9F RID: 7839
			public Func<IEnumerator> coroutineTask;

			// Token: 0x04001EA0 RID: 7840
			public Func<object, IEnumerator> coroutineTaskWithParameter;

			// Token: 0x04001EA1 RID: 7841
			public object coroutineParameter;
		}

		// Token: 0x02000439 RID: 1081
		public enum Status
		{
			// Token: 0x04001EA3 RID: 7843
			Stopped,
			// Token: 0x04001EA4 RID: 7844
			Running,
			// Token: 0x04001EA5 RID: 7845
			Resting
		}

		// Token: 0x0200043A RID: 1082
		public enum Stages
		{
			// Token: 0x04001EA7 RID: 7847
			OnEnter,
			// Token: 0x04001EA8 RID: 7848
			OnUpdate,
			// Token: 0x04001EA9 RID: 7849
			OnExit
		}
	}
}
