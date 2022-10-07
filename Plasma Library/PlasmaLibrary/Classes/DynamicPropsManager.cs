using UnityEngine;

// Token: 0x02000167 RID: 359
public class DynamicPropsManager : MonoBehaviour
{

	// Token: 0x06000E0B RID: 3595 RVA: 0x000478A8 File Offset: 0x00045AA8
	private void Awake()
	{
		this._registeredProps = new Dictionary<int, DynamicProp>();
		foreach (DynamicProp dynamicProp in base.transform.GetComponentsInChildren<DynamicProp>())
		{
			this._registeredProps.Add(dynamicProp.id, dynamicProp);
		}
	}

	// Token: 0x06000E0C RID: 3596 RVA: 0x000478F0 File Offset: 0x00045AF0
	public void StoreInterpolationState()
	{
		foreach (DynamicProp dynamicProp in this._registeredProps.Values)
		{
			dynamicProp.StoreInterpolation();
		}
	}

	// Token: 0x06000E0D RID: 3597 RVA: 0x00047948 File Offset: 0x00045B48
	public void InterpolateAndBoundsCheck(float d, Vector3 worldCenter, float worldRadiusSquared)
	{
		List<int> list = new List<int>();
		foreach (KeyValuePair<int, DynamicProp> keyValuePair in this._registeredProps)
		{
			keyValuePair.Value.Interpolate(d);
			if ((worldCenter - keyValuePair.Value.transform.position).sqrMagnitude > worldRadiusSquared)
			{
				list.Add(keyValuePair.Key);
			}
		}
		foreach (int key in list)
		{
			//Controllers.worldController.DynamicPropDestroy(this._registeredProps[key].gameObject);
			this._registeredProps.Remove(key);
		}
	}

	// Token: 0x06000E0E RID: 3598 RVA: 0x00047A38 File Offset: 0x00045C38
	public SerializedPropsManager Serialize()
	{
		SerializedPropsManager serializedPropsManager = new SerializedPropsManager();
		serializedPropsManager.serializedDyamicProps = new List<SerializedDyamicProp>();
		foreach (DynamicProp dynamicProp in this._registeredProps.Values)
		{
			SerializedDyamicProp item = default(SerializedDyamicProp);
			item.id = dynamicProp.id;
			Rigidbody componentInChildren = dynamicProp.GetComponentInChildren<Rigidbody>();
			item.position = componentInChildren.transform.position;
			item.rotation = componentInChildren.transform.rotation;
			item.linearVelocity = componentInChildren.velocity;
			item.angularVelocity = componentInChildren.angularVelocity;
			serializedPropsManager.serializedDyamicProps.Add(item);
		}
		return serializedPropsManager;
	}

	// Token: 0x06000E0F RID: 3599 RVA: 0x00047B08 File Offset: 0x00045D08
	public void Load(SerializedPropsManager serializedPropsManager)
	{
		foreach (SerializedDyamicProp serializedDyamicProp in serializedPropsManager.serializedDyamicProps)
		{
			DynamicProp dynamicProp;
			if (this._registeredProps.TryGetValue(serializedDyamicProp.id, out dynamicProp))
			{
				Rigidbody componentInChildren = dynamicProp.GetComponentInChildren<Rigidbody>();
				componentInChildren.position = serializedDyamicProp.position;
				componentInChildren.rotation = serializedDyamicProp.rotation;
				componentInChildren.velocity = serializedDyamicProp.linearVelocity;
				componentInChildren.angularVelocity = serializedDyamicProp.angularVelocity;
			}
			else
			{
				string str = "Couldn't find dynamic prop ";
				int id = serializedDyamicProp.id;
				//Plasma.LogWarning(str + id.ToString());
			}
		}
	}

	// Token: 0x04000B76 RID: 2934
	private Dictionary<int, DynamicProp> _registeredProps;
}