// Token: 0x02000227 RID: 551
using System.Collections.Generic;

public class EasyTransition
{
	// Token: 0x17000218 RID: 536
	// (get) Token: 0x06001366 RID: 4966 RVA: 0x0006395F File Offset: 0x00061B5F
	// (set) Token: 0x06001367 RID: 4967 RVA: 0x00063967 File Offset: 0x00061B67
	public EasyTransition.Types type { get; set; }

	// Token: 0x17000219 RID: 537
	// (get) Token: 0x06001368 RID: 4968 RVA: 0x00063970 File Offset: 0x00061B70
	public EasyState targetState { get; }

	// Token: 0x06001369 RID: 4969 RVA: 0x00063978 File Offset: 0x00061B78
	public EasyTransition(EasyState targetState)
	{
		this._conditions = new List<EasyTransition.Condition>();
		this.targetState = targetState;
	}

	// Token: 0x0600136A RID: 4970 RVA: 0x00063992 File Offset: 0x00061B92
	public void AddCondition(EasyTransition.Condition condition)
	{
		this._conditions.Add(condition);
	}

	// Token: 0x0600136B RID: 4971 RVA: 0x000639A0 File Offset: 0x00061BA0
	public bool Check()
	{
		EasyTransition.Types type = this.type;
		if (type == EasyTransition.Types.AllMustPass)
		{
			using (List<EasyTransition.Condition>.Enumerator enumerator = this._conditions.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!enumerator.Current())
					{
						return false;
					}
				}
			}
			return true;
		}
		if (type != EasyTransition.Types.AnySuffices)
		{
			return false;
		}
		using (List<EasyTransition.Condition>.Enumerator enumerator = this._conditions.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current())
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x04000FD6 RID: 4054
	private List<EasyTransition.Condition> _conditions;

	// Token: 0x0200043E RID: 1086
	// (Invoke) Token: 0x06002272 RID: 8818
	public delegate bool Condition();

	// Token: 0x0200043F RID: 1087
	public enum Types
	{
		// Token: 0x04001EB8 RID: 7864
		AllMustPass,
		// Token: 0x04001EB9 RID: 7865
		AnySuffices
	}
}
